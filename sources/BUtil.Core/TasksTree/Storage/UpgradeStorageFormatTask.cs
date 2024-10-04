using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.TasksTree.Storage;

public class UpgradeStorageFormatTask : SequentialBuTaskV2
{
    public UpgradeStorageFormatTask(StorageSpecificServicesIoc storageSpecificServicesIoc, TaskEvents events,
        IncrementalBackupState incrementalBackupState,
        IncrementalBackupModelOptionsV2 incrementalBackupModelOptions,
        IEnumerable<StorageFile> outdatedStorageFiles)
        : base(storageSpecificServicesIoc.CommonServices.Log, events, "Upgrade storage format")
    {
        var chunkFiles = 50;

        Children = outdatedStorageFiles
            .GroupBy(x => x.StorageRelativeFileName)
            .Select((s, i) => new { Value = s, Index = i })
            .GroupBy(x => x.Index / chunkFiles)
            .Select(grp => grp.Select(x => x.Value).ToArray())
            .Select(x => new UpgradeStorageFormatChunkTask(Events, storageSpecificServicesIoc, incrementalBackupState, incrementalBackupModelOptions, x.ToList()))
            .ToArray();
    }
}
