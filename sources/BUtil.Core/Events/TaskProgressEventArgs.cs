using System;

namespace BUtil.Core.Events;

public class TaskProgressEventArgs(
    Guid taskId,
    ProcessingStatus? status,
    string? title) : EventArgs
{
    public Guid TaskId { get; } = taskId;
    public ProcessingStatus? Status { get; } = status;
    public string? Title { get; } = title;
}
