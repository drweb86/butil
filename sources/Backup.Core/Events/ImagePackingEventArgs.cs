using System;

namespace BUtil.Core.Events
{
    public class ImagePackingEventArgs : EventArgs
    {
        public ProcessingStatus Status { get; private set; }

        public ImagePackingEventArgs(ProcessingStatus status)
        {
            Status = status;
        }
    }
}
