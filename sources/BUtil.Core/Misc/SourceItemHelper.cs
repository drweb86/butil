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

        public static string GetVersionFolder(
            DateTime backupDateUtc)
        {
            return backupDateUtc.ToString("yyyy-MM-dd-T-HH-mm-ss");
        }

        public static string GetVersionFolderMask()
        {
            return "????-??-??-T-??-??-??";
        }

        public static string GetCompressedStorageRelativeFileName(VersionState versionState)
        {
            var readableDate = GetVersionFolder(versionState.BackupDateUtc);

            return Path.Combine(
                readableDate,
                $"{Guid.NewGuid()}.7z");
        }
    }
}
