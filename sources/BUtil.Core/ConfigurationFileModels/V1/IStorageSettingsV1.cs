using System.Text.Json.Serialization;

namespace BUtil.Core.ConfigurationFileModels.V1
{
    [JsonDerivedType(typeof(FolderStorageSettingsV1), "Folder")]
    [JsonDerivedType(typeof(SambaStorageSettingsV1), "Samba")]
    public interface IStorageSettingsV1
    {
        long SingleBackupQuotaGb { get; }
    }
}