using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.MediaSyncBackupModel;

namespace BUtil.Core.BackupModels;

class ImportMediaTaskModelStrategy : ITaskModelStrategy
{
    readonly ILog _log;
    readonly TaskV2 _task;

    public ImportMediaTaskModelStrategy(ILog openedLog, TaskV2 task)
    {
        _log = openedLog;
        _task = task;
    }

    public BuTask GetTask(TaskEvents events)
    {
        return new ImportMediaTask(_log, events, _task);
    }
}
