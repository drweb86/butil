using BUtil.Core.Options;
using System;

namespace BUtil.Core.Events
{
    public class SourceItemStatusEventArgs : EventArgs
    {
        public SourceItem Item { get; private set; }
        public ProcessingStatus Status { get; private set; }

        public SourceItemStatusEventArgs(SourceItem item, ProcessingStatus status)
        {
            Item = item;
            Status = status;
        }
    }
}
