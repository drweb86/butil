using BUtil.Core.BackupModels;
using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.TasksTree.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BUtil.Core.TasksTree
{
    internal class WriteIncrementedVersionsTask : ParallelBuTask
    {
        public WriteIncrementedVersionsTask(
            ILog log,
            BackupEvents events,
            IEnumerable<GetStateOfStorageTask> storageStateTasks,
            IEnumerable<GetStateOfSourceItemTask> getSourceItemStateTasks,
            IncrementalBackupModelOptions incrementalBackupModelOptions,
            string password) :
            base(log, events, "BUtil.Core.Localization.Resources.WriteIncrementedVersionToStorages",
                TaskArea.Hdd, null)
        {
            Children = storageStateTasks
                .Select(storageStateTask => new WriteIncrementedVersionTask(Log, Events, storageStateTask, getSourceItemStateTasks, incrementalBackupModelOptions, password))
                .ToList();
        }

        public override void Execute(CancellationToken token)
        {
            UpdateStatus(ProcessingStatus.InProgress);
            base.Execute(token);
            UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        }
    }
}
