using System;
using System.Globalization;
using System.IO;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.State;

namespace BUtil.Core.Storages
{
	class FolderStorage: StorageBase<FolderStorageSettings>
	{
		internal FolderStorage(ILog log, FolderStorageSettings settings)
            :base (log, settings)
		{
		}

        public override string ReadAllText(string file)
        {
			var fullPathName = Path.Combine(Settings.DestinationFolder, file);

			if (!File.Exists(fullPathName))
				return null;

            return File.ReadAllText(fullPathName);
        }

        public override byte[] ReadAllBytes(string file)
        {
            var fullPathName = Path.Combine(Settings.DestinationFolder, file);

            if (!File.Exists(fullPathName))
                return null;

            return File.ReadAllBytes(fullPathName);
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

        public override string Test()
        {
            if (!Directory.Exists(Settings.DestinationFolder))
                return string.Format(BUtil.Core.Localization.Resources.FolderStorageFailure, Settings.Name, Settings.DestinationFolder);

            return null;
        }

        public override void Delete(string file)
        {
            var fullPathName = Path.Combine(Settings.DestinationFolder, file);

            if (File.Exists(fullPathName))
                File.Delete(fullPathName);
        }

        public override void Download(StorageFile storageFile, string targetFileName)
        {
            var file = Path.Combine(Settings.DestinationFolder, storageFile.StorageRelativeFileName);
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
    }
}
