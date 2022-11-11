﻿using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.State;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.Core;
using System.Threading;

namespace BUtil.Core.TasksTree
{
    internal class WriteSourceFileToStorageTask : BuTask
    {
        private readonly StorageFile _storageFile;
        private readonly IStorageSettings _storageSettings;

        public WriteSourceFileToStorageTask(ILog log, BackupEvents events, StorageFile storageFile, IStorageSettings storageSettings) : 
            base(log, events, string.Format(BUtil.Core.Localization.Resources.WriteSourceFileToStorage, storageFile.FileState.FileName, storageSettings.Name), TaskArea.File)
        {
            this._storageFile = storageFile;
            this._storageSettings = storageSettings;
        }

        public override void Execute(CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;

            UpdateStatus(ProcessingStatus.InProgress);

            var storage = StorageFactory.Create(Log, _storageSettings);
            var uploadResult = storage.Upload(_storageFile.FileState.FileName, _storageFile.StorageRelativeFileName);
            _storageFile.StorageFileName = uploadResult.StorageFileName;
            _storageFile.StorageFileNameSize = uploadResult.StorageFileNameSize;
            UpdateStatus(ProcessingStatus.FinishedSuccesfully);
            IsSuccess = true;
        }
    }
}
