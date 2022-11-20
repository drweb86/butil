using BUtil.Core.BackupModels;
using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.Core;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms.VisualStyles;

namespace BUtil.Core.TasksTree.Storage
{
    internal class WriteSourceFilesToStorageTask : ParallelBuTask
    {
        private readonly CalculateIncrementedVersionForStorageTask _getIncrementedVersionTask;
        private readonly IncrementalBackupModelOptions _incrementalBackupModelOptions;
        private readonly string _password;
        private readonly IStorageSettings _storageSettings;

        public WriteSourceFilesToStorageTask(
            ILog log,
            BackupEvents events,
            CalculateIncrementedVersionForStorageTask getIncrementedVersionTask,
            IncrementalBackupModelOptions incrementalBackupModelOptions,
            string password,
            IStorageSettings storageSettings)
            : base(log, events, string.Format(Localization.Resources.WriteSourceFilesToStorage, storageSettings.Name), TaskArea.Hdd, null)
        {
            _getIncrementedVersionTask = getIncrementedVersionTask;
            _incrementalBackupModelOptions = incrementalBackupModelOptions;
            _password = password;
            _storageSettings = storageSettings;
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

            var childTasks = new List<BuTask>();

            var versionState = _getIncrementedVersionTask.IncrementalBackupState.VersionStates.Last();
            var singleBackupQuotaGb = new Quota(_storageSettings.SingleBackupQuotaGb * 1024 * 1024 * 1024);
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
                        Log,
                        Events,
                        x,
                        _storageSettings,
                        singleBackupQuotaGb))
                    .ToList();
                childTasks.AddRange(copyTasks);
            }
            Events.DuringExecutionTasksAdded(Id, childTasks);
            Children = childTasks;
            base.Execute();

            // TODO: think of design.

            var skippedFiles = childTasks
                .Select(x => x as WriteSourceFileToStorageTask)
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
