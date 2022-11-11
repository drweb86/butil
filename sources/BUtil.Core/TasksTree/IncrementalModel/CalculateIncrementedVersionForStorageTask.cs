using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BUtil.Core.TasksTree
{
    internal class CalculateIncrementedVersionForStorageTask: BuTask
    {
        public bool VersionIsNeeded { get; private set; }
        public IncrementalBackupState IncrementalBackupState { get; private set; }

        private readonly GetStateOfStorageTask _storageStateTask;
        private readonly IEnumerable<GetStateOfSourceItemTask> _getSourceItemStateTasks;
        public CalculateIncrementedVersionForStorageTask(ILog log, BackupEvents events, GetStateOfStorageTask storageStateTask,
            IEnumerable<GetStateOfSourceItemTask> getSourceItemStateTasks) :
            base(log, events, string.Format(BUtil.Core.Localization.Resources.CalculateIncrementedVersionForStorage, storageStateTask.StorageSettings.Name),
                TaskArea.Hdd)
        {
            _storageStateTask = storageStateTask;
            _getSourceItemStateTasks = getSourceItemStateTasks;
        }
        
        public override void Execute(CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;

            UpdateStatus(ProcessingStatus.InProgress);

            var storageState = _storageStateTask.StorageState;
            var sourceItemStates = _getSourceItemStateTasks
                .Select(item => item.SourceItemState)
                .ToList();

            var versionState = Compare(storageState.LastSourceItemStates, sourceItemStates);
            storageState.VersionStates.Add(versionState);
            storageState.LastSourceItemStates = sourceItemStates;
            IncrementalBackupState = storageState;
            VersionIsNeeded = versionState.SourceItemChanges.Any(x => x.CreatedFiles.Any() || x.UpdatedFiles.Any() || x.CreatedFiles.Any());

            UpdateStatus(ProcessingStatus.FinishedSuccesfully);
            IsSuccess = true;
        }

        private VersionState Compare(IEnumerable<SourceItemState> a, IEnumerable<SourceItemState> b)
        {
            var matchingBtoA = b
                .ToDictionary(x => x, x => (SourceItemState)null);

            foreach (var item in a)
            {
                foreach (var pair in matchingBtoA)
                {
                    if (pair.Key.SourceItem == item.SourceItem)
                    {
                        matchingBtoA[pair.Key] = item;
                        break;
                    }
                }
            }

            var sourceItemChangesList = new List<SourceItemChanges>();
            foreach (var pair in matchingBtoA)
            {
                if (pair.Value == null)
                {
                    var addedSourceItem = pair.Key;
                    var sourceItemChanges = new SourceItemChanges(
                        addedSourceItem.SourceItem,
                        new List<string>(),
                        new List<StorageFile>(),
                        addedSourceItem.FileStates
                            .Select(x => new StorageFile(x))
                            .ToList()
                        );
                    sourceItemChangesList.Add(sourceItemChanges);
                }
                else
                {
                    var update = CompareSourceItemStates(pair.Value, pair.Key);
                    sourceItemChangesList.Add(update);
                }
            }

            var versionState = new VersionState(DateTime.UtcNow, sourceItemChangesList);
            return versionState;
        }

        private SourceItemChanges CompareSourceItemStates(SourceItemState a, SourceItemState b)
        {
            var createdFiles = b.FileStates.ToList();
            var updatedFiles = new List<FileState>();
            var deletedFiles = a.FileStates.ToList();

            foreach (var bItem in b.FileStates)
            {
                foreach (var aItem in a.FileStates)
                {
                    if (aItem.FileName == bItem.FileName)
                    {
                        deletedFiles.Remove(aItem);
                        createdFiles.Remove(bItem);

                        if (aItem != bItem)
                            updatedFiles.Add(bItem);

                        break;
                    }
                }
            }
            
            return new SourceItemChanges(a.SourceItem,
                deletedFiles.Select(x => x.FileName).ToList(),
                updatedFiles.Select(x => new StorageFile(x)).ToList(),
                createdFiles.Select(x => new StorageFile(x)).ToList());
        }
    }
}
