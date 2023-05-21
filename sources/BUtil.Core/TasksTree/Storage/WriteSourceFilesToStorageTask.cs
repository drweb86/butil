using BUtil.Core.BackupModels;
using BUtil.Core.Events;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.TasksTree.Storage
{
    internal class WriteSourceFilesToStorageTask : ParallelBuTask
    {
        private readonly StorageSpecificServicesIoc _services;
        private readonly CalculateIncrementedVersionForStorageTask _getIncrementedVersionTask;
        private readonly IncrementalBackupModelOptions _incrementalBackupModelOptions;
        private readonly string _password;

        public WriteSourceFilesToStorageTask(
            StorageSpecificServicesIoc services,
            BackupEvents events,
            CalculateIncrementedVersionForStorageTask getIncrementedVersionTask,
            IncrementalBackupModelOptions incrementalBackupModelOptions,
            string password)
            : base(services.Log, events, Localization.Resources.WriteSourceFilesToStorage, TaskArea.Hdd, null)
        {
            _services = services;
            _getIncrementedVersionTask = getIncrementedVersionTask;
            _incrementalBackupModelOptions = incrementalBackupModelOptions;
            _password = password;
        }

        public override void Execute()
        {
            UpdateStatus(ProcessingStatus.InProgress);

            if (!_getIncrementedVersionTask.VersionIsNeeded)
            {
                LogDebug("Version is not needed.");
                IsSuccess = true;
                Children = new List<BuTask>();
                UpdateStatus(ProcessingStatus.FinishedSuccesfully);
                return;
            }

            List<WriteSourceFileToStorageTask> WriteFileTasks = new List<WriteSourceFileToStorageTask>();
            var versionStates = _getIncrementedVersionTask.IncrementalBackupState.VersionStates;
            var versionState = versionStates.Last();
            var singleBackupQuotaGb = new Quota(_services.StorageSettings.SingleBackupQuotaGb * 1024 * 1024 * 1024);
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
                        string storageRelativeFileName = GetStorageRelativeFileName(versionState, sourceItemChange, sourceItemRelativeFileName);

                        x.StorageRelativeFileName = storageRelativeFileName;
                        x.StorageMethod = GetStorageMethod();
                        return x;
                    })
                    .Select(x => new WriteSourceFileToStorageTask(
                        _services,
                        Events,
                        x,
                        singleBackupQuotaGb,
                        versionStates))
                    .ToList();
                WriteFileTasks.AddRange(copyTasks);
            }
            Events.DuringExecutionTasksAdded(Id, WriteFileTasks);
            Children = WriteFileTasks;
            base.Execute();

            // TODO: think of design.

            var skippedFiles = WriteFileTasks
                .Where(x => x.IsSkipped || !x.IsSuccess)
                .Select(x => x.StorageFile.FileState.FileName)
                .ToList();

            foreach (var sourceItemChange in versionState.SourceItemChanges)
            {
                sourceItemChange.UpdatedFiles.RemoveAll(x => skippedFiles.Contains(x.FileState.FileName));
                sourceItemChange.CreatedFiles.RemoveAll(x => skippedFiles.Contains(x.FileState.FileName));
            }

            foreach (var lastSourceItemState in _getIncrementedVersionTask.IncrementalBackupState.LastSourceItemStates)
            {
                var updatedArray = lastSourceItemState.FileStates.Where(x => !skippedFiles.Contains(x.FileName)).ToList();
                lastSourceItemState.FileStates = updatedArray;
            }

            UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);

            var skippedBecauseOfQuotaFiles = WriteFileTasks
                .Where(x => x.IsSkippedBecauseOfQuota)
                .ToList();
            if (skippedBecauseOfQuotaFiles.Any())
            {
                var gigabyte = 1024 * 1024 *1024;
                Events.Message(string.Format(BUtil.Core.Localization.Resources.BackupWasPartialDueToStorageQuota, skippedBecauseOfQuotaFiles.Count(), skippedBecauseOfQuotaFiles.Sum(x => x.StorageFile.FileState.Size) / gigabyte));
            }
        }

        private string GetStorageMethod()
        {
            if (_incrementalBackupModelOptions.DisableCompressionAndEncryption)
                return StorageMethodNames.Plain;

            if (string.IsNullOrEmpty(_password))
                return StorageMethodNames.SevenZip;

            return StorageMethodNames.SevenZipEncrypted;

        }

        private string GetStorageRelativeFileName(VersionState versionState, SourceItemChanges sourceItemChange, string sourceItemRelativeFileName)
        {
            if (_incrementalBackupModelOptions.DisableCompressionAndEncryption)
                return SourceItemHelper.GetUnencryptedUncompressedStorageRelativeFileName(
                                            versionState,
                                            sourceItemChange.SourceItem,
                                            sourceItemRelativeFileName);

            return SourceItemHelper.GetCompressedStorageRelativeFileName(versionState);
        }
    }
}
