using BUtil.Core.Logs;
using BUtil.Core.Options;
using System;

namespace BUtil.Core.BackupModels
{
    public static class BackupModelStrategyFactory
    {
        public static IBackupModelStrategy Create(ILog log, BackupTask task, ProgramOptions options)
        {
            if (task.Model is IncrementalBackupModelOptions)
                return new IncrementalBackupModelStrategy(log, task, task.Model as IncrementalBackupModelOptions, options);
            throw new ArgumentOutOfRangeException(nameof(task));
        }
    }
}