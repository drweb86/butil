using System;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using BUtil.Core.Logs;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;

namespace BUtil.Core.Storages
{

    class SambaStorage: StorageBase
	{
        const string _deletingAllBUtilImageFilesFromTargetFolder = "Deleting all BUtil image files from target folder '{0}'";
        const string _COPYING = "Copying '{0}' to '{1}'";
        const string _EncryptingFormatString = "Encrypting '{0}'";

        private readonly SambaStorageSettings _settings;

        long SkipCopyingToNetworkStorageLimit
        {
            get { return _settings.SkipCopyingToNetworkStorageLimitMb * 1024 * 1024; }
        }

        internal SambaStorage(SambaStorageSettings settings)
            : base(settings.Name)
        {
            _settings = settings;
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
					string.Format(CultureInfo.CurrentCulture, _deletingAllBUtilImageFilesFromTargetFolder, _settings.DestinationFolder));
				
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
								string.Format(CultureInfo.CurrentCulture, "Cannot delete file '{0}' due to access violation {1}", filesToDelete[i], e.Message));
						}
					}
				}
			}
		}

        public override void StoreFiles(string sourceDir, List<string> sourceFiles, string directory = null)
        {
            throw new NotImplementedException();
        }
        public override byte[] ReadFile(string file)
        {
            throw new NotImplementedException();
        }

        public override void Put(string file, string directory = null)
		{
			if (string.IsNullOrEmpty(file))
				throw new ArgumentNullException("file");

			if (_settings.SkipCopyingToNetworkStorageAndWriteWarningInLogIfBackupExceeds)
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

            var folder = string.IsNullOrEmpty(directory) ? _settings.DestinationFolder : Path.Combine(_settings.DestinationFolder, directory);
            string dest = folder + file.Substring(file.LastIndexOf('\\'));
			Log.WriteLine(LoggingEvent.Debug, String.Format(CultureInfo.CurrentCulture, _COPYING, file, dest));
            File.Copy(file, dest, true);
            if (_settings.EncryptUnderLocalSystemAccount)
            {
                Log.WriteLine(LoggingEvent.Debug, String.Format(CultureInfo.CurrentCulture, _EncryptingFormatString, dest));
                File.Encrypt(dest);
            }
		}

        public override void Test()
        {

        }
	}
}
