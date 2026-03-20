using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.TasksTree.IncrementalModel;

class CalculateIncrementedStateTask(
    ILog log,
    TaskEvents events,
    Func<IncrementalBackupState> getRemoteState,
    Func<IEnumerable<SourceItemState>> getLocalStates) : BuTaskV2(log, events, BUtil.Core.Localization.Resources.IncrementalBackup_Version_Calculate)
{
    private bool _versionIsNeeded;
    private IncrementalBackupState? _updatedState;

    public (bool versionIsNeeded, IncrementalBackupState updatedState) GetSuccessResult()
    {
        this.EnsureSuccess();
        return (_versionIsNeeded, _updatedState.EnsureNotNull());
    }

    protected override void ExecuteInternal()
    {
        var remoteState = getRemoteState();
        
        var localStates = getLocalStates();

        var newVersion = SourceItemStateComparer.Compare(remoteState.LastSourceItemStates, localStates);

        remoteState.VersionStates.Add(newVersion);
        remoteState.LastSourceItemStates = [.. localStates.Select(x => x.ShallowClone())];

        _updatedState = remoteState;
        _versionIsNeeded = SourceItemStateComparer.IsNotEmpty(newVersion);
    }
}
