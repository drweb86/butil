using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.State;
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
        var itemsWithRemoteAction = syncItems
            .Where(x => x.RemoteAction != SynchronizationDecision.DoNothing)
            .ToList();

        var deleteItems = itemsWithRemoteAction
            .Where(x => x.RemoteAction == SynchronizationDecision.Delete)
            .ToList();
        var updateCreateItems = itemsWithRemoteAction
            .Where(x => x.RemoteAction == SynchronizationDecision.Update)
            .ToList();

        var gb = 1024 * 1024 * 1024;
        var quotaGb = new Quota(_model.TaskOptions.To.SingleBackupQuotaGb * gb);
        var updateCreateItemsActual = updateCreateItems
            .Where(x => quotaGb.TryQuota(x.ActualFile!.Size))
            .ToList();

        // we ignore quota if we have nothing to work with.
        if (updateCreateItemsActual.Count == 0 && updateCreateItems.Any())
        {
            updateCreateItemsActual.Add(updateCreateItems[0]);
        }

        if (updateCreateItemsActual.Count == 0 && deleteItems.Count == 0)
        {
            LogDebug("Remote version changes are not needed!");
            return;
        }

        if (updateCreateItemsActual.Count != updateCreateItems.Count)
        {
            var skippedFiles = updateCreateItems.Except(updateCreateItemsActual).ToList();
            Events.Message(string.Format(BUtil.Core.Localization.Resources.Task_Status_PartialDueToQuota, skippedFiles.Count, skippedFiles.Sum(x => x.ActualFile!.Size) / gb));
            LogDebug("Some files will be skipped because of quota:");
            skippedFiles.ForEach(x => LogDebug(x.ActualFile!.RelativeFileName));
        }

        //var sourceItemChanges = new SourceItemChanges()
        //var versionChanges = new List<SourceItemChanges>() { new So }
        //var version = new VersionState(DateTime.UtcNow, )
        //foreach (var createUpdateItem in )
        // 2. Any
        // формируем новую версию с учетом удаленных файлов и квоты
        // формируем новое последнее состояние
        // запускаем задачи
        // запускаем сохранения состояния с учетом частичных путей и успеха части либо целого по аплоаду файлов.
        // 4.

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
