using BUtil.Core.TasksTree.Core;
using System;
using System.Collections.Generic;

namespace BUtil.Core.Events;

public class DuringExecutionTasksAddedEventArgs : EventArgs
{
    public Guid TaskId { get; }
    public IEnumerable<BuTask> Tasks { get; }

    public DuringExecutionTasksAddedEventArgs(Guid taskId, IEnumerable<BuTask> tasks)
    {
        TaskId = taskId;
        Tasks = tasks;
    }
}
