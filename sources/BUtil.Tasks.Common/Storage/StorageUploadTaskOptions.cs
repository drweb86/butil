using BUtil.Core.State;
using System.Collections.Generic;

namespace BUtil.Tasks.Common.Storage;

class StorageUploadTaskOptions(
    IncrementalBackupState state,
    IEnumerable<StorageUploadTaskSourceItemChange> changes)
{
    public IncrementalBackupState State { get; } = state;
    public IEnumerable<StorageUploadTaskSourceItemChange> Changes { get; } = changes;
}
