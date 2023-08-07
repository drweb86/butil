using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.Events;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;

namespace BUtil.Core.BackupModels
{
    class IncrementalBackupModelStrategy : IBackupModelStrategy
    {
        readonly ILog _log;
        readonly BackupTask _task;

        public IncrementalBackupModelStrategy(ILog openedLog, BackupTask task)
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
