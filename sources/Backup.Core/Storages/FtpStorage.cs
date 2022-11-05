using System;
using BUtil.Core.Logs;

namespace BUtil.Core.Storages
{

	class FtpStorage: StorageBase
	{
		// const string _CopyingFormatString = "Copying '{0}' to '{1}'";

		private readonly FtpStorageSettings _settings;

		FtpConnection _connection;

		internal FtpStorage(FtpStorageSettings settings)
		{
			_settings = settings;
		}
        public override string ReadAllText(string file)
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
		}
		
		public override void Test()
		{
			_connection = new FtpConnection();
			_connection.SetLogOnInformation(_settings.User, _settings.Password);
			_connection.ServerLocation = _settings.Host;
            _connection.IsPassive = !_settings.ActiveFtpMode;
			_connection.GetFileList(_settings.DestinationFolder);
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

		public override string Upload(string sourceFile, string relativeFileName)
		{
			throw new NotImplementedException();
		}
	}
}
