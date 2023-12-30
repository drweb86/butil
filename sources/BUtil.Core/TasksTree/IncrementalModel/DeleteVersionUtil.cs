using BUtil.Core.State;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BUtil.Core.TasksTree.IncrementalModel
{
    internal class DeleteVersionUtil
    {
        private static void AddUpdatedCreatedStorageFiles(
            VersionState version,
            List<string> storageFileNames)
        {
            version.SourceItemChanges
                .ToList()
                .ForEach(sourceItemChange => AddUpdatedCreatedStorageFiles(sourceItemChange, storageFileNames));
        }

        private static void AddUpdatedCreatedStorageFiles(
            SourceItemChanges sourceItemChange,
            List<string> storageFileNames)
        {
            storageFileNames.AddRange(sourceItemChange.UpdatedFiles.Select(x => x.StorageFileName).ToList());
            storageFileNames.AddRange(sourceItemChange.CreatedFiles.Select(x => x.StorageFileName).ToList());
        }

        public static void DeleteVersion(IncrementalBackupState state, VersionState versionToDelete, 
            out IEnumerable<string> storageFilesToDelete)
        {
            var storageFilesToDeleteList = new List<string>();
            storageFilesToDelete = storageFilesToDeleteList;

            var orderedVersions = state.VersionStates
                .OrderByDescending(x => x.BackupDateUtc)
                .ToList();
            var lastVersion = orderedVersions.First();
            var firstVersion = orderedVersions.Last();

            if (state.VersionStates.Count == 1)
            {
                DeleteSingleVersion(state, versionToDelete, storageFilesToDeleteList);
            }
            else if (lastVersion == versionToDelete) // if last version is deleted
            {
                DeleteLastVersion(state, versionToDelete, storageFilesToDeleteList);
            }
            else
            {
                var toVersion = orderedVersions[orderedVersions.IndexOf(versionToDelete) - 1];
                DeletePreviousVersion(state, versionToDelete, toVersion, storageFilesToDeleteList);
            }

            RemoveUnchangedFiles(state, storageFilesToDeleteList);
        }

        private static void DeletePreviousVersion(
            IncrementalBackupState state,
            VersionState fromVersion,
            VersionState toVersion,
            List<string> storageFilesToDeleteList)
        {
            state.VersionStates.Remove(fromVersion);

            foreach (var fromSourceItemChanges in fromVersion.SourceItemChanges)
            {
                var toSourceItemChanges = toVersion.SourceItemChanges.SingleOrDefault(x => x.SourceItem.CompareTo(fromSourceItemChanges.SourceItem));
                if (toSourceItemChanges == null) // source item was deleted in new version.
                {
                    AddUpdatedCreatedStorageFiles(fromSourceItemChanges, storageFilesToDeleteList);
                }
                else // source item was changed.
                {
                    UpdateSourceItemChanges(fromSourceItemChanges, toSourceItemChanges, storageFilesToDeleteList);
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
            List<string> storageFilesToDeleteList)
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
                    storageFilesToDeleteList.Add(createdFile.StorageFileName);
                } 
                // Created=>Deleted ? Clear deleted, delete old file
                else if (TryFindStorageFile(to.DeletedFiles, createdFile))
                {
                    to.DeletedFiles.Remove(createdFile.FileState.FileName);
                    storageFilesToDeleteList.Add(createdFile.StorageFileName);
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
                    storageFilesToDeleteList.Add(updatedFile.StorageFileName);
                }
                // Updated=>Deleted ? delete old file
                else if (TryFindStorageFile(to.DeletedFiles, updatedFile))
                {
                    storageFilesToDeleteList.Add(updatedFile.StorageFileName);
                }
                else
                {
                    to.UpdatedFiles.Add(updatedFile);
                }
            }
        }

        private static void DeleteLastVersion(
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

        private static void RemoveUnchangedFiles(IncrementalBackupState state, List<string> storageFileNamesToDelete)
        {
            foreach (var version in state.VersionStates)
            {
                foreach (var sourceItemChange in version.SourceItemChanges)
                {
                    foreach(var file in  sourceItemChange.UpdatedFiles)
                    {
                        var storageFileName = file.StorageFileName;
                        if (storageFileNamesToDelete.Contains(storageFileName))
                            storageFileNamesToDelete.Remove(storageFileName);
                    }

                    foreach (var file in sourceItemChange.CreatedFiles)
                    {
                        var storageFileName = file.StorageFileName;
                        if (storageFileNamesToDelete.Contains(storageFileName))
                            storageFileNamesToDelete.Remove(storageFileName);
                    }
                }
            }
        }
    }
}
