using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Storages;
using System.IO;
using System.Linq;
using System.Threading;

namespace BUtil.Core.TasksTree
{
    internal class WriteBackupIntegrityVerificationScriptsTask : BuTask
    {
        private GetIncrementedVersionTask _getIncrementedVersionTask;
        private StorageSettings _storageSettings;

        public WriteBackupIntegrityVerificationScriptsTask(LogBase log, BackupEvents events,
            GetIncrementedVersionTask getIncrementedVersionTask, StorageSettings storageSettings)
            : base(log, events, $"Write integrity verification scripts to storage \"{storageSettings.Name}\"", TaskArea.Hdd)
        {
            _getIncrementedVersionTask = getIncrementedVersionTask;
            _storageSettings = storageSettings;
        }

        public override void Execute(CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;

            LogDebug("Write state");
            Events.TaskProgessUpdate(Id, ProcessingStatus.InProgress);

            var storage = StorageFactory.Create(_storageSettings);
            var tempFile = Path.GetRandomFileName();
            File.WriteAllText(tempFile, string.Empty);
            var versionState = _getIncrementedVersionTask.IncrementalBackupState.VersionStates.Last();
            var ps1VerificationScript = $"{versionState.BackupDateUtc.ToString("yyyyMMddTHHmmss")} integrity verification script.ps1";
            var shVerificationScript = $"{versionState.BackupDateUtc.ToString("yyyyMMddTHHmmss")} integrity verification script.sh";
            storage.Upload(tempFile, ps1VerificationScript);
            storage.Upload(tempFile, shVerificationScript);
            File.Delete(tempFile);
            Events.TaskProgessUpdate(Id, ProcessingStatus.FinishedSuccesfully);
        }
    }
}
