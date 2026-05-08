using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using BUtil.Core.Storages;
using BUtil.Storages.Nfs;
using BUtil.Windows.Util;
using System;
using System.IO;

namespace BUtil.Windows.Services;

class WindowsNfsStorage : StorageBase<NfsStorageSettingsV2>
{
    private readonly FolderStorage _proxy;
    private bool _isMounted;

    internal WindowsNfsStorage(ILog log, NfsStorageSettingsV2 settings)
        : base(log, settings)
    {
        if (string.IsNullOrWhiteSpace(Settings.Host))
            throw new InvalidDataException("NFS host is not specified.");
        if (string.IsNullOrWhiteSpace(Settings.SharePath))
            throw new InvalidDataException("NFS share path is not specified.");
        if (string.IsNullOrWhiteSpace(Settings.MountPoint))
            throw new InvalidDataException("NFS drive letter is not specified (e.g. Z:).");

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

        var uncPath = @"\\" + Settings.Host + Settings.SharePath.Replace('/', '\\');
        var mountExe = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.System), "mount.exe");

        string[] args = string.IsNullOrWhiteSpace(Settings.MountOptions)
            ? [uncPath, Settings.MountPoint]
            : ["-o", Settings.MountOptions, uncPath, Settings.MountPoint];

        if (!WindowsCmdProcessHelper.ExecuteProcess(Log, mountExe, args))
            throw new InvalidOperationException($"Cannot mount NFS share {uncPath}");

        _isMounted = true;
    }

    private void Unmount()
    {
        if (!_isMounted) return;
        Log.WriteLine(LoggingEvent.Debug, "Unmount NFS");
        var umountExe = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.System), "umount.exe");
        WindowsCmdProcessHelper.ExecuteProcess(Log, umountExe, Settings.MountPoint);
        _isMounted = false;
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
