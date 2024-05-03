using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;

namespace BUtil.Core.BackupModels;

class SynchronizationTaskModelOptionsStrategy : ITaskModelStrategy
{
    readonly ILog _log;
    readonly TaskV2 _task;

    public SynchronizationTaskModelOptionsStrategy(ILog openedLog, TaskV2 task)
    {
        _log = openedLog;
        _task = task;
    }

    public BuTask GetTask(TaskEvents events)
    {
        return new SynchronizationRootTask(_log, events, _task);
    }
}
