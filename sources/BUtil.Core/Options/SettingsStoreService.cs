using BUtil.Core.FileSystem;
using System;
using System.IO;

namespace BUtil.Core.Options;

public class SettingsStoreService
{
    private readonly ILocalFileSystem _fileSystem;

    public SettingsStoreService(ILocalFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
        _fileSystem.EnsureFolderCreated(Directories.SettingsDir);
    }

    public string Load(string name, string defaultValue)
    {
        var file = GetFileName(name);
        if (_fileSystem.FileExists(file))
            return _fileSystem.ReadAllText(file);

        return defaultValue;
    }

    public void Save(string name, string value)
    {
        var file = GetFileName(name);
        if (_fileSystem.FileExists(file))
            _fileSystem.DeleteFile(file);
        _fileSystem.WriteAllText(file, value);
    }

    private static string GetFileName(string name)
    {
        if (name.Contains("..") || name.Contains('/') || name.Contains('\\'))
            throw new ArgumentException("No .. / and \\");

        return Path.Combine(Directories.SettingsDir, name);
    }
}
