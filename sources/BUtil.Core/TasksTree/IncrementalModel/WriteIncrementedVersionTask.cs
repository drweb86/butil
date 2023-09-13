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
        private readonly WriteSourceFilesToStorageTask _writeSourceFilesToStorageTask;

        public WriteIncrementedVersionTask(
            StorageSpecificServicesIoc services,
            BackupEvents events, 
            GetStateOfStorageTask storageStateTask,
            IEnumerable<GetStateOfSourceItemTask> getSourceItemStateTasks,
            IncrementalBackupModelOptions incrementalBackupModelOptions) :
            base(services.Log, events, Localization.Resources.WriteIncrementedVersionToStorage,
                TaskArea.Hdd, null)
        {
            var childTaks = new List<BuTask>();

            var calculateIncrementedVersionForStorageTask = new CalculateIncrementedVersionForStorageTask(Log, Events, storageStateTask, getSourceItemStateTasks);
            childTaks.Add(calculateIncrementedVersionForStorageTask);

            _writeSourceFilesToStorageTask = new WriteSourceFilesToStorageTask(services, events, calculateIncrementedVersionForStorageTask, incrementalBackupModelOptions);
            childTaks.Add(_writeSourceFilesToStorageTask);

            var writeStateToStorageTask = new WriteStateToStorageTask(
                services,
                events,
                calculateIncrementedVersionForStorageTask,
                _writeSourceFilesToStorageTask,
                incrementalBackupModelOptions);

            childTaks.Add(writeStateToStorageTask);
            childTaks.Add(new WriteIntegrityVerificationScriptsToStorageTask(services, events,
                calculateIncrementedVersionForStorageTask,
                _writeSourceFilesToStorageTask,
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
