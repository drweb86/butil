using System.Collections.Generic;
using BUtil.Core.BackupModels;
using BUtil.Core.Storages;

namespace BUtil.Core.Options
{
	public class BackupTask
	{
		public BackupTask()
		{
            Items = new ();
			Storages = new ();
			Model = new IncrementalBackupModelOptions();
			FileExcludePatterns= new List<string> ();
        }

		public IBackupModelOptions Model { get; set; }

        public string Name { get; set; }
        
		public List<IStorageSettings> Storages { get; set; }
	    
	    public string Password { get; set; }
		
		public List<SourceItem> Items { get; set; }
		public List<string> FileExcludePatterns { get; set; }
    }
}