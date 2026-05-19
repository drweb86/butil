using BUtil.Interop.Tasks.Events;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.Interop.Tasks.Core;
using System;

namespace BUtil.Tasks.Common.States;

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
        : base(services.CommonServices.Log, events, BUtil.Core.Localization.Resources.DataStorage_State_Saving)
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
            IsSkipped = true;
            return;
        }

        StateFile = _services.IncrementalBackupStateService.Write(_password, actualState);
    }
}
