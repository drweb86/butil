using BUtil.Core.Storages;
using BUtil.Interop.Tasks;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BUtil.Core.Serialization;

/// <summary>
/// Polymorphic JSON converter for <see cref="ITaskModelOptionsV2"/>.
/// Uses <see cref="TaskProviderRegistry"/> to map "$type" discriminators to concrete types.
/// </summary>
public sealed class TaskJsonConverter : JsonConverter<ITaskModelOptionsV2>
{
    private const string TypePropertyName = "$type";

    private static readonly JsonSerializerOptions _innerOptions = new()
    {
        Converters = { new StorageJsonConverter() },
    };

    public override ITaskModelOptionsV2? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);

        if (!doc.RootElement.TryGetProperty(TypePropertyName, out var typeElement))
            throw new JsonException($"Missing '{TypePropertyName}' property on task model object.");

        var jsonType = typeElement.GetString()
            ?? throw new JsonException($"'{TypePropertyName}' property is null in task model.");

        var targetType = TaskProviderRegistry.FindModelType(jsonType)
            ?? throw new JsonException($"Unknown task type discriminator: '{jsonType}'. Register the provider before deserializing.");

        return (ITaskModelOptionsV2?)doc.RootElement.Deserialize(targetType, _innerOptions);
    }

    public override void Write(Utf8JsonWriter writer, ITaskModelOptionsV2 value, JsonSerializerOptions options)
    {
        var jsonType = TaskProviderRegistry.FindJsonType(value.GetType())
            ?? throw new JsonException($"No discriminator registered for model type '{value.GetType().Name}'. Register the provider before serializing.");

        var rawJson = JsonSerializer.Serialize(value, value.GetType(), _innerOptions);
        using var doc = JsonDocument.Parse(rawJson);

        writer.WriteStartObject();
        writer.WriteString(TypePropertyName, jsonType);
        foreach (var property in doc.RootElement.EnumerateObject())
            property.WriteTo(writer);
        writer.WriteEndObject();
    }
}
