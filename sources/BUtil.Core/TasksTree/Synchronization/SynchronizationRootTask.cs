using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Synchronization;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.Synchronization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace BUtil.Core.TasksTree.IncrementalModel;

class SynchronizationRootTask : SequentialBuTask
{
    private readonly SynchronizationServices _synchronizationServices;
    private readonly SynchronizationModel _model;

    private readonly SynchronizationAllStatesReadTask _synchronizationAllStatesReadTask;

    public SynchronizationRootTask(ILog log, TaskEvents backupEvents, TaskV2 task)
        : base(log, backupEvents, Resources.SynchronizationTask_Create, null)
    {
        var options = (SynchronizationTaskModelOptionsV2)task.Model;
        _model = new SynchronizationModel((SynchronizationTaskModelOptionsV2)task.Model);
        _synchronizationServices = new SynchronizationServices(log, task.Name, _model.TaskOptions.LocalFolder, FileHelper.NormalizeRelativePath(options.RepositorySubfolder), options.To, false);

        _synchronizationAllStatesReadTask = new SynchronizationAllStatesReadTask(_synchronizationServices, Events, _model);

        Children = new List<BuTask> { _synchronizationAllStatesReadTask };
    }

    public override void Execute()
    {
        Events.OnMessage += OnAddLastMinuteLogMessage;
        UpdateStatus(ProcessingStatus.InProgress);

        base.Execute();

        if (IsSuccess)
        {
            var syncItems = _synchronizationServices.DecisionService.Decide(_model.TaskOptions.SynchronizationMode, _model.LocalState, _model.ActualFiles, _model.RemoteState);
            LogDebug("Decisions");
            LogDebug(JsonSerializer.Serialize(syncItems, new JsonSerializerOptions { WriteIndented = true }));
            var tasks = new List<BuTask>();
            ExecuteActionsLocally(tasks, syncItems);
            ExecuteActionsRemotely(tasks, syncItems);

            if (syncItems.Any(x => x.RemoteAction != SynchronizationDecision.DoNothing) ||
                syncItems.Any(x => x.ActualFileAction != SynchronizationDecision.DoNothing) ||
                syncItems.Any(x => x.ForceUpdateState))
            {
                var getSourceItemStateTask = new GetStateOfSourceItemTask(Log, Events, _model.LocalSourceItem, new List<string>(), _synchronizationServices.CommonServices);
                tasks.Add(getSourceItemStateTask);

                tasks.Add(new SynchronizationLocalStateSaveTask(tasks.ToArray(), _synchronizationServices, Events, () => GetLocalState(GetActualFiles(getSourceItemStateTask))));
                if (_model.TaskOptions.SynchronizationMode == SynchronizationTaskModelMode.TwoWay)
                {
                    tasks.Add(new SynchronizationRemoteStateSaveTask(tasks.ToArray(), _synchronizationServices, Events, () => GetActualFiles(getSourceItemStateTask)));
                }
            }

            Events.DuringExecutionTasksAdded(Id, tasks);
            Children = tasks;
            base.Execute();
        }

        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        Events.OnMessage -= OnAddLastMinuteLogMessage;
        PutLastMinuteLogMessages();

        _synchronizationServices.Dispose();
    }

    private SynchronizationState GetActualFiles(GetStateOfSourceItemTask task)
    {
        if (!task.IsSuccess)
        {
            throw new InvalidOperationException("Source item state population has failed!");
        }

        var state = task.SourceItemState;
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

    private SynchronizationState GetLocalState(SynchronizationState actualFilesState)
    {
        var synced = _synchronizationServices.DecisionService.Decide(_model.TaskOptions.SynchronizationMode, _model.LocalState, actualFilesState, _model.RemoteState);

        var syncedState = new SynchronizationState
        {
            FileSystemEntries = synced
                .Where(x => x.ActualFileAction == SynchronizationDecision.DoNothing &&
                    x.RemoteAction == SynchronizationDecision.DoNothing &&
                    x.RemoteState != null && // because we cannot store in local state non-known to repository items
                    x.ExistsLocally)
                .Select(x => x.ActualFile!)
                .ToList()
        };

        return syncedState;
    }

    private void ExecuteActionsRemotely(List<BuTask> tasks, IEnumerable<SynchronizationConsolidatedFileInfo> syncItems)
    {
        foreach (var item in syncItems)
        {
            switch (item.RemoteAction)
            {
                case SynchronizationDecision.DoNothing:
                    break;
                case SynchronizationDecision.Delete:
                    tasks.Add(new SynchronizationRemoteFileDeleteTask(_synchronizationServices, Events, item.RelativeFileName));
                    break;
                case SynchronizationDecision.Update:
                    tasks.Add(new SynchronizationRemoteFileUpdateTask(_synchronizationServices, Events, _model.TaskOptions.LocalFolder, item.RelativeFileName));
                    break;

            }
        }
    }

    private void ExecuteActionsLocally(List<BuTask> tasks, IEnumerable<SynchronizationConsolidatedFileInfo> syncItems)
    {
        foreach (var item in syncItems)
        {
            switch (item.ActualFileAction)
            {
                case SynchronizationDecision.DoNothing:
                    break;
                case SynchronizationDecision.Delete:
                    tasks.Add(new SynchronizationLocalFileDeleteTask(_synchronizationServices, Events, _model.TaskOptions.LocalFolder, item.RelativeFileName));
                    break;
                case SynchronizationDecision.Update:
                    tasks.Add(new SynchronizationRemoteFileDownloadTask(_synchronizationServices, Events, _model, item.RelativeFileName));
                    break;
            }
        }
    }

    // TODO: think of partial upload.
    private void UploadFirstRemoteVersion()
    {
        LogDebug("Upload first remote version");
        var synchronizationFileUploadTasks = new List<SynchronizationRemoteFileUpdateTask>();
        
        foreach (var item in _model.ActualFiles.FileSystemEntries)
        {
            synchronizationFileUploadTasks.Add(new SynchronizationRemoteFileUpdateTask(_synchronizationServices, Events, _model.TaskOptions.LocalFolder, item.RelativeFileName));
        }

        var tasks = new List<BuTask>();
        tasks.AddRange(synchronizationFileUploadTasks);
        tasks.Add(new SynchronizationLocalStateSaveTask(synchronizationFileUploadTasks.ToArray(), _synchronizationServices, Events, () => _model.ActualFiles));
        tasks.Add(new SynchronizationRemoteStateSaveTask(synchronizationFileUploadTasks.ToArray(), _synchronizationServices, Events, () => _model.ActualFiles));

        Events.DuringExecutionTasksAdded(Id, tasks);

        Children = tasks;
        base.Execute();
    }

    private void PutLastMinuteLogMessages()
    {
        foreach (var lastMinuteLogMessage in _lastMinuteLogMessages)
            Log.WriteLine(LoggingEvent.Debug, lastMinuteLogMessage);
    }

    private readonly List<string> _lastMinuteLogMessages = new();
    private void OnAddLastMinuteLogMessage(object? sender, MessageEventArgs e)
    {
        _lastMinuteLogMessages.Add(e.Message);
    }
}
