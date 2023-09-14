using BUtil.Core.Logs;
using BUtil.Core.Events;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.MediaSyncBackupModel;
using BUtil.Core.ConfigurationFileModels.V2;

namespace BUtil.Core.BackupModels
{
    class ImportMediaBackupModelStrategy : IBackupModelStrategy
    {
        readonly ILog _log;
        readonly BackupTaskV2 _task;

        public ImportMediaBackupModelStrategy(ILog openedLog, BackupTaskV2 task)
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
