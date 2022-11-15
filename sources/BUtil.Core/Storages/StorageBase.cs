using BUtil.Core.Logs;
using BUtil.Core.State;

namespace BUtil.Core.Storages
{
    public abstract class StorageBase<TStorageSettings>: IStorage
	{
        protected StorageBase(ILog log, TStorageSettings settings)
        {
            Log = log;
            Settings = settings;
        }

        protected readonly ILog Log;
        protected readonly TStorageSettings Settings;

        public abstract IStorageUploadResult Upload(string sourceFile, string relativeFileName);
        public abstract string ReadAllText(string file);
        public abstract string Test();
        public virtual byte[] ReadAllBytes(string file)
        {
            throw new System.NotImplementedException();
        }

        public virtual void Delete(string file)
        {
            throw new System.NotImplementedException();
        }

        public virtual void Download(StorageFile storageFile, string targetFileName)
        {
            throw new System.NotImplementedException();
        }
    }
}
