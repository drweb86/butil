using BUtil.Core.BackupModels;
using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using BUtil.Core.TasksTree.States;
using BUtil.Core.TasksTree.Storage;
using System.Collections.Generic;
using System.Threading;

namespace BUtil.Core.TasksTree
{
    internal class WriteIncrementedVersionTask: SequentialBuTask
    {
        public WriteIncrementedVersionTask(
            StorageSpecificServicesIoc services,
            BackupEvents events, 
            GetStateOfStorageTask storageStateTask,
            IEnumerable<GetStateOfSourceItemTask> getSourceItemStateTasks,
            IncrementalBackupModelOptions incrementalBackupModelOptions,
            string password) :
            base(services.Log, events, Localization.Resources.WriteIncrementedVersionToStorage,
                TaskArea.Hdd, null)
        {
            var childTaks = new List<BuTask>();

            var calculateIncrementedVersionForStorageTask = new CalculateIncrementedVersionForStorageTask(Log, Events, storageStateTask, getSourceItemStateTasks);
            childTaks.Add(calculateIncrementedVersionForStorageTask);

            var writeSourceFilesToStorageTask = new WriteSourceFilesToStorageTask(services, events, calculateIncrementedVersionForStorageTask, incrementalBackupModelOptions, password);
            childTaks.Add(writeSourceFilesToStorageTask);

            var writeStateToStorageTask = new WriteStateToStorageTask(
                services,
                events,
                calculateIncrementedVersionForStorageTask,
                writeSourceFilesToStorageTask,
                incrementalBackupModelOptions,
                password);

            childTaks.Add(writeStateToStorageTask);
            childTaks.Add(new WriteIntegrityVerificationScriptsToStorageTask(services, events,
                calculateIncrementedVersionForStorageTask,
                writeSourceFilesToStorageTask,
                writeStateToStorageTask));

            Children = childTaks;
        }

        public override void Execute()
        {
            UpdateStatus(ProcessingStatus.InProgress);
            base.Execute();
            UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        }
    }
}
