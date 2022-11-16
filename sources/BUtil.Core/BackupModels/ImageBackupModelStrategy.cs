using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.Events;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;

namespace BUtil.Core.BackupModels
{
    class IncrementalBackupModelStrategy : IBackupModelStrategy
    {
        private readonly IncrementalBackupModelOptions _modelOptions;
        readonly ILog _log;
        readonly BackupTask _task;

        public IncrementalBackupModelStrategy(ILog openedLog, BackupTask task, IncrementalBackupModelOptions modelOptions)
        {
            _modelOptions = modelOptions;
            _log = openedLog;
            _task = task;
        }

        public BuTask GetTask(BackupEvents events)
        {
            var task = new IncrementalBackupTask(_log, events, _task, _modelOptions);
            return task;
        }
    }
}
