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
                return new FailoverStorageWrapper(log, new FolderStorage(log, storageSettings as FolderStorageSettings), storageSettings);
            else if (storageSettings is SambaStorageSettings)
                return new FailoverStorageWrapper(log, new SambaStorage(log, storageSettings as SambaStorageSettings), storageSettings);
            throw new ArgumentOutOfRangeException(nameof(storageSettings));
        }

        public static string Test(ILog log, IStorageSettings storageSettings)
        {
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
