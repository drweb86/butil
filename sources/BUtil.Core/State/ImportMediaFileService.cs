
using BUtil.Core.FileSystem;
using System;
using System.IO;
using System.Text.Json;

namespace BUtil.Core.State;

public class ImportMediaFileService
{
    private const string _extension = ".json";
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { WriteIndented = true };

    public static void DeleteState(string name)
    {
        var file = GetFileName(name);

        if (File.Exists(file))
            File.Delete(file);
    }

    public static void MoveState(string oldName, string newName)
    {
        var oldFile = GetFileName(oldName);
        var newFile = GetFileName(newName);
        if (File.Exists(oldFile) && oldFile != newName)
        {
            try
            {
                File.Move(oldFile, newFile);
            }
            catch { }
        }
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

    private static string GetFileName(string name)
    {
        if (name.Contains("..") || name.Contains('/') || name.Contains('\\'))
            throw new ArgumentException("No .. / and \\");

        return Path.Combine(Directories.ImportStateFolder, $"{name}{_extension}");
    }
}
