using System;
using System.IO;
using System.Collections.Generic;
using BUtil.Core.FileSystem;
using BUtil.Core.Misc;
using BUtil.Core.Options;

namespace BUtil.Core.Logs
{
	public class LogOperations // !
	{
		private readonly ProgramOptions _programOptions;
		
		public LogOperations(ProgramOptions programOptions)
		{
            _programOptions = programOptions;
		}
		
		public List<LogInfo> GetLogsInformation()
		{
			var result = new List<LogInfo>();
				
			if (!Directory.Exists(_programOptions.LogsFolder))
			{
				return result;
			}

			var logsList = Directory.GetFiles(_programOptions.LogsFolder, "*"+Files.LogFilesExtension);

			foreach (var log in logsList)
			{
				result.Add( new LogInfo(log) );
			}
			
			return result;
		}
	}
}
