
using BUtil.Core.Compression;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.FileSystem;
using BUtil.Core.Hashing;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.TasksTree.IncrementalModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;

namespace BUtil.Core.State
{
    public class IncrementalBackupStateService
    {
        private readonly ILog _log;
        private readonly StorageSpecificServicesIoc _services;
        private readonly IHashService _hashService;

        public IncrementalBackupStateService(StorageSpecificServicesIoc services, IHashService hashService)
        {
            _log = services.Log;
            _services = services;
            _hashService = hashService;
        }

        public bool TryRead(string password, [NotNullWhen(true)] out IncrementalBackupState? state)
        {
            _log.WriteLine(LoggingEvent.Debug, $"Reading state");
            using var tempFolder = new TempFolder();

            if (_services.Storage.Exists(IncrementalBackupModelConstants.StorageIncrementalEncryptedCompressedStateFile))
            {
                _log.WriteLine(LoggingEvent.Debug, $"Reading encrypted compressed state");
                var destFile = Path.Combine(tempFolder.Folder, IncrementalBackupModelConstants.StorageIncrementalEncryptedCompressedStateFile);
                _services.Storage.Download(IncrementalBackupModelConstants.StorageIncrementalEncryptedCompressedStateFile, destFile);
                var archiver = ArchiverFactory.Create(_log);
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

            state = new IncrementalBackupState();
            return true;
        }

        public StorageFile? Write(IncrementalBackupModelOptionsV2 incrementalBackupModelOptions, IncrementalBackupState state)
        {
            _log.WriteLine(LoggingEvent.Debug, $"Writing state");
            _services.Storage.Delete(IncrementalBackupModelConstants.StorageIncrementalNonEncryptedCompressedStateFile);
            _services.Storage.Delete(IncrementalBackupModelConstants.StorageIncrementalEncryptedCompressedStateFile);

            using var tempFolder = new TempFolder();
            var jsonFile = Path.Combine(tempFolder.Folder, IncrementalBackupModelConstants.StorageIncrementedNonEncryptedNonCompressedStateFile);
            using (var jsonFileWriter = File.OpenWrite(jsonFile))
                JsonSerializer.Serialize(jsonFileWriter, state, new JsonSerializerOptions { WriteIndented = true });

            var storageFile = new StorageFile
            {
                StorageMethod = StorageMethodNames.SevenZipEncrypted,
                StorageIntegrityMethod = StorageIntegrityMethod.Sha512
            };

            storageFile.StorageRelativeFileName = IncrementalBackupModelConstants.StorageIncrementalEncryptedCompressedStateFile;
            var fileToUpload = Path.Combine(tempFolder.Folder, storageFile.StorageRelativeFileName);
            var archiver = ArchiverFactory.Create(_log);
            if (!archiver.CompressFile(jsonFile, incrementalBackupModelOptions.Password, fileToUpload))
            {
                _log.WriteLine(LoggingEvent.Error, $"Failed state");
                return null;
            }

            var uploadResult = _services.Storage.Upload(fileToUpload, storageFile.StorageRelativeFileName);
            storageFile.StorageFileName = uploadResult.StorageFileName;
            storageFile.StorageFileNameSize = uploadResult.StorageFileNameSize;
            storageFile.StorageIntegrityMethodInfo = _hashService.GetSha512(fileToUpload, false);

            return storageFile;
        }
    }
}
