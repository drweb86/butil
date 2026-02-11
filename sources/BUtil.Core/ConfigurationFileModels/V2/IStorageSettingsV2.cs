using System.Text.Json.Serialization;

namespace BUtil.Core.ConfigurationFileModels.V2;

[JsonDerivedType(typeof(FtpsStorageSettingsV2), "Ftps")]
[JsonDerivedType(typeof(SftpStorageSettingsV2), "Sftp")]
[JsonDerivedType(typeof(FolderStorageSettingsV2), "Folder")]
[JsonDerivedType(typeof(SambaStorageSettingsV2), "Samba")] // name is wrong, should be SMB/CIFS
public interface IStorageSettingsV2
{
    long SingleBackupQuotaGb { get; }

    /// <summary>
    /// Its not powershell anymore on Ubuntu. SHould be renamed.
    /// </summary>
    string? MountPowershellScript { get; set; }
    /// <summary>
    /// Its not powershell anymore on Ubuntu. SHould be renamed.
    /// </summary>
    string? UnmountPowershellScript { get; set; }
}
