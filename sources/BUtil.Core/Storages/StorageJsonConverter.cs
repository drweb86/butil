using BUtil.Core.ConfigurationFileModels.V2;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BUtil.Core.Storages;

/// <summary>
/// Polymorphic JSON converter for IStorageSettingsV2.
/// Uses StorageProviderRegistry to map "$type" discriminators to concrete types,
/// enabling plugins to register new storage types without modifying this assembly.
/// The discriminator values match the legacy [JsonDerivedType] values for backward compatibility.
/// </summary>
public sealed class StorageJsonConverter : JsonConverter<IStorageSettingsV2>
{
    private const string TypePropertyName = "$type";

    // Plain options without this converter — used for inner concrete-type serialization to avoid infinite recursion.
    private static readonly JsonSerializerOptions _innerOptions = new();

    public override IStorageSettingsV2? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);

        if (!doc.RootElement.TryGetProperty(TypePropertyName, out var typeElement))
            throw new JsonException($"Missing '{TypePropertyName}' property on storage settings object.");

        var discriminator = typeElement.GetString()
            ?? throw new JsonException($"'{TypePropertyName}' property is null in storage settings.");

        var targetType = StorageProviderRegistry.GetSettingsType(discriminator)
            ?? throw new JsonException($"Unknown storage type discriminator: '{discriminator}'. Register the provider before deserializing.");

        return (IStorageSettingsV2?)doc.RootElement.Deserialize(targetType, _innerOptions);
    }

    public override void Write(Utf8JsonWriter writer, IStorageSettingsV2 value, JsonSerializerOptions options)
    {
        var discriminator = StorageProviderRegistry.GetDiscriminator(value.GetType())
            ?? throw new JsonException($"No discriminator registered for settings type '{value.GetType().Name}'. Register the provider before serializing.");

        // Serialize the concrete type without this converter to avoid infinite recursion,
        // then re-emit with the $type discriminator prepended.
        var rawJson = JsonSerializer.Serialize(value, value.GetType(), _innerOptions);
        using var doc = JsonDocument.Parse(rawJson);

        writer.WriteStartObject();
        writer.WriteString(TypePropertyName, discriminator);
        foreach (var property in doc.RootElement.EnumerateObject())
            property.WriteTo(writer);
        writer.WriteEndObject();
    }
}
