using BUtil.Core.ConfigurationFileModels.V2;
using System.Collections.Generic;

namespace BUtil.Core.Storages;

public interface IStorageSettingsProvider
{
    string StorageId { get; }
    string DisplayName { get; }
    int Order { get; }
    bool IsSupported { get; }
    IReadOnlyList<StorageFieldDescriptor> Fields { get; }

    bool CanHandle(IStorageSettingsV2 settings);

    IStorageSettingsV2 CreateSettings(
        IReadOnlyDictionary<string, string?> fieldValues,
        long quota,
        string? mountScript,
        string? unmountScript);

    IReadOnlyDictionary<string, string?> ExtractValues(IStorageSettingsV2 settings);

    // Returns an info message if trust was newly auto-detected, null otherwise.
    // updatedValues contains the fields to update in the form (e.g. certificate/fingerprint).
    string? TryApplyDetectedTrust(
        IStorageSettingsV2 testedSettings,
        IReadOnlyDictionary<string, string?> currentValues,
        out IReadOnlyDictionary<string, string?>? updatedValues);
}
