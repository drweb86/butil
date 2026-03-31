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
    private readonly ILocalFileSystem _fileSystem;
    private const string _extensionV2 = ".v2.json";
    private const string _extensionV3 = ".v3.json";

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { WriteIndented = true };

    public TaskV2StoreService(ILocalFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
#if DEBUG
        _folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BUtil Backup Tasks - DEBUG");
#else
        _folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BUtil Backup Tasks");
#endif
        _fileSystem.EnsureFolderCreated(_folder);
    }

    public TaskV2? Load(string name)
    {
        return Load(name, out var _, out var _);
    }

    public TaskV2? Load(string name, out bool isNotFound, out bool isNotSupported)
    {
        MigrateAllToV3();

        isNotFound = true;
        isNotSupported = false;

        var fileName = GetFileName(name);
        if (!_fileSystem.FileExists(fileName))
            return null;

        isNotFound = false;

        var json = _fileSystem.ReadAllText(fileName);
        var result = JsonSerializer.Deserialize<TaskV2>(json);
        isNotSupported = result == null;
        if (result != null)
            PlatformSpecificExperience.Instance.SecretService.UnprotectInPlace(result);
        return result;
    }

    public void Save(TaskV2 task)
    {
        Delete(task.Name);

        var fileName = GetFileName(task.Name);
        var protectedTask = PlatformSpecificExperience.Instance.SecretService.CreateProtectedClone(task);
        var json = JsonSerializer.Serialize(protectedTask, _jsonSerializerOptions);
        _fileSystem.WriteAllText(fileName, json);
    }

    public void Delete(string name)
    {
        var fileName = GetFileName(name);
        if (_fileSystem.FileExists(fileName))
            _fileSystem.DeleteFile(fileName);
    }

    public bool TryValidate(string name, [NotNullWhen(false)] out string? error)
    {
        if (string.IsNullOrWhiteSpace(name) || ContainsIllegalChars(name))
        {
            error = Resources.Name_Field_Validation;
            return false;
        }

        var actualFileName = GetFileName(name);
        if (actualFileName.Length > 255)
        {
            error = string.Format(Resources.Name_Field_Validation_ExceedsLimit, name, actualFileName.Length - 255);
            return false;
        }

        error = null;
        return true;
    }

    internal void MigrateAllToV3()
    {
        var v2Files = _fileSystem.GetFiles(_folder, "*" + _extensionV2);

        foreach (var v2File in v2Files)
        {
            var fileName = Path.GetFileName(v2File);
            var name = fileName[..^_extensionV2.Length];

            var v3Path = Path.Combine(_folder, $"{name}{_extensionV3}");
            if (_fileSystem.FileExists(v3Path))
            {
                _fileSystem.DeleteFile(v2File);
                continue;
            }

            try
            {
                var json = _fileSystem.ReadAllText(v2File);
                _fileSystem.DeleteFile(v2File);

                var task = JsonSerializer.Deserialize<TaskV2>(json);

                if (task == null || !MigrateToV3IsSupportedModel(task))
                    continue;

                PlatformSpecificExperience.Instance.SecretService.UnprotectInPlace(task);
                var protectedTask = PlatformSpecificExperience.Instance.SecretService.CreateProtectedClone(task);
                var v3Json = JsonSerializer.Serialize(protectedTask, _jsonSerializerOptions);
                _fileSystem.WriteAllText(v3Path, v3Json);
            }
            catch
            {
                // Skip files that cannot be read or migrated
            }
        }
    }

    private static bool MigrateToV3IsSupportedModel(TaskV2 task)
    {
        return task.Model is IncrementalBackupModelOptionsV2
            || task.Model is SynchronizationTaskModelOptionsV2
            || task.Model is ImportMediaTaskModelOptionsV2
            || task.Model is BUtilServerModelOptionsV2
            || task.Model is BUtilClientModelOptionsV2;
    }

    private static bool ContainsIllegalChars(string text)
    {
        var invalidChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
        return invalidChars.Any(ch => text.Contains(ch))
            || text.Contains("..");
    }

    public IEnumerable<string> GetNames()
    {
        MigrateAllToV3();

        return [.. _fileSystem
            .GetFiles(_folder, "*" + _extensionV3)
            .Select(x => Path.GetFileName(x) ?? throw new InvalidDataException(x))
            .Select(x => x.Replace(_extensionV3, string.Empty))
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)];
    }

    private string GetFileName(string name)
    {
        if (name.Contains("..") || name.Contains('/') || name.Contains('\\'))
            throw new ArgumentException("No .. / and \\");

        return Path.Combine(_folder, $"{name}{_extensionV3}");
    }
}
