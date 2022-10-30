using System.Collections.Generic;
using BUtil.Core.Storages;

namespace BUtil.Core.Options
{
	public class BackupTask
	{
		public BackupTask()
		{
			ExecuteBeforeBackup = new ();
			ExecuteAfterBackup = new ();
            Items = new ();
			Storages = new ();
			BackupModel = new () { ProviderName = BackupModelProviderNames.Image };
        }

		public BackupModel BackupModel { get; set; }

        public string Name { get; set; }
        
		public List<ExecuteProgramTaskInfo> ExecuteBeforeBackup { get; set; }

		public List<ExecuteProgramTaskInfo> ExecuteAfterBackup { get; set; }
      
		public List<StorageSettings> Storages { get; set; }
	    
	    public string Password { get; set; }
		
		public List<SourceItem> Items { get; set; }
	}
}