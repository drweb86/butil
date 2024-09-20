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

public class IncrementalBackupStateService(StorageSpecificServicesIoc services, IHashService hashService)
{
    private readonly ILog _log = services.CommonServices.Log;
    private readonly StorageSpecificServicesIoc _services = services;
    private readonly IHashService _hashService = hashService;
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { WriteIndented = true };

    public bool TryRead(string password, [NotNullWhen(true)] out IncrementalBackupState? state)
    {
        _log.WriteLine(LoggingEvent.Debug, $"Reading state");
        using var tempFolder = new TempFolder();
        if (_services.Storage.Exists(IncrementalBackupModelConstants.StorageIncrementalEncryptedCompressedStateFile))
        {
            _log.WriteLine(LoggingEvent.Debug, $"Reading encrypted compressed state");
            var destFile = Path.Combine(tempFolder.Folder, IncrementalBackupModelConstants.StorageIncrementalEncryptedCompressedStateFile);
            _services.Storage.Download(IncrementalBackupModelConstants.StorageIncrementalEncryptedCompressedStateFile, destFile);
            var archiver = PlatformSpecificExperience.Instance.GetArchiver(_log);
            if (!archiver.Extract(destFile, password, tempFolder.Folder))
            {
                _log.WriteLine(LoggingEvent.Error, $"Failed to read state");
                state = default;
                return false;
            }

            var uncompressedFile = Path.Combine(tempFolder.Folder, IncrementalBackupModelConstants.StorageIncrementedNonEncryptedNonCompressedStateFile);
            using var uncompressedFileStream = File.OpenRead(uncompressedFile);
            state = JsonSerializer.Deserialize<IncrementalBackupState>(uncompressedFileStream);
            return state != null;
        }

        if (_services.Storage.Exists(IncrementalBackupModelConstants.BrotliAes256V1StateFile))
        {
            _log.WriteLine(LoggingEvent.Debug, $"Reading brotli aes256 v1 state file");
            var destFile = Path.Combine(tempFolder.Folder, IncrementalBackupModelConstants.StorageIncrementalEncryptedCompressedStateFile);
            _services.Storage.Download(IncrementalBackupModelConstants.BrotliAes256V1StateFile, destFile);

            var brotliFile = destFile + ".brotli";
            _services.CommonServices.EncryptionService.DecryptFile(destFile, brotliFile, password);

            var jsonFile = destFile + ".json";
            try
            {
                _services.CommonServices.BrotliCompressionService.DecompressFile(brotliFile, jsonFile);
            }
            catch
            {
                _log.WriteLine(LoggingEvent.Error, $"Failed to read state");
                state = default;
                return false;
            }
            using var uncompressedFileStream = File.OpenRead(jsonFile);
            state = JsonSerializer.Deserialize<IncrementalBackupState>(uncompressedFileStream);
            return state != null;
        }

        state = new IncrementalBackupState();
        return true;
    }

    public StorageFile? Write(string password, IncrementalBackupState state)
    {
        _log.WriteLine(LoggingEvent.Debug, $"Writing state");
        if (_services.Storage.Exists(IncrementalBackupModelConstants.StorageIncrementalNonEncryptedCompressedStateFile))
            _services.Storage.Delete(IncrementalBackupModelConstants.StorageIncrementalNonEncryptedCompressedStateFile);
        if (_services.Storage.Exists(IncrementalBackupModelConstants.StorageIncrementalEncryptedCompressedStateFile))
            _services.Storage.Delete(IncrementalBackupModelConstants.StorageIncrementalEncryptedCompressedStateFile);
        if (_services.Storage.Exists(IncrementalBackupModelConstants.BrotliAes256V1StateFile))
            _services.Storage.Delete(IncrementalBackupModelConstants.BrotliAes256V1StateFile);

        using var tempFolder = new TempFolder();
        var jsonFile = Path.Combine(tempFolder.Folder, "state.json");
        using (var jsonFileWriter = File.OpenWrite(jsonFile))
            JsonSerializer.Serialize(jsonFileWriter, state, _jsonSerializerOptions);

        var brotliFile = Path.Combine(tempFolder.Folder, "state.brotli");
        _services.CommonServices.BrotliCompressionService.CompressFile(jsonFile, brotliFile);

        var aesFile = Path.Combine(tempFolder.Folder, "state.aes");
        _services.CommonServices.EncryptionService.EncryptFile(brotliFile, aesFile, password);

        var storageFile = new StorageFile
        {
            StorageMethod = StorageMethodNames.BrotliCompressedAes256EncryptedV1,
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
