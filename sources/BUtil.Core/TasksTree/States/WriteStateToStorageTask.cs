using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.Storage;
using Microsoft.VisualBasic;
using System.IO;
using System.Text.Json;
using System.Threading;

namespace BUtil.Core.TasksTree.States
{
    internal class WriteStateToStorageTask : BuTask
    {
        private CalculateIncrementedVersionForStorageTask _getIncrementedVersionTask;
        private IStorageSettings _storageSettings;
        private readonly WriteSourceFilesToStorageTask _writeSourceFilesToStorageTask;

        public StorageFile UploadedVersionState { get; private set; }

        public WriteStateToStorageTask(ILog log, BackupEvents events,
            CalculateIncrementedVersionForStorageTask getIncrementedVersionTask, IStorageSettings storageSettings, 
            Storage.WriteSourceFilesToStorageTask writeSourceFilesToStorageTask)
            : base(log, events, string.Format(BUtil.Core.Localization.Resources.WriteStateToStorage, storageSettings.Name), TaskArea.Hdd)
        {
            _getIncrementedVersionTask = getIncrementedVersionTask;
            _storageSettings = storageSettings;
            _writeSourceFilesToStorageTask = writeSourceFilesToStorageTask;
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
                UpdateStatus(ProcessingStatus.FinishedSuccesfully);
                return;
            }

            if (!_writeSourceFilesToStorageTask.IsSuccess)
            {
                LogDebug("Writing source files to storage has failed. Skipping.");
                IsSuccess = false;
                UpdateStatus(ProcessingStatus.FinishedWithErrors);
                return;
            }

            var storage = StorageFactory.Create(Log, _storageSettings);
            var tempFile = Path.GetRandomFileName();
            var json = JsonSerializer.Serialize(_getIncrementedVersionTask.IncrementalBackupState, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(tempFile, json);
            UploadedVersionState = new StorageFile
            {
                StorageRelativeFileName = IncrementalBackupModelConstants.StorageIncrementedNonEncryptedNonCompressedStateFile,
                StorageMethod = StorageMethodNames.Plain,
                StorageIntegrityMethod = StorageIntegrityMethod.Sha256
            };

            var uploadResult = storage.Upload(tempFile, IncrementalBackupModelConstants.StorageIncrementedNonEncryptedNonCompressedStateFile);
            UploadedVersionState.StorageFileName = uploadResult.StorageFileName;
            UploadedVersionState.StorageFileNameSize = uploadResult.StorageFileNameSize;
            UploadedVersionState.StorageIntegrityMethodInfo = HashHelper.GetSha512(tempFile);

            File.Delete(tempFile);
            IsSuccess = true;
            UpdateStatus(ProcessingStatus.FinishedSuccesfully);
        }
    }
}
