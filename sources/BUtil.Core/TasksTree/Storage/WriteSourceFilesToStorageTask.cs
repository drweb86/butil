using BUtil.Core.Events;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.TasksTree.Storage;

internal class WriteSourceFilesToStorageTask(
    StorageSpecificServicesIoc services,
    TaskEvents events,
    Func<(bool versionIsNeeded, IncrementalBackupState updatedState)> getUpdatedState) : ParallelBuTask(services.CommonServices.Log, events, Localization.Resources.File_List_Saving)
{
    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);

        var (versionIsNeeded, state) = getUpdatedState();

        if (!versionIsNeeded)
        {
            LogDebug("Version is not needed.");
            IsSuccess = true;
            Children = [];
            UpdateStatus(ProcessingStatus.FinishedSuccesfully);
            return;
        }

        List<WriteSourceFileToStorageTask> WriteFileTasks = [];
        var versions = state.VersionStates;
        var newVersion = versions.Last();
        var singleBackupQuotaGb = new Quota(services.StorageSettings.SingleBackupQuotaGb * 1024 * 1024 * 1024);
        foreach (var sourceItemChange in newVersion.SourceItemChanges)
        {
            var itemsToCopy = new List<StorageFile>();
            itemsToCopy.AddRange(sourceItemChange.UpdatedFiles);
            itemsToCopy.AddRange(sourceItemChange.CreatedFiles);

            var sourceItemDir = SourceItemHelper.GetSourceItemDirectory(sourceItemChange.SourceItem);

            var copyTasks = itemsToCopy
                .Select(x =>
                {
                    x.StorageRelativeFileName = SourceItemHelper.GetCompressedStorageRelativeFileName(newVersion);
                    x.StorageMethod = StorageMethodNames.Aes256Encrypted;
                    return x;
                })
                .GroupBy(x => x.FileState.ToDeduplicationString())
                .Select(x => new WriteSourceFileToStorageTask(
                    services,
                    Events,
                    [.. x],
                    singleBackupQuotaGb,
                    sourceItemChange.SourceItem,
                    versions,
                    x.ToList().First().FileState.FileName,
                    null,
                    true))
                .ToList();
            WriteFileTasks.AddRange(copyTasks);
        }
        Events.DuringExecutionTasksAdded(Id, WriteFileTasks);
        Children = WriteFileTasks;
        base.Execute();

        // TODO: think of design.

        var skippedFiles = WriteFileTasks
            .Where(x => x.IsSkipped || !x.IsSuccess)
            .SelectMany(x => x.StorageFiles)
            .Select(x => x.FileState.FileName)
            .ToList();

        foreach (var sourceItemChange in newVersion.SourceItemChanges)
        {
            sourceItemChange.UpdatedFiles.RemoveAll(x => skippedFiles.Contains(x.FileState.FileName));
            sourceItemChange.CreatedFiles.RemoveAll(x => skippedFiles.Contains(x.FileState.FileName));
        }

        foreach (var lastSourceItemState in state.LastSourceItemStates)
        {
            var updatedArray = lastSourceItemState.FileStates.Where(x => !skippedFiles.Contains(x.FileName)).ToList();
            lastSourceItemState.FileStates = updatedArray;
        }

        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);

        var skippedBecauseOfQuotaFiles = WriteFileTasks
            .Where(x => x.IsSkippedBecauseOfQuota)
            .SelectMany(x => x.StorageFiles)
            .ToList();
        if (skippedBecauseOfQuotaFiles.Count != 0)
        {
            var gigabyte = 1024 * 1024 * 1024;
            services.CommonServices.LastMinuteMessageService.AddLastMinuteLogMessage(string.Format(BUtil.Core.Localization.Resources.Task_Status_PartialDueToQuota, skippedBecauseOfQuotaFiles.Count, skippedBecauseOfQuotaFiles.Sum(x => x.FileState.Size) / gigabyte));
        }
    }
}
