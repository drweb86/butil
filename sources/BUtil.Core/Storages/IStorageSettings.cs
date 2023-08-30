using System.Text.Json.Serialization;

namespace BUtil.Core.Storages
{
    [JsonDerivedType(typeof(FtpsStorageSettings), "Ftps")]
    [JsonDerivedType(typeof(FolderStorageSettings), "Folder")]
    [JsonDerivedType(typeof(SambaStorageSettings), "Samba")]
    public interface IStorageSettings
    {
        long SingleBackupQuotaGb { get; }
    }
}
