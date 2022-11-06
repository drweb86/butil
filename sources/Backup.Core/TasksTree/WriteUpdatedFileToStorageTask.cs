using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.State;
using BUtil.Core.Storages;
using System.Threading;

namespace BUtil.Core.TasksTree
{
    internal class WriteUpdatedFileToStorageTask : BuTask
    {
        private readonly StorageFile _storageFile;
        private readonly StorageSettings _storageSettings;

        public WriteUpdatedFileToStorageTask(LogBase log, BackupEvents events, StorageFile storageFile, StorageSettings storageSettings) : 
            base(log, events, $"Write file {storageFile.FileState.FileName} to storage \"{storageSettings.Name}\"", TaskArea.File)
        {
            this._storageFile = storageFile;
            this._storageSettings = storageSettings;
        }

        public override void Execute(CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;

            LogDebug("Copying");
            Events.TaskProgessUpdate(Id, ProcessingStatus.InProgress);

            var storage = StorageFactory.Create(_storageSettings);
            var storageRelativeFileName = _storageFile.StorageFileName;
            _storageFile.StorageFileName = storage.Upload(_storageFile.FileState.FileName, _storageFile.StorageRelativeFileName);
            Events.TaskProgessUpdate(Id, ProcessingStatus.FinishedSuccesfully);
        }
    }
}
