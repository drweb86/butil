using BUtil.Core.Services;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace BUtil.Linux.Services;

/// <summary>
/// Blocks suspend/idle via logind's Inhibit lock (same mechanism as systemd-inhibit,
/// but in-process over the system bus — no child process and no terminal).
/// Closing the returned fds in <see cref="StopPreventSleep"/> drops the lock immediately.
/// Idle and sleep are taken as separate locks so cron (no login session) can still
/// inhibit idle suspend even when block-sleep is denied by polkit.
/// </summary>
internal sealed partial class LinuxOsSleepPreventionService : IOsSleepPreventionService
{
    private static readonly string[] InhibitWhat = ["idle", "sleep"];
    private const string Who = "BUtil";
    private const string Why = "A task is running";
    private const string Mode = "block";

    private readonly object _gate = new();
    private nint _bus;
    private readonly List<SafeFileHandle> _inhibitFds = [];

    public void PreventSleep()
    {
        lock (_gate)
        {
            if (IsLockHeld)
                return;

            try
            {
                TakeInhibitLocks();
            }
            catch
            {
                ReleaseInhibitLocks();
            }
        }
    }

    public void StopPreventSleep()
    {
        lock (_gate)
        {
            ReleaseInhibitLocks();
        }
    }

    private bool IsLockHeld => _inhibitFds.Exists(fd => fd is { IsClosed: false, IsInvalid: false });

    private void TakeInhibitLocks()
    {
        var r = NativeMethods.sd_bus_open_system(out var bus);
        if (r < 0)
            throw new InvalidOperationException($"sd_bus_open_system failed ({r}).");

        _bus = bus;

        foreach (var what in InhibitWhat)
        {
            try
            {
                var fd = Inhibit(bus, what);
                if (fd is { IsClosed: false, IsInvalid: false })
                    _inhibitFds.Add(fd);
            }
            catch
            {
                // sleep:block is often denied without an active logind session (cron).
                // idle:block is allowed; keep whatever succeeded.
            }
        }

        if (_inhibitFds.Count == 0)
            throw new InvalidOperationException("logind Inhibit returned no locks.");
    }

    private static SafeFileHandle Inhibit(nint bus, string what)
    {
        nint message = 0;
        nint reply = 0;
        try
        {
            var r = NativeMethods.sd_bus_message_new_method_call(
                bus,
                out message,
                "org.freedesktop.login1",
                "/org/freedesktop/login1",
                "org.freedesktop.login1.Manager",
                "Inhibit");
            if (r < 0)
                throw new InvalidOperationException($"sd_bus_message_new_method_call failed ({r}).");

            AppendString(message, what);
            AppendString(message, Who);
            AppendString(message, Why);
            AppendString(message, Mode);

            r = NativeMethods.sd_bus_call(bus, message, 0, 0, out reply);
            if (r < 0)
                throw new InvalidOperationException($"sd_bus_call Inhibit({what}) failed ({r}).");

            r = NativeMethods.sd_bus_message_read_basic(reply, (byte)'h', out var fd);
            if (r < 0)
                throw new InvalidOperationException($"sd_bus_message_read_basic failed ({r}).");

            var dupFd = NativeMethods.Dup(fd);
            if (dupFd < 0)
                throw new InvalidOperationException("dup of inhibit fd failed.");

            return new SafeFileHandle(dupFd, ownsHandle: true);
        }
        finally
        {
            if (message != 0)
                NativeMethods.sd_bus_message_unref(message);
            if (reply != 0)
                NativeMethods.sd_bus_message_unref(reply);
        }
    }

    private static void AppendString(nint message, string value)
    {
        var ptr = Marshal.StringToCoTaskMemUTF8(value);
        try
        {
            var r = NativeMethods.sd_bus_message_append_basic(message, (byte)'s', ptr);
            if (r < 0)
                throw new InvalidOperationException($"sd_bus_message_append_basic failed ({r}).");
        }
        finally
        {
            Marshal.FreeCoTaskMem(ptr);
        }
    }

    private void ReleaseInhibitLocks()
    {
        foreach (var handle in _inhibitFds)
        {
            try
            {
                handle.Dispose();
            }
            catch
            {
                // Closing the logind fd is best-effort; the task must still be able to finish.
            }
        }
        _inhibitFds.Clear();

        var bus = _bus;
        _bus = 0;
        if (bus != 0)
            NativeMethods.sd_bus_unref(bus);
    }

    private static partial class NativeMethods
    {
        private const string Systemd = "libsystemd.so.0";
        private const string Libc = "libc.so.6";

        [LibraryImport(Systemd)]
        public static partial int sd_bus_open_system(out nint bus);

        [LibraryImport(Systemd, StringMarshalling = StringMarshalling.Utf8)]
        public static partial int sd_bus_message_new_method_call(
            nint bus,
            out nint message,
            string destination,
            string path,
            string @interface,
            string member);

        [LibraryImport(Systemd)]
        public static partial int sd_bus_message_append_basic(nint message, byte type, nint value);

        [LibraryImport(Systemd)]
        public static partial int sd_bus_call(nint bus, nint message, ulong usec, nint error, out nint reply);

        [LibraryImport(Systemd)]
        public static partial int sd_bus_message_read_basic(nint message, byte type, out int value);

        [LibraryImport(Systemd)]
        public static partial nint sd_bus_message_unref(nint message);

        [LibraryImport(Systemd)]
        public static partial nint sd_bus_unref(nint bus);

        [LibraryImport(Libc, EntryPoint = "dup")]
        public static partial int Dup(int fd);
    }
}
