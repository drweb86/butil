using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Storages;
using MediaDevices;
using System.Security;

namespace BUtil.Windows.Services;

class MtpStorage : StorageBase<MtpStorageSettings>
{
    private readonly MediaDevice _mediaDevice;

    internal MtpStorage(ILog log, MtpStorageSettings settings)
        : base(log, settings)
    {
        if (string.IsNullOrWhiteSpace(Settings.Device))
            throw new InvalidDataException(Resources.Field_Device_Validation_Empty);
        if (string.IsNullOrWhiteSpace(Settings.Folder))
            throw new InvalidDataException(Resources.Field_Folder_Validation_Empty);

        _mediaDevice = Mount();
    }

    private readonly object _uploadLock = new();
    public override IStorageUploadResult Upload(string sourceFile, string relativeFileName)
    {
        lock (_uploadLock) // because we're limited by upload speed by Server and Internet
        {
            var remotePath = GetRemotePath(relativeFileName, false);
            _mediaDevice.UploadFile(sourceFile, remotePath);

            return new IStorageUploadResult
            {
                StorageFileName = remotePath,
                StorageFileNameSize = new FileInfo(sourceFile).Length,
            };
        }
    }

    public override void DeleteFolder(string relativeFolderName)
    {
        var remotePath = GetRemotePath(relativeFolderName, false);
        _mediaDevice.DeleteDirectory(remotePath, true);
    }

    public override string[] GetFolders(string relativeFolderName, string? mask = null)
    {
        var remotePath = GetRemotePath(relativeFolderName, true);

        return this._mediaDevice
            .GetDirectories(remotePath, mask)
            .Select(NormalizePathNotNull)
            .Select(x => remotePath == null ? x : x[remotePath.Length..])
            .Select(NormalizePathNotNull)
            .ToArray();
    }

    private MediaDevice Mount()
    {
        Log.WriteLine(LoggingEvent.Debug, $"Mount");
        var mediaDevice = MediaDevice
            .GetDevices()
            .FirstOrDefault(x => x.FriendlyName == Settings.Device)
                ?? throw new ArgumentException(Resources.Field_Device_Validation_NotFound);

        mediaDevice.Connect();

        return mediaDevice;
    }

    private void Unmount()
    {
        Log.WriteLine(LoggingEvent.Debug, $"Unmount");
        _mediaDevice?.Disconnect();
        _mediaDevice?.Dispose();
    }

    public override string? Test()
    {
        if (!_mediaDevice.DirectoryExists(Settings.Folder))
        {
            return BUtil.Core.Localization.Resources.Field_Folder_Validation_NotExist;
        }

        return null;
    }

    public override bool Exists(string relativeFileName)
    {
        var remotePath = this.GetRemotePath(relativeFileName, false);
        return _mediaDevice.FileExists(remotePath);
    }

    public override void Delete(string relativeFileName)
    {
        var remotePath = this.GetRemotePath(relativeFileName, false);
        _mediaDevice.DeleteFile(remotePath);
    }

    public override void Download(string relativeFileName, string targetFileName)
    {
        var remotePath = this.GetRemotePath(relativeFileName, false);
        _mediaDevice.DownloadFile(remotePath, targetFileName);
    }

    private static string? NormalizeNullablePath(string? path)
    {
        if (path == null)
            return null;

        return NormalizePathNotNull(path);
    }

    private static string NormalizePathNotNull(string path)
    {
        if (path.Contains(".."))
            throw new SecurityException("[..] is not allowed in path.");

        return path.Trim(['\\', '/']);
    }

    private string GetRemotePath(string? relativePath, bool allowNull)
    {
        var normalizedRelativePath = NormalizeNullablePath(relativePath);
        if (!allowNull && string.IsNullOrWhiteSpace(normalizedRelativePath))
        {
            throw new ArgumentNullException(nameof(relativePath));
        }
        return normalizedRelativePath == null ? this.Settings.Folder : Path.Combine(this.Settings.Folder, normalizedRelativePath);
    }

    public override string[] GetFiles(string? relativeFolderName = null, SearchOption option = SearchOption.TopDirectoryOnly)
    {
        var remoteFolder = GetRemotePath(relativeFolderName, true);
        return this._mediaDevice
            .GetFiles(remoteFolder, "*.*", option)
            .Select(NormalizePathNotNull)
            .Select(x => remoteFolder == null ? x : x[remoteFolder.Length..])
            .Select(NormalizePathNotNull)
            .ToArray();
    }

    public override DateTime GetModifiedTime(string relativeFileName)
    {
        var remotePath = GetRemotePath(relativeFileName, false);
        var fileInfo = _mediaDevice.GetFileInfo(remotePath);
        return MaxDate(fileInfo.CreationTime, fileInfo.LastWriteTime);
    }

    private static DateTime MaxDate(params DateTime?[] dates)
    {
        foreach (var date in dates)
        {
            if (!date.HasValue)
                continue;
            if (date.Value.Year < 1700)
                continue;
            return date.Value;
        }

        return DateTime.Now;
    }

    public override void Dispose()
    {
        Unmount();
    }

    public override void Move(string fromRelativeFileName, string toRelativeFileName)
    {
        lock (_uploadLock) // because we're limited by upload speed by Server and Internet
        {
            var fromRemotePath = GetRemotePath(fromRelativeFileName, false);
            var toRemotePath = GetRemotePath(toRelativeFileName, false);

            var destinationDirectory = Path.GetDirectoryName(toRemotePath) ?? string.Empty;
            if (!_mediaDevice.DirectoryExists(destinationDirectory))
                _mediaDevice.CreateDirectory(destinationDirectory);

            _mediaDevice.Rename(fromRemotePath, toRemotePath);
        }
    }
}
