
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.TasksTree.DeleteBackupVersion;

public class DeleteBackupVersionTask : SequentialBuTask
{
    public DeleteBackupVersionTask(
        ILog log,
        StorageSpecificServicesIoc storageSpecificServicesIoc,
        TaskEvents backupEvents,
        IncrementalBackupState state,
        IncrementalBackupModelOptionsV2 options,
        VersionState versionState)
        : base(log, backupEvents, string.Empty)
    {
        DeleteVersionUtil.DeleteVersion(state, versionState, out var storageFilesToDelete);

        var tasks = new List<BuTask>();
        storageFilesToDelete.ToList().ForEach(x => tasks.Add(new DeleteStorageFileTask(storageSpecificServicesIoc, Events, x)));
        tasks.Add(new SaveStateToStorageTask(storageSpecificServicesIoc, Events, state, options));
        Children = tasks.ToArray();
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);
        base.Execute();
        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
    }
}
