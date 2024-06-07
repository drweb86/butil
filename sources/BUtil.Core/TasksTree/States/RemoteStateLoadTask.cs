
using BUtil.Core.Events;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;

namespace BUtil.Core.TasksTree;

internal class RemoteStateLoadTask : BuTask
{
    public StorageSpecificServicesIoc _services;
    private readonly string _password;

    public IncrementalBackupState? StorageState { get; private set; }

    public RemoteStateLoadTask(StorageSpecificServicesIoc services, TaskEvents events, string password) :
        base(services.Log, events, Localization.Resources.DataStorage_State_Get)
    {
        _services = services;
        this._password = password;
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);
        IsSuccess = _services.IncrementalBackupStateService.TryRead(_password, out var state);
        StorageState = state;
        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
    }
}
