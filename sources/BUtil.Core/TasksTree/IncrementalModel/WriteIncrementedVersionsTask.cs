using BUtil.Core.BackupModels;
using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
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
            IEnumerable<StorageSpecificServicesIoc> services,
            IEnumerable<GetStateOfStorageTask> storageStateTasks,
            IEnumerable<GetStateOfSourceItemTask> getSourceItemStateTasks,
            IncrementalBackupModelOptions incrementalBackupModelOptions,
            string password) :
            base(log, events, BUtil.Core.Localization.Resources.WriteIncrementedVersionToStorages,
                TaskArea.Hdd, null)
        {
            Children = storageStateTasks
                .Select(storageStateTask => {
                    var servicesIoc = services.Single(x => x.StorageSettings == storageStateTask.StorageSettings);
                    return new WriteIncrementedVersionTask(servicesIoc, Events, storageStateTask, getSourceItemStateTasks, incrementalBackupModelOptions, password);
                })
                .ToList();
        }

        public override void Execute()
        {
            UpdateStatus(ProcessingStatus.InProgress);
            base.Execute();
            UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        }
    }
}
