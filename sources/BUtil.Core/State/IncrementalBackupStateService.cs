﻿using BUtil.Core.BackupModels;
using BUtil.Core.FileSystem;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Options;
using BUtil.Core.Storages;
using Microsoft.VisualBasic.Logging;
using System;
using System.IO;
using System.Text.Json;
using System.Threading;

namespace BUtil.Core.State
{
    public class IncrementalBackupStateService
    {
        private readonly ILog log;
        private readonly IStorageSettings storageSettings;
        private readonly BackupTask task;
        private readonly ProgramOptions programOptions;

        public IncrementalBackupStateService(ILog log, IStorageSettings storageSettings, BackupTask task, ProgramOptions programOptions)
        {
            this.log = log;
            this.storageSettings = storageSettings;
            this.task = task;
            this.programOptions = programOptions;
        }

        public bool TryRead(CancellationToken cancellationToken, out IncrementalBackupState state)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                state = null;
                return false;
            }

            log.WriteLine(LoggingEvent.Debug, $"Storage \"{storageSettings.Name}\": Reading state");
            var storage = StorageFactory.Create(log, storageSettings);
            var content = storage.ReadAllText(IncrementalBackupModelConstants.StorageIncrementedNonEncryptedNonCompressedStateFile);
            if (content != null)
            {
                log.WriteLine(LoggingEvent.Debug, $"Storage \"{storageSettings.Name}\": Reading non-encrypted non-compressed state");
                state = JsonSerializer.Deserialize<IncrementalBackupState>(content);
                return true;
            }

            var bytes = storage.ReadAllBytes(IncrementalBackupModelConstants.StorageIncrementalNonEncryptedCompressedStateFile);
            if (bytes != null)
            {
                log.WriteLine(LoggingEvent.Debug, $"Storage \"{storageSettings.Name}\": Reading non-encrypted compressed state");
                using var tempFolder = new TempFolder();
                var archive = Path.Combine(tempFolder.Folder, "archive.7z");
                File.WriteAllBytes(archive, bytes);
                if (!SevenZipProcessHelper.Extract(log, archive, null, tempFolder.Folder, programOptions.ProcessPriority, cancellationToken))
                {
                    log.WriteLine(LoggingEvent.Error, $"Storage \"{storageSettings.Name}\": Failed to read state");
                    state = null;
                    return false;
                }

                var uncompressedFile = Path.Combine(tempFolder.Folder, IncrementalBackupModelConstants.StorageIncrementedNonEncryptedNonCompressedStateFile);
                var uncompressedFileContent = File.ReadAllText(uncompressedFile);
                state = JsonSerializer.Deserialize<IncrementalBackupState>(uncompressedFileContent);
                return true;
            }

            bytes = storage.ReadAllBytes(IncrementalBackupModelConstants.StorageIncrementalEncryptedCompressedStateFile);

            if (bytes != null)
            {
                log.WriteLine(LoggingEvent.Debug, $"Storage \"{storageSettings.Name}\": Reading encrypted compressed state");
                using var tempFolder = new TempFolder();
                var archive = Path.Combine(tempFolder.Folder, "archive.7z");
                File.WriteAllBytes(archive, bytes);
                if (!SevenZipProcessHelper.Extract(log, archive, task.Password, tempFolder.Folder, programOptions.ProcessPriority, cancellationToken))
                {
                    log.WriteLine(LoggingEvent.Error, $"Storage \"{storageSettings.Name}\": Failed to read state");
                    state = null;
                    return false;
                }

                var uncompressedFile = Path.Combine(tempFolder.Folder, IncrementalBackupModelConstants.StorageIncrementedNonEncryptedNonCompressedStateFile);
                var uncompressedFileContent = File.ReadAllText(uncompressedFile);
                state = JsonSerializer.Deserialize<IncrementalBackupState>(uncompressedFileContent);
                return true;
            }

            state = new IncrementalBackupState();
            return true;
        }

        public StorageFile Write(CancellationToken cancellationToken, IncrementalBackupState state)
        {
            var model = task.Model as IncrementalBackupModelOptions;

            log.WriteLine(LoggingEvent.Debug, $"Storage \"{storageSettings.Name}\": Writing state");
            var storage = StorageFactory.Create(log, storageSettings);
            storage.Delete(IncrementalBackupModelConstants.StorageIncrementedNonEncryptedNonCompressedStateFile);
            storage.Delete(IncrementalBackupModelConstants.StorageIncrementalNonEncryptedCompressedStateFile);
            storage.Delete(IncrementalBackupModelConstants.StorageIncrementalEncryptedCompressedStateFile);

            var json = JsonSerializer.Serialize(state, new JsonSerializerOptions { WriteIndented = true });

            using var tempFolder = new TempFolder();

            var file = Path.Combine(tempFolder.Folder, IncrementalBackupModelConstants.StorageIncrementedNonEncryptedNonCompressedStateFile);
            File.WriteAllText(file, json);

            var storageFile = new StorageFile
            {
                StorageMethod = StorageMethodNames.Plain,
                StorageIntegrityMethod = StorageIntegrityMethod.Sha256
            };

            string fileToUpload;
            if (model.DisableCompressionAndEncryption)
            {
                storageFile.StorageRelativeFileName = IncrementalBackupModelConstants.StorageIncrementedNonEncryptedNonCompressedStateFile;
                fileToUpload = file;
            }
            else
            {
                var encryptionEnabled = !string.IsNullOrWhiteSpace(task.Password);
                storageFile.StorageRelativeFileName = encryptionEnabled ? IncrementalBackupModelConstants.StorageIncrementalEncryptedCompressedStateFile : IncrementalBackupModelConstants.StorageIncrementalNonEncryptedCompressedStateFile;
                fileToUpload = Path.Combine(tempFolder.Folder, storageFile.StorageRelativeFileName);

                if (!SevenZipProcessHelper.CompressFile(log, file, task.Password, fileToUpload, programOptions.ProcessPriority, cancellationToken))
                {
                    log.WriteLine(LoggingEvent.Error, $"Storage \"{storageSettings.Name}\": Failed state");
                    return null;
                }
            }

            var uploadResult = storage.Upload(fileToUpload, storageFile.StorageRelativeFileName);
            storageFile.StorageFileName = uploadResult.StorageFileName;
            storageFile.StorageFileNameSize = uploadResult.StorageFileNameSize;
            storageFile.StorageIntegrityMethodInfo = HashHelper.GetSha512(fileToUpload);

            return storageFile;
        }
    }
}