
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Security;
using System.Text.RegularExpressions;

namespace BUtil.Core.Storages;

class SftpStorage : StorageBase<SftpStorageSettingsV2>
{
    private readonly string? _normalizedFolder;
    private readonly SftpClient _client;
    private readonly bool _autodetectConnectionSettings;

    internal SftpStorage(ILog log, SftpStorageSettingsV2 settings, bool autodetectConnectionSettings)
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
        _autodetectConnectionSettings = autodetectConnectionSettings;
    }

    private readonly object _uploadLock = new();

    public override IStorageUploadResult Upload(string sourceFile, string relativeFileName)
    {
        lock (_uploadLock) // because we're limited by upload speed by Server and Internet
        {
            if (Exists(relativeFileName))
                Delete(relativeFileName);

            var remotePath = GetRemoteNotNullablePath(relativeFileName);
            using (FileStream fs = File.OpenRead(sourceFile))
            {
                _client.UploadFile(fs, remotePath);
            }

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

    private static bool FitsMask(string fileName, string fileMask)
    {
        Regex mask = new(fileMask.Replace(".", "[.]").Replace("*", ".*").Replace("?", "."));
        return mask.IsMatch(fileName);
    }

    private SftpClient Mount()
    {
        Log.WriteLine(LoggingEvent.Debug, $"Mount");
        var client = new SftpClient(Settings.Host, Settings.Port,  Settings.User, Settings.Password);
        client.HostKeyReceived += Client_HostKeyReceived;
        client.Connect();
        return client;
    }

    private void Client_HostKeyReceived(object? sender, Renci.SshNet.Common.HostKeyEventArgs e)
    {
        if (_autodetectConnectionSettings)
        {
            Settings.TrustedFingerprint = e.FingerPrintSHA256;
        }

        e.CanTrust = e.FingerPrintSHA256 == Settings.TrustedFingerprint;

        if (!e.CanTrust)
        {
            Log.WriteLine(LoggingEvent.Error, $"Received host fingerprint from SFTP server is  policy {e.FingerPrintSHA256}. Expected fingerprint is {Settings.TrustedFingerprint}. Connection will not be accepted. You're facing Man-in-the-middle attack.");
            Log.WriteLine(LoggingEvent.Error, $"If you trust this changed fingerprint, open in BUtil this task in Edit mode and click Save. Application will record this fingerprint as trusted.");
        }
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
        if (!string.IsNullOrWhiteSpace(Settings.Folder) && !_client.Exists(Settings.Folder))
        {
            return Localization.Resources.Field_Folder_Validation_NotExist;
        }
        else
        {
            try
            {
                _client.ListDirectory(Settings.Folder!);
            }
            catch (Exception e)
            {
                return ExceptionHelper.ToString(e);
            }
        }
        return null;
    }

    public override bool Exists(string relativeFileName)
    {
        var remotePath = GetRemoteNotNullablePath(relativeFileName);
        return _client.Exists(remotePath);
    }

    public override void Delete(string relativeFileName)
    {
        var remotePath = GetRemoteNotNullablePath(relativeFileName);
        _client.DeleteFile(remotePath);
    }

    public override void Download(string relativeFileName, string targetFileName)
    {
        var remotePath = GetRemoteNotNullablePath(relativeFileName);
        using var outputStream = File.OpenWrite(targetFileName);
        _client.DownloadFile(targetFileName, outputStream);
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

        return path.Trim(['\\', '/']);
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

    private void GetFilesInternal(string directory, List<string> files, System.IO.SearchOption option)
    {
        foreach (SftpFile sftpFile in _client.ListDirectory(directory))
        {
            if (sftpFile.IsDirectory)
            {
                if (option == SearchOption.AllDirectories)
                    GetFilesInternal(sftpFile.FullName, files, option);
            }
            else
            {
                files.Add(sftpFile.FullName);
            }
        }
    }

    public override string[] GetFiles(string? relativeFolderName = null, SearchOption option = SearchOption.TopDirectoryOnly)
    {
        var files = new List<string>();
        var remoteFolder = GetRemotePath(relativeFolderName, true)!;
        GetFilesInternal(remoteFolder, files, option);
        return files.ToArray(); // TODO: relative paths.
    }

    public override string[] GetFolders(string? relativeFolderName, string? mask = null)
    {
        var directories = new List<string>();
        var remoteFolder = GetRemotePath(relativeFolderName, true)!;

        foreach (SftpFile sftpFile in _client.ListDirectory(remoteFolder))
        {
            if (!sftpFile.IsDirectory)
                continue;

            if (!string.IsNullOrWhiteSpace(mask))
            {
                if (FileSystemName.MatchesSimpleExpression(mask, sftpFile.Name))
                {
                    directories.Add(sftpFile.FullName);
                }
            }
            else 
            {
                directories.Add(sftpFile.FullName);
            }
        }
        return directories.ToArray(); // RELLLALLTIVE PATH
    }

    public override DateTime GetModifiedTime(string relativeFileName)
    {
        var remotePath = GetRemoteNotNullablePath(relativeFileName);
        return _client.GetLastWriteTimeUtc(remotePath);
    }

    public override void Dispose()
    {
        Unmount();
    }

    public override void Move(string fromRelativeFileName, string toRelativeFileName)
    {
        var fromRemotePath = GetRemoteNotNullablePath(fromRelativeFileName);
        var toRemotePath = GetRemoteNotNullablePath(toRelativeFileName);

        var dir = Path.GetDirectoryName(toRemotePath)!;
        if (!_client.Exists(dir))
            _client.CreateDirectory(dir);

        _client.RenameFile(fromRemotePath, toRemotePath);
    }
}
