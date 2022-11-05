using BUtil.Core.Options;
using System.Collections.Generic;

namespace BUtil.Core.State
{
    public class SourceItemChanges
    {
        public SourceItem SourceItem { get; set; }
        public IEnumerable<string> DeletedFiles { get; set; }
        public IEnumerable<StorageFile> UpdatedFiles { get; set; }
        public IEnumerable<StorageFile> CreatedFiles { get; set; }

        public SourceItemChanges() { }

        public SourceItemChanges(SourceItem sourceItem,
            IEnumerable<string> deletedFiles,
            IEnumerable<StorageFile> updatedFiles,
            IEnumerable<StorageFile> createdFiles)
        {
            SourceItem = sourceItem;
            DeletedFiles = deletedFiles;
            UpdatedFiles = updatedFiles;
            CreatedFiles = createdFiles;
        }
    }
}
