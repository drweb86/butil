using BUtil.Core.Logs;
using System;

namespace BUtil.Core.Storages
{
    public class StorageFactory
    {
        public static IStorage Create(ILog log, IStorageSettings storageSettings)
        {
            if (storageSettings is FolderStorageSettings)
                return new FailoverStorageWrapper(log, new FolderStorage(log, storageSettings as FolderStorageSettings), storageSettings);

            throw new ArgumentOutOfRangeException(nameof(storageSettings));
        }
    }
}
