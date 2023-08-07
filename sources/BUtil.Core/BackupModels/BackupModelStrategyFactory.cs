using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.TasksTree.IncrementalModel;
using System;

namespace BUtil.Core.BackupModels
{
    public static class BackupModelStrategyFactory
    {
        public static IBackupModelStrategy Create(ILog log, BackupTask task)
        {
            if (task.Model is IncrementalBackupModelOptions)
                return new IncrementalBackupModelStrategy(log, task);
            if (task.Model is MediaSyncBackupModelOptions)
                return new MediaSyncBackupModelStrategy(log, task);
            throw new ArgumentOutOfRangeException(nameof(task));
        }
    }
}
