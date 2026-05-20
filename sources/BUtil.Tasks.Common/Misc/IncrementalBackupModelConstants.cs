using System;

namespace BUtil.Core.Misc;

public static class IncrementalBackupModelConstants
{
    [Obsolete]
    public const string StorageIncrementedNonEncryptedNonCompressedStateFile = "Incremental Non-Encrypted Non-Compressed Backup State.json";
    [Obsolete]
    public const string StorageIncrementalEncryptedCompressedStateFile = "Incremental Encrypted Compressed Backup State.7z";

    public const string BrotliAes256V1StateFile = "State.brotli." + SourceItemHelper.AES256V1Extension;
}
