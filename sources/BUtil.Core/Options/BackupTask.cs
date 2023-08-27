using BUtil.Core.BackupModels;

namespace BUtil.Core.Options
{
	public class BackupTask
	{
		public BackupTask()
		{
			Model = new IncrementalBackupModelOptions();
        }

		public IBackupModelOptions Model { get; set; }

        public string Name { get; set; }
        
        public string Password { get; set; }
    }
}