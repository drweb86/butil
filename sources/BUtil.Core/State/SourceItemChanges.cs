
using BUtil.Core.ConfigurationFileModels.V2;
using System.Collections.Generic;

namespace BUtil.Core.State;

public class SourceItemChanges(SourceItemV2 sourceItem,
    List<string> deletedFiles,
    List<StorageFile> updatedFiles,
    List<StorageFile> createdFiles)
{
    public SourceItemV2 SourceItem { get; set; } = sourceItem;
    public List<string> DeletedFiles { get; set; } = deletedFiles;
    public List<StorageFile> UpdatedFiles { get; set; } = updatedFiles;
    public List<StorageFile> CreatedFiles { get; set; } = createdFiles;
}
