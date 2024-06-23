using BUtil.Core.Events;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using System;

namespace BUtil.Core.TasksTree.States;

internal class SaveStateToStorageTask : BuTaskV2
{
    private readonly StorageSpecificServicesIoc _services;
    private readonly IncrementalBackupState? _state;
    private readonly Func<IncrementalBackupState?>? _getState;
    private readonly string _password;

    public StorageFile? StateFile { get; private set; }

    public SaveStateToStorageTask(
        StorageSpecificServicesIoc services,
        TaskEvents events,
        Func<IncrementalBackupState?> getState,
        string password)
        : this(services, events, password)
    {
        _getState = getState;
    }

    public SaveStateToStorageTask(
        StorageSpecificServicesIoc services,
        TaskEvents events,
        IncrementalBackupState? state,
        string password)
        : this(services, events, password)
    {
        _state = state;
    }

    private SaveStateToStorageTask(StorageSpecificServicesIoc services, TaskEvents events, string password)
        : base(services.Log, events, Localization.Resources.DataStorage_State_Saving)
    {
        _services = services;
        _password = password;
    }

    protected override void ExecuteInternal()
    {
        var actualState = _state ?? _getState!();

        if (actualState == null)
        {
            LogDebug("State is null. Version is not needed. Skipping save.");
            return;
        }

        StateFile = _services.IncrementalBackupStateService.Write(_password, actualState);
        if (StateFile == null)
        {
            throw new Exception("Failed to save state!");
        }
    }
}
