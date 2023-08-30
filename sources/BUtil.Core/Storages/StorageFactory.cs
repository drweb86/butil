using BUtil.Core.Logs;
using System;
using System.Windows.Forms;

namespace BUtil.Core.Storages
{
    public class StorageFactory
    {
        public static IStorage Create(ILog log, IStorageSettings storageSettings)
        {
            if (storageSettings is FolderStorageSettings)
                return new FailoverStorageWrapper(log, new FolderStorage(log, storageSettings as FolderStorageSettings));
            else if (storageSettings is SambaStorageSettings)
                return new FailoverStorageWrapper(log, new SambaStorage(log, storageSettings as SambaStorageSettings));
            else if (storageSettings is FtpsStorageSettings)
                return new FailoverStorageWrapper(log, new FtpsStorage(log, storageSettings as FtpsStorageSettings));
            throw new ArgumentOutOfRangeException(nameof(storageSettings));
        }

        public static string Test(ILog log, IStorageSettings storageSettings)
        {
            if (storageSettings == null)
                return BUtil.Core.Localization.Resources.StorageIsNotSpecified;

            try
            {
                using (var storage = Create(new StubLog(), storageSettings))
                {
                    return storage.Test();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
