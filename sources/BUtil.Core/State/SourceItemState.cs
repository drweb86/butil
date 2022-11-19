using BUtil.Core.Options;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.State
{
    public class SourceItemState
    {
        public SourceItem SourceItem { get; set; }
        public List<FileState> FileStates { get; set; }
        public SourceItemState() { } // deserialization
        public SourceItemState(SourceItem sourceItem, List<FileState> fileStates)
        {
            SourceItem = sourceItem;
            FileStates = fileStates;
        }

        public SourceItemState ShallowClone()
        {
            return new SourceItemState(SourceItem, FileStates.ToList());
        }
    }
}
