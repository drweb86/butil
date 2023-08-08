using BUtil.Core.BackupModels;
using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.Core;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BUtil.Core.TasksTree.IncrementalModel
{
    class MoveFilesTask : SequentialBuTask
    {
        private readonly BackupTask _task;

        public MoveFilesTask(ILog log, BackupEvents backupEvents, BackupTask backupTask)
            : base(log, backupEvents, "Move files", TaskArea.ProgramInRunBeforeAfterBackupChain, null)
        {
            Children = new List<BuTask>();
            _task = backupTask;
        }

        public override void Execute()
        {
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
            UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        }
    }
}
