using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.Storages;
using System.Text.Json;
using System.Threading;

namespace BUtil.Core.TasksTree
{
    internal class GetStorageStateTask : BuTask
    {
        public StorageSettings StorageSettings { get; }
        public IncrementalBackupState StorageState { get; private set; }

        public GetStorageStateTask(LogBase log, BackupEvents events, StorageSettings storageSettings) : 
            base(log, events, $"Get storage \"{storageSettings.Name}\" state", TaskArea.Hdd)
        {
            StorageSettings = storageSettings;
        }

        public override void Execute(CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;

            LogDebug("Reading state");
            Events.TaskProgessUpdate(Id, ProcessingStatus.InProgress);
            var storage = StorageFactory.Create(StorageSettings);
            var content = storage.ReadAllText(IncrementalBackupModelConstants.StorageStateFile);
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
        }
    }
}
