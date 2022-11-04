using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using System.Collections.Generic;

namespace BUtil.Core.TasksTree
{
    class IncrementalBackupTask : SequentialBuTask
    {
        public IncrementalBackupTask(LogBase log, BackupEvents backupEvents, BackupTask backupTask, ProgramOptions programOptions)
            : base(log, backupEvents, "Incremental Backup", TaskArea.ProgramInRunBeforeAfterBackupChain, null)
        {
            var tasks = new List<BuTask>();

            foreach (var executeBeforeBackup in backupTask.ExecuteBeforeBackup)
            {
                tasks.Add(new ExecuteProgramTask(log, backupEvents, executeBeforeBackup, programOptions.ProcessPriority));
            }

            foreach (var executeAfterBackup in backupTask.ExecuteAfterBackup)
            {
                tasks.Add(new ExecuteProgramTask(log, backupEvents, executeAfterBackup, programOptions.ProcessPriority));
            }
            Children = tasks;
        }
    }
}
