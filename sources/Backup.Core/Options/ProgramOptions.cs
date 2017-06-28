using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using BUtil.Core.Logs;
using BUtil.Core.Storages;

namespace BUtil.Core.Options
{
	/// <summary>
	/// Profile options
	/// </summary>
    public sealed class ProgramOptions
	{
    	#region Consts
    	
    	const string _INVALID_CPU_LOADING = "CPU loading is out of bounds of valid values!";
    	
    	#endregion
    	
		#region Private Fields
		
        byte _puttingOffBackupCpuLoading;
        bool _showSchedulerInTray = true;

		readonly Dictionary<string, BackupTask> _backupTasks = new Dictionary<string, BackupTask> ();
        
        #endregion
	  
		#region Properties
		
		/// <summary>
		/// Hides some functional in Configurator
		/// </summary>
		public bool HaveNoNetworkAndInternet {get; set;}
		
		/// <summary>
		/// Disables configuring start up of  scheduler in current user account
		/// </summary>
		public bool DontCareAboutSchedulerStartup { get; set; }
		
		/// <summary>
		/// Hides about tab of Configurator
		/// </summary>
		public bool HideAboutTab { get; set; }
		
		/// <summary>
		/// Shows wheather scheduler configuration should be available in Configurator
		/// </summary>
		public bool DontNeedScheduler { get; set; }

		/// <summary>
		/// Backup tasks
		/// </summary>
		public Dictionary<string, BackupTask> BackupTasks
		{
			get { return _backupTasks; }
		}
		
		/// <summary>
		/// Enables or disables length checking for password
		/// </summary>
		public bool DontCareAboutPasswordLength { get; set; }
		
		/// <summary>
		/// Synchronous storage processing amount
		/// </summary>
		public int AmountOfStoragesToProcessSynchronously {get; set;}

		/// <summary>
		/// Synchronous 7-zip processing amount
		/// </summary>
		public int AmountOf7ZipProcessesToProcessSynchronously { get; set; }
		
		/// <summary>
		/// Folder were the logs must be
		/// Setting overrides directories.logsfolder setting
		/// </summary>
		public string LogsFolder { get; set; }
		
		/// <summary>
		/// Logging level
		/// </summary>
		public LogLevel LoggingLevel { get; set; }

	    /// <summary>
	    /// Priority of the 7-zip processes and threads of main processing
	    /// </summary>
	    public ThreadPriority Priority { get; set; }
		
		/// <summary>
		/// Converts ThreadPriority to ProcessPriorityClass
		/// </summary>
		public ProcessPriorityClass ProcessPriority
		{
			get
			{
				switch (Priority)
				{
					case ThreadPriority.AboveNormal:
						return ProcessPriorityClass.AboveNormal;
					case ThreadPriority.BelowNormal:
						return ProcessPriorityClass.BelowNormal;
					case ThreadPriority.Normal:
						return ProcessPriorityClass.Normal;
					case ThreadPriority.Lowest:
						return ProcessPriorityClass.Idle;
					default:
						// Highest case
						throw new NotSupportedException(Priority.ToString());
				}
			}
		}

		/// <summary>
		/// Shows the icon of scheduler in tray
		/// </summary>
		public bool ShowSchedulerInTray 
		{
			get { return _showSchedulerInTray; }
			set { _showSchedulerInTray = value; }
		}
        
        /// <summary>
        /// The cpu loading that postpones backup(in scheduler only)
        /// </summary>
        public byte PuttingOffBackupCpuLoading
		{
            get { return _puttingOffBackupCpuLoading; }
            set 
            { 
				if ((value < Constants.MinimumCpuLoading) || 
                  (value > Constants.MaximumCpuLoading))
            	{
            		throw new ArgumentException(_INVALID_CPU_LOADING);
            	}
            	_puttingOffBackupCpuLoading = value;
            }
		}
        
        #endregion
        
        #region Public Methods
		
		/// <summary>
		/// Checks if settings contains any password
		/// </summary>
		/// <returns>True if requires</returns>
		public bool RequiresEncryptionForSafety()
		{
			bool requireEncryption = false;
            
            foreach (KeyValuePair<string, BackupTask> pair  in _backupTasks)
            {
            	if (pair.Value.EnableEncryption)
            	{
            		requireEncryption = true;
            		break;
            	}
            	
            	foreach ( StorageBase storage in pair.Value.Storages)
            	{
            		if (storage is FtpStorage)
            		{
            			if (!string.IsNullOrEmpty(((FtpStorage)storage).Password))
            			{
            				requireEncryption = true;
            				break;
            			}
            		}
            	}
            }
            
            return requireEncryption;
		}
		
	    #endregion
	}
}
