using System;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

using BUtil.Core.Logs;
using BUtil.Core.FileSystem;

namespace BUtil.Core.Storages
{
	public sealed class FtpStorage: StorageBase
	{
		#region Locals

		const string _DeletingAllBUtilImageFilesFromTargetFolder = "Deleting all BUtil image files from target folder '{0}'";
		const string _CannotDeleteFileDueToFormatString = "Cannot delete file '{0}' due to: {1}";
		const string _CopyingFormatString = "Copying '{0}' to '{1}'";

		#endregion

        bool _ftpModeIsActive;
        string _user;
        string _password;
        string _remoteServer;
        string _destinationFolder;
        bool _deleteBUtilFilesInDestinationFolderBeforeBackup;

		FtpConnection _connection;
		
		public bool FtpModeIsActive
		{
            get { return _ftpModeIsActive; }
            set { _ftpModeIsActive = value; }
		}

		public string User
		{
            get { return _user; }
			set 
			{
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("User");

                _user = value;
			}
		}

		public string Password
		{
            get { return _password; }
			set 
			{
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("Password");

                _password = value;
			}
		}
		
		
		public string RemoteHostServer
		{
            get { return _remoteServer; }
			set 
			{
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("RemoteServer");

                _remoteServer = value;
			}
		}
		
		public string DestinationFolder
		{
            get { return _destinationFolder; }
			set 
			{
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("DestinationFolder");

                _destinationFolder = value;
			}
		}
		
		public bool DeleteBUtilFilesInDestinationFolderBeforeBackup
		{
            get { return _deleteBUtilFilesInDestinationFolderBeforeBackup; }
            set { _deleteBUtilFilesInDestinationFolderBeforeBackup = value; }
		}

        public override string Hint
        {
            get { return _remoteServer + DestinationFolder; }
        }

        public FtpStorage(string storageName, string destinationFolder, bool deleteBUtilFilesInDestinationFolderBeforeBackup,
		                 string host, string user, string password, bool activeFtpMode):
			base(storageName, true)
		{
			DestinationFolder = destinationFolder;
			DeleteBUtilFilesInDestinationFolderBeforeBackup = deleteBUtilFilesInDestinationFolderBeforeBackup;
            RemoteHostServer = host;
            User = user;
            Password = password;
            FtpModeIsActive = activeFtpMode;
		}

        /// <summary>
        /// For reflection purpose only!
        /// </summary>
        /// <param name="settings"></param>
        public FtpStorage(Dictionary<string, string> settings):
        	this(settings["Name"], settings["DestinationFolder"], bool.Parse(settings["DeleteBUtilFilesInDestinationFolderBeforeBackup"]), 
        	     settings["RemoteServer"], settings["User"], settings["Password"], bool.Parse(settings["FtpModeIsActive"]))
		{
        	;
		}
        
		public override void Open(LogBase log)
		{
            log.ProcedureCall("Open");
			Log = log;

            _connection = new FtpConnection();
			_connection.SetLogOnInformation(User, Password);
			_connection.ServerLocation = this.RemoteHostServer;
			_connection.IsPassive = !this.FtpModeIsActive;

            if (_deleteBUtilFilesInDestinationFolderBeforeBackup)
			{
				log.WriteLine(LoggingEvent.Debug, string.Format(CultureInfo.CurrentCulture, _DeletingAllBUtilImageFilesFromTargetFolder, DestinationFolder));
				
				string [] filesToDelete = null;
			
				try
				{
					filesToDelete = _connection.GetFileList(DestinationFolder);
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
			_connection.SetLogOnInformation(User, Password);
			_connection.ServerLocation = this.RemoteHostServer;
            _connection.IsPassive = !this.FtpModeIsActive;
			_connection.GetFileList(this.DestinationFolder);
		}
		
		public override void Process(string fileName)
		{
			_connection = new FtpConnection();
            _connection.SetLogOnInformation(User, Password);
			_connection.ServerLocation = this.RemoteHostServer;
            _connection.IsPassive = !this.FtpModeIsActive;

			Log.WriteLine(LoggingEvent.Debug, String.Format(CultureInfo.CurrentCulture, _CopyingFormatString, fileName, DestinationFolder));
			
			_connection.Upload(this.DestinationFolder, fileName);
		}
		
        public override Dictionary<string, string> SaveSettings()
		{
        	Dictionary<string, string> result = new Dictionary<string, string>();
			result.Add("Name", StorageName);
        	result.Add("DestinationFolder", _destinationFolder);
			result.Add("DeleteBUtilFilesInDestinationFolderBeforeBackup", _deleteBUtilFilesInDestinationFolderBeforeBackup.ToString());
			result.Add("RemoteServer", _remoteServer);
			result.Add("FtpModeIsActive", _ftpModeIsActive.ToString());
			result.Add("User", _user);
			result.Add("Password", _password);
			return result;
		}
	}
}
