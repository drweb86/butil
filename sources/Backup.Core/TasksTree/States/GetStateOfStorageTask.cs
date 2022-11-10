using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.Core;
using System.Text.Json;
using System.Threading;

namespace BUtil.Core.TasksTree
{
    internal class GetStateOfStorageTask : BuTask
    {
        public StorageSettings StorageSettings { get; }
        public IncrementalBackupState StorageState { get; private set; }

        public GetStateOfStorageTask(LogBase log, BackupEvents events, StorageSettings storageSettings) : 
            base(log, events, string.Format(BUtil.Core.Localization.Resources.GetStateOfStorage, storageSettings.Name), TaskArea.Hdd)
        {
            StorageSettings = storageSettings;
        }

        public override void Execute(CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;

            UpdateStatus(ProcessingStatus.InProgress);
            var storage = StorageFactory.Create(Log, StorageSettings);
            var content = storage.ReadAllText(IncrementalBackupModelConstants.StorageIncrementedNonEncryptedNonCompressedStateFile);
            if (content == null)
            {
                LogDebug("State is missing. Vanilla backup.");
                StorageState = new IncrementalBackupState();
            }
            else
            {
                LogDebug("Deserializing.");
                StorageState = JsonSerializer.Deserialize<IncrementalBackupState>(content);
            }
            UpdateStatus(ProcessingStatus.FinishedSuccesfully);
        }
    }
}
