using BUtil.Core.Events;
using BUtil.Core.Logs;
using System.Collections.Generic;

namespace BUtil.Core.TasksTree
{
    internal class WriteIncrementedVersionTask: SequentialBuTask
    {
        public WriteIncrementedVersionTask(LogBase log, BackupEvents events, GetStorageStateTask storageStateTask,
            IEnumerable<GetSourceItemStateTask> getSourceItemStateTasks) :
            base(log, events, $"Write incremented version to storage\"{storageStateTask.StorageSettings.Name}\"",
                TaskArea.Hdd, null)
        {
            var childTaks = new List<BuTask>();

            var getIncrementedVersionTask = new GetIncrementedVersionTask(Log, Events, storageStateTask, getSourceItemStateTasks);
            childTaks.Add(getIncrementedVersionTask);
            childTaks.Add(new WriteUpdatedFilesToStorageTask(log, events, getIncrementedVersionTask, storageStateTask.StorageSettings));
            childTaks.Add(new WriteStateToStorageTask(log, events, getIncrementedVersionTask, storageStateTask.StorageSettings));
            childTaks.Add(new WriteBackupIntegrityVerificationScriptsTask(log, events, getIncrementedVersionTask, storageStateTask.StorageSettings));

            Children = childTaks;
        }
    }
}
