using BUtil.Core.Events;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.TasksTree.Storage;

internal class WriteSourceFilesToStorageTask(
    StorageSpecificServicesIoc services,
    TaskEvents events,
    CalculateIncrementedVersionForStorageTask getIncrementedVersionTask) : ParallelBuTask(services.CommonServices.Log, events, Localization.Resources.File_List_Saving)
{
    private readonly StorageSpecificServicesIoc _services = services;
    private readonly CalculateIncrementedVersionForStorageTask _getIncrementedVersionTask = getIncrementedVersionTask;

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);

        if (!_getIncrementedVersionTask.VersionIsNeeded)
        {
            LogDebug("Version is not needed.");
            IsSuccess = true;
            Children = [];
            UpdateStatus(ProcessingStatus.FinishedSuccesfully);
            return;
        }

        List<WriteSourceFileToStorageTask> WriteFileTasks = [];
        var versionStates = (_getIncrementedVersionTask.IncrementalBackupState ?? throw new Exception()).VersionStates;
        var versionState = versionStates.Last();
        var singleBackupQuotaGb = new Quota(_services.StorageSettings.SingleBackupQuotaGb * 1024 * 1024 * 1024);
        foreach (var sourceItemChange in versionState.SourceItemChanges)
        {
            var itemsToCopy = new List<StorageFile>();
            itemsToCopy.AddRange(sourceItemChange.UpdatedFiles);
            itemsToCopy.AddRange(sourceItemChange.CreatedFiles);

            var sourceItemDir = SourceItemHelper.GetSourceItemDirectory(sourceItemChange.SourceItem);

            var copyTasks = itemsToCopy
                .Select(x =>
                {
                    var sourceItemRelativeFileName = SourceItemHelper.GetSourceItemRelativeFileName(sourceItemDir, x.FileState);
                    string storageRelativeFileName = GetStorageRelativeFileName(versionState);

                    x.StorageRelativeFileName = storageRelativeFileName;
                    x.StorageMethod = StorageMethodNames.SevenZipEncrypted;
                    return x;
                })
                .GroupBy(x => x.FileState.ToDeduplicationString())
                .Select(x => new WriteSourceFileToStorageTask(
                    _services,
                    Events,
                    [.. x],
                    singleBackupQuotaGb,
                    sourceItemChange.SourceItem,
                    versionStates,
                    x.ToList().First().FileState.FileName,
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

        foreach (var sourceItemChange in versionState.SourceItemChanges)
        {
            sourceItemChange.UpdatedFiles.RemoveAll(x => skippedFiles.Contains(x.FileState.FileName));
            sourceItemChange.CreatedFiles.RemoveAll(x => skippedFiles.Contains(x.FileState.FileName));
        }

        foreach (var lastSourceItemState in _getIncrementedVersionTask.IncrementalBackupState.LastSourceItemStates)
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
            _services.CommonServices.LastMinuteMessageService.AddLastMinuteLogMessage(string.Format(BUtil.Core.Localization.Resources.Task_Status_PartialDueToQuota, skippedBecauseOfQuotaFiles.Count, skippedBecauseOfQuotaFiles.Sum(x => x.FileState.Size) / gigabyte));
        }
    }

    private static string GetStorageRelativeFileName(VersionState versionState)
    {
        return SourceItemHelper.GetCompressedStorageRelativeFileName(versionState);
    }
}
