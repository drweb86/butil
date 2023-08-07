using BUtil.Core.BackupModels;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.Core;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BUtil.Core.TasksTree.IncrementalModel
{
    class MediaSyncBackupTask : SequentialBuTask
    {
        private readonly CommonServicesIoc _commonServicesIoc;
        private readonly StorageSpecificServicesIoc _storageService;
        private readonly BackupTask _task;

        public MediaSyncBackupTask(ILog log, BackupEvents backupEvents, BackupTask backupTask)
            : base(log, backupEvents, Resources.IncrementalBackup, TaskArea.ProgramInRunBeforeAfterBackupChain, null)
        {
            var tasks = new List<BuTask>();

            _commonServicesIoc = new CommonServicesIoc();

            var storage = backupTask
                .Storages
                .First();

            _storageService = new StorageSpecificServicesIoc(Log, storage, _commonServicesIoc.HashService);

            Children = tasks;
            _task = backupTask;
        }

        public override void Execute()
        {
            Events.OnMessage += OnAddLastMinuteLogMessage;
            UpdateStatus(ProcessingStatus.InProgress);

            var fromFolder = _task.Items[0].Target;
            var destinationFolder = ((FolderStorageSettings)_task.Storages[0]).DestinationFolder;
            var transformFileName = ((MediaSyncBackupModelOptions)_task.Model).TransformFileName;

            var tasks = Directory
                .GetFiles(fromFolder, "*.*", SearchOption.AllDirectories)
                .Select(x => new MoveFileTask(Log, this.Events, x, destinationFolder, transformFileName))
                .ToList();
            Events.DuringExecutionTasksAdded(Id, tasks);
            Children = tasks;

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

        private List<string> _lastMinuteLogMessages = new List<string>();
        private void OnAddLastMinuteLogMessage(object sender, MessageEventArgs e)
        {
            _lastMinuteLogMessages.Add(e.Message);
        }
    }
}
