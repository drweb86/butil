using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.FileSystem;
using BUtil.Core.Hashing;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace BUtil.Core.State;

public class ApplicationStorageService(ICachedHashService hashService, StorageSpecificServicesIoc services)
{
    private readonly ILog _log = services.CommonServices.Log;
    private readonly ICachedHashService _hashService = hashService;
    private readonly StorageSpecificServicesIoc _services = services;

    public void Download(StorageFile storageFile, string destinationFileName)
    {
        _log.WriteLine(LoggingEvent.Debug, $"Storage: downloading \"{storageFile.FileState.FileName}\"");

        var destinationFolder = Path.GetDirectoryName(destinationFileName)!;
        FileHelper.EnsureFolderCreatedForFile(destinationFileName);
        DownloadInternal(storageFile, destinationFileName, destinationFolder);
    }

    public void Download(SourceItemV2 sourceItem, StorageFile storageFile, string destinationFolder)
    {
        _log.WriteLine(LoggingEvent.Debug, $"Storage: downloading \"{storageFile.FileState.FileName}\"");

        var sourceItemDiectory = SourceItemHelper.GetSourceItemDirectory(sourceItem);
        var sourceItemRelativeFileName = SourceItemHelper.GetSourceItemRelativeFileName(sourceItemDiectory, storageFile.FileState);
        var destinationFileName = Path.Combine(destinationFolder, sourceItemRelativeFileName);
        FileHelper.EnsureFolderCreatedForFile(destinationFileName);
        DownloadInternal(storageFile, destinationFileName, destinationFolder);
    }

    private bool IsFileAlreadyExists(StorageFile storageFile, string destinationFileName)
    {
        if (!File.Exists(destinationFileName))
            return false;

        var sha512 = _hashService.GetSha512(destinationFileName, false);
        var size = new FileInfo(destinationFileName).Length;

        if (storageFile.FileState != null && (storageFile.FileState.Sha512 == sha512 && storageFile.FileState.Size == size))
        {
            _log.WriteLine(LoggingEvent.Debug, $"Skip {destinationFileName} because size and SHA-512 match");
            return true;
        }
        return false;
    }

    private void VerifyAndMoveFile(StorageFile storageFile, string resultFile, string destinationFileName)
    {
        var sha512 = _services.CommonServices.CachedHashService.GetSha512(resultFile, false);
        if (!string.IsNullOrWhiteSpace(storageFile.FileState.Sha512) && !string.Equals(storageFile.FileState.Sha512, sha512, StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException($"Downloaded content for \"{destinationFileName}\" is invalid: SHAv2 512 of file is {sha512}, but expected SHAv2 512 is {storageFile.FileState.Sha512}.");

        var size = new FileInfo(resultFile).Length;
        if (storageFile.FileState.Size != 0 && storageFile.FileState.Size != size)
            throw new InvalidDataException($"Downloaded content for \"{destinationFileName}\" is invalid: size of file is {size}, but expected size is {storageFile.FileState.Size}.");

        File.Move(resultFile, destinationFileName, true);

        if (storageFile.FileState.LastWriteTimeUtc != default)
            File.SetLastWriteTimeUtc(destinationFileName, storageFile.FileState.LastWriteTimeUtc);
    }

    private void DownloadInternal(StorageFile storageFile, string destinationFileName, string destinationFolder)
    {
        if (IsFileAlreadyExists(storageFile, destinationFileName))
            return;
        
        switch (storageFile.StorageMethod)
        {
            case StorageMethodNames.Aes256Encrypted:
                ExtractAes256Encrypted(storageFile, destinationFolder, destinationFileName);
                break;
            case StorageMethodNames.BrotliCompressedAes256Encrypted:
                ExtractBrotliCompressedAes256Encrypted(storageFile, destinationFolder, destinationFileName);
                break;
            default:
                throw new ArgumentOutOfRangeException(storageFile.StorageMethod);
        }
    }

    private void ExtractBrotliCompressedAes256Encrypted(StorageFile storageFile, string destinationFolder, string destinationFileName)
    {
        using var tempFolder = new TempFolder(destinationFolder);

        var downloadedFile = Path.Combine(tempFolder.Folder, "file.brotliAes256");
        _services.Storage.Download(storageFile.StorageRelativeFileName, downloadedFile);

        var brotliFile = Path.Combine(tempFolder.Folder, "file.brotli");
        _services.CommonServices.EncryptionService.DecryptAes256File(downloadedFile, brotliFile, storageFile.StoragePassword);

        var file = Path.Combine(tempFolder.Folder, "file");
        _services.CommonServices.CompressionService.DecompressBrotliFile(brotliFile, file);

        VerifyAndMoveFile(storageFile, file, destinationFileName);
    }

    private void ExtractAes256Encrypted(StorageFile storageFile, string destinationFolder, string destinationFileName)
    {
        using var tempFolder = new TempFolder(destinationFolder);

        var downloadedFile = Path.Combine(tempFolder.Folder, "file.aes256");
        _services.Storage.Download(storageFile.StorageRelativeFileName, downloadedFile);

        var file = Path.Combine(tempFolder.Folder, "file");
        _services.CommonServices.EncryptionService.DecryptAes256File(downloadedFile, file, storageFile.StoragePassword);

        VerifyAndMoveFile(storageFile, file, destinationFileName);
    }

    public bool Upload(StorageFile storageFile)
    {
        using var tempFolder = new TempFolder();
        var file = Path.Combine(tempFolder.Folder, "file");

        storageFile.StoragePassword = RandomString();
        _services.CommonServices.EncryptionService.EncryptAes256File(
            storageFile.FileState.FileName,
            file,
            storageFile.StoragePassword);

        var uploadResult = _services.Storage.Upload(file, storageFile.StorageRelativeFileName);
        
        storageFile.StorageMethod = StorageMethodNames.Aes256Encrypted;
        storageFile.StorageFileNameSize = uploadResult.StorageFileNameSize;
#pragma warning disable CS0618 // Type or member is obsolete
        storageFile.StorageFileName = uploadResult.StorageFileName;
#pragma warning restore CS0618 // Type or member is obsolete
        storageFile.StorageIntegrityMethod = StorageIntegrityMethod.Sha512;
        storageFile.StorageIntegrityMethodInfo = _hashService.GetSha512(file, false);
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
