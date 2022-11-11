using BUtil.Core.Logs;
using System;
using System.Text.Json;

namespace BUtil.Core.Storages
{

    public class StorageFactory
    {
        public static IStorage Create(ILog log, StorageSettings storageSettings)
        {
            switch (storageSettings.ProviderName)
            {
                case StorageProviderNames.Hdd:
                    var hddSettings = CreateHddStorageSettings(storageSettings);
                    return new HddStorage(log, hddSettings);
                case StorageProviderNames.Ftp:
                    var ftpSettings = CreateFtpStorageSettings(storageSettings);
                    return Create(log, ftpSettings);
                case StorageProviderNames.Samba:
                    var sambaSettings = CreateSambaStorageSettings(storageSettings);
                    return new SambaStorage(log, sambaSettings);
                default:
                    throw new ArgumentOutOfRangeException(nameof(storageSettings));
            }
        }

        public static IStorage Create(ILog log, FtpStorageSettings settings)
        {
            return new FtpStorage(log, settings);
        }

        public static FtpStorageSettings CreateFtpStorageSettings(StorageSettings storageSettings)
        {
            var ftpSettings = JsonSerializer.Deserialize<FtpStorageSettings>(storageSettings.Options);
            ftpSettings.Name = storageSettings.Name;
            return ftpSettings;
        }

        public static SambaStorageSettings CreateSambaStorageSettings(StorageSettings storageSettings)
        {
            var sambaSettings = JsonSerializer.Deserialize<SambaStorageSettings>(storageSettings.Options);
            sambaSettings.Name = storageSettings.Name;
            return sambaSettings;
        }

        public static HddStorageSettings CreateHddStorageSettings(StorageSettings storageSettings)
        {
            var hddSettings = JsonSerializer.Deserialize<HddStorageSettings>(storageSettings.Options);
            hddSettings.Name = storageSettings.Name;
            return hddSettings;
        }

        public static StorageSettings CreateStorageSettings(HddStorageSettings settings)
        {
            return new StorageSettings
            {
                Name = settings.Name,
                ProviderName = StorageProviderNames.Hdd,
                Options = JsonSerializer.Serialize(settings)
            };
        }


        public static StorageSettings CreateStorageSettings(FtpStorageSettings settings)
        {
            return new StorageSettings
            {
                Name = settings.Name,
                ProviderName = StorageProviderNames.Ftp,
                Options = JsonSerializer.Serialize(settings)
            };
        }


        public static StorageSettings CreateStorageSettings(SambaStorageSettings settings)
        {
            return new StorageSettings
            {
                Name = settings.Name,
                ProviderName = StorageProviderNames.Samba,
                Options = JsonSerializer.Serialize(settings)
            };

        }
    }
}
