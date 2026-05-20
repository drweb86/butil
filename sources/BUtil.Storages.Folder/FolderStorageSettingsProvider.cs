using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using System.Collections.Generic;

namespace BUtil.Core.Storages;

public class FolderStorageSettingsProvider : IStorageSettingsProvider
{
    public IReadOnlyList<StorageFieldDescriptor> Fields { get; } =
    [
        new StorageFieldDescriptor
        {
            Key = "folder",
            Label = Resources.Field_Folder,
            Type = StorageFieldType.Folder,
            Placeholder = "P:\\Storage\\Backup1",
            DefaultValue = string.Empty,
        },
    ];

    public IReadOnlyList<string> SecretSettingsProperties { get; } = [];


    public IStorageSettingsV2 GetSettings(
        IReadOnlyDictionary<string, string?> fieldValues,
        long quota,
        string? mountScript,
        string? unmountScript) =>
        new FolderStorageSettingsV2
        {
            SingleBackupQuotaGb = quota,
            MountPowershellScript = mountScript,
            UnmountPowershellScript = unmountScript,
            DestinationFolder = fieldValues.GetValueOrDefault("folder") ?? string.Empty,
        };

    public IReadOnlyDictionary<string, string?> GetFieldValues(IStorageSettingsV2 settings)
    {
        var s = (FolderStorageSettingsV2)settings;
        return new Dictionary<string, string?> { ["folder"] = s.DestinationFolder };
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
