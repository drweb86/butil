using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using System;

namespace BUtil.Core.Storages
{
    public class StorageFactory
    {
        public static IStorage Create(ILog log, IStorageSettingsV2 storageSettings)
        {
            if (storageSettings is FolderStorageSettingsV2)
                return new FailoverStorageWrapper(log, new FolderStorage(log, storageSettings as FolderStorageSettingsV2));
            else if (storageSettings is SambaStorageSettingsV2)
                return new FailoverStorageWrapper(log, new SambaStorage(log, storageSettings as SambaStorageSettingsV2));
            else if (storageSettings is FtpsStorageSettingsV2)
                return new FailoverStorageWrapper(log, new FtpsStorage(log, storageSettings as FtpsStorageSettingsV2));
            else if (storageSettings is MtpStorageSettings)
            {
                var mtpStorage = PlatformSpecificExperience.Instance.GetMtpStorage(log, storageSettings as MtpStorageSettings);
                if (mtpStorage == null)
                    throw new NotSupportedException("Your OS does not support MTP storage");
                return new FailoverStorageWrapper(log, mtpStorage);
            }
            throw new ArgumentOutOfRangeException(nameof(storageSettings));
        }

        public static string Test(ILog log, IStorageSettingsV2 storageSettings)
        {
            if (storageSettings == null)
                return BUtil.Core.Localization.Resources.DataStorage_Validation_NotSpecified;

            if (storageSettings.SingleBackupQuotaGb < 0)
                return BUtil.Core.Localization.Resources.DataStorage_Field_UploadQuota_Validation;

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
