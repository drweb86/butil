using BUtil.Core.State;
using System.Collections.Generic;

namespace BUtil.Core.TasksTree.Storage;

class StorageUploadTaskOptions
{ 
    public IncrementalBackupState PreviousState { get; }
    public IEnumerable<StorageUploadTaskSourceItemChange> Changes { get; }

    public StorageUploadTaskOptions(
        IncrementalBackupState previousState,
        IEnumerable<StorageUploadTaskSourceItemChange> changes)
    {
        PreviousState = previousState;
        Changes = changes;
    }
}
