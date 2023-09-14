﻿using System.Text.Json.Serialization;

namespace BUtil.Core.ConfigurationFileModels.V2
{
    [JsonDerivedType(typeof(FtpsStorageSettingsV2), "Ftps")]
    [JsonDerivedType(typeof(FolderStorageSettingsV2), "Folder")]
    [JsonDerivedType(typeof(SambaStorageSettingsV2), "Samba")]
    public interface IStorageSettingsV2
    {
        long SingleBackupQuotaGb { get; }
    }
}
