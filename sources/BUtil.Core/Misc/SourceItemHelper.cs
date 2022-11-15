using BUtil.Core.BackupModels;
using BUtil.Core.Options;
using BUtil.Core.State;
using System;
using System.IO;

namespace BUtil.Core.Misc
{
    public static class SourceItemHelper
    {
        public static string GetSourceItemDirectory(SourceItem sourceItem)
        {
            var dir = sourceItem.IsFolder ?
                sourceItem.Target :
                Path.GetDirectoryName(sourceItem.Target);

            return dir.TrimEnd(Path.DirectorySeparatorChar);
        }

        public static string GetSourceItemRelativeFileName(string sourceItemDirectory, FileState fileState)
        {
            return fileState.FileName.Substring(sourceItemDirectory.Length + 1);
        }

        public static string GetUnencryptedUncompressedStorageRelativeFileName(
            VersionState versionState,
            SourceItem sourceItem,
            string sourceItemRelativeFileName)
        {
            var readableDate = versionState.BackupDateUtc.ToString("yyyy-MM-dd-T-HH-mm-ss");

            return Path.Combine(
                readableDate,
                GetUnencryptedUncompressedSourceItemTargetFriendlyName(sourceItem),
                sourceItemRelativeFileName);
        }

        public static string GetCompressedStorageRelativeFileName(VersionState versionState)
        {
            var readableDate = versionState.BackupDateUtc.ToString("yyyy-MM-dd-T-HH-mm-ss");

            return Path.Combine(
                readableDate,
                $"{Guid.NewGuid()}.7z");
        }

        public static string GetUnencryptedUncompressedSourceItemTargetFriendlyName(SourceItem sourceItem)
        {
            var lastPart = Path.GetFileName(sourceItem.Target);

            return $"{lastPart}_{sourceItem.Id}";
        }
    }
}
