using System;
using System.IO;
using BUtil.Core.Logs;
using BUtil.Core.Misc;

namespace BUtil.Core.Storages
{
    class SambaStorage : StorageBase<SambaStorageSettings>
    {
        internal SambaStorage(ILog log, SambaStorageSettings settings)
            : base(log, settings)
        {
            Mount();
        }

        private readonly object _uploadLock = new();
        public override IStorageUploadResult Upload(string sourceFile, string relativeFileName)
        {
            lock (_uploadLock) // because we're limited by upload speed and Samba has limit of 6 parallel uploads usually
            {
                var destinationFile = Path.Combine(Settings.Url, relativeFileName);
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
            Log.WriteLine(LoggingEvent.Debug, $"Mount \"{Settings.Name}\"");

            var command = string.IsNullOrWhiteSpace(Settings.User)
                ? @$"net use {Settings.Url}"
                : @$"net use {Settings.Url} /user:{Settings.User} {Settings.Password}";

            if (!CmdProcessHelper.Execute(Log, command))
                throw new InvalidOperationException($"Cannot mount \"{Settings.Name}\"");
        }

        private void Unmount()
        {
            Log.WriteLine(LoggingEvent.Debug, $"Unmount \"{Settings.Name}\"");

            var command = @$"net use {Settings.Url} /delete /y";
            if (!CmdProcessHelper.Execute(Log, command))
                throw new InvalidOperationException($"Cannot unmount \"{Settings.Name}\"");
        }

        public override string Test()
        {
            if (!Directory.Exists(Settings.Url))
                return string.Format(BUtil.Core.Localization.Resources.FolderStorageFailure, Settings.Name, Settings.Url);

            return null;
        }

        public override bool Exists(string relativeFileName)
        {
            var fullPathName = Path.Combine(Settings.Url, relativeFileName);

            return File.Exists(fullPathName);
        }

        public override void Delete(string relativeFileName)
        {
            var fullPathName = Path.Combine(Settings.Url, relativeFileName);

            if (File.Exists(fullPathName))
                File.Delete(fullPathName);
        }

        public override void Download(string relativeFileName, string targetFileName)
        {
            var file = Path.Combine(Settings.Url, relativeFileName);
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
