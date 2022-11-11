using BUtil.Core.Options;
using System.Collections.Generic;

namespace BUtil.Core.State
{
    public class SourceItemState
    {
        public SourceItem SourceItem { get; set; }
        public IEnumerable<FileState> FileStates { get; set; }
        public SourceItemState() { } // deserialization
        public SourceItemState(SourceItem sourceItem, IEnumerable<FileState> fileStates)
        {
            SourceItem = sourceItem;
            FileStates = fileStates;
        }
    }
}
