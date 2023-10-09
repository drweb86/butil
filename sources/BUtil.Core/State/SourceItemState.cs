
using BUtil.Core.ConfigurationFileModels.V2;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.State
{
    public class SourceItemState
    {
        public SourceItemV2 SourceItem { get; set; } = new();
        public List<FileState> FileStates { get; set; } = new();
        public SourceItemState() { } // deserialization
        public SourceItemState(SourceItemV2 sourceItem, List<FileState> fileStates)
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
