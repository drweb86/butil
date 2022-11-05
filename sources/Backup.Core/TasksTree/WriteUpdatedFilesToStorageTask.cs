using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.State;
using BUtil.Core.Storages;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace BUtil.Core.TasksTree
{
    internal class WriteUpdatedFilesToStorageTask : ParallelBuTask
    {
        private GetIncrementedVersionTask _getIncrementedVersionTask;
        private StorageSettings _storageSettings;

        public WriteUpdatedFilesToStorageTask(LogBase log, BackupEvents events,
            GetIncrementedVersionTask getIncrementedVersionTask, StorageSettings storageSettings)
            : base(log, events, $"Write updated files to storage \"{storageSettings.Name}\"", TaskArea.Hdd, null)
        {
            _getIncrementedVersionTask = getIncrementedVersionTask;
            _storageSettings = storageSettings;
        }

        public override void Execute(CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;

            LogDebug("Create tasks");
            Events.TaskProgessUpdate(Id, ProcessingStatus.InProgress);
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

                        x.StorageFileName = Path.Combine(
                            versionState.BackupDateUtc.ToString("yyyyMMddTHHmmss"),
                            sourceItemChange.SourceItem.Target.GetHashCode().ToString(),
                            sourceItemRelativeFileName);
                        x.StorageMethod = StorageMethodNames.Plain;
                        x.StorageIntegrityMethod = StorageIntegrityMethod.Sha256;
                        x.StorageIntegrityMethodInfo = x.FileState.Sha512;
                        return x;
                    })
                    .Select(x => new WriteUpdatedFileToStorageTask(Log, Events, x, _storageSettings))
                    .ToList();
            }
            Events.DuringExecutionTasksAdded(Id, childTasks);
            Children = childTasks;
            base.Execute(token);
            Events.TaskProgessUpdate(Id, ProcessingStatus.FinishedSuccesfully);
        }
    }
}
