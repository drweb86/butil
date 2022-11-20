using BUtil.Core.BackupModels;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.TasksTree.Apps;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.States;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BUtil.Core.TasksTree.IncrementalModel
{
    class IncrementalBackupTask : SequentialBuTask
    {
        public IncrementalBackupTask(ILog log, BackupEvents backupEvents, BackupTask backupTask)
            : base(log, backupEvents, Resources.IncrementalBackup, TaskArea.ProgramInRunBeforeAfterBackupChain, null)
        {
            var tasks = new List<BuTask>();

            backupTask
                .ExecuteBeforeBackup
                .Select(executeBeforeBackup => new ExecuteProgramTask(log, backupEvents, executeBeforeBackup))
                .ToList()
                .ForEach(tasks.Add);

            var readSatesTask = new GetStateOfSourceItemsAndStoragesTask(Log, Events, backupTask.Items, backupTask.Storages, backupTask.FileExcludePatterns, backupTask.Password);
            tasks.Add(readSatesTask);

            tasks.Add(new WriteIncrementedVersionsTask(Log, Events, readSatesTask.StorageStateTasks, readSatesTask.GetSourceItemStateTasks, backupTask.Model as IncrementalBackupModelOptions, backupTask.Password));

            backupTask
                .ExecuteAfterBackup
                .Select(executeAfterBackup => new ExecuteProgramTask(log, backupEvents, executeAfterBackup))
                .ToList()
                .ForEach(tasks.Add);

            Children = tasks;
        }

        public override void Execute()
        {
            UpdateStatus(ProcessingStatus.InProgress);
            base.Execute();
            UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        }
    }
}
