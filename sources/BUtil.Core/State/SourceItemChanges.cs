
using BUtil.Core.ConfigurationFileModels.V2;
using System.Collections.Generic;

namespace BUtil.Core.State;

public class SourceItemChanges
{
    public SourceItemV2 SourceItem { get; set; } = new();
    public List<string> DeletedFiles { get; set; } = new();
    public List<StorageFile> UpdatedFiles { get; set; } = new();
    public List<StorageFile> CreatedFiles { get; set; } = new();

    public SourceItemChanges(SourceItemV2 sourceItem,
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
