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

namespace BUtil.Core.TasksTree;

internal class WriteSourceFileToStorageTask : BuTask
{
    private readonly StorageSpecificServicesIoc _services;
    private readonly Quota _singleBackupQuotaGb;
    private readonly List<VersionState> _versionStates;
    private readonly string _actualFile;
    private readonly bool _ignoreLastVersion;

    public List<StorageFile> StorageFiles { get; }
    public bool IsSkipped { get; private set; }
    public bool IsSkippedBecauseOfQuota { get; private set; }

    public WriteSourceFileToStorageTask(
        StorageSpecificServicesIoc services,
        TaskEvents events,
        List<StorageFile> storageFiles,
        Quota singleBackupQuotaGb,
        SourceItemV2 sourceItem,
        List<VersionState> versionStates,
        string actualFile,
        bool ignoreLastVersion) :
        base(services.Log, events, string.Format(Localization.Resources.File_Saving,
            string.Join(", ", storageFiles
                .Select(x => SourceItemHelper.GetFriendlyFileName(sourceItem, x.FileState.FileName)))))
    {
        _services = services;
        StorageFiles = storageFiles;
        if (StorageFiles.Count == 0)
            throw new InvalidOperationException("StorageFiles should not be 0 elements");

        var firstFileState = StorageFiles[0].FileState;
        var fileStates = StorageFiles.Select(x => x.FileState).ToList();
        if (!fileStates.All(x => x.CompareTo(firstFileState, true, true)))
            throw new InvalidOperationException("StorageFiles deduplication: " + string.Join(", ", fileStates));

        _singleBackupQuotaGb = singleBackupQuotaGb;
        _versionStates = versionStates;
        _actualFile = actualFile;
        _ignoreLastVersion = ignoreLastVersion;
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);

        var actualStorageFile = new StorageFile(StorageFiles.First());
        actualStorageFile.FileState.FileName = _actualFile;

        if (FileAlreadyInStorage(out var matchingFile))
        {
            LogDebug("Skipped because file is already is in storage.");
            IsSuccess = true;
            StorageFiles
                    .ForEach(x => x.SetStoragePropertiesFrom(matchingFile));
        }
        else if (_singleBackupQuotaGb.TryQuota(actualStorageFile.FileState.Size))
        {
            try
            {
                IsSuccess = _services.IncrementalBackupFileService.Upload(actualStorageFile);
                StorageFiles
                    .ToList()
                    .ForEach(x => x.SetStoragePropertiesFrom(actualStorageFile));
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

    private bool FileAlreadyInStorage([NotNullWhen(true)] out StorageFile? matchingStorageFile)
    {
        matchingStorageFile = null;
        var ignoreVersionsCount = _ignoreLastVersion ? 1 : 0;
        if (this._versionStates.Count <= ignoreVersionsCount)
        {
            return false;
        }

        var previousVersions = this._versionStates.Take(this._versionStates.Count - ignoreVersionsCount).ToArray();
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
