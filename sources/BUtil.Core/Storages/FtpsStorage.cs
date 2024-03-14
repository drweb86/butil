
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using FluentFTP;
using System;
using System.IO;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;

namespace BUtil.Core.Storages;

class FtpsStorage : StorageBase<FtpsStorageSettingsV2>
{
    private readonly string? _normalizedFolder;
    private readonly FtpClient _client;

    internal FtpsStorage(ILog log, FtpsStorageSettingsV2 settings)
        : base(log, settings)
    {
        if (string.IsNullOrWhiteSpace(Settings.Host))
            throw new InvalidDataException(BUtil.Core.Localization.Resources.Server_Field_Address_Validation);
        if (Settings.Port < 0) // 0 - default.
            throw new InvalidDataException(BUtil.Core.Localization.Resources.Server_Field_Port_Validation);
        if (string.IsNullOrWhiteSpace(Settings.User))
            throw new InvalidDataException(BUtil.Core.Localization.Resources.User_Field_Validation);
        if (string.IsNullOrWhiteSpace(Settings.Password))
            throw new InvalidDataException(BUtil.Core.Localization.Resources.Password_Field_Validation_NotSpecified);

        _normalizedFolder = NormalizeNullablePath(Settings.Folder);

        _client = Mount();
    }

    private readonly object _uploadLock = new();
    public override IStorageUploadResult Upload(string sourceFile, string relativeFileName)
    {
        lock (_uploadLock) // because we're limited by upload speed by Server and Internet
        {
            var remotePath = GetRemoteNotNullablePath(relativeFileName);
            var status = _client.UploadFile(sourceFile, remotePath, FtpRemoteExists.Overwrite, true);
            if (status != FtpStatus.Success)
                throw new Exception("Failed to upload.");

            return new IStorageUploadResult
            {
                StorageFileName = remotePath,
                StorageFileNameSize = new FileInfo(sourceFile).Length,
            };
        }
    }

    public override void DeleteFolder(string relativeFolderName)
    {
        var remotePath = GetRemoteNotNullablePath(relativeFolderName);
        _client.DeleteDirectory(remotePath);
    }

    public override string[] GetFolders(string? relativeFolderName, string? mask = null)
    {
        FtpListOption listOption = FtpListOption.Auto;
        var remotePath = GetRemotePath(relativeFolderName, true);

        return this._client
            .GetListing(remotePath, listOption)
            .Where(x => x.Type == FtpObjectType.Directory)
            .Select(x => x.FullName)
            .Select(NormalizeNotNullablePath)
            .Select(x => remotePath == null ? x : x.Substring(remotePath.Length))
            .Select(NormalizeNotNullablePath)
            .Where(x => mask == null || FitsMask(Path.GetFileName(x), mask))
            .ToArray();
    }

    private static bool FitsMask(string fileName, string fileMask)
    {
        Regex mask = new(fileMask.Replace(".", "[.]").Replace("*", ".*").Replace("?", "."));
        return mask.IsMatch(fileName);
    }

    private FtpClient Mount()
    {
        Log.WriteLine(LoggingEvent.Debug, $"Mount");
        var client = new FtpClient(Settings.Host, Settings.User, Settings.Password, Settings.Port);
        client.Config.EncryptionMode = GetFtpEncryptionMode();
        client.Config.ValidateAnyCertificate = true;
        client.Connect();
        return client;
    }

    private FtpEncryptionMode GetFtpEncryptionMode()
    {
        return Settings.Encryption switch
        {
            FtpsStorageEncryptionV2.Explicit => FtpEncryptionMode.Explicit,
            FtpsStorageEncryptionV2.Implicit => FtpEncryptionMode.Implicit,
            _ => throw new ArgumentOutOfRangeException(nameof(Settings.Encryption)),
        };
    }

    private void Unmount()
    {
        Log.WriteLine(LoggingEvent.Debug, $"Unmount");
        if (_client.IsConnected)
        {
            _client.Disconnect();
        }
        _client.Dispose();
    }

