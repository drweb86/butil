﻿using BUtil.Core.State;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BUtil.Core.TasksTree.IncrementalModel;

public class DeleteVersionUtil
{
    private static void AddUpdatedCreatedStorageFiles(
        VersionState version,
        List<string> storageRelativeFileNames)
    {
        version.SourceItemChanges
            .ToList()
            .ForEach(sourceItemChange => AddUpdatedCreatedStorageFiles(sourceItemChange, storageRelativeFileNames));
    }

    private static void AddUpdatedCreatedStorageFiles(
        SourceItemChanges sourceItemChange,
        List<string> storageRelativeFileNames)
    {
        storageRelativeFileNames.AddRange(sourceItemChange.UpdatedFiles.Select(x => x.StorageRelativeFileName).ToList());
        storageRelativeFileNames.AddRange(sourceItemChange.CreatedFiles.Select(x => x.StorageRelativeFileName).ToList());
    }

    public static void DeleteVersion(IncrementalBackupState state, VersionState versionToDelete,
        out IEnumerable<string> storageRelativeFileNamesToDelete)
    {
        var storageRelativeFileNamesToDeleteList = new List<string>();
        storageRelativeFileNamesToDelete = storageRelativeFileNamesToDeleteList;

        var orderedVersionsDesc = state.VersionStates
            .OrderByDescending(x => x.BackupDateUtc)
            .ToList();
        var recentVersion = orderedVersionsDesc.First();
        var initialVersion = orderedVersionsDesc.Last();

        if (state.VersionStates.Count == 1)
        {
            throw new System.NotSupportedException("Not supported!");
            // DeleteSingleVersion(state, versionToDelete, storageFilesToDeleteList);
        }
        else if (recentVersion == versionToDelete)
        {
            throw new System.NotSupportedException("Not supported!");
            // DeleteRecentVersion(state, versionToDelete, storageFilesToDeleteList);
        }
        else
        {
            var toVersion = orderedVersionsDesc[orderedVersionsDesc.IndexOf(versionToDelete) - 1];
            DeletePreviousVersion(state, versionToDelete, toVersion, storageRelativeFileNamesToDeleteList);
        }

        RemoveUnchangedFiles(state, storageRelativeFileNamesToDeleteList);
    }

    private static void DeletePreviousVersion(
        IncrementalBackupState state,
        VersionState fromVersion,
        VersionState toVersion,
        List<string> storageRelativeFileNamesToDeleteList)
    {
        state.VersionStates.Remove(fromVersion);

        foreach (var fromSourceItemChanges in fromVersion.SourceItemChanges)
        {
            var toSourceItemChanges = toVersion.SourceItemChanges.SingleOrDefault(x => x.SourceItem.CompareTo(fromSourceItemChanges.SourceItem));
            if (toSourceItemChanges == null) // source item was deleted in new version.
            {
                AddUpdatedCreatedStorageFiles(fromSourceItemChanges, storageRelativeFileNamesToDeleteList);
            }
            else // source item was changed.
            {
                UpdateSourceItemChanges(fromSourceItemChanges, toSourceItemChanges, storageRelativeFileNamesToDeleteList);
            }
        }
    }

    private static bool TryFindStorageFile(
        List<StorageFile> files,
        string file,
        [NotNullWhen(true)]
        out StorageFile? storageFile)
    {
        foreach (var toCreatedFile in files)
        {
            if (toCreatedFile.FileState.FileName == file)
            {
                storageFile = toCreatedFile;
                return true;
            }
        }
        storageFile = null;
        return false;
    }

    private static bool TryFindStorageFile(
        List<string> files,
        StorageFile file)
    {
        foreach (var toCreatedFile in files)
        {
            if (toCreatedFile == file.FileState.FileName)
            {
                return true;
            }
        }
        return false;
    }

