using System;
using System.IO;
using System.Collections.Generic;
using BUtil.Core.FileSystem;
using BUtil.Core.Misc;

namespace BUtil.Core.Logs
{
	/// <summary>
	/// Operations with logs
	/// </summary>
	public class LogOperations
	{
		#region Fields
		
		readonly string _logsFolder;

		#endregion
		
		#region Constructors
		
		/// <summary>
		/// The default constructor
		/// </summary>
		/// <param name="logsFolder">The logs folder</param>
		public LogOperations(string logsFolder)
		{
			if (string.IsNullOrEmpty(logsFolder))
			{
				throw new ArgumentNullException("logsFolder");
			}

			_logsFolder = logsFolder;
		}
		
		#endregion
		
		#region Public methods
		
		public void OpenLogsFolderInExplorer()
		{
            ProcessHelper.ShellExecute(_logsFolder);
		}
		
		/// <summary>
		/// Gets the logs information
		/// </summary>
		/// <returns>Set of information types</returns>
		public List<LogInfo> GetLogsInformation()
		{
			var result = new List<LogInfo>();
				
			if (!Directory.Exists(_logsFolder))
			{
				return result;
			}

			var logsList = Directory.GetFiles(_logsFolder, "*"+Files.LogFilesExtension);

			foreach (var log in logsList)
			{
				result.Add( new LogInfo(log) );
			}
			
			return result;
		}
		
		public void OpenLogInBrowser(LogInfo info)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			
			ProcessHelper.ShellExecute(info.LogFile);
		}
		
		public void DeleteLog(LogInfo info)
		{
			File.Delete(info.LogFile);
		}
		
		public void DeleteSetOfLogs(IEnumerable<LogInfo> infos)
		{
			foreach (var info in infos)
				DeleteLog(info);
		}
		
		#endregion
	}
}
