using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using System;

namespace BUtil.Core.TasksTree.States;

internal class SaveStateToStorageTask : BuTask
{
    private readonly StorageSpecificServicesIoc _services;
    private readonly IncrementalBackupState _state;
    private readonly IncrementalBackupModelOptionsV2 _options;

    public StorageFile? StateFile { get; private set; }

    public SaveStateToStorageTask(
        StorageSpecificServicesIoc services,
        TaskEvents events,
        IncrementalBackupState state,
        IncrementalBackupModelOptionsV2 options)
        : base(services.Log, events, Localization.Resources.DataStorage_State_Saving)
    {
        _services = services;
        _state = state;
        _options = options;
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);

        try
        {
            StateFile = _services.IncrementalBackupStateService.Write(_options, _state);
            IsSuccess = StateFile != null;
        }
        catch (Exception ex)
        {
            LogError(ex.Message);
            IsSuccess = false;
        }
        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
    }
}
