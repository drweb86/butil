
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.State;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BUtil.Core.Misc;

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

    public static List<StorageFile> BuildVersionFiles(IncrementalBackupState state, SourceItemV2 sourceItem, VersionState selectedVersion)
    {
        List<StorageFile>? result = null;

        foreach (var versionState in state.VersionStates)
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

        result.EnsureNotNull(string.Empty);

        return result!
            .OrderBy(x => x.FileState.FileName)
            .ToList();
    }
}
