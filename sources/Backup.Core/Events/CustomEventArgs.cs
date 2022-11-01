using System;

namespace BUtil.Core.Events
{
    public class CustomEventArgs : EventArgs
    {
        public ProcessingStatus Status { get; private set; }
        public object Tag { get; private set; }

        public CustomEventArgs(object tag, ProcessingStatus status)
        {
            Tag = tag;
            Status = status;
        }
    }
}
