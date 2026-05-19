using BUtil.Interop.Tasks.Core;

namespace BUtil.Interop.Tasks.Events;

public class DuringExecutionTasksAddedEventArgs(Guid? taskId, IEnumerable<BuTask> tasks) : EventArgs
{
    public Guid? TaskId { get; } = taskId;
    public IEnumerable<BuTask> Tasks { get; } = tasks;
}
