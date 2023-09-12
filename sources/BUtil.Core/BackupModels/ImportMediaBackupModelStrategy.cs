using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.Events;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.MediaSyncBackupModel;

namespace BUtil.Core.BackupModels
{
    class ImportMediaBackupModelStrategy : IBackupModelStrategy
    {
        readonly ILog _log;
        readonly BackupTask _task;

        public ImportMediaBackupModelStrategy(ILog openedLog, BackupTask task)
        {
            _log = openedLog;
            _task = task;
        }

        public BuTask GetTask(BackupEvents events)
        {
            return new ImportMediaBackupTask(_log, events, _task);
        }
    }
}
