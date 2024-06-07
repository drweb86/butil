
using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.TasksTree;

internal class CalculateIncrementedVersionForStorageTask : BuTask
{
    public bool VersionIsNeeded { get; private set; }
    public IncrementalBackupState? IncrementalBackupState { get; private set; }

    private readonly RemoteStateLoadTask _storageStateTask;
    private readonly IEnumerable<GetStateOfSourceItemTask> _getSourceItemStateTasks;
    public CalculateIncrementedVersionForStorageTask(ILog log, TaskEvents events, RemoteStateLoadTask storageStateTask,
        IEnumerable<GetStateOfSourceItemTask> getSourceItemStateTasks) :
        base(log, events, BUtil.Core.Localization.Resources.IncrementalBackup_Version_Calculate)
    {
        _storageStateTask = storageStateTask;
        _getSourceItemStateTasks = getSourceItemStateTasks;
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);

        var storageState = _storageStateTask.StorageState;
        if (storageState == null)
        {
            UpdateStatus(ProcessingStatus.FinishedWithErrors);
            IsSuccess = false;
            return;
        }

        var sourceItemStates = _getSourceItemStateTasks
            .Select(item => item.SourceItemState ?? throw new InvalidOperationException())
            .ToList();


        var versionState = SourceItemStateComparer.Compare(storageState.LastSourceItemStates, sourceItemStates);
        storageState.VersionStates.Add(versionState);
        storageState.LastSourceItemStates = sourceItemStates
            .Select(x => x.ShallowClone())
            .ToList();
        IncrementalBackupState = storageState;
        VersionIsNeeded = versionState.SourceItemChanges.Any(x => x.CreatedFiles.Any() || x.UpdatedFiles.Any() || x.DeletedFiles.Any());

        UpdateStatus(ProcessingStatus.FinishedSuccesfully);
        IsSuccess = true;
    }
}
