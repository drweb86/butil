using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.Synchronization;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using BUtil.Core.TasksTree.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace BUtil.Core.TasksTree.Synchronization;

internal class SynchronizationAllStatesReadTask : SequentialBuTask
{
    private readonly SynchronizationServices _synchronizationServices;
    private readonly SynchronizationModel _model;

    private readonly SynchronizationLocalStateLoadTask _synchronizationLocalStateLoadTask;
    private readonly GetStateOfSourceItemTask _setStateOfSourceItemTask;
    private readonly RemoteStateLoadTask _remoteStateLoadTask;
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { WriteIndented = true };

    public SynchronizationAllStatesReadTask(
        SynchronizationServices synchronizationServices, 
        TaskEvents events,
        SynchronizationModel model) : 
        base(synchronizationServices.CommonServices.Log, events, Resources.State_LoadFromEverywhere)
    {
        _synchronizationServices = synchronizationServices;
        _model = model;

        var tasks = new List<BuTask>();

        _synchronizationLocalStateLoadTask = new SynchronizationLocalStateLoadTask(_synchronizationServices, Events);
        tasks.Add(_synchronizationLocalStateLoadTask);

        _setStateOfSourceItemTask = new GetStateOfSourceItemTask(Events, _model.LocalSourceItem, [], synchronizationServices.CommonServices);
        tasks.Add(_setStateOfSourceItemTask);

        _remoteStateLoadTask = new RemoteStateLoadTask(synchronizationServices.StorageSpecificServices, Events, model.TaskOptions.Password);
        tasks.Add(_remoteStateLoadTask);

        if (model.TaskOptions.SynchronizationMode == ConfigurationFileModels.V2.SynchronizationTaskModelMode.TwoWay)
        {
            var deleteUnversionedFilesStorageTask = new DataStorageMaintananceTask(synchronizationServices.StorageSpecificServices, Events, _remoteStateLoadTask, new ConfigurationFileModels.V2.IncrementalBackupModelOptionsV2 { Password = model.TaskOptions.Password });
            tasks.Add(deleteUnversionedFilesStorageTask);
        }

        Children = tasks;
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
                _model.RemoteStorageState = GetRemoteStorageState();

                var lastVersion = _model.RemoteStorageState.VersionStates
                    .OrderByDescending(x => x.BackupDateUtc)
                    .FirstOrDefault();

                _model.RemoteSourceItem = lastVersion?.SourceItemChanges.SingleOrDefault(x => SynchronizationHelper.IsSynchronizationSourceItem(x.SourceItem))?.SourceItem ?? SynchronizationModel.CreateVirtualSourceItem();

                _model.RemoteStorageFiles = lastVersion != null
                    ? SourceItemHelper.BuildVersionFiles(
                        _model.RemoteStorageState,
                        _model.RemoteSourceItem,
                        lastVersion)
                    : [];
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
        if (!_setStateOfSourceItemTask.IsSuccess)
        {
            throw new InvalidOperationException("Source item state population has failed!");
        }

        var state = _setStateOfSourceItemTask.SourceItemState ?? throw new InvalidOperationException("Source item state is corrupted (null)!");
        return new SynchronizationState()
        {
            FileSystemEntries = state.FileStates
                .Select(x => new SynchronizationStateFile(state.SourceItem, x))
                .ToList(),
        };
    }

    private SynchronizationState GetLocalState()
    {
        if (_remoteStateLoadTask.StorageState == null || _remoteStateLoadTask.StorageState.VersionStates.Count == 0)
        {
            LogDebug("Remote version state is missing => treating local state as empty to avoid data loss!");

            return new SynchronizationState();
        }

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

    private IncrementalBackupState GetRemoteStorageState()
    {
        if (!_remoteStateLoadTask.IsSuccess)
        {
            throw new InvalidOperationException("Remote state population has failed!");
        }

        return _remoteStateLoadTask.StorageState!;
    }

    private SynchronizationState GetRemoteState()
    {
        var remoteState = GetRemoteStorageState();

        if (remoteState == null)
        {
            LogDebug("Remote state is missing!");
            return new SynchronizationState();
        }

        var state = remoteState.LastSourceItemStates.SingleOrDefault(x => SynchronizationHelper.IsSynchronizationSourceItem(x.SourceItem));
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
