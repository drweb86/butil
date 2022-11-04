using System;

namespace BUtil.Core.Events
{
    public class BackupEvents
    {
        public event EventHandler<TaskProgressEventArgs> OnTaskProgress;

        public void TaskProgessUpdate(Guid taskId, ProcessingStatus status)
        {
            var handler = OnTaskProgress;
            if (handler == null)
                return;
            handler(this, new TaskProgressEventArgs(taskId, status));
        }
    }
}
