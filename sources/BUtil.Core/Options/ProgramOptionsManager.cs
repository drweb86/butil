using BUtil.Core.BackupModels;

namespace BUtil.Core.Options
{
    public static class ProgramOptionsManager
	{
        public static BackupTask GetDefaultBackupTask(string name)
        {
            return new BackupTask
			{
				Name = name,
				Model = new IncrementalBackupModelOptions()
				{
                    Items =
                    {
                        new SourceItem
                        {
                            IsFolder = true,
                            Target = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)
                        },
                         new SourceItem
                        {
                            IsFolder = true,
                            Target = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop)
                        }
                    }
                },
			};
        }
	}
}
