using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.TasksTree.Storage;

public class WriteStorageFilesToSourceFileRootTask : ParallelBuTask
{
    private readonly CommonServicesIoc _commonServicesIoc;
    private readonly StorageSpecificServicesIoc _storageSpecificServicesIoc;

    public WriteStorageFilesToSourceFileRootTask(ILog log, TaskEvents events,
        IStorageSettingsV2 storageSettings,
        SourceItemV2 sourceItem,
        IEnumerable<StorageFile> storageFiles,
        string destinationFolder,
        Action<string?> onGetLastMinuteMessage)
        : base(log, events, Resources.Task_Restore)
    {
        _commonServicesIoc = new CommonServicesIoc(log, onGetLastMinuteMessage);
        _storageSpecificServicesIoc = new StorageSpecificServicesIoc(_commonServicesIoc, storageSettings, true);

        Children = storageFiles
            .Select(x => new WriteStorageFileToSourceFileTask(_storageSpecificServicesIoc, events, sourceItem, x, destinationFolder))
            .ToArray();
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);
        base.Execute();
        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        _commonServicesIoc.Dispose();
        _storageSpecificServicesIoc.Dispose();
    }
}
