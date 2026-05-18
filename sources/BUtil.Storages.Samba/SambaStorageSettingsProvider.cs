using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Storages;
using System;
using System.Collections.Generic;

namespace BUtil.Storages.Samba;

public class SambaStorageSettingsProvider : IStorageSettingsProvider
{
    public IReadOnlyList<StorageFieldDescriptor> Fields { get; } =
    [
        new StorageFieldDescriptor
        {
            Key = "url",
            Label = Resources.Url_Field,
            Type = StorageFieldType.Text,
            Placeholder = @"\\192.168.11.1\share\folder",
        },
        new StorageFieldDescriptor
        {
            Key = "user",
            Label = Resources.User_Field,
            Type = StorageFieldType.Text,
            IsOptional = true,
        },
        new StorageFieldDescriptor
        {
            Key = "password",
            Label = Resources.Password_Field,
            Type = StorageFieldType.Password,
            IsOptional = true,
        },
    ];

    public IReadOnlyList<string> SecretSettingsProperties { get; } = ["password"];


    public IStorageSettingsV2 GetSettings(
        IReadOnlyDictionary<string, string?> fieldValues,
        long quota,
        string? mountScript,
        string? unmountScript) =>
        new SambaStorageSettingsV2
        {
            SingleBackupQuotaGb = quota,
            MountPowershellScript = mountScript,
            UnmountPowershellScript = unmountScript,
            Url = fieldValues.GetValueOrDefault("url") ?? string.Empty,
            User = fieldValues.GetValueOrDefault("user"),
            Password = fieldValues.GetValueOrDefault("password"),
        };

    public IReadOnlyDictionary<string, string?> GetFieldValues(IStorageSettingsV2 settings)
    {
        var s = (SambaStorageSettingsV2)settings;
        return new Dictionary<string, string?>
        {
            ["url"] = s.Url,
            ["user"] = s.User,
            ["password"] = s.Password,
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
