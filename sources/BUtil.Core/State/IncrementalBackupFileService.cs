using BUtil.Core.Compression;
using BUtil.Core.FileSystem;
using BUtil.Core.Hashing;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Options;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.IncrementalModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace BUtil.Core.State
{
    public class IncrementalBackupFileService
    {
        private readonly ILog _log;
        private readonly IStorageSettings _storageSettings;
        private readonly IHashService _hashService;
        private readonly StorageSpecificServicesIoc _services;

        public IncrementalBackupFileService(IHashService hashService, StorageSpecificServicesIoc services)
        {
            _log = services.Log;
            _storageSettings = services.StorageSettings;
            _hashService = hashService;
            _services = services;
        }

        public bool Download(SourceItem sourceItem, StorageFile storageFile, string destinationFolder)
        {
            _log.WriteLine(LoggingEvent.Debug, $"Storage \"{_storageSettings.Name}\": downloading \"{storageFile.FileState.FileName}\"");

            var sourceItemDiectory = SourceItemHelper.GetSourceItemDirectory(sourceItem);
            var sourceItemRelativeFileName = SourceItemHelper.GetSourceItemRelativeFileName(sourceItemDiectory, storageFile.FileState);
            var destinationFileName = Path.Combine(destinationFolder, sourceItemRelativeFileName);
            var destinationDir = Path.GetDirectoryName(destinationFileName);
            if (!Directory.Exists(destinationDir))
                Directory.CreateDirectory(destinationDir);

            if (storageFile.StorageMethod == StorageMethodNames.Plain)
            {
                _services.Storage.Download(storageFile.StorageRelativeFileName, destinationFileName);
            }
            else
            {
                using var tempFolder = new TempFolder();
                var tempArchive = Path.Combine(tempFolder.Folder, "archive.7z");
                _services.Storage.Download(storageFile.StorageRelativeFileName, tempArchive);
                var archiver = ArchiverFactory.Create(_log);
                if (!archiver.Extract(tempArchive, storageFile.StoragePassword, destinationDir))
                {
                    _log.WriteLine(LoggingEvent.Error, $"Storage \"{_storageSettings.Name}\": extracting \"{storageFile.FileState.FileName}\" failed");
                    return false;
                }
            }
            return true;
        }

        public bool Upload(StorageFile storageFile)
        {
            _log.WriteLine(LoggingEvent.Debug, $"Upload \"{_storageSettings.Name}\": Upload \"{storageFile.FileState.FileName}\"");

            if (storageFile.StorageMethod == StorageMethodNames.Plain)
            {
                var uploadResult = _services.Storage.Upload(storageFile.FileState.FileName, storageFile.StorageRelativeFileName);
                storageFile.StorageFileNameSize = uploadResult.StorageFileNameSize;
                storageFile.StorageFileName= uploadResult.StorageFileName;
                storageFile.StorageIntegrityMethod = StorageIntegrityMethod.Sha512;
                storageFile.StorageIntegrityMethodInfo = storageFile.FileState.Sha512;
                return true;
            }
            else
            {
                using var tempFolder = new TempFolder();
                var archiveFile = Path.Combine(tempFolder.Folder, "archive.7z");

                var encryptionEnabled = storageFile.StorageMethod == StorageMethodNames.SevenZipEncrypted;
                if (encryptionEnabled)
                {
                    storageFile.StoragePassword = RandomString();
                }

                var archiver = ArchiverFactory.Create(_log);
                if (!archiver.CompressFile(
                    storageFile.FileState.FileName,
                    storageFile.StoragePassword,
                    archiveFile))
                {
                    _log.WriteLine(LoggingEvent.Error, $"Upload \"{_storageSettings.Name}\": Error compressing \"{storageFile.FileState.FileName}\"");
                    return false;
                }

                var uploadResult = _services.Storage.Upload(archiveFile, storageFile.StorageRelativeFileName);
                storageFile.StorageFileNameSize = uploadResult.StorageFileNameSize;
                storageFile.StorageFileName = uploadResult.StorageFileName;
                storageFile.StorageIntegrityMethod = StorageIntegrityMethod.Sha512;
                storageFile.StorageIntegrityMethodInfo = _hashService.GetSha512(archiveFile, false);
                return true;
            }
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
}
