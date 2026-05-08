using System.Collections.Generic;

namespace BUtil.Core.Storages;

public sealed class StorageFieldDescriptor
{
    public required string Key { get; init; }
    public required string Label { get; init; }
    public required StorageFieldType Type { get; init; }
    public string? Placeholder { get; init; }
    public bool IsOptional { get; init; }
    public object? DefaultValue { get; init; }
    public long? Min { get; init; }
    public long? Max { get; init; }

    /// <summary>
    /// For <see cref="StorageFieldType.Enum"/>: choices; <see cref="StorageEnumOption.Value"/> is persisted.
    /// </summary>
    public IReadOnlyList<StorageEnumOption>? Options { get; init; }

    /// <summary>
    /// For <see cref="StorageFieldType.Enum"/>: optional layout rules when the current serialized value matches <see cref="EnumSelectionUiRule.WhenValue"/>.
    /// Rules from multiple enum fields are applied in field order; later patches override earlier ones for the same target key.
    /// </summary>
    public IReadOnlyList<EnumSelectionUiRule>? EnumSelectionUiRules { get; init; }
}
