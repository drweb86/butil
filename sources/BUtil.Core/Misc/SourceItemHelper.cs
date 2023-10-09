
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.State;
using System;
using System.IO;

namespace BUtil.Core.Misc
{
    public static class SourceItemHelper
    {
        public static string GetFriendlyFileName(SourceItemV2 item, string fileName)
        {
            if (!item.IsFolder)
                return fileName;
            
            var target = item.Target.TrimEnd(new char[] { '\\', '/' });
            return fileName.Substring(target.Length + 1);
        }

        public static string GetSourceItemDirectory(SourceItemV2 sourceItem)
        {
            var dir = sourceItem.IsFolder ?
                sourceItem.Target :
                Path.GetDirectoryName(sourceItem.Target) ?? throw new InvalidDataException(sourceItem.Target);

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
