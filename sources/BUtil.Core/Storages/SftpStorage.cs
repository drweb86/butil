
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.FileSystem;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BUtil.Core.Storages;

class SftpStorage : StorageBase<SftpStorageSettingsV2>
{
    public static readonly int DefaultPort = 22;
    private readonly SftpClient _client;

    internal SftpStorage(ILog log, SftpStorageSettingsV2 settings, bool testingConnection)
        : base(log, settings)
    {
        if (string.IsNullOrWhiteSpace(Settings.Host))
            throw new InvalidDataException(Localization.Resources.Server_Field_Address_Validation);
        if (Settings.Port < 0) // 0 - default.
            throw new InvalidDataException(Localization.Resources.Server_Field_Port_Validation);
        if (string.IsNullOrWhiteSpace(Settings.User))
            throw new InvalidDataException(Localization.Resources.User_Field_Validation);
        if (string.IsNullOrWhiteSpace(Settings.Password) &&
            string.IsNullOrWhiteSpace(Settings.KeyFile))
            throw new InvalidDataException(Localization.Resources.SFTP_Validation_PasswordAndKeyNotSpecified);
        if (!testingConnection && string.IsNullOrWhiteSpace(Settings.FingerPrintSHA256))
            throw new InvalidDataException(Localization.Resources.FingerPrintSHA256_Field_Validation_Empty);
        if (string.IsNullOrWhiteSpace(Settings.Folder))
            throw new InvalidDataException(Localization.Resources.Field_Folder_Validation_Empty);
        if (Settings.Folder == "/")
            throw new InvalidDataException(Localization.Resources.SFTPFolder_Field_Validation_RootFolder);
        if (!Settings.Folder.StartsWith("/"))
            throw new InvalidDataException(Localization.Resources.SFTPFolder_Field_Validation_InvalidPrefix);
        if (Settings.Folder.EndsWith("/"))
            throw new InvalidDataException(Localization.Resources.SFTPFolder_Field_Validation_InvalidPostfix);

        _testingConnection = testingConnection;

        _client = Mount();
        
    }

    private readonly object _uploadLock = new();
    private readonly bool _testingConnection;

    public override IStorageUploadResult Upload(string sourceFile, string relativeFileName)
    {
        lock (_uploadLock) // because we're limited by upload speed by Server and Internet
        {
            if (Exists(relativeFileName))
                Delete(relativeFileName);

            var destinationDirectory = Path.GetDirectoryName(relativeFileName) ?? string.Empty;
            if (destinationDirectory != string.Empty)
                CreateDirectory(destinationDirectory);

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

        var files = GetFiles(relativeFolderName, SearchOption.AllDirectories);
        foreach (var file in files)
            Delete(file);

        _client.DeleteDirectory(remotePath);
    }


    private SftpClient Mount()
    {
        Log.WriteLine(LoggingEvent.Debug, $"Mount");

        var authenticationMethods = new List<AuthenticationMethod>();
        if (!string.IsNullOrWhiteSpace(Settings.Password))
            authenticationMethods.Add(new PasswordAuthenticationMethod(Settings.User, Settings.Password));
        if (!string.IsNullOrWhiteSpace(Settings.KeyFile))
            authenticationMethods.Add(new PrivateKeyAuthenticationMethod(Settings.KeyFile));

        var connectionInfo = new ConnectionInfo(
            Settings.Host,
            Settings.Port == 0 ? DefaultPort : Settings.Port,
            Settings.User,
            authenticationMethods.ToArray());

        var client = new SftpClient(connectionInfo);
        client.HostKeyReceived += Client_HostKeyReceived;
        client.Connect();
        return client;
    }

    private void Client_HostKeyReceived(object? sender, Renci.SshNet.Common.HostKeyEventArgs e)
    {
        if (_testingConnection && string.IsNullOrWhiteSpace(Settings.FingerPrintSHA256))
        {
            Settings.FingerPrintSHA256 = e.FingerPrintSHA256;
        }

        e.CanTrust = e.FingerPrintSHA256.Cmp(Settings.FingerPrintSHA256);
        
        if (!e.CanTrust)
        {
            Log.WriteLine(LoggingEvent.Error, $"Fingerprint from SFTP server is {e.FingerPrintSHA256}. Expected fingerprint is {Settings.FingerPrintSHA256}. Connection will not be accepted (wrong fingerprint or MITM attack?)");
            Log.WriteLine(LoggingEvent.Error, $"If you trust this fingerprint, update connection settings.");
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

    private void CreateDirectory(string relativeFileName)
    {
        var remotePath = GetRemoteNotNullablePath(relativeFileName);
        if (!_client.Exists(remotePath))
            _client.CreateDirectory(remotePath);
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
        _client.DownloadFile(remotePath, outputStream);
    }

    private string? GetRemotePath(string? relativePath, bool allowNull)
    {
        var normalizedRelativePath = LinuxFileHelper.NormalizeNullablePath(relativePath);
        if (!allowNull && string.IsNullOrWhiteSpace(normalizedRelativePath))
            throw new ArgumentNullException(nameof(relativePath));
        return normalizedRelativePath == null ? Settings.Folder : string.IsNullOrWhiteSpace(Settings.Folder) ? normalizedRelativePath : FileHelper.Combine('/', Settings.Folder, normalizedRelativePath);
    }

    private string GetRemoteNotNullablePath(string relativePath)
    {
        var normalizedRelativePath = LinuxFileHelper.NormalizeNullablePath(relativePath);
        if (string.IsNullOrWhiteSpace(normalizedRelativePath))
            throw new ArgumentNullException(nameof(relativePath));
        return FileHelper.Combine('/', Settings.Folder, normalizedRelativePath);
    }

    private void GetFilesInternal(string directory, List<string> files, System.IO.SearchOption option)
    {
        foreach (SftpFile sftpFile in _client.ListDirectory(directory))
        {
            if (sftpFile.Name == "..")
                continue;
            if (sftpFile.Name == ".")
                continue;

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
        return files
            .Select(x => x[Settings.Folder.Length..])
            .Select(LinuxFileHelper.NormalizeNotNullablePath)
            .ToArray();
    }

    public override string[] GetFolders(string? relativeFolderName, string? mask = null)
    {
        var directories = new List<string>();
        var remoteFolder = GetRemotePath(relativeFolderName, true)!;

        foreach (SftpFile sftpFile in _client.ListDirectory(remoteFolder))
        {
            if (!sftpFile.IsDirectory)
                continue;
            if (sftpFile.Name == "..")
                continue;
            if (sftpFile.Name == ".")
                continue;

            directories.Add(sftpFile.FullName);
        }
        return directories
            .Select(x => x[Settings.Folder.Length..])
            .Select(LinuxFileHelper.NormalizeNotNullablePath)
            .Where(x => mask == null || LinuxFileHelper.FitsMask(Path.GetFileName(x), mask))
            .ToArray();
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
