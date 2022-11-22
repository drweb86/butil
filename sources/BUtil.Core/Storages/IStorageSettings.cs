using System.Text.Json.Serialization;

namespace BUtil.Core.Storages
{
    [JsonDerivedType(typeof(FolderStorageSettings), "Folder")]
    public interface IStorageSettings
    {
        string Name { get; }
        bool Enabled { get; }
        long SingleBackupQuotaGb { get; }
    }
}
