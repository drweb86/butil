using System;
using System.IO;
using System.Linq;
using System.Security;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using MediaDevices;

namespace BUtil.Core.Storages
{
    class MtpStorage : StorageBase<MtpStorageSettings>
    {
        private MediaDevice _mediaDevice;

        internal MtpStorage(ILog log, MtpStorageSettings settings)
            : base(log, settings)
        {
            if (string.IsNullOrWhiteSpace(Settings.Device))
                throw new InvalidDataException(BUtil.Core.Localization.Resources.Field_Device_Validation_Empty);
            if (string.IsNullOrWhiteSpace(Settings.Folder))
                throw new InvalidDataException(BUtil.Core.Localization.Resources.Field_Folder_Validation_Empty);

            Mount();
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

        public override string[] GetFolders(string relativeFolderName, string mask = null)
        {
            var remotePath = GetRemotePath(relativeFolderName, true);

            return this._mediaDevice
                .GetDirectories(remotePath, mask)
                .Select(NormalizePath)
                .Select(x => remotePath == null ? x : x.Substring(remotePath.Length))
                .Select(NormalizePath)
                .ToArray();
        }

        private void Mount()
        {
            Log.WriteLine(LoggingEvent.Debug, $"Mount");
            _mediaDevice = MediaDevice
                .GetDevices()
                .FirstOrDefault(x => x.FriendlyName == Settings.Device);

            if (_mediaDevice == null)
                throw new ArgumentException(Resources.Field_Device_Validation_NotFound);

            _mediaDevice.Connect();
        }

        private void Unmount()
        {
            Log.WriteLine(LoggingEvent.Debug, $"Unmount");
            _mediaDevice?.Disconnect();
            _mediaDevice?.Dispose();
        }

        public override string Test()
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
            return normalizedRelativePath == null ? this.Settings.Folder : Path.Combine(this.Settings.Folder, normalizedRelativePath);
        }

        public override string[] GetFiles(string relativeFolderName = null, SearchOption option = SearchOption.TopDirectoryOnly)
        {
            var remoteFolder = GetRemotePath(relativeFolderName, true);
            return this._mediaDevice
                .GetFiles(remoteFolder, "*.*", option)
                .Select(NormalizePath)
                .Select(x => remoteFolder == null ? x : x.Substring(remoteFolder.Length))
                .Select(NormalizePath)
                .ToArray();
        }

        public override DateTime GetModifiedTime(string relativeFileName)
        {
            var remotePath = GetRemotePath(relativeFileName, false);
            var fileInfo = _mediaDevice.GetFileInfo(remotePath);
            return MaxDate(fileInfo.CreationTime, fileInfo.LastWriteTime);
        }

        private DateTime MaxDate(params DateTime?[] dates)
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
    }
}
