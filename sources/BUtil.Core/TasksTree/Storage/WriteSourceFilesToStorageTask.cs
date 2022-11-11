using BUtil.Core.Events;
using BUtil.Core.Logs;
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
        private StorageSettings _storageSettings;

        public WriteSourceFilesToStorageTask(ILog log, BackupEvents events,
            CalculateIncrementedVersionForStorageTask getIncrementedVersionTask, StorageSettings storageSettings)
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
            var childTasks = new List<BuTask>();

            var versionState = _getIncrementedVersionTask.IncrementalBackupState.VersionStates.Last();
            foreach (var sourceItemChange in versionState.SourceItemChanges)
            {
                var itemsToCopy = new List<StorageFile>();
                itemsToCopy.AddRange(sourceItemChange.UpdatedFiles);
                itemsToCopy.AddRange(sourceItemChange.CreatedFiles);

                itemsToCopy
                    .Select(x =>
                    {
                        var sourceItemDir = sourceItemChange.SourceItem.IsFolder ?
                            sourceItemChange.SourceItem.Target :
                            Path.GetDirectoryName(sourceItemChange.SourceItem.Target);

                        var sourceItemRelativeFileName = x.FileState.FileName.Substring(sourceItemDir.Length);

                        x.StorageRelativeFileName = Path.Combine(
                            versionState.BackupDateUtc.ToString("yyyyMMddTHHmmss"),
                            sourceItemChange.SourceItem.Target.GetHashCode().ToString(),
                            sourceItemRelativeFileName);
                        x.StorageMethod = StorageMethodNames.Plain;
                        x.StorageIntegrityMethod = StorageIntegrityMethod.Sha256;
                        x.StorageIntegrityMethodInfo = x.FileState.Sha512;
                        return x;
                    })
                    .Select(x => new WriteSourceFileToStorageTask(Log, Events, x, _storageSettings))
                    .ToList();
            }
            Events.DuringExecutionTasksAdded(Id, childTasks);
            Children = childTasks;
            base.Execute(token);
            UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        }
    }
}
