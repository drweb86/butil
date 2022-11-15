using System;
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
					Priority = System.Threading.ThreadPriority.BelowNormal,
					LogsFolder = Directories.LogsFolder,
					PuttingOffBackupCpuLoading = Constants.DefaultCpuLoading,
					Parallel = Environment.ProcessorCount
				};
			}
		}
	}
}
