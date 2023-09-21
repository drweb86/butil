using System;
using System.IO;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using FluentFTP;

namespace BUtil.Core.Storages
{
    class FtpsStorage : StorageBase<FtpsStorageSettingsV2>
    {
        private readonly string _normalizedFolder;
        private FtpClient _client;

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

            _normalizedFolder = NormalizePath(Settings.Folder);

            Mount();
        }

        private readonly object _uploadLock = new();
        public override IStorageUploadResult Upload(string sourceFile, string relativeFileName)
        {
            lock (_uploadLock) // because we're limited by upload speed by Server and Internet
            {
                var remotePath = GetRemotePath(relativeFileName, false);
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
            var remotePath = GetRemotePath(relativeFolderName, false);
            _client.DeleteDirectory(remotePath);
        }

        public override string[] GetFolders(string relativeFolderName, string mask = null)
        {
            FtpListOption listOption = FtpListOption.Auto;
            var remotePath = GetRemotePath(relativeFolderName, true);

            return this._client
                .GetListing(remotePath, listOption)
                .Where(x => x.Type == FtpObjectType.Directory)
                .Select(x => x.FullName)
                .Select(NormalizePath)
                .Select(x => remotePath == null ? x : x.Substring(remotePath.Length))
                .Select(NormalizePath)
                .Where(x => mask == null || FitsMask(Path.GetFileName(x), mask))
                .ToArray();
        }

        private static bool FitsMask(string fileName, string fileMask)
        {
            Regex mask = new Regex(fileMask.Replace(".", "[.]").Replace("*", ".*").Replace("?", "."));
            return mask.IsMatch(fileName);
        }

        private void Mount()
        {
            Log.WriteLine(LoggingEvent.Debug, $"Mount");
            _client = new FtpClient(Settings.Host, Settings.User, Settings.Password, Settings.Port);
            _client.Config.EncryptionMode = FtpEncryptionMode.Auto;
            _client.Config.ValidateAnyCertificate = true;
            _client.Config.SslProtocols = System.Security.Authentication.SslProtocols.None;
            _client.AutoConnect();
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

        public override string Test()
        {
            if (string.IsNullOrWhiteSpace(Settings.Folder) && !_client.DirectoryExists(Settings.Folder))
            {
                return BUtil.Core.Localization.Resources.Field_Folder_Validation_NotExist;
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
            var remotePath = this.GetRemotePath(relativeFileName, false);
            return _client.FileExists(remotePath);
        }

        public override void Delete(string relativeFileName)
        {
            var remotePath = this.GetRemotePath(relativeFileName, false);
            _client.DeleteFile(remotePath);
        }

        public override void Download(string relativeFileName, string targetFileName)
        {
            var remotePath = this.GetRemotePath(relativeFileName, false);
            var status = _client.DownloadFile(targetFileName, remotePath);
            if (status != FtpStatus.Success)
            {
                throw new Exception();
            }
        }

        private static string NormalizePath(string path)
        {
            if (path != null && path.Contains(".."))
                throw new SecurityException("[..] is not allowed in path.");

            return path?.Trim(new[] { '\\', '/' });
        }

        private string GetRemotePath(string relativePath, bool allowNull)
        {
            var normalizedRelativePath = NormalizePath(relativePath);
            if (!allowNull && string.IsNullOrWhiteSpace(normalizedRelativePath))
            {
                throw new ArgumentNullException(nameof(relativePath));
            }
            return normalizedRelativePath == null ? this._normalizedFolder : Path.Combine(this._normalizedFolder, normalizedRelativePath);
        }

        public override string[] GetFiles(string relativeFolderName = null, SearchOption option = SearchOption.TopDirectoryOnly)
        {
            FtpListOption listOption = FtpListOption.Auto;
            if (option == SearchOption.AllDirectories)
                listOption |= FtpListOption.Recursive;

            var remoteFolder = GetRemotePath(relativeFolderName, true);
            return this._client
                .GetListing(remoteFolder, listOption)
                .Where(x => x.Type == FtpObjectType.File)
                .Select(x => x.FullName)
                .Select(NormalizePath)
                .Select(x => remoteFolder == null ? x : x.Substring(remoteFolder.Length))
                .Select(NormalizePath)
                .ToArray();
        }

        public override DateTime GetModifiedTime(string relativeFileName)
        {
            var remotePath = GetRemotePath(relativeFileName, false);
            return _client.GetModifiedTime(remotePath);
        }

        public override void Dispose()
        {
            Unmount();
        }
    }
}
