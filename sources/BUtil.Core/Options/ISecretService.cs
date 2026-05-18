using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Serialization;
using BUtil.Core.Storages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace BUtil.Core.Options;

public interface ISecretService
{
    TaskV2 CreateProtectedClone(TaskV2 task);
    void UnprotectInPlace(TaskV2 task);
}

public abstract class SecretServiceBase : ISecretService
{
    private const string _prefix = "enc::";
#pragma warning disable IDE0028 // Simplify collection initialization
    private static readonly HashSet<string> _taskSecretPropertyNames = new(StringComparer.Ordinal) { "Password" };
#pragma warning restore IDE0028 // Simplify collection initialization

    public TaskV2 CreateProtectedClone(TaskV2 task)
    {
        var serialized = JsonSerializer.Serialize(task, JsonOptions.TaskSerialization);
        var clone = JsonSerializer.Deserialize<TaskV2>(serialized, JsonOptions.TaskSerialization) ?? throw new InvalidDataException("Unable to clone task.");
        TransformInPlace(clone, ProtectString);
        return clone;
    }

    public void UnprotectInPlace(TaskV2 task)
    {
        TransformInPlace(task, UnprotectString);
    }

    private static void TransformInPlace(object root, Func<string, string> transform)
    {
        var visited = new HashSet<object>(ReferenceEqualityComparer.Instance);
        _ = TransformObject(root, transform, visited);
    }

    private static object? TransformObject(object? current, Func<string, string> transform, HashSet<object> visited)
    {
        if (current == null)
            return null;

        var type = current.GetType();
        if (type == typeof(string) || type.IsValueType)
            return current;

        if (!visited.Add(current))
            return current;

        if (current is IStorageSettingsV2 storageSettings &&
            TransformStorageSettings(storageSettings, transform))
            return current;

        if (current is IEnumerable enumerable and not IDictionary)
        {
            foreach (var item in enumerable)
                TransformObject(item, transform, visited);
            return current;
        }

        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var property in properties)
        {
            if (property.GetIndexParameters().Length > 0 || !property.CanRead)
                continue;

            if (property.PropertyType == typeof(string) &&
                property.CanWrite &&
                _taskSecretPropertyNames.Contains(property.Name))
            {
                if (property.GetValue(current) is string value)
                    property.SetValue(current, transform(value));
                continue;
            }

            if (property.PropertyType == typeof(string))
                continue;

            var nested = property.GetValue(current);
            var transformed = TransformObject(nested, transform, visited);
            if (property.CanWrite && !ReferenceEquals(nested, transformed))
                property.SetValue(current, transformed);
        }

        return current;
    }

    private static bool TransformStorageSettings(IStorageSettingsV2 settings, Func<string, string> transform)
    {
        var provider = StorageProviderRegistry.FindForSettings(settings);
        if (provider == null)
            return false;

        var protectedPropertyNames = provider.Provider.SecretSettingsProperties
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Distinct(StringComparer.OrdinalIgnoreCase);

        var properties = settings.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(property =>
                property.PropertyType == typeof(string) &&
                property.CanRead &&
                property.CanWrite)
            .ToDictionary(property => property.Name, StringComparer.OrdinalIgnoreCase);

        foreach (var propertyName in protectedPropertyNames)
        {
            if (!properties.TryGetValue(propertyName, out var property))
                continue;

            if (property.GetValue(settings) is string value)
                property.SetValue(settings, transform(value));
        }

        return true;
    }

    private string ProtectString(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return value;
        if (value.StartsWith(_prefix, StringComparison.Ordinal))
            return value;

        var plainBytes = Encoding.UTF8.GetBytes(value);
        var protectedBytes = ProtectBytes(plainBytes);

        return _prefix + Convert.ToBase64String(protectedBytes);
    }

    private string UnprotectString(string value)
    {
        if (!value.StartsWith(_prefix, StringComparison.Ordinal))
            return value;

        var rawBytes = Convert.FromBase64String(value[_prefix.Length..]);
        var plainBytes = UnprotectBytes(rawBytes);

        return Encoding.UTF8.GetString(plainBytes);
    }

    protected abstract byte[] ProtectBytes(byte[] plainBytes);

    protected abstract byte[] UnprotectBytes(byte[] encryptedBytes);

    private sealed class ReferenceEqualityComparer : IEqualityComparer<object>
    {
        public static ReferenceEqualityComparer Instance { get; } = new();
        public new bool Equals(object? x, object? y) => ReferenceEquals(x, y);
        public int GetHashCode(object obj) => System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
    }
}
