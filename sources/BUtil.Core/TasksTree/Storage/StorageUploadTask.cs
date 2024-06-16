using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.FileSystem;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.Synchronization;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BUtil.Core.TasksTree.Storage;

internal class StorageUploadTask : SequentialBuTask
{
    private readonly StorageSpecificServicesIoc _services;
    private readonly StorageUploadTaskOptions _options;

    public StorageUploadTask(
        StorageSpecificServicesIoc services,
        TaskEvents events,
        StorageUploadTaskOptions options) : base(services.Log, events, "Sending data to storage")
    {
        _services = services;
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

        // Patch state

        // Save state

        // Save scripts

        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
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
