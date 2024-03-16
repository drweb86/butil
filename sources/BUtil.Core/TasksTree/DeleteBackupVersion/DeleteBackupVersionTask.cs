
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using BUtil.Core.TasksTree.States;
using BUtil.Core.TasksTree.Storage;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.TasksTree.DeleteBackupVersion;

public class DeleteBackupVersionTask : SequentialBuTask
{
    public DeleteBackupVersionTask(
        StorageSpecificServicesIoc storageSpecificServicesIoc,
        TaskEvents events,
        IncrementalBackupState state,
        IncrementalBackupModelOptionsV2 options,
        VersionState versionToDelete)
        : base(storageSpecificServicesIoc.Log, events, $"Delete backup version {versionToDelete.BackupDateUtc}")
    {
        DeleteVersionUtil.DeleteVersion(state, versionToDelete, out var storageFilesToDelete, out var storageFileMovements);

        var tasks = new List<BuTask>();

        storageFilesToDelete.ToList().ForEach(x => tasks.Add(new DeleteStorageFileTask(storageSpecificServicesIoc, Events, x)));

        storageFileMovements.ToList().ForEach(x => tasks.Add(new MoveStorageFileTask(storageSpecificServicesIoc, Events, x.Key, x.Value)));

        var saveStateTask = new SaveStateToStorageTask(storageSpecificServicesIoc, Events, state, options);
        tasks.Add(saveStateTask);

        tasks.Add(new WriteIntegrityVerificationScriptsToStorageTask(storageSpecificServicesIoc, Events, () => true,
            () => state, saveStateTask, saveStateTask, () => saveStateTask.StateFile!));

        Children = tasks.ToArray();
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);
        base.Execute();
        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
    }
}
