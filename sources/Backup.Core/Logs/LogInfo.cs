using System;
using System.IO;
using BUtil.Core.FileSystem;

namespace BUtil.Core.Logs
{
	public sealed class LogInfo
	{
		readonly string _logFile;
		readonly BackupResult _result;
		readonly DateTime _timeStamp;
		
		public string LogFile
		{
			get { return _logFile; }
		}
			
		public BackupResult Result
		{
			get { return _result; }
		}
			
		public DateTime TimeStamp
		{
			get { return _timeStamp; }
		}
		
		public LogInfo(string file)
		{
			if (string.IsNullOrEmpty(file))
			{
				throw new ArgumentNullException("file");
			}

			_logFile = file;
			_timeStamp = File.GetCreationTime(_logFile);

			try
			{
				string contents = File.ReadAllText(_logFile);
				if (contents.Contains(Files.SuccesfullBackupMarkInHtmlLog))
				{
					_result = BackupResult.Successfull;
				}
				else if (contents.Contains(Files.ErroneousBackupMarkInHtmlLog))
				{
				_result = BackupResult.Erroneous;
				}
				else
				{
					_result = BackupResult.Unknown;
				}
			}
			catch (IOException e)
			{
				_result = BackupResult.Unknown;
				System.Diagnostics.Debug.WriteLine("Cannot open file: " + file + ". This log file is still opened so it marked with unknown status:" + e.ToString());
			}
		}
	}
}
