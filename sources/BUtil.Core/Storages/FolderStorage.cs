using System;
using System.IO;
using System.Linq;
using System.Security;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using BUtil.Core.Misc;

namespace BUtil.Core.Storages
{
    class FolderStorage : StorageBase<FolderStorageSettingsV2>
    {
        internal FolderStorage(ILog log, FolderStorageSettingsV2 settings)
            : base(log, settings)
        {
            Mount();
        }

        private readonly object _uploadLock = new();
        public override IStorageUploadResult Upload(string sourceFile, string relativeFileName)
        {
            lock (_uploadLock) // because we're limited by upload speed and Samba has limit of 6 parallel uploads usually
            {
                var destinationFile = Path.Combine(Settings.DestinationFolder, relativeFileName);
                var destinationDirectory = Path.GetDirectoryName(destinationFile);

                Log.WriteLine(LoggingEvent.Debug, $"Copying \"{sourceFile}\" to \"{destinationFile}\"");

                if (!Directory.Exists(destinationDirectory))
                    Directory.CreateDirectory(destinationDirectory);

                Copy(sourceFile, destinationFile);

                return new IStorageUploadResult
                {
                    StorageFileName = destinationFile,
                    StorageFileNameSize = new FileInfo(destinationFile).Length,
                };
            }
        }

        private void Mount()
        {
            Log.WriteLine(LoggingEvent.Debug, $"Mount");
            if (!string.IsNullOrWhiteSpace(this.Settings.MountPowershellScript))
            {
                if (!PowershellProcessHelper.Execute(Log, this.Settings.MountPowershellScript))
                    throw new InvalidOperationException($"Cannot mount");
            }
        }

        private void Unmount()
        {
            Log.WriteLine(LoggingEvent.Debug, $"Unmount");
            if (!string.IsNullOrWhiteSpace(this.Settings.UnmountPowershellScript))
            {
                if (!PowershellProcessHelper.Execute(Log, this.Settings.UnmountPowershellScript))
                    throw new InvalidOperationException($"Cannot unmount");
            }
        }

        public override string Test()
        {
            if (string.IsNullOrWhiteSpace(Settings.DestinationFolder))
                return BUtil.Core.Localization.Resources.DestinationFolderIsNotSpecified;

            if (!Directory.Exists(Settings.DestinationFolder))
                return string.Format(BUtil.Core.Localization.Resources.FolderStorageFailure, Settings.DestinationFolder);

            return null;
        }

        public override bool Exists(string relativeFileName)
        {
            var fullPathName = Path.Combine(Settings.DestinationFolder, relativeFileName);

            return File.Exists(fullPathName);
        }

        public override void Delete(string relativeFileName)
        {
            var fullPathName = Path.Combine(Settings.DestinationFolder, relativeFileName);

            if (File.Exists(fullPathName))
                File.Delete(fullPathName);
        }

        public override void DeleteFolder(string relativeFolderName)
        {
            var fullPathName = string.IsNullOrWhiteSpace(relativeFolderName)
                ? Settings.DestinationFolder
                : Path.Combine(Settings.DestinationFolder, relativeFolderName);

            if (Directory.Exists(fullPathName))
                Directory.Delete(fullPathName, true);
        }

        public override string[] GetFolders(string relativeFolderName, string mask = null)
        {
            var fullPathName = string.IsNullOrWhiteSpace(relativeFolderName)
                ? Settings.DestinationFolder
                : Path.Combine(Settings.DestinationFolder, relativeFolderName);

            return Directory
                .GetDirectories(fullPathName, mask)
                .Select(x => x.Substring(fullPathName.Length))
                .Select(x => x.Trim(new[] { '\\', '/' }))
                .ToArray();
        }

        public override void Download(string relativeFileName, string targetFileName)
        {
            var file = Path.Combine(Settings.DestinationFolder, relativeFileName);
            Copy(file, targetFileName);
        }

        public static void Copy(string inputFile, string outputFilePath)
        {
            int bufferSize = 16 * 1024 * 1024;

            using var inputFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, bufferSize);
            using var outputFileStream = new FileStream(outputFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, bufferSize);
            outputFileStream.SetLength(inputFileStream.Length);
            int bytesRead = -1;
            byte[] bytes = new byte[bufferSize];

            while ((bytesRead = inputFileStream.Read(bytes, 0, bufferSize)) > 0)
            {
                outputFileStream.Write(bytes, 0, bytesRead);
            }
        }

        public override string[] GetFiles(string relativeFolderName = null, SearchOption option = SearchOption.TopDirectoryOnly)
        {
            if (relativeFolderName != null)
            {
                if (relativeFolderName.Contains(".."))
                    throw new SecurityException(nameof(relativeFolderName));
            }

            var actualFolder = relativeFolderName == null ?
                Settings.DestinationFolder :
                Path.Combine(Settings.DestinationFolder, relativeFolderName);

            return Directory
                .GetFiles(actualFolder, "*.*", option)
                .Select(x => x.Substring(actualFolder.Length))
                .Select(x => x.Trim(new[] { '\\', '/' }))
                .ToArray();
        }

        public override DateTime GetModifiedTime(string relativeFileName)
        {
            var actualFile = Path.Combine(Settings.DestinationFolder, relativeFileName);

            return File.GetLastWriteTime(actualFile);
        }

        public override void Dispose()
        {
            Unmount();
        }
    }
}
