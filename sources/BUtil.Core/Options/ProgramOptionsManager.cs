using System;
using BUtil.Core.BackupModels;
using BUtil.Core.FileSystem;

namespace BUtil.Core.Options
{
    public static class ProgramOptionsManager
	{
        public static BackupTask GetDefaultBackupTask(string name)
        {
            return new BackupTask
			{
				Name = name,
				Model = new IncrementalBackupModelOptions { DisableCompressionAndEncryption = false },
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
			};
        }

		public static ProgramOptions Default
		{
			get 
			{
				return new ProgramOptions
				{
					LogsFolder = Directories.LogsFolder,
				};
			}
		}
	}
}
