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

            var options = (ImportMediaBackupModelOptions)_task.Model;

            var fromStorage = StorageFactory.Create(this.Log, options.From);
            var toStorage = StorageFactory.Create(this.Log, new FolderStorageSettings { DestinationFolder = options.DestinationFolder });
            var transformFileName = options.TransformFileName;

            var tasks = fromStorage.GetFiles(null, SearchOption.AllDirectories)
                .Select(x => new MoveFileTask(Log, Events, x, fromStorage, toStorage, transformFileName))
                .ToList();
            Events.DuringExecutionTasksAdded(Id, tasks);
            Children = tasks;

            base.Execute();
            UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        }
    }
}
