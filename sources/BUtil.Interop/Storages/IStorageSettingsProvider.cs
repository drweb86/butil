using BUtil.Core.ConfigurationFileModels.V2;
using System.Collections.Generic;

namespace BUtil.Core.Storages;

public interface IStorageSettingsProvider
{
    IReadOnlyList<StorageFieldDescriptor> Fields { get; }
    IReadOnlyList<string> SecretSettingsProperties { get; }

    IStorageSettingsV2 GetSettings(
        IReadOnlyDictionary<string, string?> fieldValues,
        long quota,
        string? mountScript,
        string? unmountScript);

    IReadOnlyDictionary<string, string?> GetFieldValues(IStorageSettingsV2 settings);

    // Returns an info message if trust was newly auto-detected, null otherwise.
    // updatedValues contains the fields to update in the form (e.g. certificate/fingerprint).
    string? TryApplyDetectedTrust(
        IStorageSettingsV2 testedSettings,
        IReadOnlyDictionary<string, string?> currentValues,
        out IReadOnlyDictionary<string, string?>? updatedValues);
}
