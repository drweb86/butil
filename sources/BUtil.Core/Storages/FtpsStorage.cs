using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using FluentFTP;

namespace BUtil.Core.Storages
{
    class FtpsStorage : StorageBase<FtpsStorageSettings>
    {
        internal FtpsStorage(ILog log, FtpsStorageSettings settings)
            : base(log, settings)
        {
            if (string.IsNullOrWhiteSpace(Settings.Host))
                throw new InvalidDataException(BUtil.Core.Localization.Resources.HostIsNotBeSpecified);
            if (Settings.Port < 0) // 0 - default.
                throw new InvalidDataException(BUtil.Core.Localization.Resources.PortIsInvalid);
            if (string.IsNullOrWhiteSpace(Settings.User))
                throw new InvalidDataException(BUtil.Core.Localization.Resources.UserIsNotSpecified);
            if (string.IsNullOrWhiteSpace(Settings.Password))
                throw new InvalidDataException(BUtil.Core.Localization.Resources.PasswordIsNotSpecified);

            Mount();
        }

        private readonly object _uploadLock = new();
        public override IStorageUploadResult Upload(string sourceFile, string relativeFileName)
        {
            lock (_uploadLock) // because we're limited by upload speed by Server and Internet
            {
                var actualFileName = this.Settings.Folder == null ? relativeFileName : Path.Combine(this.Settings.Folder, relativeFileName);
                var status = client.UploadFile(sourceFile, actualFileName);
                if (status != FtpStatus.Success)
                    throw new Exception();
                
                return new IStorageUploadResult
                {
                    StorageFileName = actualFileName,
                    StorageFileNameSize = new FileInfo(sourceFile).Length,
                };
            }
        }

        public override void DeleteFolder(string relativeFolderName)
        {
            var actualFileName = this.Settings.Folder == null ? relativeFolderName : Path.Combine(this.Settings.Folder, relativeFolderName);
            client.DeleteDirectory(actualFileName);
        }

        public override string[] GetFolders(string relativeFolderName, string mask = null)
        {
            FtpListOption listOption = FtpListOption.Auto;
            var actualFolder = relativeFolderName == null ? this.Settings.Folder : Path.Combine(this.Settings.Folder, relativeFolderName);

            return this.client
                .GetListing(actualFolder, listOption)
                .Where(x => x.Type == FtpObjectType.Directory)
                .Select(x => x.FullName)
                .Select(x => actualFolder == null ? x : x.Substring(actualFolder.Length))
                .Select(x => x.Trim(new[] { '\\', '/' }))
                .Where(x => mask == null || FitsMask(Path.GetFileName(x), mask))
                .ToArray();
        }

        private bool FitsMask(string fileName, string fileMask)
        {
            Regex mask = new Regex(fileMask.Replace(".", "[.]").Replace("*", ".*").Replace("?", "."));
            return mask.IsMatch(fileName);
        }

        private FtpClient client;
        private void Mount()
        {
            Log.WriteLine(LoggingEvent.Debug, $"Mount");
            client = new FtpClient(Settings.Host, Settings.User, Settings.Password, Settings.Port);
            client.Config.EncryptionMode = FtpEncryptionMode.Auto;
            client.Config.ValidateAnyCertificate = true;
            client.Config.SslProtocols = System.Security.Authentication.SslProtocols.None;
            client.AutoConnect();
        }

        private void Unmount()
        {
            Log.WriteLine(LoggingEvent.Debug, $"Unmount");
            if (client.IsConnected)
            {
                client.Disconnect();
            }
            client.Dispose();
        }

        public override string Test()
        {
            client.GetListing(Settings.Folder);
            return null;
        }

        public override bool Exists(string relativeFileName)
        {
            var actualFileName = this.Settings.Folder == null ? relativeFileName : Path.Combine(this.Settings.Folder, relativeFileName);
            return client.FileExists(actualFileName);
        }

        public override void Delete(string relativeFileName)
        {
            var actualFileName = this.Settings.Folder == null ? relativeFileName : Path.Combine(this.Settings.Folder, relativeFileName);
            client.DeleteFile(actualFileName);
        }

        public override void Download(string relativeFileName, string targetFileName)
        {
            var actualFileName = this.Settings.Folder == null ? relativeFileName : Path.Combine(this.Settings.Folder, relativeFileName);
            var status = client.DownloadFile(targetFileName, actualFileName);
            if (status != FtpStatus.Success)
            {
                throw new Exception();
            }
        }

        public override string[] GetFiles(string relativeFolderName = null, SearchOption option = SearchOption.TopDirectoryOnly)
        {
            FtpListOption listOption = FtpListOption.Auto;
            if (option == SearchOption.AllDirectories)
                listOption |= FtpListOption.Recursive;

            var actualFolder = relativeFolderName == null ? this.Settings.Folder : Path.Combine(this.Settings.Folder, relativeFolderName);

            return this.client
                .GetListing(actualFolder, listOption)
                .Where(x => x.Type == FtpObjectType.File)
                .Select(x => x.FullName)
                .Select(x => actualFolder == null ? x : x.Substring(actualFolder.Length))
                .Select(x => x.Trim(new[] { '\\', '/' }))
                .ToArray();
        }

        public override DateTime GetModifiedTime(string relativeFileName)
        {
            var actualFileName = this.Settings.Folder == null ? relativeFileName : Path.Combine (this.Settings.Folder, relativeFileName);
            return client.GetModifiedTime(actualFileName);
        }

        public override void Dispose()
        {
            Unmount();
        }
    }
}
