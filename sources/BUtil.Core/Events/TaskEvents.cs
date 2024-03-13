
using BUtil.Core.TasksTree.Core;
using System;
using System.Collections.Generic;

namespace BUtil.Core.Events;

public class TaskEvents
{
    public event EventHandler<TaskProgressEventArgs>? OnTaskProgress;
    public event EventHandler<DuringExecutionTasksAddedEventArgs>? OnDuringExecutionTasksAdded;
    public event EventHandler<MessageEventArgs>? OnMessage;

    public void TaskProgessUpdate(Guid taskId, ProcessingStatus status)
    {
        var handler = OnTaskProgress;
        if (handler == null)
            return;
        handler(this, new TaskProgressEventArgs(taskId, status));
    }

    public void DuringExecutionTasksAdded(Guid taskId, IEnumerable<BuTask> tasks)
    {
        var handler = OnDuringExecutionTasksAdded;
        if (handler == null)
            return;
        handler(this, new DuringExecutionTasksAddedEventArgs(taskId, tasks));
    }

    public void Message(string message)
    {
        var handler = OnMessage;
        if (handler == null)
            return;
        handler(this, new MessageEventArgs(message));
    }
}
