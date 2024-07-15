using BUtil.Core.ConfigurationFileModels.V1;
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
    private const string _genericFilter = "*.json";
    private const string _extensionV1 = ".json";
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

            if (pair.Key == 1)
            {
                try
                {
                    var task = JsonSerializer.Deserialize<BackupTaskV1>(json);
                    if (task == null)
                        continue;

                    var result = UpgradeV1ToLatest(task);
                    isNotSupported = result == null;
                    return result;
                }
                catch (System.Text.Json.JsonException)
                {
                    return null;
                }
            }
            else if (pair.Key == 2)
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

    private TaskV2? UpgradeV1ToLatest(BackupTaskV1 task)
    {
        if (task.Model is not IncrementalBackupModelOptionsV1 incrementalModelV1)
        {
            return null;
        }

        return new TaskV2
        {
            Name = task.Name,
            Model = new IncrementalBackupModelOptionsV2
            {
                Password = task.Password,
                FileExcludePatterns = task.FileExcludePatterns,
                Items = task.Items.Select(UpgradeSourceItemV1ToLatest).ToList(),
                To = UpgradeStorageSettingsV1ToLatest(task.Storages.Single()),
            }
        };
    }

    private static IStorageSettingsV2 UpgradeStorageSettingsV1ToLatest(IStorageSettingsV1 storageSettingsV1)
    {
        if (storageSettingsV1 is SambaStorageSettingsV1)
        {
            var typedStorage = (SambaStorageSettingsV1)storageSettingsV1;
            return new SambaStorageSettingsV2
            {
                Password = typedStorage.Password,
                SingleBackupQuotaGb = typedStorage.SingleBackupQuotaGb,
                Url = typedStorage.Url,
                User = typedStorage.User,
            };
        }
        else if (storageSettingsV1 is FolderStorageSettingsV1 typedStorage)
        {
            return new FolderStorageSettingsV2
            {
                DestinationFolder = typedStorage.DestinationFolder,
                MountPowershellScript = typedStorage.MountPowershellScript,
                SingleBackupQuotaGb = typedStorage.SingleBackupQuotaGb,
                UnmountPowershellScript = typedStorage.UnmountPowershellScript,
            };
        }
        else
        {
            throw new InvalidDataException();
        }
    }

    private SourceItemV2 UpgradeSourceItemV1ToLatest(SourceItemV1 sourceItemV1)
    {
        return new SourceItemV2
        {
            Id = sourceItemV1.Id,
            IsFolder = sourceItemV1.IsFolder,
            Target = sourceItemV1.Target,
        };
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
            .GetFiles(_folder, _genericFilter)
            .Select(x => Path.GetFileName(x) ?? throw new InvalidDataException(x))
            .Select(x => x.Replace(_extensionV2, string.Empty))
            .Select(x => x.Replace(_extensionV1, string.Empty))
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)];
    }

    private Dictionary<int, string> GetFileNames(string name)
    {
        if (name.Contains("..") || name.Contains('/') || name.Contains('\\'))
            throw new ArgumentException("No .. / and \\");

        return new Dictionary<int, string>
        {
            { 1, Path.Combine(_folder, $"{name}{_extensionV1}") },
            { 2, Path.Combine(_folder, $"{name}{_extensionV2}") },
        };
    }
}
