using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.FileSystem;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.States;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BUtil.Core.TasksTree.Storage;

public class UpgradeStorageFormatChunkTask : SequentialBuTaskV2
{
    private readonly StorageSpecificServicesIoc _storageSpecificServicesIoc;
    private readonly TempFolder _tempFolder = new TempFolder();
    private readonly List<IGrouping<string, StorageFile>> _outdatedStorageFiles;

    public UpgradeStorageFormatChunkTask(TaskEvents events,
        StorageSpecificServicesIoc storageSpecificServicesIoc,
        IncrementalBackupState incrementalBackupState,
        IncrementalBackupModelOptionsV2 incrementalBackupModelOptions,
        List<IGrouping<string, StorageFile>> outdatedStorageFiles)
        : base(storageSpecificServicesIoc.CommonServices.Log, events, "Upgrade storage format chunk")
    {
        _outdatedStorageFiles = outdatedStorageFiles;
        _storageSpecificServicesIoc = storageSpecificServicesIoc;

        var tasks = new List<BuTaskV2>();

        var noQuota = new Quota();
        var versionStates = new List<VersionState>();
        var deleteStorageFileTasks = new List<DeleteStorageFileTask>();

        int fileIndex = 0;
        foreach (var group in outdatedStorageFiles)
        {
            var localFile = Path.Combine(_tempFolder.Folder, $"{fileIndex}");
            var storageDownloadTask = new StorageDownloadTask(_storageSpecificServicesIoc, Events, group.First(), localFile, System.IO.Path.GetFileName(localFile));
            var writeSourceFileToStorageTask = new WriteSourceFileToStorageTask(_storageSpecificServicesIoc, Events, group.ToList(), noQuota, null, versionStates, localFile, false);
            var deleteStorageFilesTask = new DeleteStorageFileTask(_storageSpecificServicesIoc, Events, group.Key);
            tasks.Add(storageDownloadTask);
            tasks.Add(writeSourceFileToStorageTask);
            deleteStorageFileTasks.Add(deleteStorageFilesTask);

            fileIndex++;
        }

        var writeStateToStorageDirectTask = new WriteStateToStorageDirectTask(_storageSpecificServicesIoc, Events, incrementalBackupState, incrementalBackupModelOptions);
        tasks.Add(writeStateToStorageDirectTask);
        tasks.Add(new WriteIntegrityVerificationScriptsToStorageDirectTask(_storageSpecificServicesIoc, Events, incrementalBackupState, () => writeStateToStorageDirectTask.StateStorageFile!));
        tasks.AddRange(deleteStorageFileTasks);

        Children = tasks;
    }

    protected override void ExecuteInternal()
    {
        using (_tempFolder)
        {
            base.ExecuteInternal();
        }
    }
}
