using BUtil.Core.ConfigurationFileModels.V1;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.FileSystem;
using BUtil.Core.Misc;
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
    private readonly IncrementalBackupState _incrementalBackupState;
    private readonly IncrementalBackupModelOptionsV2 _incrementalBackupModelOptions;
    private readonly List<IGrouping<string, StorageFile>> _outdatedStorageFiles;

    public UpgradeStorageFormatChunkTask(TaskEvents events,
        StorageSpecificServicesIoc storageSpecificServicesIoc,
        IncrementalBackupState incrementalBackupState,
        IncrementalBackupModelOptionsV2 incrementalBackupModelOptions,
        List<IGrouping<string, StorageFile>> outdatedStorageFiles,
        int chunkIndex)
        : base(storageSpecificServicesIoc.CommonServices.Log, events, $"Upgrade storage format (chunk {chunkIndex + 1})")
    {
        _outdatedStorageFiles = outdatedStorageFiles;
        _storageSpecificServicesIoc = storageSpecificServicesIoc;
        _incrementalBackupState = incrementalBackupState;
        _incrementalBackupModelOptions = incrementalBackupModelOptions;
        Children = [];
    }

    protected override void ExecuteInternal()
    {
        using var tempFolder = new TempFolder();

        var tasks = new List<BuTaskV2>();

        var noQuota = new Quota();
        var versionStates = new List<VersionState>();
        var deleteStorageFileTasks = new List<DeleteStorageFileTask>();

        int fileIndex = 0;
        foreach (var group in _outdatedStorageFiles)
        {
            var localFile = Path.Combine(tempFolder.Folder, $"{fileIndex}");
            var downloadStorageFile = new StorageFile(group.First());
            var storageDownloadTask = new StorageDownloadTask(_storageSpecificServicesIoc, Events, downloadStorageFile, localFile, group.Key);

            var items = group.ToList();
            items.ForEach(x =>
            {
                x.StorageMethod = StorageMethodNames.Aes256Encrypted;
                x.StorageRelativeFileName = x.StorageRelativeFileName.Substring(0, x.StorageRelativeFileName.Length - "7z".Length) + SourceItemHelper.AES256V1Extension;
            });
            var writeSourceFileToStorageTask = new WriteSourceFileToStorageTask(_storageSpecificServicesIoc, Events, items, noQuota, null, versionStates, localFile, items[0].StorageRelativeFileName, false);
            var deleteStorageFilesTask = new DeleteStorageFileTask(_storageSpecificServicesIoc, Events, group.Key);
            tasks.Add(storageDownloadTask);
            tasks.Add(writeSourceFileToStorageTask);
            deleteStorageFileTasks.Add(deleteStorageFilesTask);
            fileIndex++;
        }

        var writeStateToStorageDirectTask = new WriteStateToStorageDirectTask(_storageSpecificServicesIoc, Events, _incrementalBackupState, _incrementalBackupModelOptions);
        tasks.Add(writeStateToStorageDirectTask);
        tasks.Add(new WriteIntegrityVerificationScriptsToStorageDirectTask(_storageSpecificServicesIoc, Events, _incrementalBackupState, () => writeStateToStorageDirectTask.StateStorageFile!));
        tasks.AddRange(deleteStorageFileTasks);

        Events.DuringExecutionTasksAdded(Id, tasks);
        Children = tasks;
        base.ExecuteInternal();
    }
}
