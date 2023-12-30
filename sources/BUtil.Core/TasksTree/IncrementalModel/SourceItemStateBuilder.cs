using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.State;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.TasksTree.IncrementalModel
{
    class SourceItemStateBuilder
    {
        public static List<SourceItemState> Build(IEnumerable<VersionState> versions, VersionState selectedVersion)
        {
            var orderedVersions = versions
                .OrderBy(x => x.BackupDateUtc)
                .ToList();
            var sourceItemStates = new List<SourceItemState>();

            return GetTreeViewFiles(orderedVersions, selectedVersion);
        }

        private static List<SourceItemState> GetTreeViewFiles(
            IEnumerable<VersionState> versions,
            VersionState selectedVersion)
        {
            var result = new List<SourceItemState>();

            var sourceItems = selectedVersion.SourceItemChanges
                .Select(a => a.SourceItem)
                .OrderBy(x => x.Target)
                .ToList();

            foreach (var sourceItem in sourceItems)
            {
                result.Add(new SourceItemState(sourceItem, BuildVersionFiles(versions, sourceItem, selectedVersion)));
            }

            return result;
        }
        private static List<FileState> BuildVersionFiles(
            IEnumerable<VersionState> versions,
            SourceItemV2 sourceItem,
            VersionState selectedVersion)
        {
            var state = new SourceItemState();

            List<StorageFile>? result = null;

            foreach (var versionState in versions)
            {
                var sourceItemChanges = versionState.SourceItemChanges.FirstOrDefault(x => x.SourceItem.CompareTo(sourceItem));
                if (sourceItemChanges == null)
                {
                    result = null;
                }
                else
                {
                    if (result == null)
                    {
                        result = sourceItemChanges.CreatedFiles.ToList();
                    }
                    else
                    {
                        result.AddRange(sourceItemChanges.CreatedFiles);
                        foreach (var deletedFile in sourceItemChanges.DeletedFiles)
                        {
                            var itemToRemove = result.First(x => x.FileState.FileName == deletedFile);
                            result.Remove(itemToRemove);
                        }
                        foreach (var updatedFile in sourceItemChanges.UpdatedFiles)
                        {
                            var itemToRemove = result.First(x => x.FileState.FileName == updatedFile.FileState.FileName);
                            result.Remove(itemToRemove);

                            result.Add(updatedFile);
                        }
                    }
                }

                if (versionState == selectedVersion)
                    break;
            }

            return (result ?? new List<StorageFile>())
                .Select(x => x.FileState)
                .ToList();
        }
    }
}
