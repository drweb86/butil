
using BUtil.Core.Events;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;

namespace BUtil.Core.TasksTree;

internal class RemoteStateLoadTask : BuTaskV2
{
    public StorageSpecificServicesIoc _services;
    private readonly string _password;

    public IncrementalBackupState? StorageState { get; private set; }

    public RemoteStateLoadTask(StorageSpecificServicesIoc services, TaskEvents events, string password) :
        base(services.CommonServices.Log, events, Localization.Resources.DataStorage_State_Get)
    {
        _services = services;
        _password = password;
    }

    protected override void ExecuteInternal()
    {
        if (!_services.IncrementalBackupStateService.TryRead(_password, out var state))
            throw new System.InvalidOperationException("Failed to read state!");
        StorageState = state;
    }
}
