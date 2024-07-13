
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Services;
using BUtil.Core.TasksTree.Core;
using System;
using System.Collections.Generic;

namespace BUtil.Core.TasksTree.MediaSyncBackupModel;

class ImportMediaRootTask : SequentialBuTask
{
    private readonly CommonServicesIoc _commonServicesIoc = new();

    public ImportMediaRootTask(ILog log, TaskEvents backupEvents, TaskV2 backupTask)
        : base(log, backupEvents, string.Empty)
    {
        var typedModel = (ImportMediaTaskModelOptionsV2)backupTask.Model;
        var sourceItem = new SourceItemV2(typedModel.DestinationFolder, true);

        var getStateOfSourceItemTask = new GetStateOfSourceItemTask(log, backupEvents, sourceItem, Array.Empty<string>(), _commonServicesIoc);
        var importFiles = new ImportFilesTask(log, backupEvents, backupTask, getStateOfSourceItemTask, _commonServicesIoc);

        Children = [getStateOfSourceItemTask, importFiles];
    }

    public override void Execute()
    {
        Events.OnMessage += OnAddLastMinuteLogMessage;
        UpdateStatus(ProcessingStatus.InProgress);

        base.Execute();

        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        Events.OnMessage -= OnAddLastMinuteLogMessage;
        PutLastMinuteLogMessages();
        _commonServicesIoc.Dispose();
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
