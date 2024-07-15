
using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.TasksTree;

internal class CalculateIncrementedVersionForStorageTask(ILog log, TaskEvents events, RemoteStateLoadTask storageStateTask,
    IEnumerable<GetStateOfSourceItemTask> getSourceItemStateTasks) : BuTaskV2(log, events, BUtil.Core.Localization.Resources.IncrementalBackup_Version_Calculate)
{
    public bool VersionIsNeeded { get; private set; }
    public IncrementalBackupState? IncrementalBackupState { get; private set; }

    private readonly RemoteStateLoadTask _storageStateTask = storageStateTask;
    private readonly IEnumerable<GetStateOfSourceItemTask> _getSourceItemStateTasks = getSourceItemStateTasks;

    protected override void ExecuteInternal()
    {
        var storageState = _storageStateTask.StorageState ?? throw new ArgumentNullException(nameof(_storageStateTask.StorageState));
        var sourceItemStates = _getSourceItemStateTasks
            .Select(item => item.SourceItemState ?? throw new InvalidOperationException())
            .ToList();

        var versionState = SourceItemStateComparer.Compare(storageState.LastSourceItemStates, sourceItemStates);
        storageState.VersionStates.Add(versionState);
        storageState.LastSourceItemStates = sourceItemStates
            .Select(x => x.ShallowClone())
            .ToList();
        IncrementalBackupState = storageState;
        VersionIsNeeded = versionState.SourceItemChanges.Any(x => x.CreatedFiles.Count != 0 || x.UpdatedFiles.Count != 0 || x.DeletedFiles.Count != 0);
    }
}
