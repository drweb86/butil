using System.Text.Json.Serialization;

namespace BUtil.Core.Storages
{
    [JsonDerivedType(typeof(FolderStorageSettings), "Folder")]
    [JsonDerivedType(typeof(SambaStorageSettings), "FTP")]
    [JsonDerivedType(typeof(FtpStorageSettings), "Samba")]
    public interface IStorageSettings
    {
        string Name { get; }
        bool Enabled { get; }
        long SingleBackupQuotaGb { get; }
    }
}
