using System;
using System.Collections.Generic;

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
            var ftpSettings = SemicolumnSeparatedStringSerializer.Deserialize<FtpStorageSettings>(storageSettings.ConnectionString);
            ftpSettings.Name = storageSettings.Name;
            return ftpSettings;
        }

        public static SambaStorageSettings CreateSambaStorageSettings(StorageSettings storageSettings)
        {
            var sambaSettings = SemicolumnSeparatedStringSerializer.Deserialize<SambaStorageSettings>(storageSettings.ConnectionString);
            sambaSettings.Name = storageSettings.Name;
            return sambaSettings;
        }

        public static HddStorageSettings CreateHddStorageSettings(StorageSettings storageSettings)
        {
            var hddSettings = SemicolumnSeparatedStringSerializer.Deserialize<HddStorageSettings>(storageSettings.ConnectionString);
            hddSettings.Name = storageSettings.Name;
            return hddSettings;
        }

        public static StorageSettings CreateStorageSettings(HddStorageSettings settings)
        {
            var hddStorageSettings = (HddStorageSettings)settings;
            return new StorageSettings
            {
                Name = hddStorageSettings.Name,
                ProviderName = "Hdd",
                ConnectionString = SemicolumnSeparatedStringSerializer.Serialize(settings, new List<string>() { "Name" })
            };
        }


        public static StorageSettings CreateStorageSettings(FtpStorageSettings settings)
        {

            var ftpStorageSettings = (FtpStorageSettings)settings;
            return new StorageSettings
            {
                Name = ftpStorageSettings.Name,
                ProviderName = "Ftp",
                ConnectionString = SemicolumnSeparatedStringSerializer.Serialize(settings, new List<string>() { "Name" })
            };
        }


        public static StorageSettings CreateStorageSettings(SambaStorageSettings settings)
        {
            var sambaStorageSettings = (SambaStorageSettings)settings;
            return new StorageSettings
            {
                Name = sambaStorageSettings.Name,
                ProviderName = "Samba",
                ConnectionString = SemicolumnSeparatedStringSerializer.Serialize(settings, new List<string>() { "Name" })
            };

        }
    }
}
