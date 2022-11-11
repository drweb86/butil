using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.Core;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace BUtil.Core.TasksTree.Storage
{
    internal class WriteSourceFilesToStorageTask : ParallelBuTask
    {
        private CalculateIncrementedVersionForStorageTask _getIncrementedVersionTask;
        private IStorageSettings _storageSettings;

        public WriteSourceFilesToStorageTask(ILog log, BackupEvents events,
            CalculateIncrementedVersionForStorageTask getIncrementedVersionTask, IStorageSettings storageSettings)
            : base(log, events, string.Format(BUtil.Core.Localization.Resources.WriteSourceFilesToStorage, storageSettings.Name), TaskArea.Hdd, null)
        {
            _getIncrementedVersionTask = getIncrementedVersionTask;
            _storageSettings = storageSettings;
        }

        public override void Execute(CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;

            UpdateStatus(ProcessingStatus.InProgress);

            if (!_getIncrementedVersionTask.VersionIsNeeded)
            {
                LogDebug("Version is not needed.");
                IsSuccess = true;
                Children = new List<BuTask>();
                UpdateStatus(ProcessingStatus.FinishedSuccesfully);
                return;
            }

            var childTasks = new List<BuTask>();

            var versionState = _getIncrementedVersionTask.IncrementalBackupState.VersionStates.Last();
            foreach (var sourceItemChange in versionState.SourceItemChanges)
            {
                var itemsToCopy = new List<StorageFile>();
                itemsToCopy.AddRange(sourceItemChange.UpdatedFiles);
                itemsToCopy.AddRange(sourceItemChange.CreatedFiles);

                var sourceItemDir = SourceItemHelper.GetSourceItemDirectory(sourceItemChange.SourceItem);

                var copyTasks = itemsToCopy
                    .Select(x =>
                    {
                        var sourceItemRelativeFileName = SourceItemHelper.GetSourceItemRelativeFileName(sourceItemDir, x.FileState);

                        var storageRelativeFileName = SourceItemHelper.GetUnencryptedUncompressedStorageRelativeFileName(
                            versionState,
                            sourceItemChange.SourceItem,
                            sourceItemRelativeFileName);

                        x.StorageRelativeFileName = storageRelativeFileName;
                        x.StorageMethod = StorageMethodNames.Plain;
                        x.StorageIntegrityMethod = StorageIntegrityMethod.Sha256;
                        x.StorageIntegrityMethodInfo = x.FileState.Sha512;
                        return x;
                    })
                    .Select(x => new WriteSourceFileToStorageTask(Log, Events, x, _storageSettings))
                    .ToList();
                childTasks.AddRange(copyTasks);
            }
            Events.DuringExecutionTasksAdded(Id, childTasks);
            Children = childTasks;
            base.Execute(token);
            UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        }
    }
}
