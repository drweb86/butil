using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace BUtil.Core.TasksTree
{
    internal class WriteSourceFileToStorageTask : BuTask
    {
        private readonly StorageSpecificServicesIoc _services;
        private readonly Quota _singleBackupQuotaGb;
        private readonly List<VersionState> _versionStates;

        public List<StorageFile> StorageFiles { get; }
        public bool IsSkipped { get; private set; }
        public bool IsSkippedBecauseOfQuota { get; private set; }

        public WriteSourceFileToStorageTask(
            StorageSpecificServicesIoc services,
            TaskEvents events,
            List<StorageFile> storageFiles,
            Quota singleBackupQuotaGb,
            SourceItemV2 sourceItem,
            List<VersionState> versionStates) : 
            base(services.Log, events, string.Format(Localization.Resources.File_Saving, 
                string.Join(", ", storageFiles
                    .Select(x => SourceItemHelper.GetFriendlyFileName(sourceItem, x.FileState.FileName)     ))))
        {
            _services = services;
            StorageFiles = storageFiles;
            if (StorageFiles.Count == 0)
                throw new InvalidOperationException("StorageFiles should not be 0 elements");
            if (!StorageFiles.All(x => x.FileState.CompareTo(StorageFiles[0].FileState, true)))
                throw new InvalidOperationException("StorageFiles deduplication");
            _singleBackupQuotaGb = singleBackupQuotaGb;
            _versionStates = versionStates;
        }

        public override void Execute()
        {
            UpdateStatus(ProcessingStatus.InProgress);
            if (FileAlreadyInStorage(out var matchingFile))
            {
                LogDebug("Skipped because file is already is in storage.");
                IsSuccess = true;
                StorageFiles
                        .ForEach(x => x.SetStoragePropertiesFrom(matchingFile));
            }
            else if (_singleBackupQuotaGb.TryQuota(StorageFiles[0].FileState.Size))
            {
                try
                {
                    IsSuccess = _services.IncrementalBackupFileService.Upload(StorageFiles[0]);
                    StorageFiles.Skip(1)
                        .ToList()
                        .ForEach(x => x.SetStoragePropertiesFrom(StorageFiles[0]));
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
                IsSkippedBecauseOfQuota = true;
                IsSuccess = true;
            }
            UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        }

        private bool FileAlreadyInStorage([NotNullWhen(true)]out StorageFile? matchingStorageFile)
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
                        if (createdFile.FileState.CompareTo(StorageFiles[0].FileState, true, true))
                        {
                            matchingStorageFile = createdFile;
                            return true;
                        }
                    }

                    foreach (var updatedFile in sourceItemChange.UpdatedFiles)
                    {
                        if (updatedFile.FileState.CompareTo(StorageFiles[0].FileState, true, true))
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
