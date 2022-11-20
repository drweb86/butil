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

            if (storageSettings is FtpStorageSettings)
                return new FailoverStorageWrapper(log, new FtpStorage(log, storageSettings as FtpStorageSettings), storageSettings);

            if (storageSettings is SambaStorageSettings)
                return new FailoverStorageWrapper(log, new SambaStorage(log, storageSettings as SambaStorageSettings), storageSettings);

            throw new ArgumentOutOfRangeException(nameof(storageSettings));
        }

        public static IStorage Create(ILog log, FtpStorageSettings settings)
        {
            return new FtpStorage(log, settings);
        }
    }
}
