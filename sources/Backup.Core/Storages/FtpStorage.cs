using System;
using System.Globalization;

using BUtil.Core.Logs;
using BUtil.Core.FileSystem;
using System.IO;
using System.Collections.Generic;

namespace BUtil.Core.Storages
{

	class FtpStorage: StorageBase
	{
		#region Locals

		const string _DeletingAllBUtilImageFilesFromTargetFolder = "Deleting all BUtil image files from target folder '{0}'";
		const string _CannotDeleteFileDueToFormatString = "Cannot delete file '{0}' due to: {1}";
		const string _CopyingFormatString = "Copying '{0}' to '{1}'";

		#endregion

		private readonly FtpStorageSettings _settings;

		FtpConnection _connection;

		internal FtpStorage(FtpStorageSettings settings)
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
            log.ProcedureCall("Open");
			Log = log;

            _connection = new FtpConnection();
			_connection.SetLogOnInformation(_settings.User, _settings.Password);
			_connection.ServerLocation = _settings.Host;
			_connection.IsPassive = !_settings.ActiveFtpMode;

            if (_settings.DeleteBUtilFilesInDestinationFolderBeforeBackup)
			{
				log.WriteLine(LoggingEvent.Debug, string.Format(CultureInfo.CurrentCulture, _DeletingAllBUtilImageFilesFromTargetFolder, _settings.DestinationFolder));
				
				string [] filesToDelete = null;
			
				try
				{
					filesToDelete = _connection.GetFileList(_settings.DestinationFolder);
				}
				catch (Exception e)
				{
					log.WriteLine(LoggingEvent.Warning, e.Message);
					return;
				}
			
				foreach (string file in filesToDelete)
				{
					if (file.EndsWith(Files.ImageFilesExtension))
					{
						log.WriteLine(LoggingEvent.Debug, "X " + file);
						try
						{
							_connection.DeleteFtp(file);
						}
						catch (Exception e)
						{
							log.WriteLine(LoggingEvent.Warning, string.Format(CultureInfo.CurrentCulture, _CannotDeleteFileDueToFormatString, file, e.Message));
						}
					}
				}
			}
		}
		
		public override void Test()
		{
			_connection = new FtpConnection();
			_connection.SetLogOnInformation(_settings.User, _settings.Password);
			_connection.ServerLocation = _settings.Host;
            _connection.IsPassive = !_settings.ActiveFtpMode;
			_connection.GetFileList(_settings.DestinationFolder);
		}
		
		public override void Put(string fileName, string directory = null)
		{
			_connection = new FtpConnection();
            _connection.SetLogOnInformation(_settings.User, _settings.Password);
			_connection.ServerLocation = _settings.Host;
            _connection.IsPassive = !_settings.ActiveFtpMode;

			Log.WriteLine(LoggingEvent.Debug, String.Format(CultureInfo.CurrentCulture, _CopyingFormatString, fileName, _settings.DestinationFolder));
			
			var folder = string.IsNullOrEmpty(directory) ? _settings.DestinationFolder : Path.Combine(_settings.DestinationFolder, directory);
			_connection.Upload(folder, fileName);
		}
	}
}
