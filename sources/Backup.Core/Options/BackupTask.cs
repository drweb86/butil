
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BUtil.Core.Storages;

namespace BUtil.Core.Options
{
	public sealed class BackupTask
	{
		#region Fields
			
		readonly Collection<StorageBase> _storages = new Collection<StorageBase>();

		#endregion

		public BackupTask()
		{
			BeforeBackupTasksChain = new List<BackupEventTaskInfo>();
			AfterBackupTasksChain = new List<BackupEventTaskInfo>();
			ScheduledDays = new List<DayOfWeek>();
			SchedulerTime = new TimeSpan(Constants.DefaultHours, Constants.DefaultMinutes, 0);
            What = new List<CompressionItem>();
        }

        #region Properties

        public List<DayOfWeek> ScheduledDays { get; set; }

        public TimeSpan SchedulerTime { get; set; }

        public List<BackupEventTaskInfo> BeforeBackupTasksChain { get; set; }

		public List<BackupEventTaskInfo> AfterBackupTasksChain { get; set; }

      
        public string Name { get; set; }
        
        /// <summary>
		/// Places where to store backup
		/// </summary>
		public Collection<StorageBase> Storages
		{
			get { return _storages; }
		}
	    
	    public string Password { get; set; }
		
		public List<CompressionItem> What { get; set; }
		
		#endregion
	}
}