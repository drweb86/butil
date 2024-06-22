using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.FileSystem;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.Synchronization;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using BUtil.Core.TasksTree.States;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.TasksTree.Storage;

internal class StorageUploadTask : SequentialBuTask
{
    private readonly StorageSpecificServicesIoc _services;
    private readonly string _password;
    private readonly StorageUploadTaskOptions _options;

    public StorageUploadTask(
        StorageSpecificServicesIoc services,
        TaskEvents events,
        string password,
        StorageUploadTaskOptions options) : base(services.Log, events, "Sending data to storage")
    {
        _services = services;
        _password = password;
        _options = options;
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);

        var gb = 1024 * 1024 * 1024;
        var quotaGb = new Quota(_services.StorageSettings.SingleBackupQuotaGb * gb);
        var versionUtc = DateTime.UtcNow;

        var storageToWriteTasks = new Dictionary<SourceItemV2, List<WriteSourceFileToStorageTask>>();

        foreach (var change in _options.Changes)
        {
            var writeTasks = change.CreatedUpdatedFiles
                .Select(x =>
                {
                    var storageFile = new StorageFile(x);
                    storageFile.StorageRelativeFileName = SourceItemHelper.GetCompressedStorageRelativeFileName(versionUtc);
                    storageFile.StorageMethod = StorageMethodNames.SevenZipEncrypted;
                    return storageFile;
                })
                .GroupBy(x => x.FileState.ToDeduplicationString())
                .Select(x => new WriteSourceFileToStorageTask(
                    _services,
                    Events,
                    x
                        .ToList()
                        .Select(x =>
                        {
                            var storageFile = new StorageFile(x);
                            storageFile.FileState.FileName = change.ActualFileToRemoteFileConverter(x.FileState.FileName);
                            return storageFile;
                        })
                        .ToList(),
                    quotaGb,
                    change.SourceItem,
                    _options.PreviousState.VersionStates,
                    x.ToList().First().FileState.FileName))
                .ToList();

            storageToWriteTasks.Add(change.SourceItem, writeTasks);
        }

        var tasks = new List<BuTask>();
        Children = tasks;

        ActualUpload(storageToWriteTasks, tasks);

        SaveState(storageToWriteTasks, tasks, versionUtc);

        Events.DuringExecutionTasksAdded(Id, tasks);
        base.Execute();

        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
    }

    private void SaveState(Dictionary<SourceItemV2, List<WriteSourceFileToStorageTask>> storageToWriteTasks, List<BuTask> tasks, DateTime versionUtc)
    {
        Func<IncrementalBackupState?> getState = () => GetIncrementedBackupState(storageToWriteTasks, _options.PreviousState, versionUtc);
        Func<bool> isVersionNeeded = () => true;

        var saveStateToStorageTask = new SaveStateToStorageTask(_services, Events, getState, _password);

        tasks.Add(saveStateToStorageTask);
        tasks.Add(new WriteIntegrityVerificationScriptsToStorageTask(_services, Events, isVersionNeeded, getState, new CompletedTask(Log, Events, true), saveStateToStorageTask, () => saveStateToStorageTask.StateFile!));
    }

    private IncrementalBackupState? GetIncrementedBackupState(
        Dictionary<SourceItemV2, List<WriteSourceFileToStorageTask>> storageToWriteTasks,
        IncrementalBackupState previousState,
        DateTime versionUtc)
    {
        var sourceItemChanges = new List<SourceItemChanges>();
        var versionState = new VersionState(versionUtc, sourceItemChanges);
        previousState.VersionStates.Add(versionState);

        bool versionIsNeeded = false;
        foreach (var change in _options.Changes)
        {
            var deletedFiles = new List<string>();
            var updatedFiles = new List<StorageFile>();
            var createdFiles = new List<StorageFile>();
            var sourceItemChangesItem = new SourceItemChanges(change.SourceItem, deletedFiles, updatedFiles, createdFiles);
            sourceItemChanges.Add(sourceItemChangesItem);


            // state.
            var state = previousState.LastSourceItemStates.SingleOrDefault(x => x.SourceItem.Id == change.SourceItem.Id);
            if (state == null)
            {
                state = new SourceItemState(change.SourceItem, new List<FileState>());
                previousState.LastSourceItemStates.Add(state);
                versionIsNeeded = true;
            }

            // register deleted files
            foreach (var deletedFile in change.DeletedFiles)
            {
                var actualFile = change.ActualFileToRemoteFileConverter(deletedFile);
                var itemToDelete = state.FileStates.Single(x => x.FileName == actualFile);
                state.FileStates.Remove(itemToDelete);
                deletedFiles.Add(actualFile);
                versionIsNeeded = true;
            }

            // register updated file
            if (storageToWriteTasks.Any(x => x.Key.Id == change.SourceItem.Id))
            {
                var createUpdateTasks = storageToWriteTasks.Single(x => x.Key.Id == change.SourceItem.Id).Value;
                foreach (var createdUpdateTask in createUpdateTasks)
                {
                    if (!createdUpdateTask.IsSuccess ||
                        !createdUpdateTask.IsSkipped ||
                        !createdUpdateTask.IsSkippedBecauseOfQuota)
                        continue;

                    versionIsNeeded = true;
                    
                    foreach (var storageFile in createdUpdateTask.StorageFiles)
                    {
                        var existingItem = state.FileStates.SingleOrDefault(x => x.FileName == storageFile.FileState.FileName);
                        if (existingItem != null)
                        {
                            state.FileStates.Remove(existingItem);
                            state.FileStates.Add(storageFile.FileState);
                            updatedFiles.Add(storageFile);
                        }
                        else
                        {
                            state.FileStates.Add(storageFile.FileState);
                            createdFiles.Add(storageFile);
                        }
                    }
                }
            }
        }

        if (versionIsNeeded)
        {
            return previousState;
        }
        return null;
    }

    private void ActualUpload(Dictionary<SourceItemV2, List<WriteSourceFileToStorageTask>> storageToWriteTasks, List<BuTask> tasks)
    {
        foreach (var tasksPair in storageToWriteTasks)
        {
            tasks.AddRange(tasksPair.Value);
        }
        Events.DuringExecutionTasksAdded(Id, tasks);
        base.Execute();
    }
}
