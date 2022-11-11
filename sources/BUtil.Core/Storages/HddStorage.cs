using System;
using System.Globalization;
using System.IO;
using BUtil.Core.Logs;

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

        public override void Test()
        {
            ;
        }
        
	}
}
