using BUtil.Core.Misc;
using System.Collections.Generic;
using BUtil.Core.State;
using System.Linq;

namespace butil_ui.Controls
{
    public class VersionViewItem
    {
        public VersionViewItem(VersionState version)
        {
            var totalSize = GetSizeOfVersion(version);
            Title = $"{version.BackupDateUtc} ({SizeHelper.BytesToString(totalSize)})";
            Version = version;
        }

        public string Title { get; }
        public VersionState Version { get; }

        private static long GetSizeOfVersion(VersionState version)
        {
            var versionFolder = SourceItemHelper.GetVersionFolder(version.BackupDateUtc);
            return version.SourceItemChanges
                .SelectMany(x =>
                {
                    var storageFiles = new List<StorageFile>();
                    storageFiles.AddRange(x.UpdatedFiles);
                    storageFiles.AddRange(x.CreatedFiles);
                    return storageFiles;
                })
                .Where(x => x.StorageRelativeFileName.StartsWith(versionFolder))
                .GroupBy(x => x.StorageFileName)
                .Select(x => x.First().StorageFileNameSize)
                .Sum();
        }
    }
}
