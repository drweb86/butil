
using BUtil.Core.Events;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;

namespace BUtil.Core.TasksTree;

internal class RemoteStateLoadTask(StorageSpecificServicesIoc services, TaskEvents events, string password) : BuTaskV2(services.CommonServices.Log, events, Localization.Resources.DataStorage_State_Get)
{
    public StorageSpecificServicesIoc _services = services;
    private readonly string _password = password;

    public IncrementalBackupState? StorageState { get; private set; }

    protected override void ExecuteInternal()
    {
        if (!_services.IncrementalBackupStateService.TryRead(_password, out var state))
            throw new System.InvalidOperationException("Failed to read state!");
        StorageState = state;
    }
}
