using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.FileSystem;
using BUtil.Core.Hashing;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.TasksTree.IncrementalModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace BUtil.Core.State;

public class IncrementalBackupFileService
{
    private readonly ILog _log;
    private readonly IHashService _hashService;
    private readonly StorageSpecificServicesIoc _services;

    public IncrementalBackupFileService(IHashService hashService, StorageSpecificServicesIoc services)
    {
        _log = services.Log;
        _hashService = hashService;
        _services = services;
    }

    public bool Download(SourceItemV2 sourceItem, StorageFile storageFile, string destinationFolder)
    {
        _log.WriteLine(LoggingEvent.Debug, $"Storage: downloading \"{storageFile.FileState.FileName}\"");

        var sourceItemDiectory = SourceItemHelper.GetSourceItemDirectory(sourceItem);
        var sourceItemRelativeFileName = SourceItemHelper.GetSourceItemRelativeFileName(sourceItemDiectory, storageFile.FileState);
        var destinationFileName = Path.Combine(destinationFolder, sourceItemRelativeFileName);
        var destinationDir = Path.GetDirectoryName(destinationFileName);
        if (destinationDir != null && !Directory.Exists(destinationDir))
            Directory.CreateDirectory(destinationDir);

        if (File.Exists(destinationFileName))
        {
            var sha512 = _hashService.GetSha512(destinationFileName, false);
            var size = new FileInfo(destinationFileName).Length;

            if (storageFile.FileState.Sha512 == sha512 && storageFile.FileState.Size == size)
            {
                _log.WriteLine(LoggingEvent.Debug, $"Skip {destinationFileName} because size and SHA-512 match");
                return true;
            }
        }

        using var tempFolder = new TempFolder();
        var tempArchive = Path.Combine(tempFolder.Folder, "archive.7z");
        
        // to make recovery twice faster we extract to folder near destination
        using var tempFolderAtDestination = new TempFolder(destinationFolder);
        var extractedFolder = Path.Combine(tempFolderAtDestination.Folder, "Extracted");
        _services.Storage.Download(storageFile.StorageRelativeFileName, tempArchive);
        var archiver = PlatformSpecificExperience.Instance.GetArchiver(_log);
        // file can be renamed in real life.
        if (!archiver.Extract(tempArchive, storageFile.StoragePassword, extractedFolder))
        {
            _log.WriteLine(LoggingEvent.Error, $"Extracting \"{storageFile.FileState.FileName}\" failed");
            return false;
        }
        var sourceFile = Directory.GetFiles(extractedFolder).Single();
        if (File.Exists(destinationFileName))
            File.Delete(destinationFileName);
        File.Move(sourceFile, destinationFileName);

        return true;
    }

    public bool Upload(StorageFile storageFile)
    {
        using var tempFolder = new TempFolder();
        var archiveFile = Path.Combine(tempFolder.Folder, "archive.7z");

        var encryptionEnabled = storageFile.StorageMethod == StorageMethodNames.SevenZipEncrypted;
        if (encryptionEnabled)
        {
            storageFile.StoragePassword = RandomString();
        }

        var archiver = PlatformSpecificExperience.Instance.GetArchiver(_log);
        if (!archiver.CompressFile(
            storageFile.FileState.FileName,
            storageFile.StoragePassword,
            archiveFile))
        {
            _log.WriteLine(LoggingEvent.Error, $"Error compressing \"{storageFile.FileState.FileName}\"");
            return false;
        }

        var uploadResult = _services.Storage.Upload(archiveFile, storageFile.StorageRelativeFileName);
        storageFile.StorageFileNameSize = uploadResult.StorageFileNameSize;
        storageFile.StorageFileName = uploadResult.StorageFileName;
        storageFile.StorageIntegrityMethod = StorageIntegrityMethod.Sha512;
        storageFile.StorageIntegrityMethodInfo = _hashService.GetSha512(archiveFile, false);
        return true;
    }
    private static string RandomString()
    {
        using var generator = RandomNumberGenerator.Create();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789&?%$@";
        var bytes = new byte[255];
        generator.GetBytes(bytes);

        var randomChars = bytes
            .Select(x => chars[x % chars.Length])
            .ToArray();
        return new string(randomChars);
    }
}
