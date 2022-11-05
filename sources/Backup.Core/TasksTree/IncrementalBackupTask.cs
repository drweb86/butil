using BUtil.Core.BackupModels;
using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using System.Collections.Generic;

namespace BUtil.Core.TasksTree
{
    class IncrementalBackupTask : SequentialBuTask
    {
#pragma warning disable IDE0052 // Remove unread private members
        private readonly IncrementalBackupModelOptions _incrementalBackupModelOptions;
#pragma warning restore IDE0052 // Remove unread private members

        public IncrementalBackupTask(LogBase log, BackupEvents backupEvents, BackupTask backupTask,
            ProgramOptions programOptions, IncrementalBackupModelOptions incrementalBackupModelOptions)
            : base(log, backupEvents, "Incremental Backup", TaskArea.ProgramInRunBeforeAfterBackupChain, null)
        {
            var tasks = new List<BuTask>();

            foreach (var executeBeforeBackup in backupTask.ExecuteBeforeBackup)
            {
                tasks.Add(new ExecuteProgramTask(log, backupEvents, executeBeforeBackup, programOptions.ProcessPriority));
            }

            var readSatesTask = new ReadStatesTask(Log, Events, backupTask);
            tasks.Add(readSatesTask);

            foreach (var storageStateTask in readSatesTask.StorageStateTasks)
            {
                tasks.Add(new WriteIncrementedVersionTask(Log, Events, storageStateTask, readSatesTask.GetSourceItemStateTasks));
            }

            foreach (var executeAfterBackup in backupTask.ExecuteAfterBackup)
            {
                tasks.Add(new ExecuteProgramTask(log, backupEvents, executeAfterBackup, programOptions.ProcessPriority));
            }
            Children = tasks;
            this._incrementalBackupModelOptions = incrementalBackupModelOptions;
        }
    }
}
