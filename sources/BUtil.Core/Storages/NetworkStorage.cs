using System;
using BUtil.Core.Logs;

namespace BUtil.Core.Storages
{
    class SambaStorage: StorageBase<SambaStorageSettings>
	{
        const string _COPYING = "Copying '{0}' to '{1}'";
        const string _EncryptingFormatString = "Encrypting '{0}'";

        long SkipCopyingToNetworkStorageLimit
        {
            get { return Settings.SkipCopyingToNetworkStorageLimitMb * 1024 * 1024; }
        }

        internal SambaStorage(ILog log, SambaStorageSettings settings)
            :base(log, settings)
        {
        }

        public override string ReadAllText(string file)
        {
            throw new NotImplementedException();
        }

        public override string Test()
        {
            return null;
        }

        public override IStorageUploadResult Upload(string sourceFile, string relativeFileName)
        {
            throw new NotImplementedException();
        }
    }
}
