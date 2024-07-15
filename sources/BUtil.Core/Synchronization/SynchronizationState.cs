using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.Synchronization;

class SynchronizationState
{
    public List<SynchronizationStateFile> FileSystemEntries { get; set; } = [];

    internal SynchronizationState Clone()
    {
        return new SynchronizationState
        {
            FileSystemEntries = FileSystemEntries
                .Select(x => x.Clone())
                .ToList(),
        };
    }
}
