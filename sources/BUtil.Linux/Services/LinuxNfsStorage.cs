using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Interop.Logs;
using BUtil.Core.Storages;
using BUtil.Storages.Nfs;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BUtil.Linux.Services;

class LinuxNfsStorage : StorageBase<NfsStorageSettingsV2>
{
    private readonly FolderStorage _proxy;
    private bool _isMounted;

    internal LinuxNfsStorage(ILog log, NfsStorageSettingsV2 settings)
        : base(log, settings)
    {
        if (string.IsNullOrWhiteSpace(Settings.Host))
            throw new InvalidDataException("NFS host is not specified.");
        if (string.IsNullOrWhiteSpace(Settings.SharePath))
            throw new InvalidDataException("NFS share path is not specified.");
        if (string.IsNullOrWhiteSpace(Settings.MountPoint))
            throw new InvalidDataException("NFS mount point is not specified.");

        Directory.CreateDirectory(Settings.MountPoint);
        Mount();

        _proxy = new FolderStorage(log, new FolderStorageSettingsV2
        {
            DestinationFolder = Settings.MountPoint,
            SingleBackupQuotaGb = Settings.SingleBackupQuotaGb,
        });
    }

    private void Mount()
    {
        Log.WriteLine(LoggingEvent.Debug, "Mount NFS");

        var source = $"{Settings.Host}:{Settings.SharePath}";
        string[] args = string.IsNullOrWhiteSpace(Settings.MountOptions)
            ? ["-t", "nfs", source, Settings.MountPoint]
            : ["-t", "nfs", "-o", Settings.MountOptions, source, Settings.MountPoint];

        if (!RunProcess("mount", args))
            throw new InvalidOperationException($"Cannot mount NFS share {source}");

        _isMounted = true;
    }

    private void Unmount()
    {
        if (!_isMounted) return;
        Log.WriteLine(LoggingEvent.Debug, "Unmount NFS");
        RunProcess("umount", Settings.MountPoint);
        _isMounted = false;
    }

    private bool RunProcess(string executable, params string[] arguments)
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = executable,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            }
        };
        foreach (var arg in arguments)
            process.StartInfo.ArgumentList.Add(arg);

        var stdOut = new StringBuilder();
        var stdErr = new StringBuilder();
        process.OutputDataReceived += (_, a) => stdOut.AppendLine(a.Data);
        process.ErrorDataReceived += (_, a) => stdErr.AppendLine(a.Data);

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();

        var success = process.ExitCode == 0;
        var outStr = stdOut.ToString();
        var errStr = stdErr.ToString();
        if (!string.IsNullOrWhiteSpace(outStr)) Log.LogProcessOutput(outStr, success);
        if (!string.IsNullOrWhiteSpace(errStr)) Log.LogProcessOutput(errStr, success);
        return success;
    }

    public override string? Test() => _proxy.Test();
    public override IStorageUploadResult Upload(string s, string r) => _proxy.Upload(s, r);
    public override void Delete(string r) => _proxy.Delete(r);
    public override void DeleteFolder(string r) => _proxy.DeleteFolder(r);
    public override string[] GetFolders(string r, string? mask = null) => _proxy.GetFolders(r, mask);
    public override bool Exists(string r) => _proxy.Exists(r);
    public override void Download(string r, string t) => _proxy.Download(r, t);
    public override string[] GetFiles(string? r = null, SearchOption o = SearchOption.TopDirectoryOnly) => _proxy.GetFiles(r, o);
    public override DateTime GetModifiedTime(string r) => _proxy.GetModifiedTime(r);
    public override void Move(string f, string t) => _proxy.Move(f, t);

    public override void Dispose()
    {
        _proxy.Dispose();
        Unmount();
    }
}
