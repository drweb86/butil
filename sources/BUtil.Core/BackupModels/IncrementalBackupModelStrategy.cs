using BUtil.Core.Logs;
using BUtil.Core.Events;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using BUtil.Core.ConfigurationFileModels.V2;

namespace BUtil.Core.BackupModels
{
    class IncrementalBackupModelStrategy : IBackupModelStrategy
    {
        readonly ILog _log;
        readonly BackupTaskV2 _task;

        public IncrementalBackupModelStrategy(ILog openedLog, BackupTaskV2 task)
        {
            _log = openedLog;
            _task = task;
        }

        public BuTask GetTask(BackupEvents events)
        {
            return new IncrementalBackupTask(_log, events, _task);
        }
    }
}
