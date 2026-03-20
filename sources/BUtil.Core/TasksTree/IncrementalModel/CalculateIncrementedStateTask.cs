using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.TasksTree.IncrementalModel;

internal class CalculateIncrementedStateTask(
    ILog log,
    TaskEvents events,
    Func<IncrementalBackupState> getRemoteState,
    Func<IEnumerable<SourceItemState>> getLocalStates) : BuTaskV2(log, events, BUtil.Core.Localization.Resources.IncrementalBackup_Version_Calculate)
{
    public (bool versionIsNeeded, IncrementalBackupState updatedState) GetSuccessResult()
    {
        this.EnsureSuccess();
        return (VersionIsNeeded, UpdatedState.EnsureNotNull());
    }

    public bool VersionIsNeeded { get; private set; }
    public IncrementalBackupState? UpdatedState { get; private set; }

    protected override void ExecuteInternal()
    {
        var remoteState = getRemoteState();
        
        var localStates = getLocalStates();

        var newVersion = SourceItemStateComparer.Compare(remoteState.LastSourceItemStates, localStates);

        remoteState.VersionStates.Add(newVersion);
        remoteState.LastSourceItemStates = [.. localStates.Select(x => x.ShallowClone())];

        UpdatedState = remoteState;
        VersionIsNeeded = SourceItemStateComparer.IsNotEmpty(newVersion);
    }
}
