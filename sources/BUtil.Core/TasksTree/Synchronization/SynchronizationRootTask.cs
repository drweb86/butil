using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.Synchronization;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.Storage;
using BUtil.Core.TasksTree.Synchronization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace BUtil.Core.TasksTree.IncrementalModel;

class SynchronizationRootTask : SequentialBuTask
{
    private readonly SynchronizationServices _synchronizationServices;
    private readonly SynchronizationModel _model;
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { WriteIndented = true };

    private readonly SynchronizationAllStatesReadTask _synchronizationAllStatesReadTask;

    public SynchronizationRootTask(ILog log, TaskEvents backupEvents, TaskV2 task, Action<string?> onGetLastMinuteMessage)
        : base(log, backupEvents, Resources.SynchronizationTask_Create, null)
    {
        var options = (SynchronizationTaskModelOptionsV2)task.Model;
        _model = new SynchronizationModel((SynchronizationTaskModelOptionsV2)task.Model);
        _synchronizationServices = new SynchronizationServices(log, task.Name, _model.TaskOptions.LocalFolder, FileHelper.NormalizeRelativePath(options.RepositorySubfolder), options.To, false, onGetLastMinuteMessage);

        _synchronizationAllStatesReadTask = new SynchronizationAllStatesReadTask(_synchronizationServices, Events, _model);

        Children = [_synchronizationAllStatesReadTask];
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);

        base.Execute();

        if (IsSuccess)
        {
            var syncItems = _synchronizationServices.DecisionService.Decide(_model.TaskOptions.SynchronizationMode, _model.LocalState, _model.ActualFiles, _model.RemoteState);
            LogDebug("Decisions");
            LogDebug(JsonSerializer.Serialize(syncItems, _jsonSerializerOptions));
            var tasks = new List<BuTask>();
            ExecuteActionsLocally(tasks, syncItems);
            ExecuteActionsRemotely(tasks, syncItems);


            // TODO: partial state.
            if (syncItems.Any(x => x.RemoteAction != SynchronizationDecision.DoNothing) ||
                syncItems.Any(x => x.ActualFileAction != SynchronizationDecision.DoNothing) ||
                syncItems.Any(x => x.ForceUpdateState))
            {
                var getSourceItemStateTask = new GetStateOfSourceItemTask(Events, _model.LocalSourceItem, [], _synchronizationServices.CommonServices);
                tasks.Add(getSourceItemStateTask);
                tasks.Add(new SynchronizationLocalStateSaveTask([.. tasks], _synchronizationServices, Events, () => GetLocalState(GetActualFiles(getSourceItemStateTask))));
            }

            Events.DuringExecutionTasksAdded(Id, tasks);
            Children = tasks;
            base.Execute();
        }

        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);

        _synchronizationServices.Dispose();
    }

    private static SynchronizationState GetActualFiles(GetStateOfSourceItemTask task)
    {
        if (!task.IsSuccess)
        {
            throw new InvalidOperationException("Source item state population has failed!");
        }

        var state = task.SourceItemState ?? throw new InvalidOperationException("Source item state is corrupted (null)!");
        return new SynchronizationState()
        {
            FileSystemEntries = state.FileStates
                .Select(x => new SynchronizationStateFile(state.SourceItem, x))
                .ToList(),
        };
    }

    private SynchronizationState GetRemoteState()
    {
        var state = _model.RemoteStorageState.LastSourceItemStates.Single(x => SynchronizationHelper.IsSynchronizationSourceItem(x.SourceItem));
        return new SynchronizationState()
        {
            FileSystemEntries = state.FileStates
                .Select(x => new SynchronizationStateFile(state.SourceItem, x))
                .ToList(),
        };
    }

    private SynchronizationState GetLocalState(SynchronizationState actualFilesState)
    {
        var remoteState = GetRemoteState();
        var synced = _synchronizationServices.DecisionService.Decide(_model.TaskOptions.SynchronizationMode, _model.LocalState, actualFilesState, remoteState);

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
        if (_model.TaskOptions.SynchronizationMode != SynchronizationTaskModelMode.TwoWay)
            return;


        var itemsWithRemoteAction = syncItems
            .Where(x => x.RemoteAction != SynchronizationDecision.DoNothing)
            .ToList();

        var deleteItems = itemsWithRemoteAction
            .Where(x => x.RemoteAction == SynchronizationDecision.Delete)
            .ToList();
        var updateCreateItems = itemsWithRemoteAction
            .Where(x => x.RemoteAction == SynchronizationDecision.Update)
            .ToList();

        var deletedFiles = new List<string>();
        foreach (var deletedFile in deleteItems)
        {
            var actualLocalFile = Path.Combine(_model.LocalSourceItem.Target, deletedFile.RelativeFileName);
            deletedFiles.Add(actualLocalFile);
        }

        var versionUtc = DateTime.UtcNow;
        var sourceItemDir = SourceItemHelper.GetSourceItemDirectory(_model.RemoteSourceItem);

        var actualRemoteSourceItem = _model.RemoteSourceItem ?? SynchronizationModel.CreateVirtualSourceItem();
        var storageUploadTaskOptions = new StorageUploadTaskOptions(
            _model.RemoteStorageState ?? new IncrementalBackupState(),
            [
                new StorageUploadTaskSourceItemChange
                (
                    actualRemoteSourceItem,
                    deletedFiles,
                    updateCreateItems
                        .Select(x => {
                            var actualFile = x.ActualFile!;
                            var fileStateFileName = FileHelper.Combine(_model.LocalSourceItem.Target, actualFile.RelativeFileName);
                            return new FileState(fileStateFileName, actualFile.ModifiedAtUtc, actualFile.Size, actualFile.Sha512);
                        })
                        .ToList(),
                    fileName => ConvertFileNameToStorageRelatedByRepositorySubfolderAndVirtualizedRemoteSourceItem(actualRemoteSourceItem, fileName)
                )
            ]
        );
        tasks.Add(new StorageUploadTask(_synchronizationServices.StorageSpecificServices, Events, _model.TaskOptions.Password, storageUploadTaskOptions));
    }

    private string ConvertFileNameToStorageRelatedByRepositorySubfolderAndVirtualizedRemoteSourceItem(SourceItemV2 actualRemoteSourceItem, string fileName)
    {
        var relativeFileName = FileHelper.GetRelativeFileName(_model.LocalSourceItem.Target, fileName);
        var sourceItemRelativeFileName = FileHelper.Combine(FileHelper.NormalizeRelativePath(_model.TaskOptions.RepositorySubfolder), relativeFileName);
        var sourceItemVirtualFilePath = FileHelper.Combine('\\', actualRemoteSourceItem.Target, sourceItemRelativeFileName);
        return sourceItemVirtualFilePath;
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
}
