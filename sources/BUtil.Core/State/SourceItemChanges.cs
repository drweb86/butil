using BUtil.Core.Options;
using System.Collections.Generic;

namespace BUtil.Core.State
{
    public class SourceItemChanges
    {
        public SourceItem SourceItem { get; set; }
        public List<string> DeletedFiles { get; set; }
        public List<StorageFile> UpdatedFiles { get; set; }
        public List<StorageFile> CreatedFiles { get; set; }

        public SourceItemChanges() { }

        public SourceItemChanges(SourceItem sourceItem,
            List<string> deletedFiles,
            List<StorageFile> updatedFiles,
            List<StorageFile> createdFiles)
        {
            SourceItem = sourceItem;
            DeletedFiles = deletedFiles;
            UpdatedFiles = updatedFiles;
            CreatedFiles = createdFiles;
        }
    }
}
