using System;
using System.Globalization;
using System.IO;
using BUtil.Core.Logs;
using BUtil.Core.State;

namespace BUtil.Core.Storages
{
	class HddStorage: StorageBase<HddStorageSettings>
	{
        const string _COPYING = "Copying '{0}' to '{1}'";

		internal HddStorage(ILog log, HddStorageSettings settings)
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

            if (!File.Exists(file))
                return null;

            return File.ReadAllBytes(fullPathName);
        }

        public override IStorageUploadResult Upload(string sourceFile, string relativeFileName)
		{
			var destinationFile = Path.Combine(Settings.DestinationFolder, relativeFileName);
			var destinationDirectory = Path.GetDirectoryName(destinationFile);

            Log.WriteLine(LoggingEvent.Debug, String.Format(CultureInfo.CurrentUICulture, _COPYING, sourceFile, destinationFile));

            if (!Directory.Exists(destinationDirectory))
				Directory.CreateDirectory(destinationDirectory);

            System.IO.File.Copy(sourceFile, destinationFile, true);

            return new IStorageUploadResult
            {
                StorageFileName = destinationFile,
                StorageFileNameSize = new FileInfo(destinationFile).Length,
            };
        }

        public override string Test()
        {
            if (!Directory.Exists(Settings.DestinationFolder))
                return string.Format(BUtil.Core.Localization.Resources.HddStorageFailure, Settings.Name, Settings.DestinationFolder);

            return null;
        }

        public override void Delete(string file)
        {
            var fullPathName = Path.Combine(Settings.DestinationFolder, file);

            if (File.Exists(file))
                File.Delete(file);
        }

        public override void Download(StorageFile storageFile, string targetFileName)
        {
            var file = Path.Combine(Settings.DestinationFolder, storageFile.StorageRelativeFileName);
            File.Copy(file, targetFileName, true);
        }

    }
}
