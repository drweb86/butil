using BUtil.Core.Events;
using BUtil.Core.Logs;
using System.Collections.Generic;

namespace BUtil.Core.TasksTree
{
    internal class WriteIncrementedVersionTask: SequentialBuTask
    {
        private GetStorageStateTask _storageStateTask;
        private IEnumerable<GetSourceItemStateTask> _getSourceItemStateTasks;

        public WriteIncrementedVersionTask(LogBase log, BackupEvents events, GetStorageStateTask storageStateTask,
            IEnumerable<GetSourceItemStateTask> getSourceItemStateTasks) :
            base(log, events, $"Write incremented version to storage\"{storageStateTask.StorageSettings.Name}\"",
                TaskArea.Hdd, null)
        {
            _storageStateTask = storageStateTask;
            _getSourceItemStateTasks = getSourceItemStateTasks;

            var childTaks = new List<BuTask>();

            var getDeltaTask = new GetDeltaTask();
            childTaks.Add(getDeltaTask);
            childTaks.Add(new WriteUpdatedFilesToStorageTask( log, events, getDeltaTask));
            childTaks.Add(new WriteStateTask(log, events, getDeltaTask));
            childTaks.Add(new WriteBackupIntegrityVerificationScriptsTask(log, events, getDeltaTask));

            Children = childTaks;
        }
    }
}
