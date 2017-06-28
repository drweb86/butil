using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

using BUtil.Core.Logs;
using BUtil.Core.FileSystem;

namespace BUtil.Core.Storages
{
	public sealed class HddStorage: StorageBase
	{
        const string _DeletingBUtilImagesInTargetFolderBeforeBackup = "Deleting BUtil image files from target folder {0}";
        const string _CANNOTDELETEDUETOACESSVIOLATIONS = "Cannot delete file '{0}' due to access violation {1}";
        const string _COPYING = "Copying '{0}' to '{1}'";
        const string _NETWORK_STORAGES_ARE_NOT_ALLOWED_HERE = "Network storages are not allowed to be the destination folder";

        string _destinationFolder;
        bool _deleteBUtilFilesInDestinationFolderBeforeBackup;

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidDataException">tried to add network storage</exception>
		public string DestinationFolder
		{
            get { return _destinationFolder; }
			set 
			{
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("DestinationFolder");

				if (value.StartsWith(@"\\", StringComparison.Ordinal))
					throw new InvalidDataException(_NETWORK_STORAGES_ARE_NOT_ALLOWED_HERE);

                _destinationFolder = value;
			}
		}

        public override string Hint
        {
            get { return _destinationFolder; }
        }

		public bool DeleteBUtilFilesInDestinationFolderBeforeBackup
		{
            get { return _deleteBUtilFilesInDestinationFolderBeforeBackup; }
            set { _deleteBUtilFilesInDestinationFolderBeforeBackup = value; }
		}

        public HddStorage(string storageName, string destinationFolder, bool deleteBUtilFilesInDestinationFolderBeforeBackup):
            base(storageName, false)
		{
			DestinationFolder = destinationFolder;
			DeleteBUtilFilesInDestinationFolderBeforeBackup = deleteBUtilFilesInDestinationFolderBeforeBackup;
		}
		
		/// <summary>
		/// This constructor is for Reflection only!
		/// </summary>
		public HddStorage(Dictionary<string, string> settings):
			this (settings["Name"], settings["DestinationFolder"], bool.Parse(settings["DeleteBUtilFilesInDestinationFolderBeforeBackup"]))
		{
			;
		}
		
		public override void Open(LogBase log)
		{
			if (log == null)
				throw new ArgumentNullException("log");

			log.ProcedureCall("Open");
			Log = log;
			
			if (DeleteBUtilFilesInDestinationFolderBeforeBackup)
			{
                log.WriteLine(LoggingEvent.Debug,
					string.Format(CultureInfo.CurrentCulture, _DeletingBUtilImagesInTargetFolderBeforeBackup, DestinationFolder));
				
				string [] filesToDelete = null;
			
				try
				{
					filesToDelete = Directory.GetFiles(DestinationFolder, "*"+Files.ImageFilesExtension);
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
		
		public override void Process(string file)
		{
			if (string.IsNullOrEmpty(file))
				throw new ArgumentNullException("file");

            string dest = DestinationFolder + file.Substring(file.LastIndexOf('\\'));
			Log.WriteLine(LoggingEvent.Debug, String.Format(CultureInfo.CurrentCulture, _COPYING, file, dest));
            File.Copy(file, dest, true);
		}

        public override void Test()
        {
            ;
        }
        
		public override Dictionary<string, string> SaveSettings()
		{
			Dictionary<string, string> result = new Dictionary<string, string>();
			result.Add("Name", StorageName);
			result.Add("DestinationFolder", _destinationFolder);
			result.Add("DeleteBUtilFilesInDestinationFolderBeforeBackup", _deleteBUtilFilesInDestinationFolderBeforeBackup.ToString());
			
			return result;
		}
	}
}
