using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

using BUtil.Core.Logs;
using BUtil.Core.FileSystem;
using System.Reflection;

namespace BUtil.Core.Storages
{
	class HddStorage: StorageBase
	{
        const string _DeletingBUtilImagesInTargetFolderBeforeBackup = "Deleting BUtil image files from target folder {0}";
        const string _CANNOTDELETEDUETOACESSVIOLATIONS = "Cannot delete file '{0}' due to access violation {1}";
        const string _COPYING = "Copying '{0}' to '{1}'";
        const string _NETWORK_STORAGES_ARE_NOT_ALLOWED_HERE = "Network storages are not allowed to be the destination folder";

		private readonly HddStorageSettings _settings;


		internal HddStorage(HddStorageSettings settings)
		{
			_settings = settings;
		}

        public override void StoreFiles(string sourceDir, List<string> sourceFiles, string directory = null)
        {
            throw new NotImplementedException();
        }
        public override byte[] ReadFile(string file)
        {
            throw new NotImplementedException();
        }

        public override void Open(LogBase log)
		{
			if (log == null)
				throw new ArgumentNullException("log");

			log.ProcedureCall("Open");
			Log = log;
			
			if (_settings.DeleteBUtilFilesInDestinationFolderBeforeBackup)
			{
                log.WriteLine(LoggingEvent.Debug,
					string.Format(CultureInfo.CurrentCulture, _DeletingBUtilImagesInTargetFolderBeforeBackup, _settings.DestinationFolder));
				
				string [] filesToDelete = null;
			
				try
				{
					filesToDelete = Directory.GetFiles(_settings.DestinationFolder, "*"+Files.ImageFilesExtension);
				}
				catch (DirectoryNotFoundException e)
				{
					log.WriteLine(LoggingEvent.Warning, e.Message);
					return;
				}
			
				if (filesToDelete.Length > 0)
				{
					for (int i = 0; i < filesToDelete.Length; i++)
					{
						log.WriteLine(LoggingEvent.Debug, "X " + filesToDelete[i]);
						try
						{
							File.Delete(filesToDelete[i]);
						}
						catch (UnauthorizedAccessException e)
						{
							log.WriteLine(LoggingEvent.Warning,
								string.Format(CultureInfo.CurrentCulture, _CANNOTDELETEDUETOACESSVIOLATIONS, filesToDelete[i], e.Message));
						}
					}
				}
			}
		}
		
		public override void Put(string file, string directory = null)
		{
			if (string.IsNullOrEmpty(file))
				throw new ArgumentNullException("file");

            var folder = string.IsNullOrEmpty(directory) ? _settings.DestinationFolder : Path.Combine(_settings.DestinationFolder, directory);
            string dest = folder + file.Substring(file.LastIndexOf('\\'));
            
            Log.WriteLine(LoggingEvent.Debug, String.Format(CultureInfo.CurrentCulture, _COPYING, file, dest));
            
			File.Copy(file, dest, true);
		}

        public override void Test()
        {
            ;
        }
        
	}
}
