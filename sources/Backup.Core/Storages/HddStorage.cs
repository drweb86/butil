using System;
using System.Globalization;
using System.IO;
using BUtil.Core.Logs;

namespace BUtil.Core.Storages
{
	class HddStorage: StorageBase
	{
        const string _COPYING = "Copying '{0}' to '{1}'";

		private readonly HddStorageSettings _settings;


		internal HddStorage(HddStorageSettings settings)
		{
			_settings = settings;
		}

        public override string ReadAllText(string file)
        {
			var fullPathName = Path.Combine(_settings.DestinationFolder, file);

			if (!File.Exists(fullPathName))
				return null;

            return File.ReadAllText(fullPathName);
        }

        public override void Open(LogBase log)
		{
			Log = log;
		}

        public override string Upload(string sourceFile, string relativeFileName)
		{
			var destinationFile = Path.Combine(_settings.DestinationFolder, relativeFileName);
			var destinationDirectory = Path.GetDirectoryName(destinationFile);

            Log.WriteLine(LoggingEvent.Debug, String.Format(CultureInfo.CurrentCulture, _COPYING, sourceFile, destinationFile));

            if (!Directory.Exists(destinationDirectory))
				Directory.CreateDirectory(destinationDirectory);

            System.IO.File.Copy(sourceFile, destinationFile, true);

			return destinationDirectory;
        }


        public override void Test()
        {
            ;
        }
        
	}
}
