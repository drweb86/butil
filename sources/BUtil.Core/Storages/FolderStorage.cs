using System;
using System.IO;
using BUtil.Core.Logs;
using BUtil.Core.Misc;

namespace BUtil.Core.Storages
{
    class FolderStorage : StorageBase<FolderStorageSettings>
    {
        internal FolderStorage(ILog log, FolderStorageSettings settings)
            : base(log, settings)
        {
            Mount();
        }

        public override IStorageUploadResult Upload(string sourceFile, string relativeFileName)
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

        private void Mount()
        {
            Log.WriteLine(LoggingEvent.Debug, $"Mount \"{Settings.Name}\"");
            if (!string.IsNullOrWhiteSpace(this.Settings.MountPowershellScript))
            {
                if (!PowershellProcessHelper.Execute(Log, this.Settings.MountPowershellScript))
                    throw new InvalidOperationException($"Cannot mount \"{Settings.Name}\"");
            }
        }

        private void Unmount()
        {
            Log.WriteLine(LoggingEvent.Debug, $"Unmount \"{Settings.Name}\"");
            if (!string.IsNullOrWhiteSpace(this.Settings.UnmountPowershellScript))
            {
                if (!PowershellProcessHelper.Execute(Log, this.Settings.UnmountPowershellScript))
                    throw new InvalidOperationException($"Cannot unmount \"{Settings.Name}\"");
            }
        }

        public override string Test()
        {
            if (!Directory.Exists(Settings.DestinationFolder))
                return string.Format(BUtil.Core.Localization.Resources.FolderStorageFailure, Settings.Name, Settings.DestinationFolder);

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

        public override void Dispose()
        {
            Unmount();
        }
    }
}
