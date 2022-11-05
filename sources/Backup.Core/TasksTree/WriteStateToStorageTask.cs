using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Storages;
using System.IO;
using System.Text.Json;
using System.Threading;

namespace BUtil.Core.TasksTree
{
    internal class WriteStateToStorageTask : BuTask
    {
        private GetIncrementedVersionTask _getIncrementedVersionTask;
        private StorageSettings _storageSettings;

        public WriteStateToStorageTask(LogBase log, BackupEvents events,
            GetIncrementedVersionTask getIncrementedVersionTask, StorageSettings storageSettings)
            : base(log, events, $"Write state file to storage \"{storageSettings.Name}\"", TaskArea.Hdd)
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
            var json = JsonSerializer.Serialize(_getIncrementedVersionTask.IncrementalBackupState);
            File.WriteAllText(tempFile, json);
            storage.Upload(tempFile, IncrementalBackupModelConstants.StorageStateFile);
            File.Delete(tempFile);
            Events.TaskProgessUpdate(Id, ProcessingStatus.FinishedSuccesfully);
        }
    }
}
