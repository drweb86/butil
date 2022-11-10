using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.States;
using BUtil.Core.TasksTree.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BUtil.Core.TasksTree
{
    internal class WriteIncrementedVersionTask: SequentialBuTask
    {
        public WriteIncrementedVersionTask(LogBase log, BackupEvents events, GetStateOfStorageTask storageStateTask,
            IEnumerable<GetStateOfSourceItemTask> getSourceItemStateTasks) :
            base(log, events, string.Format(BUtil.Core.Localization.Resources.WriteIncrementedVersionToStorage, storageStateTask.StorageSettings.Name),
                TaskArea.Hdd, null)
        {
            var childTaks = new List<BuTask>();

            var getIncrementedVersionTask = new CalculateIncrementedVersionForStorageTask(Log, Events, storageStateTask, getSourceItemStateTasks);
            childTaks.Add(getIncrementedVersionTask);
            childTaks.Add(new WriteSourceFilesToStorageTask(log, events, getIncrementedVersionTask, storageStateTask.StorageSettings));
            childTaks.Add(new WriteStateToStorageTask(log, events, getIncrementedVersionTask, storageStateTask.StorageSettings));
            childTaks.Add(new WriteIntegrityVerificationScriptsToStorageTask(log, events, getIncrementedVersionTask, storageStateTask.StorageSettings));

            Children = childTaks;
        }

        public override void Execute(CancellationToken token)
        {
            UpdateStatus(ProcessingStatus.InProgress);
            base.Execute(token);
            UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        }
    }
}
