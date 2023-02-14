using BUtil.Core.Events;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.TasksTree
{
    internal class WriteSourceFileToStorageTask : BuTask
    {
        private readonly StorageSpecificServicesIoc _services;
        private readonly Quota _singleBackupQuotaGb;
        private readonly List<VersionState> _versionStates;

        public StorageFile StorageFile { get; }
        public bool IsSkipped { get; private set; }

        public WriteSourceFileToStorageTask(
            StorageSpecificServicesIoc services,
            BackupEvents events,
            StorageFile storageFile,
            Quota singleBackupQuotaGb,
            System.Collections.Generic.List<VersionState> versionStates) : 
            base(services.Log, events, string.Format(BUtil.Core.Localization.Resources.WriteSourceFileToStorage, storageFile.FileState.FileName, services.StorageSettings.Name), TaskArea.File)
        {
            _services = services;
            StorageFile = storageFile;
            _singleBackupQuotaGb = singleBackupQuotaGb;
            _versionStates = versionStates;
        }

        public override void Execute()
        {
            UpdateStatus(ProcessingStatus.InProgress);

            if (StorageFile.StorageMethod != StorageMethodNames.Plain // in plain storage - this optimization must be disabled.
                                                                    // because this mode is for trust and user must see changed files
                                                                    // even if it will cost him space of storage.
                && FileAlreadyInStorage(out var matchingFile))
            {
                LogDebug("Skipped because file is already is in storage.");
                IsSuccess = true;
                SetFileReferences(matchingFile);
            }
            else if (_singleBackupQuotaGb.TryQuota(StorageFile.FileState.Size))
            {
                try
                {
                    IsSuccess = _services.IncrementalBackupFileService.Upload(StorageFile);
                }
                catch
                {
                    IsSkipped = true; // because some files can be locked
                    IsSuccess = true;
                }
            }
            else
            {
                LogDebug("Skipped because of quota.");
                IsSkipped = true;
                IsSuccess = true;
            }
            UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        }

        private void SetFileReferences(StorageFile matchingFile)
        {
            StorageFile.SetStoragePropertiesFrom(matchingFile);
        }

        private bool FileAlreadyInStorage(out StorageFile matchingStorageFile)
        {
            matchingStorageFile = null;
            if (this._versionStates.Count < 2)
            {
                return false;
            }

            var previousVersions = this._versionStates.Take(this._versionStates.Count - 1).ToArray();
            foreach (var previousVersion in previousVersions)
            {
                foreach (var sourceItemChange in previousVersion.SourceItemChanges)
                {
                    foreach (var createdFile in sourceItemChange.CreatedFiles)
                    {
                        if (createdFile.FileState.CompareTo(StorageFile.FileState, true, true))
                        {
                            matchingStorageFile = createdFile;
                            return true;
                        }
                    }

                    foreach (var updatedFile in sourceItemChange.UpdatedFiles)
                    {
                        if (updatedFile.FileState.CompareTo(StorageFile.FileState, true, true))
                        {
                            matchingStorageFile = updatedFile;
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
