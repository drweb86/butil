using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.FileSystem;
using BUtil.Core.Hashing;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;

namespace BUtil.Core.State;

public class IncrementalBackupStateService(StorageSpecificServicesIoc services, ICachedHashService hashService)
{
    private readonly ILog _log = services.CommonServices.Log;
    private readonly StorageSpecificServicesIoc _services = services;
    private readonly ICachedHashService _hashService = hashService;
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { WriteIndented = true };

    public bool TryRead(string password, [NotNullWhen(true)] out IncrementalBackupState? state)
    {
        _log.WriteLine(LoggingEvent.Debug, $"Reading state");
        using var tempFolder = new TempFolder();
        string destFile = Path.Combine(tempFolder.Folder, "file.json");

        if (_services.Storage.Exists(IncrementalBackupModelConstants.BrotliAes256V1StateFile))
        {
            _services.ApplicationStorageService.Download(new StorageFile { StorageRelativeFileName = IncrementalBackupModelConstants.BrotliAes256V1StateFile, StorageMethod = StorageMethodNames.BrotliCompressedAes256Encrypted, StoragePassword = password, FileState = new FileState() { FileName = IncrementalBackupModelConstants.BrotliAes256V1StateFile } }, destFile);
            using var uncompressedFileStream = File.OpenRead(destFile);
            state = JsonSerializer.Deserialize<IncrementalBackupState>(uncompressedFileStream);
            return state != null;
        }

#pragma warning disable CS0612 // Type or member is obsolete
        if (_services.Storage.Exists(IncrementalBackupModelConstants.StorageIncrementalEncryptedCompressedStateFile))
        {
            throw new System.NotSupportedException($"Prevent data loss! Support for file {IncrementalBackupModelConstants.StorageIncrementalEncryptedCompressedStateFile} is dropped. Upgrade to the latest storage format using the version v.2024.12.16. Or recreate storage from scratch!");
        }
#pragma warning restore CS0612 // Type or member is obsolete


        state = new IncrementalBackupState();
        return true;
    }

    public StorageFile? Write(string password, IncrementalBackupState state)
    {
        _log.WriteLine(LoggingEvent.Debug, $"Writing state");
        if (_services.Storage.Exists(IncrementalBackupModelConstants.BrotliAes256V1StateFile))
            _services.Storage.Delete(IncrementalBackupModelConstants.BrotliAes256V1StateFile);

        using var tempFolder = new TempFolder();
        var jsonFile = Path.Combine(tempFolder.Folder, "state.json");
        using (var jsonFileWriter = File.OpenWrite(jsonFile))
            JsonSerializer.Serialize(jsonFileWriter, state, _jsonSerializerOptions);

        var brotliFile = Path.Combine(tempFolder.Folder, "state.brotli");
        _services.CommonServices.CompressionService.CompressBrotliFile(jsonFile, brotliFile);

        var aesFile = Path.Combine(tempFolder.Folder, "state.aes");
        _services.CommonServices.EncryptionService.EncryptAes256File(brotliFile, aesFile, password);

        var storageFile = new StorageFile
        {
            StorageMethod = StorageMethodNames.BrotliCompressedAes256Encrypted,
            StorageIntegrityMethod = StorageIntegrityMethod.Sha512,
            StorageRelativeFileName = IncrementalBackupModelConstants.BrotliAes256V1StateFile
        };

        var uploadResult = _services.Storage.Upload(aesFile, storageFile.StorageRelativeFileName);
        storageFile.StorageFileName = uploadResult.StorageFileName;
        storageFile.StorageFileNameSize = uploadResult.StorageFileNameSize;
        storageFile.StorageIntegrityMethodInfo = _hashService.GetSha512(aesFile, false);

        return storageFile;
    }
}
