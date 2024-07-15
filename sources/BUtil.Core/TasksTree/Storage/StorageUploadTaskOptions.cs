using BUtil.Core.State;
using System.Collections.Generic;

namespace BUtil.Core.TasksTree.Storage;

class StorageUploadTaskOptions(
    IncrementalBackupState state,
    IEnumerable<StorageUploadTaskSourceItemChange> changes)
{
    public IncrementalBackupState State { get; } = state;
    public IEnumerable<StorageUploadTaskSourceItemChange> Changes { get; } = changes;
}
