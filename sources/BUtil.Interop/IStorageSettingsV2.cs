namespace BUtil.Core.ConfigurationFileModels.V2;

// Polymorphic JSON deserialization is handled by StorageJsonConverter (BUtil.Core hosting layer),
// which consults StorageProviderRegistry (BUtil.Interop at runtime — enabling plugin assemblies to
// register new storage types without modifying this interface.
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
