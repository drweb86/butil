using System;
using System.IO;
using System.Collections.Generic;
using BULocalization;
using BUtil.Core.FileSystem;
using BUtil.Core.Misc;
using BUtil.Core.PL;

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
		
		/// <summary>
		/// Opens the logs folder in explorer
		/// </summary>
		public void OpenLogsFolderInExplorer()
		{
			SupportManager.StartProcess(_logsFolder, true);
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
		
		/// <summary>
		/// Requests OS to open the log
		/// </summary>
		/// <param name="info">Log information</param>
		public void OpenLogInBrowser(LogInfo info)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			
			SupportManager.OpenWebLink(info.LogFile);
		}
		
		/// <summary>
		/// Deletes the specified log
		/// </summary>
		/// <param name="info">The log to delete</param>
		public void DeleteLog(LogInfo info)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			
			File.Delete(info.LogFile);
		}
		
		/// <summary>
		/// Deletes set of logs
		/// </summary>
		/// <param name="infos">The logs to delete</param>
		/// <param name="confirmationRequired">Shows wheather to show confirm dialog when needed</param>
		/// <returns>True if deleted log</returns>
		public bool DeleteSetOfLogs(List<LogInfo> infos, bool confirmationRequired)
		{
			if (infos == null)
			{
				throw new ArgumentNullException("info");
			}
			
			if (infos.Count > 1 && confirmationRequired)
			{
				if (!Messages.ShowYesNoDialog(string.Format(Translation.Current[619], infos.Count)))
				{
					return false;
				}
			}
			
			foreach (var info in infos)
			{
				DeleteLog(info);
			}
			
			return true;
		}
		
		#endregion
	}
}
