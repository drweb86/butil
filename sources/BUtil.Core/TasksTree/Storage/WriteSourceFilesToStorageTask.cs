using BUtil.Core.BackupModels;
using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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
                        string storageRelativeFileName = GetStorageRelativeFileName(versionState, sourceItemChange, sourceItemRelativeFileName);

                        x.StorageRelativeFileName = storageRelativeFileName;
                        x.StorageMethod = GetStorageMethod();
                        return x;
                    })
                    .Select(x => new WriteSourceFileToStorageTask(
                        Log, Events, x, _storageSettings))
                    .ToList();
                childTasks.AddRange(copyTasks);
            }
            Events.DuringExecutionTasksAdded(Id, childTasks);
            Children = childTasks;
            base.Execute(token);
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
