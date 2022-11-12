using BUtil.Core.Logs;

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

	}
}
