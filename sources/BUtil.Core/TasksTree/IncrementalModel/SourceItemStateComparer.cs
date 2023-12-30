using BUtil.Core.State;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.TasksTree.IncrementalModel
{
    class SourceItemStateComparer
    {
        public static VersionState Compare(IEnumerable<SourceItemState> a, IEnumerable<SourceItemState> b)
        {
            var matchingBtoA = b
                .ToDictionary(x => x, x => (SourceItemState?)null);

            foreach (var item in a)
            {
                foreach (var pair in matchingBtoA)
                {
                    if (pair.Key.SourceItem.CompareTo(item.SourceItem))
                    {
                        matchingBtoA[pair.Key] = item;
                        break;
                    }
                }
            }

            var sourceItemChangesList = new List<SourceItemChanges>();
            foreach (var pair in matchingBtoA)
            {
                if (pair.Value == null)
                {
                    var addedSourceItem = pair.Key;
                    var sourceItemChanges = new SourceItemChanges(
                        addedSourceItem.SourceItem,
                        new List<string>(),
                        new List<StorageFile>(),
                        addedSourceItem.FileStates
                            .Select(x => new StorageFile(x))
                            .ToList()
                        );
                    sourceItemChangesList.Add(sourceItemChanges);
                }
                else
                {
                    var update = CompareSourceItemStates(pair.Value, pair.Key);
                    sourceItemChangesList.Add(update);
                }
            }

            var versionState = new VersionState(DateTime.UtcNow, sourceItemChangesList);
            return versionState;
        }

        private static SourceItemChanges CompareSourceItemStates(SourceItemState a, SourceItemState b)
        {
            var createdFiles = b.FileStates.ToList();
            var updatedFiles = new List<FileState>();
            var deletedFiles = a.FileStates.ToList();

            foreach (var bItem in b.FileStates)
            {
                foreach (var aItem in a.FileStates)
                {
                    if (aItem.FileName == bItem.FileName)
                    {
                        deletedFiles.Remove(aItem);
                        createdFiles.Remove(bItem);

                        if (!aItem.CompareTo(bItem))
                            updatedFiles.Add(bItem);

                        break;
                    }
                }
            }

            return new SourceItemChanges(a.SourceItem,
                deletedFiles.Select(x => x.FileName).ToList(),
                updatedFiles.Select(x => new StorageFile(x)).ToList(),
                createdFiles.Select(x => new StorageFile(x)).ToList());
        }
    }
}
