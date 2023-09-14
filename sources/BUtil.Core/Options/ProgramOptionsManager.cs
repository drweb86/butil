using BUtil.Core.BackupModels;
using BUtil.Core.ConfigurationFileModels.V2;

namespace BUtil.Core.Options
{
    public static class ProgramOptionsManager
	{
        public static BackupTaskV2 GetDefaultBackupTask(string name)
        {
            return new BackupTaskV2
			{
				Name = name,
				Model = new IncrementalBackupModelOptionsV2()
				{
                    Items =
                    {
                        new SourceItemV2
                        {
                            IsFolder = true,
                            Target = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)
                        },
                         new SourceItemV2
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
