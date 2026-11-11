using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Text;

namespace BUtil.Windows.Services;

public static class WindowsStorePackage
{
    private const int AppModelErrorNoPackage = 15700;
    private const uint FileShareReadWriteDelete = 0x1 | 0x2 | 0x4;
    private const uint OpenExisting = 3;
    private const uint FileFlagBackupSemantics = 0x02000000;

    public static bool IsSandboxed { get; } = Detect();

    public static string UnsandboxPath(string path)
    {
        if (!IsSandboxed || string.IsNullOrWhiteSpace(path))
            return path;

        using var handle = CreateFileW(
            path,
            0,
            FileShareReadWriteDelete,
            IntPtr.Zero,
            OpenExisting,
            FileFlagBackupSemantics,
            IntPtr.Zero);
        if (handle.IsInvalid)
            return path;

        var buffer = new StringBuilder(512);
        var length = GetFinalPathNameByHandleW(handle, buffer, buffer.Capacity, 0);
        if (length == 0)
            return path;
        if (length > buffer.Capacity)
        {
            buffer.EnsureCapacity(length);
            length = GetFinalPathNameByHandleW(handle, buffer, buffer.Capacity, 0);
            if (length == 0)
                return path;
        }

        return StripDosDevicePrefix(buffer.ToString());
    }

    private static string StripDosDevicePrefix(string path)
    {
        const string uncPrefix = @"\\?\UNC\";
        const string dosPrefix = @"\\?\";
        if (path.StartsWith(uncPrefix, StringComparison.OrdinalIgnoreCase))
            return @"\\" + path[uncPrefix.Length..];
        if (path.StartsWith(dosPrefix, StringComparison.Ordinal))
            return path[dosPrefix.Length..];
        return path;
    }

    private static bool Detect()
    {
        try
        {
            var length = 0;
            var result = GetCurrentPackageFullName(ref length, null);
            return result != AppModelErrorNoPackage;
        }
        catch
        {
            return false;
        }
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
    private static extern int GetCurrentPackageFullName(
        ref int packageFullNameLength,
        StringBuilder? packageFullName);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
    private static extern SafeFileHandle CreateFileW(
        string lpFileName,
        uint dwDesiredAccess,
        uint dwShareMode,
        IntPtr lpSecurityAttributes,
        uint dwCreationDisposition,
        uint dwFlagsAndAttributes,
        IntPtr hTemplateFile);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
    private static extern int GetFinalPathNameByHandleW(
        SafeFileHandle hFile,
        StringBuilder lpszFilePath,
        int cchFilePath,
        uint dwFlags);
}
