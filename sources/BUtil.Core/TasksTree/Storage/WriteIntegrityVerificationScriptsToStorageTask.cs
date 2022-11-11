using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.Core;
using System.IO;
using System.Linq;
using System.Threading;

namespace BUtil.Core.TasksTree.Storage
{
    internal class WriteIntegrityVerificationScriptsToStorageTask : BuTask
    {
        private CalculateIncrementedVersionForStorageTask _getIncrementedVersionTask;
        private IStorageSettings _storageSettings;

        public WriteIntegrityVerificationScriptsToStorageTask(ILog log, BackupEvents events,
            CalculateIncrementedVersionForStorageTask getIncrementedVersionTask, IStorageSettings storageSettings)
            : base(log, events, string.Format(BUtil.Core.Localization.Resources.WriteIntegrityVerificationScriptsToStorage, storageSettings.Name), TaskArea.Hdd)
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
                UpdateStatus(ProcessingStatus.FinishedSuccesfully);
                return;
            }

            var storage = StorageFactory.Create(Log, _storageSettings);
            var tempFile = Path.GetRandomFileName();
            File.WriteAllText(tempFile, string.Empty);
            var ps1VerificationScript = BUtil.Core.Localization.Resources.IntegrityVerificationScriptPs1;
            var shVerificationScript = BUtil.Core.Localization.Resources.IntegrityVerificationScriptSh;
            storage.Upload(tempFile, ps1VerificationScript);
            storage.Upload(tempFile, shVerificationScript);
            File.Delete(tempFile);
            IsSuccess = true;
            UpdateStatus(ProcessingStatus.FinishedSuccesfully);
        }
    }
}
