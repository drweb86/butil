
using BUtil.Core.FileSystem;
using System;
using System.IO;
using System.Text.Json;

namespace BUtil.Core.State;

public class ImportMediaFileService
{
    private readonly string _folder;
    private const string _extension = ".json";
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { WriteIndented = true };

    public ImportMediaFileService()
    {
#if DEBUG
        _folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BUtil Backup Tasks - DEBUG - States");
#else
        _folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BUtil Backup Tasks - States");
#endif
        FileHelper.EnsureFolderCreated(_folder);
    }

    public ImportMediaState? Load(string name)
    {
        var fileName = GetFileName(name);
        if (!File.Exists(fileName))
            return null;
        var json = File.ReadAllText(fileName);
        return JsonSerializer.Deserialize<ImportMediaState>(json);
    }


    public void Save(ImportMediaState task, string name)
    {
        var fileName = GetFileName(name);
        var json = JsonSerializer.Serialize(task, _jsonSerializerOptions);
        if (File.Exists(fileName))
            File.Delete(fileName);
        File.WriteAllText(fileName, json);
    }

    private string GetFileName(string name)
    {
        if (name.Contains("..") || name.Contains('/') || name.Contains('\\'))
            throw new ArgumentException("No .. / and \\");

        return Path.Combine(_folder, $"{name}{_extension}");
    }
}
