using BUtil.Core.BackupModels;
using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using System;
using System.Collections.Generic;

namespace BUtil.Core.TasksTree.MediaSyncBackupModel
{
    class ImportMediaBackupTask : SequentialBuTask
    {
        private readonly CommonServicesIoc _commonServicesIoc = new();

        public ImportMediaBackupTask(ILog log, BackupEvents backupEvents, BackupTask backupTask)
            : base(log, backupEvents, string.Empty, TaskArea.ProgramInRunBeforeAfterBackupChain, null)
        {
            var typedModel = backupTask.Model as ImportMediaBackupModelOptions;
            var sourceItem = new SourceItem(typedModel.DestinationFolder, true);

            var getStateOfSourceItemTask = new GetStateOfSourceItemTask(log, backupEvents, sourceItem, Array.Empty<string>(), _commonServicesIoc);
            var importFiles = new ImportFilesTask(log, backupEvents, backupTask, getStateOfSourceItemTask, _commonServicesIoc);

            Children = new BuTask[] { getStateOfSourceItemTask, importFiles };
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

        private List<string> _lastMinuteLogMessages = new List<string>();
        private void OnAddLastMinuteLogMessage(object sender, MessageEventArgs e)
        {
            _lastMinuteLogMessages.Add(e.Message);
        }
    }
}
