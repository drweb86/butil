using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using System.Collections.Generic;

namespace BUtil.Core.TasksTree
{
    internal class ReadStatesTask: ParallelBuTask
    {
        public IEnumerable<GetSourceItemStateTask> GetSourceItemStateTasks { get; }
        public IEnumerable<GetStorageStateTask> StorageStateTasks { get; }

        public ReadStatesTask(LogBase log, BackupEvents events, BackupTask backupTask)
            : base(log, events, "Read items and storages states", TaskArea.File, null)
        {
            var childTasks = new List<BuTask>();
            var setSourceItemStateTasks = new List<GetSourceItemStateTask>();
            
            foreach (var item in backupTask.Items)
            {
                var getSourceItemStateTask = new GetSourceItemStateTask(log, Events, item);
                setSourceItemStateTasks.Add(getSourceItemStateTask);
                childTasks.Add(getSourceItemStateTask);
            }
            GetSourceItemStateTasks = setSourceItemStateTasks;

            var storageStateTasks = new List<GetStorageStateTask>();
            foreach (var storage in backupTask.Storages)
            {
                var getStorageStateTask = new GetStorageStateTask(Log, Events, storage);
                storageStateTasks.Add(getStorageStateTask);
                childTasks.Add(getStorageStateTask);
            }

            StorageStateTasks = storageStateTasks;

            Children = childTasks;
        }
    }
}
