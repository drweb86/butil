﻿namespace BUtil.Core.Misc;

public static class IncrementalBackupModelConstants
{
    public const string StorageIncrementedNonEncryptedNonCompressedStateFile = "Incremental Non-Encrypted Non-Compressed Backup State.json";
    public const string StorageIncrementalEncryptedCompressedStateFile = "Incremental Encrypted Compressed Backup State.7z";
    public const string BrotliAes256V1StateFile = "State.brotli.aes256v1";
}
