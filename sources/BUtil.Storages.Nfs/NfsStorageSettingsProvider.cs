using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Storages;
using System;
using System.Collections.Generic;
using System.IO;

namespace BUtil.Storages.Nfs;

public class NfsStorageSettingsProvider : IStorageSettingsProvider
{
    public string StorageId => "Nfs";
    public string DisplayName => "NFS";
    public int Order => 7;

    public bool IsSupported
    {
        get
        {
            if (OperatingSystem.IsLinux()) return true;
            if (OperatingSystem.IsWindows())
                return File.Exists(Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.System), "mount.exe"));
            return false;
        }
    }

    private static readonly IReadOnlyList<StorageFieldDescriptor> LinuxFields =
    [
        new StorageFieldDescriptor
        {
            Key = "host",
            Label = Resources.Server_Field_Address,
            Type = StorageFieldType.Text,
            Placeholder = "192.168.1.100",
        },
        new StorageFieldDescriptor
        {
            Key = "sharePath",
            Label = Resources.Nfs_Field_SharePath,
            Type = StorageFieldType.Text,
            Placeholder = "/export/backups",
        },
        new StorageFieldDescriptor
        {
            Key = "mountPoint",
            Label = Resources.Nfs_Field_MountPoint,
            Type = StorageFieldType.Text,
            Placeholder = "/mnt/nfs-backup",
        },
        new StorageFieldDescriptor
        {
            Key = "mountOptions",
            Label = Resources.Nfs_Field_MountOptions,
            Type = StorageFieldType.Text,
            Placeholder = "vers=4,rw",
            IsOptional = true,
        },
    ];

    private static readonly IReadOnlyList<StorageFieldDescriptor> WindowsFields =
    [
        new StorageFieldDescriptor
        {
            Key = "host",
            Label = Resources.Server_Field_Address,
            Type = StorageFieldType.Text,
            Placeholder = "192.168.1.100",
        },
        new StorageFieldDescriptor
        {
            Key = "sharePath",
            Label = Resources.Nfs_Field_SharePath,
            Type = StorageFieldType.Text,
            Placeholder = "/export/backups",
        },
        new StorageFieldDescriptor
        {
            Key = "mountPoint",
            Label = Resources.Nfs_Field_DriveLetter,
            Type = StorageFieldType.Text,
            Placeholder = "Z:",
        },
        new StorageFieldDescriptor
        {
            Key = "mountOptions",
            Label = Resources.Nfs_Field_MountOptions,
            Type = StorageFieldType.Text,
            Placeholder = "anon",
            IsOptional = true,
        },
    ];

    public IReadOnlyList<StorageFieldDescriptor> Fields =>
        OperatingSystem.IsWindows() ? WindowsFields : LinuxFields;

    public bool CanHandle(IStorageSettingsV2 settings) => settings is NfsStorageSettingsV2;

    public IStorageSettingsV2 CreateSettings(
        IReadOnlyDictionary<string, string?> fieldValues,
        long quota,
        string? mountScript,
        string? unmountScript) =>
        new NfsStorageSettingsV2
        {
            SingleBackupQuotaGb = quota,
            MountPowershellScript = mountScript,
            UnmountPowershellScript = unmountScript,
            Host = fieldValues.GetValueOrDefault("host") ?? string.Empty,
            SharePath = fieldValues.GetValueOrDefault("sharePath") ?? string.Empty,
            MountPoint = fieldValues.GetValueOrDefault("mountPoint") ?? string.Empty,
            MountOptions = fieldValues.GetValueOrDefault("mountOptions"),
        };

    public IReadOnlyDictionary<string, string?> ExtractValues(IStorageSettingsV2 settings)
    {
        var s = (NfsStorageSettingsV2)settings;
        return new Dictionary<string, string?>
        {
            ["host"] = s.Host,
            ["sharePath"] = s.SharePath,
            ["mountPoint"] = s.MountPoint,
            ["mountOptions"] = s.MountOptions,
        };
    }

    public string? TryApplyDetectedTrust(
        IStorageSettingsV2 testedSettings,
        IReadOnlyDictionary<string, string?> currentValues,
        out IReadOnlyDictionary<string, string?>? updatedValues)
    {
        updatedValues = null;
        return null;
    }
}
