
using System;
using System.Collections.Generic;
using BUtil.Core.Storages;

namespace BUtil.Core.Options
{
	public class BackupTask
	{
		public BackupTask()
		{
			ExecuteBeforeBackup = new List<ExecuteProgramTaskInfo>();
			ExecuteAfterBackup = new List<ExecuteProgramTaskInfo>();
			SchedulerDays = new List<DayOfWeek>();
			SchedulerTime = new TimeSpan(Constants.DefaultHours, Constants.DefaultMinutes, 0);
            Items = new List<SourceItem>();
			Storages = new List<StorageSettings>();
        }

        public string Name { get; set; }


        public List<DayOfWeek> SchedulerDays { get; set; }

        public TimeSpan SchedulerTime { get; set; }

        
		public List<ExecuteProgramTaskInfo> ExecuteBeforeBackup { get; set; }

		public List<ExecuteProgramTaskInfo> ExecuteAfterBackup { get; set; }
      
        
		public List<StorageSettings> Storages { get; set; }
	    
	    public string Password { get; set; }
		
		public List<SourceItem> Items { get; set; }
	}
}