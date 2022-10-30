using BUtil.Core.Logs;
using BUtil.Core.Options;
using System;

namespace BUtil.Core
{
	public static class BackupModelStrategyFactory
	{
		public static IBackupModelStrategy Create(LogBase log, BackupTask task, ProgramOptions options)
		{
			switch (task.BackupModel.ProviderName)
			{
				case BackupModelProviderNames.Image:
                    return new ImageBackupModelStrategy(log, task, options);
				default:
					throw new ArgumentOutOfRangeException(nameof(task));
            }
        }
	}

    public delegate void BackupFinished();
	public delegate void CriticalErrorOccur(string message);
}
