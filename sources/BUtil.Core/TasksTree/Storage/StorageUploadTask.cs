using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
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
    private readonly Quota _quota;
    private readonly DateTime _versionUtc;

    public StorageUploadTask(
        StorageSpecificServicesIoc services,
        TaskEvents events,
        string password,
        StorageUploadTaskOptions options) : base(services.CommonServices.Log, events, Resources.DataStorage_Data_Saving)
    {
        _services = services;
        _password = password;
        _options = options;
        var gb = 1024 * 1024 * 1024;
        _quota = new Quota(services.StorageSettings.SingleBackupQuotaGb * gb);
        _versionUtc = DateTime.UtcNow;
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);

        var tasks = new List<BuTask>();
        Children = tasks;

        var storageToWriteTasks = CreateWriteSourceFileToStorageTasks(tasks);

        CreateSaveStateTasks(storageToWriteTasks, tasks);

        Events.DuringExecutionTasksAdded(Id, tasks);
        base.Execute();

        var skippedBecauseOfQuotaFiles = storageToWriteTasks
            .SelectMany(x => x.Value)
            .Where(x => x.IsSkippedBecauseOfQuota)
            .SelectMany(x => x.StorageFiles)
            .ToList();
        if (skippedBecauseOfQuotaFiles.Count != 0)
        {
            var gigabyte = 1024 * 1024 * 1024;
            _services.CommonServices.LastMinuteMessageService.AddLastMinuteLogMessage(string.Format(BUtil.Core.Localization.Resources.Task_Status_PartialDueToQuota, skippedBecauseOfQuotaFiles.Count, skippedBecauseOfQuotaFiles.Sum(x => x.FileState.Size) / gigabyte));

        }
        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
    }

    private Dictionary<SourceItemV2, List<WriteSourceFileToStorageTask>> CreateWriteSourceFileToStorageTasks(List<BuTask> tasks)
    {
        var storageToWriteTasks = new Dictionary<SourceItemV2, List<WriteSourceFileToStorageTask>>();

        foreach (var change in _options.Changes)
        {
            var writeTasks = change.CreatedUpdatedFiles
                .Select(x => new StorageFile(x, StorageMethodNames.BrotliCompressedAes256Encrypted, SourceItemHelper.GetCompressedStorageRelativeFileName(_versionUtc)))
                .GroupBy(x => x.FileState.ToDeduplicationString())
                .Select(x => new WriteSourceFileToStorageTask(_services, Events,
                    PatchRemoteFileNames([.. x], change), _quota, change.SourceItem,
                    _options.State.VersionStates, x.ToList().First().FileState.FileName, null, false))
                .ToList();

            storageToWriteTasks.Add(change.SourceItem, writeTasks);

            tasks.AddRange(writeTasks);
        }

        return storageToWriteTasks;
    }

    private static List<StorageFile> PatchRemoteFileNames(IEnumerable<StorageFile> storageFiles, StorageUploadTaskSourceItemChange change)
    {
        return storageFiles
            .Select(x =>
            {
                var storageFile = new StorageFile(x);
                storageFile.FileState.FileName = change.ActualFileToRemoteFileConverter(x.FileState.FileName);
                return storageFile;
            })
            .ToList();
    }

    private void CreateSaveStateTasks(Dictionary<SourceItemV2, List<WriteSourceFileToStorageTask>> storageToWriteTasks, List<BuTask> tasks)
    {
        var isVersionNeeded = new Lazy<bool>(() => IncrementState(storageToWriteTasks));

        var saveStateToStorageTask = new SaveStateToStorageTask(_services, Events, () => isVersionNeeded.Value ? _options.State : null, _password);

        tasks.Add(saveStateToStorageTask);
        tasks.Add(new WriteIntegrityVerificationScriptsToStorageTask(_services, Events, () => isVersionNeeded.Value, () => _options.State, new CompletedTask(Log, Events, true), saveStateToStorageTask, () => saveStateToStorageTask.StateFile!));
    }

    private bool IncrementState(Dictionary<SourceItemV2, List<WriteSourceFileToStorageTask>> storageToWriteTasks)
    {
        var sourceItemChanges = new List<SourceItemChanges>();
        var versionState = new VersionState(_versionUtc, sourceItemChanges);

        bool versionIsNeeded = false;
        foreach (var change in _options.Changes)
        {
            var deletedFiles = new List<string>();
            var updatedFiles = new List<StorageFile>();
            var createdFiles = new List<StorageFile>();
            var sourceItemChangesItem = new SourceItemChanges(change.SourceItem, deletedFiles, updatedFiles, createdFiles);
            sourceItemChanges.Add(sourceItemChangesItem);

            var state = GetOrCreateSourceItemState(change);

            RegisterDeletedFiles(change, deletedFiles, state);
            versionIsNeeded = versionIsNeeded || change.DeletedFiles.Any();

            // register updated file
            if (storageToWriteTasks.Any(x => x.Key.Id == change.SourceItem.Id))
            {
                var completedTasks = storageToWriteTasks
                    .Single(x => x.Key.Id == change.SourceItem.Id).Value
                    .Where(x => x.IsSuccess && !x.IsSkipped && !x.IsSkippedBecauseOfQuota)
                    .ToList();

                versionIsNeeded = versionIsNeeded || completedTasks.Count != 0;
                foreach (var completedTask in completedTasks)
                {
                    RegisterFileUpload(updatedFiles, createdFiles, state, completedTask);
                }
            }
        }

        if (versionIsNeeded)
        {
            _options.State.VersionStates.Add(versionState);
        }

        return versionIsNeeded;
    }

    private static void RegisterFileUpload(List<StorageFile> updatedFiles, List<StorageFile> createdFiles, SourceItemState state, WriteSourceFileToStorageTask createdUpdateTask)
    {
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

    private static void RegisterDeletedFiles(StorageUploadTaskSourceItemChange change, List<string> deletedFiles, SourceItemState state)
    {
        foreach (var deletedFile in change.DeletedFiles)
        {
            var actualFile = change.ActualFileToRemoteFileConverter(deletedFile);
            var itemToDelete = state.FileStates.Single(x => x.FileName == actualFile);
            state.FileStates.Remove(itemToDelete);
            deletedFiles.Add(actualFile);
        }
    }

    private SourceItemState GetOrCreateSourceItemState(StorageUploadTaskSourceItemChange change)
    {
        var state = _options.State.LastSourceItemStates.SingleOrDefault(x => x.SourceItem.Id == change.SourceItem.Id);
        if (state == null)
        {
            state = new SourceItemState(change.SourceItem, []);
            _options.State.LastSourceItemStates.Add(state);
        }

        return state;
    }
}
