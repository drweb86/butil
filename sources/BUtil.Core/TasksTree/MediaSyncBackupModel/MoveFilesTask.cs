using BUtil.Core.BackupModels;
using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.Core;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BUtil.Core.TasksTree.MediaSyncBackupModel
{
    class MoveFilesTask : SequentialBuTask
    {
        private readonly BackupTask _task;

        public MoveFilesTask(ILog log, BackupEvents backupEvents, BackupTask backupTask)
            : base(log, backupEvents, BUtil.Core.Localization.Resources.MoveFilesTask_MoveFiles, TaskArea.ProgramInRunBeforeAfterBackupChain, null)
        {
            Children = new List<BuTask>();
            _task = backupTask;
        }

        public override void Execute()
        {
            UpdateStatus(ProcessingStatus.InProgress);

            var options = (MediaSyncBackupModelOptions)_task.Model;

            var fromFolder = ((FolderStorageSettings)options.From).DestinationFolder;
            var destinationFolder = ((FolderStorageSettings)options.To).DestinationFolder;
            var transformFileName = ((MediaSyncBackupModelOptions)_task.Model).TransformFileName;

            var tasks = Directory
                .GetFiles(fromFolder, "*.*", SearchOption.AllDirectories)
                .Select(x => new MoveFileTask(Log, Events, x, destinationFolder, transformFileName))
                .ToList();
            Events.DuringExecutionTasksAdded(Id, tasks);
            Children = tasks;

            base.Execute();
            UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        }
    }
}
