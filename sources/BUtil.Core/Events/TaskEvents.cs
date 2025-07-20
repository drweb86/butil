
using BUtil.Core.TasksTree.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.Events;

public class TaskEvents
{
    public event EventHandler<TaskProgressEventArgs>? OnTaskProgress;
    public event EventHandler<DuringExecutionTasksAddedEventArgs>? OnDuringExecutionTasksAdded;

    public void TaskProgessUpdate(Guid taskId, ProcessingStatus status)
    {
        var handler = OnTaskProgress;
        if (handler == null)
            return;
        handler(this, new TaskProgressEventArgs(taskId, status));
    }

    public void DuringExecutionTasksAdded(Guid? taskId, IEnumerable<BuTask> tasks)
    {
        if (!tasks.Any())
        {
            return;
        }

        var handler = OnDuringExecutionTasksAdded;
        if (handler == null)
            return;
        handler(this, new DuringExecutionTasksAddedEventArgs(taskId, tasks));
    }
}
