using BUtil.Interop.Tasks;
using BUtil.Core.Storages;
using System.Text.Json;

namespace BUtil.Core.Serialization;

/// <summary>
/// Shared JsonSerializerOptions for TaskV2 serialization.
/// Must be used anywhere TaskV2 (or any object containing IStorageSettingsV2) is serialized or deserialized.
/// </summary>
public static class JsonOptions
{
    public static readonly JsonSerializerOptions TaskSerialization = new()
    {
        WriteIndented = true,
        Converters = { new StorageJsonConverter(), new TaskJsonConverter() },
    };
}
