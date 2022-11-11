using BUtil.Core.Options;
using System;

namespace BUtil.Core.Events
{
    public class TaskProgressEventArgs : EventArgs
    {
        public Guid TaskId { get; }

        public ProcessingStatus Status { get; }

        public TaskProgressEventArgs(
            Guid taskId, 
            ProcessingStatus status)
        {
            TaskId = taskId;
            Status = status;
        }
    }
}
