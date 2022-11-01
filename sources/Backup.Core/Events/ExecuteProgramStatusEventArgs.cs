using BUtil.Core.Options;
using System;

namespace BUtil.Core.Events
{
    public class ExecuteProgramStatusEventArgs : EventArgs
    {
        public ExecuteProgramTaskInfo TaskInfo { get; private set; }

        public ProcessingStatus Status { get; private set; }

        public ExecuteProgramStatusEventArgs(
            ExecuteProgramTaskInfo taskInfo, 
            ProcessingStatus status)
        {
            TaskInfo = taskInfo;
            Status = status;
        }
    }
}
