using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Storages;
using System.Collections.Generic;

namespace BUtil.Storages.AzureBlob;

public class AzureBlobStorageSettingsProvider : IStorageSettingsProvider
{
    public IReadOnlyList<StorageFieldDescriptor> Fields { get; } =
    [
        new StorageFieldDescriptor
        {
            Key = "accountName",
            Label = Resources.Storage_Field_AccountName,
            Type = StorageFieldType.Text,
            Placeholder = "mystorageaccount",
        },
        new StorageFieldDescriptor
        {
            Key = "accountKey",
            Label = Resources.Storage_Field_AccountKey,
            Type = StorageFieldType.Password,
        },
        new StorageFieldDescriptor
        {
            Key = "container",
            Label = Resources.Storage_Field_Container,
            Type = StorageFieldType.Text,
            Placeholder = "backups",
        },
        new StorageFieldDescriptor
        {
            Key = "pathPrefix",
            Label = Resources.Storage_Field_PathPrefix,
            Type = StorageFieldType.Text,
            Placeholder = "my-pc",
            IsOptional = true,
        },
    ];

    public IReadOnlyList<string> SecretSettingsProperties { get; } = ["accountKey"];


    public IStorageSettingsV2 GetSettings(
        IReadOnlyDictionary<string, string?> fieldValues,
        long quota,
        string? mountScript,
        string? unmountScript) =>
        new AzureBlobStorageSettingsV2
        {
            SingleBackupQuotaGb = quota,
            MountPowershellScript = mountScript,
            UnmountPowershellScript = unmountScript,
            AccountName = fieldValues.GetValueOrDefault("accountName") ?? string.Empty,
            AccountKey = fieldValues.GetValueOrDefault("accountKey") ?? string.Empty,
            ContainerName = fieldValues.GetValueOrDefault("container") ?? string.Empty,
            PathPrefix = fieldValues.GetValueOrDefault("pathPrefix") ?? string.Empty,
        };

    public IReadOnlyDictionary<string, string?> GetFieldValues(IStorageSettingsV2 settings)
    {
        var s = (AzureBlobStorageSettingsV2)settings;
        return new Dictionary<string, string?>
        {
            ["accountName"] = s.AccountName,
            ["accountKey"] = s.AccountKey,
            ["container"] = s.ContainerName,
            ["pathPrefix"] = s.PathPrefix,
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
