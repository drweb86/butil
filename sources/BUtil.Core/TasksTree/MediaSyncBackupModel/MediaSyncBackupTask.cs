using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.TasksTree.MediaSyncBackupModel
{
    class MediaSyncBackupTask : SequentialBuTask
    {
        private readonly CommonServicesIoc _commonServicesIoc;
        private readonly StorageSpecificServicesIoc _storageService;

        public MediaSyncBackupTask(ILog log, BackupEvents backupEvents, BackupTask backupTask)
            : base(log, backupEvents, string.Empty, TaskArea.ProgramInRunBeforeAfterBackupChain, new[] { new MoveFilesTask(log, backupEvents, backupTask) })
        {
            _commonServicesIoc = new CommonServicesIoc();

            var storage = backupTask
                .Storages
                .First();

            _storageService = new StorageSpecificServicesIoc(Log, storage, _commonServicesIoc.HashService);
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

        private List<string> _lastMinuteLogMessages = new List<string>();
        private void OnAddLastMinuteLogMessage(object sender, MessageEventArgs e)
        {
            _lastMinuteLogMessages.Add(e.Message);
        }
    }
}
