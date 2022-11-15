using System.Collections.Generic;

namespace BUtil.Core.Misc
{
    public static class IncrementalBackupModelConstants
    {
        public const string StorageIncrementedNonEncryptedNonCompressedStateFile = "Incremental Non-Encrypted Non-Compressed Backup State.json";
        public const string StorageIncrementalNonEncryptedCompressedStateFile = "Incremental Non-Encrypted Compressed Backup State.7z";
        public const string StorageIncrementalEncryptedCompressedStateFile = "Incremental Encrypted Compressed Backup State.7z";

        public static readonly IEnumerable<string> Files = new List<string> 
        {
            StorageIncrementedNonEncryptedNonCompressedStateFile ,
            StorageIncrementalNonEncryptedCompressedStateFile,
            StorageIncrementalEncryptedCompressedStateFile
        };
    }
}
