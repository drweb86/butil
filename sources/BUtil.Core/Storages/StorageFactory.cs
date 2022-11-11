using BUtil.Core.Logs;
using System;

namespace BUtil.Core.Storages
{
    public class StorageFactory
    {
        public static IStorage Create(ILog log, IStorageSettings storageSettings)
        {
            if (storageSettings is HddStorageSettings)
                return new HddStorage(log, storageSettings as HddStorageSettings);

            if (storageSettings is FtpStorageSettings)
                return new FtpStorage(log, storageSettings as FtpStorageSettings);

            if (storageSettings is SambaStorageSettings)
                return new SambaStorage(log, storageSettings as SambaStorageSettings);

            throw new ArgumentOutOfRangeException(nameof(storageSettings));
        }

        public static IStorage Create(ILog log, FtpStorageSettings settings)
        {
            return new FtpStorage(log, settings);
        }
    }
}
