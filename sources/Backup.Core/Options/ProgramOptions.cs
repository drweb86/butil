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

        #endregion
	  
		#region Properties
		
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
    }
}
