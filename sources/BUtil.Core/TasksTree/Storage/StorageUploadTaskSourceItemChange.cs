using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.State;
using System;
using System.Collections.Generic;

namespace BUtil.Core.TasksTree.Storage;

class StorageUploadTaskSourceItemChange
{
    public SourceItemV2 SourceItem { get; }
    public IEnumerable<string> DeletedFiles { get; }
    public IEnumerable<FileState> CreatedUpdatedFiles { get; }
    public Func<string, string> ActualFileToRemoteFileConverter { get; }

    public StorageUploadTaskSourceItemChange(
        SourceItemV2 sourceItem,
        IEnumerable<string> deletedFiles,
        IEnumerable<FileState> createdUpdatedFiles,
        Func<string, string> actualFileToRemoteFileConverter)
    {
        SourceItem = sourceItem;
        DeletedFiles = deletedFiles;
        CreatedUpdatedFiles = createdUpdatedFiles;
        ActualFileToRemoteFileConverter = actualFileToRemoteFileConverter;
    }
}
