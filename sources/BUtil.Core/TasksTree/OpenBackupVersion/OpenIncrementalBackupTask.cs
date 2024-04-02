using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.TasksTree;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using System.Collections.Generic;
using System.Linq;

namespace butil_ui.UiProgressTasks;

public class OpenIncrementalBackupTask : SequentialBuTask
{
    private readonly CommonServicesIoc _commonServicesIoc = new();
    private readonly StorageSpecificServicesIoc _storageSpecificServicesIoc;
    private readonly GetStateOfStorageTask _getStateOfStorageTask;

    public OpenIncrementalBackupTask(ILog log, TaskEvents events, IStorageSettingsV2 storageSettings, string password) 
        : base(log, events, "Open incremental copy")
    {
        _storageSpecificServicesIoc = new StorageSpecificServicesIoc(log, storageSettings, _commonServicesIoc.HashService);
        _getStateOfStorageTask = new(_storageSpecificServicesIoc, events, password);
        Children = new List<BuTask> { _getStateOfStorageTask };
    }

    public override void Execute()
    {
        Events.OnMessage += OnAddLastMinuteLogMessage;
        UpdateStatus(ProcessingStatus.InProgress);

        base.Execute();

        if (IsSuccess)
        {
            if (!_getStateOfStorageTask.StorageState!.VersionStates.Any())
            {
                Events.Message(string.Format(Resources.RestoreFrom_Field_Validation_NoStateFiles, IncrementalBackupModelConstants.StorageIncrementalEncryptedCompressedStateFile));
                IsSuccess = false;
            }
        }

        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        Events.OnMessage -= OnAddLastMinuteLogMessage;
        PutLastMinuteLogMessages();
        LastMinuteMessage = string.Join(". ", _lastMinuteLogMessages);
        _commonServicesIoc.Dispose();
        _storageSpecificServicesIoc.Dispose();
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

    public string? LastMinuteMessage { get; private set; }
    public IncrementalBackupState? StorageState => _getStateOfStorageTask.StorageState;
}
