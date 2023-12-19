using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUtil.Core.TasksTree.IncrementalModel
{
    internal class DeleteVersionUtil
    {
        public static void DeleteVersion(IncrementalBackupState state, VersionState versionToDelete, 
            out IEnumerable<string> storageFilesToDelete)
        {
            var storageFilesToDeleteList = new List<string>();
            storageFilesToDelete = storageFilesToDeleteList;


            var orderedVersions = state.VersionStates
                .OrderByDescending(x => x.BackupDateUtc)
                .ToList();
            var lastVersion = orderedVersions.First();

            // if last version is deleted
            if (lastVersion == versionToDelete)
            {
                state.VersionStates.Remove(versionToDelete);
                versionToDelete.SourceItemChanges
                    .ToList()
                    .ForEach(sourceItemChange =>
                    {
                        storageFilesToDeleteList.AddRange(sourceItemChange.UpdatedFiles.Select(x => x.StorageFileName).ToList());
                        storageFilesToDeleteList.AddRange(sourceItemChange.CreatedFiles.Select(x => x.StorageFileName).ToList());
                    });
            }
            else
            {
                throw new NotSupportedException();
            }

            RemoveUnchangedFiles(state, storageFilesToDeleteList);
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
