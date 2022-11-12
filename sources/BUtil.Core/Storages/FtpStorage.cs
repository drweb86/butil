using System;
using BUtil.Core.Logs;

namespace BUtil.Core.Storages
{

	class FtpStorage: StorageBase<FtpStorageSettings>
	{
		// const string _CopyingFormatString = "Copying '{0}' to '{1}'";

		FtpConnection _connection;

		internal FtpStorage(ILog log, FtpStorageSettings settings)
			:base(log, settings)
		{
		}
        public override string ReadAllText(string file)
        {
            throw new NotImplementedException();
        }

		public override string Test()
		{
			_connection = new FtpConnection();
			_connection.SetLogOnInformation(Settings.User, Settings.Password);
			_connection.ServerLocation = Settings.Host;
            _connection.IsPassive = !Settings.ActiveFtpMode;
			_connection.GetFileList(Settings.DestinationFolder);
			return null;
		}
		
		//public override void Put(string fileName, string directory = null)
		//{
		//	_connection = new FtpConnection();
  //          _connection.SetLogOnInformation(_settings.User, _settings.Password);
		//	_connection.ServerLocation = _settings.Host;
  //          _connection.IsPassive = !_settings.ActiveFtpMode;

		//	Log.WriteLine(LoggingEvent.Debug, String.Format(CultureInfo.CurrentCulture, _CopyingFormatString, fileName, _settings.DestinationFolder));
			
		//	var folder = string.IsNullOrEmpty(directory) ? _settings.DestinationFolder : Path.Combine(_settings.DestinationFolder, directory);
		//	_connection.Upload(folder, fileName);
		//}

		public override IStorageUploadResult Upload(string sourceFile, string relativeFileName)
		{
			throw new NotImplementedException();
		}
	}
}
