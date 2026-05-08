using System.Collections.Generic;

namespace BUtil.Core.Storages;

/// <summary>
/// One choice for a storage field of type <see cref="StorageFieldType.Enum"/>.
/// <see cref="Value"/> is persisted; <see cref="DisplayLabel"/> is shown in the UI.
/// </summary>
public sealed record StorageEnumOption(string Value, string DisplayLabel, string? Help = null);

/// <summary>
/// UI adjustments applied to another field when a controlling enum equals <see cref="WhenValue"/>.
/// </summary>
public sealed record EnumUiPatch(
    string TargetFieldKey,
    /// <summary>When set, replaces the target field&apos;s label until the enum selection changes.</summary>
    string? LabelOverride = null,
    /// <summary>When set, shows or hides the target row.</summary>
    bool? Hidden = null,
    /// <summary>When set, written into the target field&apos;s stored value while this enum value is active.</summary>
    string? ValueWhenSelected = null);

/// <summary>
/// Describes UI behavior for one serialized enum value on the field that declares <see cref="StorageFieldDescriptor.EnumSelectionUiRules"/>.
/// </summary>
public sealed record EnumSelectionUiRule(string WhenValue, IReadOnlyList<EnumUiPatch> Patches);
