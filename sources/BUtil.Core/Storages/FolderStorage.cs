﻿
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.FileSystem;
using BUtil.Core.Logs;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;

namespace BUtil.Core.Storages;

public class FolderStorage : StorageBase<FolderStorageSettingsV2>
{
    public FolderStorage(ILog log, FolderStorageSettingsV2 settings)
        : base(log, settings)
    {
        if (string.IsNullOrWhiteSpace(Settings.DestinationFolder))
            throw new InvalidDataException(BUtil.Core.Localization.Resources.DirectoryStorage_Field_Directory_Validation_Empty);

        Mount();
    }

    private readonly object _uploadLock = new();
    public override IStorageUploadResult Upload(string sourceFile, string relativeFileName)
    {
        lock (_uploadLock) // because we're limited by upload speed and Samba has limit of 6 parallel uploads usually
        {
            var destinationFile = Path.Combine(Settings.DestinationFolder, relativeFileName);

            Log.WriteLine(LoggingEvent.Debug, $"Copying \"{sourceFile}\" to \"{destinationFile}\"");

            var destinationDirectory = Path.GetDirectoryName(destinationFile) ?? string.Empty;
            FileHelper.EnsureFolderCreated(destinationDirectory);

            Copy(sourceFile, destinationFile);

            return new IStorageUploadResult
            {
                StorageFileName = destinationFile,
                StorageFileNameSize = new FileInfo(destinationFile).Length,
            };
        }
    }

    private void Mount()
    {
        Log.WriteLine(LoggingEvent.Debug, $"Mount");
        if (!string.IsNullOrWhiteSpace(this.Settings.MountPowershellScript))
        {
            if (PlatformSpecificExperience.Instance.SupportManager.CanLaunchScripts &&
                !PlatformSpecificExperience.Instance.SupportManager.LaunchScript(Log, this.Settings.MountPowershellScript, "***"))
                throw new InvalidOperationException($"Cannot mount");
        }
    }

    private void Unmount()
    {
        Log.WriteLine(LoggingEvent.Debug, $"Unmount");
        if (!string.IsNullOrWhiteSpace(this.Settings.UnmountPowershellScript))
        {
            if (PlatformSpecificExperience.Instance.SupportManager.CanLaunchScripts &&
                !PlatformSpecificExperience.Instance.SupportManager.LaunchScript(Log, this.Settings.UnmountPowershellScript, "***"))
                throw new InvalidOperationException($"Cannot unmount");
        }
    }

    public override string? Test()
    {
        /*if (!Directory.Exists(Settings.DestinationFolder))
            return string.Format(Localization.Resources.DirectoryStorage_Field_Directory_Validation_NotFound, Settings.DestinationFolder);
        if (!Path.IsPathFullyQualified(Settings.DestinationFolder))
            return string.Format(Localization.Resources.DirectoryStorage_Field_Directory_Validation_NotFound, Settings.DestinationFolder);

        return null;*/


        if (!Directory.Exists(Settings.DestinationFolder))
            return string.Format(Localization.Resources.DirectoryStorage_Field_Directory_Validation_NotFound, Settings.DestinationFolder);
        if (!Path.IsPathFullyQualified(Settings.DestinationFolder))
            return string.Format(Localization.Resources.DirectoryStorage_Field_Directory_Validation_NotFound, Settings.DestinationFolder);

        return null;
    }

    public override bool Exists(string relativeFileName)
    {
        var fullPathName = Path.Combine(Settings.DestinationFolder, relativeFileName);

        return File.Exists(fullPathName);
    }

    public override void Delete(string relativeFileName)
    {
        var fullPathName = Path.Combine(Settings.DestinationFolder, relativeFileName);

        if (File.Exists(fullPathName))
            File.Delete(fullPathName);
    }

    public override void DeleteFolder(string relativeFolderName)
    {
        var fullPathName = string.IsNullOrWhiteSpace(relativeFolderName)
            ? Settings.DestinationFolder
            : Path.Combine(Settings.DestinationFolder, relativeFolderName);

        if (Directory.Exists(fullPathName))
            Directory.Delete(fullPathName, true);
    }

    public override string[] GetFolders(string relativeFolderName, string? mask = null)
    {
        var fullPathName = string.IsNullOrWhiteSpace(relativeFolderName)
            ? Settings.DestinationFolder
            : Path.Combine(Settings.DestinationFolder, relativeFolderName);

        return Directory
            .GetDirectories(fullPathName, mask ?? "*.*")
            .Select(x => x[fullPathName.Length..])
            .Select(x => x.Trim(['\\', '/']))
            .ToArray();
    }

    public override void Download(string relativeFileName, string targetFileName)
    {
        var file = Path.Combine(Settings.DestinationFolder, relativeFileName);
        Copy(file, targetFileName);
    }

    public static void Copy(string inputFile, string outputFilePath)
    {
        int bufferSize = 16 * 1024 * 1024;

        using var inputFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, bufferSize);
        using var outputFileStream = new FileStream(outputFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, bufferSize);
        outputFileStream.SetLength(inputFileStream.Length);
        int bytesRead = -1;
        byte[] bytes = new byte[bufferSize];

        while ((bytesRead = inputFileStream.Read(bytes, 0, bufferSize)) > 0)
        {
            outputFileStream.Write(bytes, 0, bytesRead);
        }
    }

    public override string[] GetFiles(string? relativeFolderName = null, SearchOption option = SearchOption.TopDirectoryOnly)
    {
        if (relativeFolderName != null)
        {
            if (relativeFolderName.Contains(".."))
                throw new SecurityException(nameof(relativeFolderName));
        }

        var actualFolder = relativeFolderName == null ?
            Settings.DestinationFolder :
            Path.Combine(Settings.DestinationFolder, relativeFolderName);

        return Directory
            .GetFiles(actualFolder, "*.*", option)
            .Select(x => x[actualFolder.Length..])
            .Select(x => x.Trim(['\\', '/']))
            .ToArray();
    }

    public override DateTime GetModifiedTime(string relativeFileName)
    {
        var actualFile = Path.Combine(Settings.DestinationFolder, relativeFileName);

        return File.GetLastWriteTime(actualFile);
    }

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
    public override void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
    {
        Unmount();
    }

    public override void Move(string fromRelativeFileName, string toRelativeFileName)
    {
        var fromPath = Path.Combine(Settings.DestinationFolder, fromRelativeFileName);
        var toPath = Path.Combine(Settings.DestinationFolder, toRelativeFileName);

        var destinationDirectory = Path.GetDirectoryName(toPath) ?? string.Empty;
        FileHelper.EnsureFolderCreated(destinationDirectory);

        File.Move(fromPath, toPath);
    }
}
