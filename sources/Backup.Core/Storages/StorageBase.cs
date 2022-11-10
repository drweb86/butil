using BUtil.Core.Logs;

namespace BUtil.Core.Storages
{
    public abstract class StorageBase<TStorageSettings>: IStorage
	{
        protected StorageBase(LogBase log, TStorageSettings settings)
        {
            Log = log;
            Settings = settings;
        }

        protected readonly LogBase Log;
        protected readonly TStorageSettings Settings;

        public abstract string Upload(string sourceFile, string relativeFileName);
        public abstract string ReadAllText(string file);
        public abstract void Test();

	}
}
