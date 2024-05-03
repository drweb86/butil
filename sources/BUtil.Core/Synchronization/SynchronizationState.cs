using System.Collections.Generic;

namespace BUtil.Core.Synchronization;

class SynchronizationState
{
    public List<SynchronizationStateFile> FileSystemEntries { get; set; } = new ();
}
