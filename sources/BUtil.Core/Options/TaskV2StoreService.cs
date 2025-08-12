using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace BUtil.Core.Options;

public class TaskV2StoreService
{
    private readonly string _folder;
    private const string _extensionV2 = ".v2.json";
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { WriteIndented = true };

    public TaskV2StoreService()
    {
#if DEBUG
        _folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BUtil Backup Tasks - DEBUG");
#else
        _folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BUtil Backup Tasks");
#endif
        FileHelper.EnsureFolderCreated(_folder);
    }

    public TaskV2? Load(string name)
    {
        return Load(name, out var _, out var _);
    }

    public TaskV2? Load(string name, out bool isNotFound, out bool isNotSupported)
    {
        isNotFound = true;
        isNotSupported = false;

        foreach (var pair in GetFileNames(name))
        {
            if (!File.Exists(pair.Value))
                continue;

            isNotFound = false;
            isNotSupported = true;

            var json = File.ReadAllText(pair.Value);

            if (pair.Key == 2)
            {
                var result = JsonSerializer.Deserialize<TaskV2>(json);
                isNotSupported = result == null;
                return result;
            }
            else
            {
                return null;
            }
        }

        return null;
    }

    public void Save(TaskV2 task)
    {
        Delete(task.Name);

        var fileName = GetFileNames(task.Name).Last().Value;
        var json = JsonSerializer.Serialize(task, _jsonSerializerOptions);
        File.WriteAllText(fileName, json);
    }

    public void Delete(string name)
    {
        foreach (var pair in GetFileNames(name))
            if (File.Exists(pair.Value))
                File.Delete(pair.Value);
    }

    public bool TryValidate(string name, [NotNullWhen(false)] out string? error)
    {
        if (string.IsNullOrWhiteSpace(name) || ContainsIllegalChars(name))
        {
            error = Resources.Name_Field_Validation;
            return false;
        }

        var actualFileName = GetFileNames(name).Last().Value;
        if (actualFileName.Length > 255)
        {
            error = string.Format(Resources.Name_Field_Validation_ExceedsLimit, name, actualFileName.Length - 255);
            return false;
        }

        error = null;
        return true;
    }

    private static bool ContainsIllegalChars(string text)
    {
        var invalidChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
        return invalidChars.Any(ch => text.Contains(ch))
            || text.Contains("..");
    }

    public IEnumerable<string> GetNames()
    {
        return [.. Directory
            .GetFiles(_folder, _extensionV2)
            .Select(x => Path.GetFileName(x) ?? throw new InvalidDataException(x))
            .Select(x => x.Replace(_extensionV2, string.Empty))
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)];
    }

    private Dictionary<int, string> GetFileNames(string name)
    {
        if (name.Contains("..") || name.Contains('/') || name.Contains('\\'))
            throw new ArgumentException("No .. / and \\");

        return new Dictionary<int, string>
        {
            { 2, Path.Combine(_folder, $"{name}{_extensionV2}") },
        };
    }
}
