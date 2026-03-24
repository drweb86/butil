using BUtil.Core.ConfigurationFileModels.V2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    private static readonly HashSet<string> _secretPropertyNames = new(StringComparer.Ordinal) { "Password" };
#pragma warning restore IDE0028 // Simplify collection initialization

    public TaskV2 CreateProtectedClone(TaskV2 task)
    {
        var serialized = JsonSerializer.Serialize(task);
        var clone = JsonSerializer.Deserialize<TaskV2>(serialized) ?? throw new InvalidDataException("Unable to clone task.");
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
        TransformObject(root, transform, visited);
    }

    private static void TransformObject(object? current, Func<string, string> transform, HashSet<object> visited)
    {
        if (current == null)
            return;

        var type = current.GetType();
        if (type == typeof(string) || type.IsPrimitive || type.IsEnum)
            return;

        if (!visited.Add(current))
            return;

        if (current is IEnumerable enumerable and not IDictionary)
        {
            foreach (var item in enumerable)
                TransformObject(item, transform, visited);
            return;
        }

        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var property in properties)
        {
            if (property.GetIndexParameters().Length > 0 || !property.CanRead)
                continue;

            if (property.PropertyType == typeof(string) &&
                property.CanWrite &&
                _secretPropertyNames.Contains(property.Name))
            {
                if (property.GetValue(current) is string value)
                    property.SetValue(current, transform(value));
                continue;
            }

            if (property.PropertyType == typeof(string))
                continue;

            var nested = property.GetValue(current);
            TransformObject(nested, transform, visited);
        }
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
