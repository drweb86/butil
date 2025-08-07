using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using System;

namespace BUtil.Core.Storages;

public class StorageFactory
{
    public static IStorage Create(ILog log, IStorageSettingsV2 storageSettings, bool autodetectConnectionSettings, int? triesCount = null)
    {
        if (storageSettings is FolderStorageSettingsV2 folder)
            return new FailoverStorageWrapper(log, new FolderStorage(log, folder), triesCount);
        else if (storageSettings is SambaStorageSettingsV2 samba)
            return new FailoverStorageWrapper(log, PlatformSpecificExperience.Instance.GetSmbCifsStorage(log, samba), triesCount);
        else if (storageSettings is FtpsStorageSettingsV2 ftps)
            return new FailoverStorageWrapper(log, new FtpsStorage(log, ftps, autodetectConnectionSettings), triesCount);
        else if (storageSettings is SftpStorageSettingsV2 sftp)
            return new FailoverStorageWrapper(log, new SftpStorage(log, sftp, autodetectConnectionSettings), triesCount);
        throw new ArgumentOutOfRangeException(nameof(storageSettings));
    }

    public static string? Test(ILog log, IStorageSettingsV2 storageSettings, bool writeMode)
    {
        if (storageSettings == null)
            return BUtil.Core.Localization.Resources.DataStorage_Validation_NotSpecified;

        if (storageSettings.SingleBackupQuotaGb < 0)
            return BUtil.Core.Localization.Resources.DataStorage_Field_UploadQuota_Validation;

        try
        {
            using var storage = Create(log, storageSettings, true, 1);
            var readonlyChecksError = storage.Test();
            if (!string.IsNullOrWhiteSpace(readonlyChecksError))
                return readonlyChecksError;

            if (writeMode)
                StorageHelper.WriteTest(storage);

            return null;
        }
        catch (Exception ex)
        {
            return ExceptionHelper.ToString(ex);
        }
    }
}
