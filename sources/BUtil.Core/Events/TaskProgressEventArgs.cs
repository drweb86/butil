using System;

namespace BUtil.Core.Events;

public class TaskProgressEventArgs(
    Guid taskId,
    ProcessingStatus status) : EventArgs
{
    public Guid TaskId { get; } = taskId;

    public ProcessingStatus Status { get; } = status;
}
