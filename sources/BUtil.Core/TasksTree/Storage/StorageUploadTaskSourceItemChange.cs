using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.State;
using System;
using System.Collections.Generic;

namespace BUtil.Core.TasksTree.Storage;

class StorageUploadTaskSourceItemChange(
    SourceItemV2 sourceItem,
    IEnumerable<string> deletedFiles,
    IEnumerable<FileState> createdUpdatedFiles,
    Func<string, string> actualFileToRemoteFileConverter)
{
    public SourceItemV2 SourceItem { get; } = sourceItem;
    public IEnumerable<string> DeletedFiles { get; } = deletedFiles;
    public IEnumerable<FileState> CreatedUpdatedFiles { get; } = createdUpdatedFiles;
    public Func<string, string> ActualFileToRemoteFileConverter { get; } = actualFileToRemoteFileConverter;
}