    private static bool TryFindStorageFile(
        List<StorageFile> files,
        StorageFile file,
        [NotNullWhen(true)]
        out StorageFile? storageFile)
    {
        foreach (var toCreatedFile in files)
        {
            if (toCreatedFile.FileState.FileName == file.FileState.FileName)
            {
                storageFile = toCreatedFile;
                return true;
            }
        }
        storageFile = null;
        return false;
    }

    private static void UpdateSourceItemChanges(
        SourceItemChanges from,
        SourceItemChanges to,
        List<string> storageRelativeFileNamesToDeleteList)
    {
        foreach (var fromDeletedFile in from.DeletedFiles)
        {
            // deleted file is created.
            if (TryFindStorageFile(to.CreatedFiles, fromDeletedFile, out var storageFile))
            {
                to.CreatedFiles.Remove(storageFile);
                to.UpdatedFiles.Add(storageFile);
            }
            else
            {
                to.DeletedFiles.Add(fromDeletedFile);
            }
        }

        foreach (var createdFile in from.CreatedFiles)
        {
            // Created=>Updated ? Change status to Created, delete old file
            if (TryFindStorageFile(to.UpdatedFiles, createdFile, out var storageFile))
            {
                to.UpdatedFiles.Remove(storageFile);
                to.CreatedFiles.Add(storageFile);
                storageRelativeFileNamesToDeleteList.Add(createdFile.StorageRelativeFileName);
            }
            // Created=>Deleted ? Clear deleted, delete old file
            else if (TryFindStorageFile(to.DeletedFiles, createdFile))
            {
                to.DeletedFiles.Remove(createdFile.FileState.FileName);
                storageRelativeFileNamesToDeleteList.Add(createdFile.StorageRelativeFileName);
            }
            else
            {
                to.CreatedFiles.Add(createdFile);
            }
        }

        foreach (var updatedFile in from.UpdatedFiles)
        {
            // Updated=>Updated ? delete old file
            if (TryFindStorageFile(to.UpdatedFiles, updatedFile, out var storageFile))
            {
                storageRelativeFileNamesToDeleteList.Add(updatedFile.StorageRelativeFileName);
            }
            // Updated=>Deleted ? delete old file
            else if (TryFindStorageFile(to.DeletedFiles, updatedFile))
            {
                storageRelativeFileNamesToDeleteList.Add(updatedFile.StorageRelativeFileName);
            }
            else
            {
                to.UpdatedFiles.Add(updatedFile);
            }
        }
    }

    private static void DeleteRecentVersion(
        IncrementalBackupState state,
        VersionState versionToDelete,
        List<string> storageFilesToDeleteList)
    {
        state.VersionStates.Remove(versionToDelete);
        AddUpdatedCreatedStorageFiles(versionToDelete, storageFilesToDeleteList);
        state.LastSourceItemStates = SourceItemStateBuilder.Build(state.VersionStates, state.VersionStates.First());
    }

    private static void DeleteSingleVersion(
        IncrementalBackupState state,
        VersionState versionToDelete,
        List<string> storageFilesToDeleteList)
    {
        state.VersionStates.Remove(versionToDelete);
        AddUpdatedCreatedStorageFiles(versionToDelete, storageFilesToDeleteList);
        state.LastSourceItemStates.Clear();
    }

    private static void RemoveUnchangedFiles(IncrementalBackupState state, List<string> storageRelativeFileNamesToDeleteList)
    {
        foreach (var version in state.VersionStates)
        {
            foreach (var sourceItemChange in version.SourceItemChanges)
            {
                foreach (var file in sourceItemChange.UpdatedFiles)
                {
                    var storageFileName = file.StorageRelativeFileName;
                    if (storageRelativeFileNamesToDeleteList.Contains(storageFileName))
                        storageRelativeFileNamesToDeleteList.Remove(storageFileName);
                }

                foreach (var file in sourceItemChange.CreatedFiles)
                {
                    var storageFileName = file.StorageRelativeFileName;
                    if (storageRelativeFileNamesToDeleteList.Contains(storageFileName))
                        storageRelativeFileNamesToDeleteList.Remove(storageFileName);
                }
            }
        }
    }
}
