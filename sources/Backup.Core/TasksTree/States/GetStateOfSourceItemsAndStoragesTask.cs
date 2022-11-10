using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.TasksTree.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BUtil.Core.TasksTree.States
{
    internal class GetStateOfSourceItemsAndStoragesTask : ParallelBuTask
    {
        public IEnumerable<GetStateOfSourceItemTask> GetSourceItemStateTasks { get; }
        public IEnumerable<GetStateOfStorageTask> StorageStateTasks { get; }

        public GetStateOfSourceItemsAndStoragesTask(LogBase log, BackupEvents events, BackupTask backupTask)
            : base(log, events, Localization.Resources.GetStateOfSourceItemsAndStorages, TaskArea.File, null)
        {
            var childTasks = new List<BuTask>();
            var setSourceItemStateTasks = new List<GetStateOfSourceItemTask>();

            foreach (var item in backupTask.Items)
            {
                var getSourceItemStateTask = new GetStateOfSourceItemTask(log, Events, item);
                setSourceItemStateTasks.Add(getSourceItemStateTask);
                childTasks.Add(getSourceItemStateTask);
            }
            GetSourceItemStateTasks = setSourceItemStateTasks;

            var storageStateTasks = new List<GetStateOfStorageTask>();
            foreach (var storage in backupTask.Storages)
            {
                var getStorageStateTask = new GetStateOfStorageTask(Log, Events, storage);
                storageStateTasks.Add(getStorageStateTask);
                childTasks.Add(getStorageStateTask);
            }

            StorageStateTasks = storageStateTasks;

            Children = childTasks;
        }

        public override void Execute(CancellationToken token)
        {
            UpdateStatus(ProcessingStatus.InProgress);
            base.Execute(token);
            UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        }
    }
}
