using BUtil.Core.FileSystem;
using System;
using System.IO;

namespace BUtil.Core.Options;

public class SettingsStoreService
{
    public SettingsStoreService()
    {
        FileHelper.EnsureFolderCreated(Directories.SettingsDir);
    }

    public string Load(string name, string defaultValue)
    {
        var file = GetFileName(name);
        if (File.Exists(file))
            return File.ReadAllText(file);

        return defaultValue;
    }

    public void Save(string name, string value)
    {
        var file = GetFileName(name);
        if (File.Exists(file))
            File.Delete(file);
        File.WriteAllText(file, value);
    }

    private string GetFileName(string name)
    {
        if (name.Contains("..") || name.Contains('/') || name.Contains('\\'))
            throw new ArgumentException(nameof(name));

        return Path.Combine(Directories.SettingsDir, name);
    }
}
