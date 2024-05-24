using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Synchronization;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.Synchronization;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.TasksTree.IncrementalModel;

class SynchronizationRootTask : SequentialBuTask
{
    private readonly SynchronizationServices _synchronizationServices;
    private readonly SynchronizationAllStatesReadTask _synchronizationAllStatesReadTask;
    private readonly string _localFolder;

    private SynchronizationState? _localState;
    private SynchronizationState? _remoteState;
    private SynchronizationState _actualFiles = null!;

    public SynchronizationRootTask(ILog log, TaskEvents backupEvents, TaskV2 task)
        : base(log, backupEvents, Resources.IncrementalBackup_Title, null)
    {
        var options = (SynchronizationTaskModelOptionsV2)task.Model;
        _localFolder = options.LocalFolder;
        _synchronizationServices = new SynchronizationServices(log, task.Name, _localFolder, options.To, false);

        _synchronizationAllStatesReadTask = new SynchronizationAllStatesReadTask(_synchronizationServices, Events, _localFolder);

        Children = new List<BuTask> { _synchronizationAllStatesReadTask };
    }

    public override void Execute()
    {
        Events.OnMessage += OnAddLastMinuteLogMessage;
        UpdateStatus(ProcessingStatus.InProgress);

        base.Execute();

        if (IsSuccess)
        {
            _localState = _synchronizationAllStatesReadTask.LocalState;
            _remoteState = _synchronizationAllStatesReadTask.RemoteState;
            _actualFiles = _synchronizationAllStatesReadTask.ActualFiles;

            if (_remoteState == null)
            {
                UploadFirstRemoteVersion();
            }
            else
            {
                if (_localState == null)
                {
                    DownloadFirstVesion();
                }

                if (IsSuccess)
                {
                    NormalUpdate();
                }
            }
        }


        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        Events.OnMessage -= OnAddLastMinuteLogMessage;
        PutLastMinuteLogMessages();

        _synchronizationServices.Dispose();
    }

    private void NormalUpdate()
    {
        LogDebug("Normal update.");
        var syncService = new SynchronizationDecisionService();
        var syncItems = syncService.Decide(_localState!, _actualFiles, _remoteState!);
        
        var tasks = new List<BuTask>();
        ExecuteActionsLocally(tasks, syncItems);
        ExecuteActionsRemotely(tasks, syncItems);

        if (syncItems.Any(x => x.RemoteAction != SynchronizationDecision.DoNothing) ||
            syncItems.Any(x => x.ActualFileAction != SynchronizationDecision.DoNothing))
        {
            var loadActualFilesStateTask = new SynchronizationReadActualFilesTask(_synchronizationServices, Events, _localFolder);
            tasks.Add(loadActualFilesStateTask);

            tasks.Add(new SynchronizationLocalStateSaveTask(tasks.ToArray(), _synchronizationServices, Events, () => loadActualFilesStateTask.SynchronizationState!));
            tasks.Add(new SynchronizationRemoteStateSaveTask(tasks.ToArray(), _synchronizationServices, Events, () => loadActualFilesStateTask.SynchronizationState!));
        }

        Events.DuringExecutionTasksAdded(Id, tasks);
        Children = tasks;
        base.Execute();
    }

    private void ExecuteActionsRemotely(List<BuTask> tasks, IEnumerable<SynchronizationConsolidatedFileInfo> syncItems)
    {
        foreach (var item in syncItems)
        {
            switch (item.RemoteAction)
            {
                case SynchronizationDecision.DoNothing:
                    break;
                case SynchronizationDecision.Delete:
                    tasks.Add(new SynchronizationRemoteFileDeleteTask(_synchronizationServices, Events, item.RelativeFileName));
                    break;
                case SynchronizationDecision.Create:
                case SynchronizationDecision.Update:
                    tasks.Add(new SynchronizationRemoteFileUpdateTask(_synchronizationServices, Events, _localFolder, item.RelativeFileName));
                    break;

            }
        }
    }

    private void ExecuteActionsLocally(List<BuTask> tasks, IEnumerable<SynchronizationConsolidatedFileInfo> syncItems)
    {
        foreach (var item in syncItems)
        {
            switch (item.ActualFileAction)
            {
                case SynchronizationDecision.DoNothing:
                    break;
                case SynchronizationDecision.Delete:
                    tasks.Add(new SynchronizationLocalFileDeleteTask(_synchronizationServices, Events, _localFolder, item.RelativeFileName));
                    break;
                case SynchronizationDecision.Create:
                case SynchronizationDecision.Update:
                    tasks.Add(new SynchronizationRemoteFileDownloadTask(_synchronizationServices, Events, _localFolder, item.RelativeFileName));
                    break;
            }
        }
    }

    private void DownloadFirstVesion()
    {
        LogDebug("Download first version");
        var synchronizationFileDownloadTasks = new List<SynchronizationRemoteFileDownloadTask>();

        foreach (var item in _remoteState!.FileSystemEntries)
        {
            synchronizationFileDownloadTasks.Add(new SynchronizationRemoteFileDownloadTask(_synchronizationServices, Events, _localFolder, item.RelativeFileName));
        }

        var tasks = new List<BuTask>();
        tasks.AddRange(synchronizationFileDownloadTasks);
        tasks.Add(new SynchronizationLocalStateSaveTask(synchronizationFileDownloadTasks.ToArray(), _synchronizationServices, Events, () => _remoteState));

        Events.DuringExecutionTasksAdded(Id, tasks);
        Children = tasks;
        base.Execute();

        if (IsSuccess)
        {
            _localState = _remoteState.Clone();
        }
    }

    // TODO: think of format
    // TODO: think of compression
    // TODO: think of partial upload.
    private void UploadFirstRemoteVersion()
    {
        LogDebug("Upload first remote version");
        var synchronizationFileUploadTasks = new List<SynchronizationRemoteFileUpdateTask>();
        
        foreach (var item in _actualFiles.FileSystemEntries)
        {
            synchronizationFileUploadTasks.Add(new SynchronizationRemoteFileUpdateTask(_synchronizationServices, Events, _localFolder, item.RelativeFileName));
        }

        var tasks = new List<BuTask>();
        tasks.AddRange(synchronizationFileUploadTasks);
        tasks.Add(new SynchronizationLocalStateSaveTask(synchronizationFileUploadTasks.ToArray(), _synchronizationServices, Events, () => _actualFiles));
        tasks.Add(new SynchronizationRemoteStateSaveTask(synchronizationFileUploadTasks.ToArray(), _synchronizationServices, Events, () => _actualFiles));

        Events.DuringExecutionTasksAdded(Id, tasks);

        Children = tasks;
        base.Execute();
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
