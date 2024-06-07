using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Synchronization;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using BUtil.Core.TasksTree.States;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.TasksTree.Synchronization;

internal class SynchronizationAllStatesReadTask : ParallelBuTask
{
    private readonly SynchronizationServices _synchronizationServices;
    private readonly SynchronizationModel _model;

    private readonly GetStateOfSourceItemsAndStoragesTask _getStateOfSourceItemsAndStoragesTask;
    private readonly SynchronizationLocalStateLoadTask _synchronizationLocalStateLoadTask;

    public SynchronizationAllStatesReadTask(
        SynchronizationServices synchronizationServices, 
        TaskEvents events,
        SynchronizationModel model) : 
        base(synchronizationServices.Log, events, Resources.State_LoadFromEverywhere)
    {
        _synchronizationServices = synchronizationServices;
        _model = model;

        _getStateOfSourceItemsAndStoragesTask = new GetStateOfSourceItemsAndStoragesTask(Log, Events, [_model.ToSourceItem()], 
            synchronizationServices.CommonServices, synchronizationServices.StorageSpecificServices, new List<string>(), _model.TaskOptions.Password);
        _synchronizationLocalStateLoadTask = new SynchronizationLocalStateLoadTask(_synchronizationServices, Events);

        Children = new List<BuTask>
        {
            _getStateOfSourceItemsAndStoragesTask,
            _synchronizationLocalStateLoadTask,
        };
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);
        try
        {
            base.Execute();
            UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
            if (IsSuccess)
            {
                _model.RemoteState = GetRemoteState();
                _model.LocalState = GetLocalState();
                _model.ActualFiles = GetActualFiles();
            }
        }
        catch (Exception ex)
        {
            LogError(ex.ToString());
            UpdateStatus(ProcessingStatus.FinishedWithErrors);
        }
    }

    private SynchronizationState GetActualFiles()
    {
        var getStateOfSourceItemTask = _getStateOfSourceItemsAndStoragesTask.GetSourceItemStateTasks.Single();
        if (!getStateOfSourceItemTask.IsSuccess)
        {
            throw new InvalidOperationException("Source item state population has failed!");
        }

        var state = getStateOfSourceItemTask.SourceItemState;
        if (state == null)
        {
            throw new InvalidOperationException("Source item state is corrupted (null)!");
        }

        return new SynchronizationState()
        {
            FileSystemEntries = state.FileStates
                .Select(x => new SynchronizationStateFile(state.SourceItem, x))
                .ToList(),
        };
    }

    private SynchronizationState GetLocalState()
    {
        if (!_synchronizationLocalStateLoadTask.IsSuccess)
        {
            throw new InvalidOperationException("Local state population has failed!");
        }

        if (_synchronizationLocalStateLoadTask.SynchronizationState == null)
        {
            LogDebug("Local state is missing!");
            return new SynchronizationState();
        }

        return _synchronizationLocalStateLoadTask.SynchronizationState;
    }

    private SynchronizationState GetRemoteState()
    {
        if (!_getStateOfSourceItemsAndStoragesTask.RemoteStateLoadTask.IsSuccess)
        {
            throw new InvalidOperationException("Remote state population has failed!");
        }

        if (_getStateOfSourceItemsAndStoragesTask.RemoteStateLoadTask.StorageState == null)
        {
            LogDebug("Remote state is missing!");
            return new SynchronizationState();
        }

        var state = _getStateOfSourceItemsAndStoragesTask.RemoteStateLoadTask.StorageState.LastSourceItemStates.SingleOrDefault(x => x.SourceItem.Id == Guid.Empty);
        if (state == null)
        {
            LogDebug("Remote state source item state is missing!");
            return new SynchronizationState();
        }
        return new SynchronizationState()
        {
            FileSystemEntries = state.FileStates
                .Select(x => new SynchronizationStateFile(state.SourceItem, x))
                .ToList(),
        };
    }
}
