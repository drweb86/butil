using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Services;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.States;
using System;

namespace BUtil.Core.TasksTree.IncrementalModel;

class IncrementalBackupRootTask : SequentialBuTask
{
    private readonly CommonServicesIoc _commonServicesIoc;
    private readonly StorageSpecificServicesIoc _storageService;

    public IncrementalBackupRootTask(ILog log, TaskEvents backupEvents, TaskV2 backupTask, Action<string?> onGetLastMinuteMessage)
        : base(log, backupEvents, Resources.IncrementalBackup_Title, null)
    {
        if (backupTask.Model is not IncrementalBackupModelOptionsV2 modelOptions)
            throw new ArgumentException("Task model should be IncrementalBackupModelOptionsV2.", nameof(backupTask));
        _commonServicesIoc = new CommonServicesIoc(log, onGetLastMinuteMessage);

        _storageService = new StorageSpecificServicesIoc(_commonServicesIoc, modelOptions.To);

        var readStatesTask = new GetStateOfSourceItemsAndStoragesTask(Events, modelOptions.Items, _commonServicesIoc, _storageService, modelOptions.FileExcludePatterns, modelOptions.Password);
        Children =
        [
            readStatesTask,
            new WriteIncrementedVersionTask(_storageService, Events, readStatesTask.RemoteStateLoadTask.GetSuccessResult, readStatesTask.GetSourceItemStateTasks, modelOptions)
        ];
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
            DisposeWithLogging(_storageService.Dispose, nameof(_storageService));
            DisposeWithLogging(_commonServicesIoc.Dispose, nameof(_commonServicesIoc));
            UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        }
    }

    private void DisposeWithLogging(Action disposeAction, string serviceName)
    {
        try
        {
            disposeAction();
        }
        catch (Exception ex)
        {
            IsSuccess = false;
            LogError($"Disposal failed for {serviceName}. {ex}");
        }
    }
}
