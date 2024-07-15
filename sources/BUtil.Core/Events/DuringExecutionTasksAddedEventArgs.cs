using BUtil.Core.TasksTree.Core;
using System;
using System.Collections.Generic;

namespace BUtil.Core.Events;

public class DuringExecutionTasksAddedEventArgs(Guid taskId, IEnumerable<BuTask> tasks) : EventArgs
{
    public Guid TaskId { get; } = taskId;
    public IEnumerable<BuTask> Tasks { get; } = tasks;
}
