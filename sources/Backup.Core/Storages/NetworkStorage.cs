using System;
using System.Globalization;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using System.Xml;



using BUtil.Core.Logs;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;

namespace BUtil.Core.Storages
{

    class SambaStorage: StorageBase
	{
        const string _deletingAllBUtilImageFilesFromTargetFolder = "Deleting all BUtil image files from target folder '{0}'";
        const string _NOT_A_NETWORK_DIR = "Not a network location";
        const string _CANNOTDELETEDUETOACESSVIOLATIONS = "Cannot delete file '{0}' due to access violation {1}";
        const string _COPYING = "Copying '{0}' to '{1}'";
        const string _EncryptingFormatString = "Encrypting '{0}'";

        string _destinationFolder;
        bool _deleteBUtilFilesInDestinationFolderBeforeBackup;
        bool _encryptUnderLocalSystemAccount;
        bool _skipCopyingToNetworkStorageAndWriteWarningInLogIfBackupExceeds;
        long _skipCopyingToNetworkStorageLimitMb;

        long SkipCopyingToNetworkStorageLimit
        {
            get { return _skipCopyingToNetworkStorageLimitMb * 1024 * 1024; }
        }
        
        public bool SkipCopyingToNetworkStorageAndWriteWarningInLogIfBackupExceeds
        {
            get { return _skipCopyingToNetworkStorageAndWriteWarningInLogIfBackupExceeds; }
            set { _skipCopyingToNetworkStorageAndWriteWarningInLogIfBackupExceeds = value; }
        }

        public long SkipCopyingToNetworkStorageLimitMb
        {
            get { return _skipCopyingToNetworkStorageLimitMb; }
            set { _skipCopyingToNetworkStorageLimitMb = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentException">not a network dir</exception>
		public string DestinationFolder
		{
            get { return _destinationFolder; }
			set 
			{
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("DestinationFolder");

				if (!value.StartsWith(@"\\", StringComparison.InvariantCulture))
					throw new ArgumentException(_NOT_A_NETWORK_DIR);

                _destinationFolder = value;
			}
		}
		
		public bool DeleteBUtilFilesInDestinationFolderBeforeBackup
		{
            get { return _deleteBUtilFilesInDestinationFolderBeforeBackup; }
            set { _deleteBUtilFilesInDestinationFolderBeforeBackup = value; }
		}

        public bool EncryptUnderLocalSystemAccount
        {
            get { return _encryptUnderLocalSystemAccount; }
            set { _encryptUnderLocalSystemAccount = value; }
        }

        public SambaStorage(SambaStorageSettings settings)
            : this(settings.Name, settings.DestinationFolder, settings.DeleteBUtilFilesInDestinationFolderBeforeBackup,
                  settings.EncryptUnderLocalSystemAccount, settings.SkipCopyingToNetworkStorageAndWriteWarningInLogIfBackupExceeds,
                  settings.SkipCopyingToNetworkStorageLimitMb)
        {

        }

        public SambaStorage(string storageName, string destinationFolder, bool deleteBUtilFilesInDestinationFolderBeforeBackup, bool encryptUnderLocalSystemAccount, bool skipCopyingToNetworkStorageAndWriteWarningInLogIfBackupExceeds, long skipCopyingToNetworkStorageLimitMb) :
            base(storageName, true)
		{
			DestinationFolder = destinationFolder;
			DeleteBUtilFilesInDestinationFolderBeforeBackup = deleteBUtilFilesInDestinationFolderBeforeBackup;
            EncryptUnderLocalSystemAccount = encryptUnderLocalSystemAccount;
            _skipCopyingToNetworkStorageAndWriteWarningInLogIfBackupExceeds = skipCopyingToNetworkStorageAndWriteWarningInLogIfBackupExceeds;
            _skipCopyingToNetworkStorageLimitMb = skipCopyingToNetworkStorageLimitMb;
		}
        
        public SambaStorage(Dictionary<string, string> settings):
        	this(settings["Name"], settings["DestinationFolder"], 
        	     bool.Parse(settings["DeleteBUtilFilesInDestinationFolderBeforeBackup"]),
        	     bool.Parse(settings["EncryptUnderLocalSystemAccount"]),
        	     bool.Parse(settings["SkipCopyingToNetworkStorageAndWriteWarningInLogIfBackupExceeds"]),
        	     long.Parse(settings["SkipCopyingToNetworkStorageLimitMb"]))
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
					string.Format(CultureInfo.CurrentCulture, _deletingAllBUtilImageFilesFromTargetFolder, DestinationFolder));
				
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

			if (SkipCopyingToNetworkStorageAndWriteWarningInLogIfBackupExceeds)
            {
                FileInfo info = new FileInfo(file);
                long size = info.Length;

                if (size > SkipCopyingToNetworkStorageLimit)
                {
                    Log.WriteLine(LoggingEvent.Warning,
                        string.Format(CultureInfo.CurrentCulture, Resources.CopyingOfBackupTo0NetworkStorageSkippedBecauseItsSize1ExceededSpecifiedLimit2, StorageName, size, SkipCopyingToNetworkStorageLimit));

                    return;
                }
            }

            string dest = DestinationFolder + file.Substring(file.LastIndexOf('\\'));
			Log.WriteLine(LoggingEvent.Debug, String.Format(CultureInfo.CurrentCulture, _COPYING, file, dest));
            File.Copy(file, dest, true);
            if (EncryptUnderLocalSystemAccount)
            {
                Log.WriteLine(LoggingEvent.Debug, String.Format(CultureInfo.CurrentCulture, _EncryptingFormatString, dest));
                File.Encrypt(dest);
            }
		}

        public override void Test()
        {

        }
        
        public override Dictionary<string, string> SaveSettings()
		{
        	Dictionary<string, string> result = new Dictionary<string, string>();
			result.Add("Name", StorageName);
        	result.Add("DestinationFolder", _destinationFolder);
			result.Add("DeleteBUtilFilesInDestinationFolderBeforeBackup", _deleteBUtilFilesInDestinationFolderBeforeBackup.ToString());
			result.Add("EncryptUnderLocalSystemAccount", _encryptUnderLocalSystemAccount.ToString());
			result.Add("SkipCopyingToNetworkStorageAndWriteWarningInLogIfBackupExceeds", _skipCopyingToNetworkStorageAndWriteWarningInLogIfBackupExceeds.ToString());
			result.Add("SkipCopyingToNetworkStorageLimitMb", _skipCopyingToNetworkStorageLimitMb.ToString());
			
			return result;
		}
        
		public void LoadSettings(Dictionary<string, string> dictionary)
		{

		}
	}
}