    public override string? Test()
    {
        if (!string.IsNullOrWhiteSpace(Settings.Folder) && !_client.DirectoryExists(Settings.Folder))
        {
            return Localization.Resources.Field_Folder_Validation_NotExist;
        }
        else
        {
            try
            {
                _client.GetListing();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        return null;
    }

    public override bool Exists(string relativeFileName)
    {
        var remotePath = GetRemoteNotNullablePath(relativeFileName);
        return _client.FileExists(remotePath);
    }

    public override void Delete(string relativeFileName)
    {
        var remotePath = GetRemoteNotNullablePath(relativeFileName);
        _client.DeleteFile(remotePath);
    }

    public override void Download(string relativeFileName, string targetFileName)
    {
        var remotePath = GetRemoteNotNullablePath(relativeFileName);
        var status = _client.DownloadFile(targetFileName, remotePath);
        if (status != FtpStatus.Success)
        {
            throw new Exception();
        }
    }

    private static string? NormalizeNullablePath(string? path)
    {
        if (path == null)
            return null;
        return NormalizeNotNullablePath(path);
    }

    private static string NormalizeNotNullablePath(string path)
    {
        if (path.Contains(".."))
            throw new SecurityException("[..] is not allowed in path.");

        return path.Trim(new[] { '\\', '/' });
    }

    private string? GetRemotePath(string? relativePath, bool allowNull)
    {
        var normalizedRelativePath = NormalizeNullablePath(relativePath);
        if (!allowNull && string.IsNullOrWhiteSpace(normalizedRelativePath))
        {
            throw new ArgumentNullException(nameof(relativePath));
        }
        return normalizedRelativePath == null ? _normalizedFolder : string.IsNullOrWhiteSpace(_normalizedFolder) ? normalizedRelativePath : Path.Combine(_normalizedFolder, normalizedRelativePath);
    }

    private string GetRemoteNotNullablePath(string relativePath)
    {
        var normalizedRelativePath = NormalizeNullablePath(relativePath);
        if (string.IsNullOrWhiteSpace(normalizedRelativePath))
            throw new ArgumentNullException(nameof(relativePath));
        return string.IsNullOrWhiteSpace(this._normalizedFolder)
            ? normalizedRelativePath
            : Path.Combine(this._normalizedFolder, normalizedRelativePath);
    }

    public override string[] GetFiles(string? relativeFolderName = null, SearchOption option = SearchOption.TopDirectoryOnly)
    {
        FtpListOption listOption = FtpListOption.Auto;
        if (option == SearchOption.AllDirectories)
            listOption |= FtpListOption.Recursive;

        var remoteFolder = GetRemotePath(relativeFolderName, true);
        return this._client
            .GetListing(remoteFolder, listOption)
            .Where(x => x.Type == FtpObjectType.File)
            .Select(x => x.FullName)
            .Select(NormalizeNotNullablePath)
            .Select(x => remoteFolder == null ? x : x.Substring(remoteFolder.Length))
            .Select(NormalizeNotNullablePath)
            .ToArray();
    }

    public override DateTime GetModifiedTime(string relativeFileName)
    {
        var remotePath = GetRemoteNotNullablePath(relativeFileName);
        return _client.GetModifiedTime(remotePath);
    }

    public override void Dispose()
    {
        Unmount();
    }

    public override void Move(string fromRelativeFileName, string toRelativeFileName)
    {
        var fromRemotePath = GetRemoteNotNullablePath(fromRelativeFileName);
        var toRemotePath = GetRemoteNotNullablePath(toRelativeFileName);

        var dir = Path.GetDirectoryName(toRemotePath);
        if (!_client.DirectoryExists(dir))
            _client.CreateDirectory(dir);

        if (!_client.MoveFile(fromRemotePath, toRemotePath))
            throw new Exception();
    }
}
