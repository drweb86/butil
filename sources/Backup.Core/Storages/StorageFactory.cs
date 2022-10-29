using BUtil.Core.Options;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace BUtil.Core.Storages
{
    public class StorageFactory
    {
        public static StorageBase Create(StorageSettings storageSettings)
        {
            switch (storageSettings.ProviderName.ToLowerInvariant())
            {
                case "hdd":
                    var hddSettings = CreateHddStorageSettings(storageSettings);
                    return new HddStorage(hddSettings);
                case "ftp":
                    var ftpSettings = CreateFtpStorageSettings(storageSettings);
                    return Create(ftpSettings);
                case "samba":
                    var sambaSettings = CreateSambaStorageSettings(storageSettings);
                    return new SambaStorage(sambaSettings);
                default:
                    throw new ArgumentOutOfRangeException(nameof(storageSettings));
            }
        }

        public static StorageBase Create(FtpStorageSettings settings)
        {
            return new FtpStorage(settings);
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
                ProviderName = "Hdd",
                Options = JsonSerializer.Serialize(settings)
            };
        }


        public static StorageSettings CreateStorageSettings(FtpStorageSettings settings)
        {
            return new StorageSettings
            {
                Name = settings.Name,
                ProviderName = "Ftp",
                Options = JsonSerializer.Serialize(settings)
            };
        }


        public static StorageSettings CreateStorageSettings(SambaStorageSettings settings)
        {
            return new StorageSettings
            {
                Name = settings.Name,
                ProviderName = "Samba",
                Options = JsonSerializer.Serialize(settings)
            };

        }
    }
}
