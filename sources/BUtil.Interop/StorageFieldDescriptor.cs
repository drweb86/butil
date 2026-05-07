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
    // For Enum type: value is the serialized key, DisplayLabel is shown in the combobox
    public IReadOnlyList<(string Value, string DisplayLabel)>? Options { get; init; }
}
