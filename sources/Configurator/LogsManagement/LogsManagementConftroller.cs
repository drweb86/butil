using System;
using System.Collections.Generic;
using BUtil.Configurator.Localization;
using BUtil.Core.Logs;

namespace BUtil.Configurator.LogsManagement
{
    /// <summary>
    /// Controller for logs management
    /// </summary>
    public sealed class LogManagementConftroller
	{
		#region Fields

		readonly LogOperations _logOperations;
		
		#endregion
		
		#region Constructors
		
		/// <summary>
		/// The default constructor
		/// </summary>
		/// <param name="logsFolder">The logs folder</param>
		public LogManagementConftroller(string logsFolder)
		{
			_logOperations = new LogOperations(logsFolder);
		}
		
		#endregion
		
		#region Public Methods
		
		/// <summary>
		/// Entry point of a controller
		/// </summary>
		public void Execute()
		{
			using (var form = new LogsViewerForm(this))
			{
				form.ShowDialog();
			}
		}
		
		/// <summary>
		/// Opens the current logs folder in explorer
		/// </summary>
		public void OpenLogsFolderInExplorer()
		{
			_logOperations.OpenLogsFolderInExplorer();
		}
		
		/// <summary>
		/// Gets the logs information
		/// </summary>
		/// <returns>Set of information types</returns>
		public List<LogInfo> GetLogsInformation()
		{
			return _logOperations.GetLogsInformation();
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
			
			_logOperations.OpenLogInBrowser(info);
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
			
			_logOperations.DeleteLog(info);
		}
		
		/// <summary>
		/// Deletes set of logs
		/// </summary>
		/// <param name="infos">The logs to delete</param>
		/// <returns>True if deleted log</returns>
		public bool DeleteSetOfLogs(List<LogInfo> infos)
		{
			if (infos == null)
			{
				throw new ArgumentNullException("info");
			}

            if (!Messages.ShowYesNoDialog(string.Format(Resources.PleaseConfirmDeletionOf0Logs, infos.Count)))
            {
                return false;
            }
            _logOperations.DeleteSetOfLogs(infos);
			return true;
		}
		
		#endregion
	}
}
