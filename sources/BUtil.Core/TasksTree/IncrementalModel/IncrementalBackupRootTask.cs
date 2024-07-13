﻿using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Services;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.States;
using System.Collections.Generic;

namespace BUtil.Core.TasksTree.IncrementalModel;

class IncrementalBackupRootTask : SequentialBuTask
{
    private readonly CommonServicesIoc _commonServicesIoc;
    private readonly StorageSpecificServicesIoc _storageService;

    public IncrementalBackupRootTask(ILog log, TaskEvents backupEvents, TaskV2 backupTask)
        : base(log, backupEvents, Resources.IncrementalBackup_Title, null)
    {
        var tasks = new List<BuTask>();

        _commonServicesIoc = new CommonServicesIoc(log);
        var modelOptions = (IncrementalBackupModelOptionsV2)backupTask.Model;
        var storage = modelOptions.To;

        _storageService = new StorageSpecificServicesIoc(_commonServicesIoc, storage);

        var readSatesTask = new GetStateOfSourceItemsAndStoragesTask(Events, modelOptions.Items, _commonServicesIoc, _storageService, modelOptions.FileExcludePatterns, ((IncrementalBackupModelOptionsV2)backupTask.Model).Password);
        tasks.Add(readSatesTask);

        tasks.Add(new WriteIncrementedVersionTask(_storageService, Events, readSatesTask.RemoteStateLoadTask, readSatesTask.GetSourceItemStateTasks, (IncrementalBackupModelOptionsV2)backupTask.Model));

        Children = tasks;
    }

    public override void Execute()
    {
        Events.OnMessage += OnAddLastMinuteLogMessage;
        UpdateStatus(ProcessingStatus.InProgress);
        base.Execute();

        _storageService.Dispose();
        _commonServicesIoc.Dispose();

        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        Events.OnMessage -= OnAddLastMinuteLogMessage;
        PutLastMinuteLogMessages();
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
