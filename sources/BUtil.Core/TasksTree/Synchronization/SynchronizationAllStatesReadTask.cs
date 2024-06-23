using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.Synchronization;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
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

    public SynchronizationAllStatesReadTask(
        SynchronizationServices synchronizationServices, 
        TaskEvents events,
        SynchronizationModel model) : 
        base(synchronizationServices.Log, events, Resources.State_LoadFromEverywhere)
    {
        _synchronizationServices = synchronizationServices;
        _model = model;

        var tasks = new List<BuTask>();

        _synchronizationLocalStateLoadTask = new SynchronizationLocalStateLoadTask(_synchronizationServices, Events);
        tasks.Add(_synchronizationLocalStateLoadTask);

        _setStateOfSourceItemTask = new GetStateOfSourceItemTask(Log, Events, _model.LocalSourceItem, new List<string>(), synchronizationServices.CommonServices);
        tasks.Add(_setStateOfSourceItemTask);

        _remoteStateLoadTask = new RemoteStateLoadTask(synchronizationServices.StorageSpecificServices, Events, model.TaskOptions.Password);
        tasks.Add(_remoteStateLoadTask);

        if (model.TaskOptions.SynchronizationMode == ConfigurationFileModels.V2.SynchronizationTaskModelMode.TwoWay)
        {
            var deleteUnversionedFilesStorageTask = new DeleteUnversionedFilesStorageTask(synchronizationServices.StorageSpecificServices, Events, _remoteStateLoadTask);
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

                _model.RemoteSourceItem = lastVersion?.SourceItemChanges.SingleOrDefault(x => SynchronizationHelper.IsSynchronizationSourceItem(x.SourceItem))?.SourceItem ?? _model.CreateVirtualSourceItem();

                _model.RemoteStorageFiles = lastVersion != null
                    ? SourceItemHelper.BuildVersionFiles(
                        _model.RemoteStorageState,
                        _model.RemoteSourceItem,
                        lastVersion)
                    : new List<StorageFile>();
                _model.LocalState = GetLocalState();
                _model.ActualFiles = GetActualFiles();

                LogDebug("Local state");
                LogDebug(JsonSerializer.Serialize(_model.LocalState, new JsonSerializerOptions { WriteIndented = true }));
                LogDebug("Actual files");
                LogDebug(JsonSerializer.Serialize(_model.ActualFiles, new JsonSerializerOptions { WriteIndented = true }));
                LogDebug("Remote state");
                LogDebug(JsonSerializer.Serialize(_model.RemoteState, new JsonSerializerOptions { WriteIndented = true }));
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

        var state = _setStateOfSourceItemTask.SourceItemState;
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
