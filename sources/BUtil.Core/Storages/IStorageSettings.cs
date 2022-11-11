using System.Text.Json.Serialization;

namespace BUtil.Core.Storages
{
    [JsonDerivedType(typeof(HddStorageSettings), "HDD")]
    [JsonDerivedType(typeof(SambaStorageSettings), "FTP")]
    [JsonDerivedType(typeof(FtpStorageSettings), "Samba")]
    public interface IStorageSettings
    {
        string Name { get; }
    }
}
