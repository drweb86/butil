using BUtil.Core.State;
using System.Collections.Generic;

namespace BUtil.Core.TasksTree.Storage;

class StorageUploadTaskOptions
{ 
    public IncrementalBackupState State { get; }
    public IEnumerable<StorageUploadTaskSourceItemChange> Changes { get; }

    public StorageUploadTaskOptions(
        IncrementalBackupState state,
        IEnumerable<StorageUploadTaskSourceItemChange> changes)
    {
        State = state;
        Changes = changes;
    }
}
