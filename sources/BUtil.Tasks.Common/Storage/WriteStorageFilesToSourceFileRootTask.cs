using BUtil.Interop.Tasks;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Interop.Tasks.Events;
using BUtil.Core.Localization;
using BUtil.Interop.Logs;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.Interop.Tasks.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Tasks.Common.Storage;

public class WriteStorageFilesToSourceFileRootTask : SequentialBuTask
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

        Children = [.. storageFiles.Select(x => new WriteStorageFileToSourceFileTask(_storageSpecificServicesIoc, events, sourceItem, x, destinationFolder))];
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);
        try
        {
            base.Execute();
        }
        finally
        {
            try
            {
                UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
            }
            finally
            {
                _storageSpecificServicesIoc.Dispose();
                _commonServicesIoc.Dispose();
            }
        }
    }
}
