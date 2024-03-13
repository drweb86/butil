
using System;
using System.IO;
using System.Reflection;

namespace BUtil.Core.FileSystem;

public static class Directories
{
    private static readonly string _assembly = Assembly.GetExecutingAssembly().Location;
    private static readonly string _binariesDir = Path.GetDirectoryName(_assembly) ?? throw new DirectoryNotFoundException("binaries");
    private static readonly string _applicationDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

#if DEBUG
    private static readonly string _userDataFolder = Path.Combine(_applicationDataFolder, "BUtil-Development");
#else
    private static readonly string _userDataFolder = Path.Combine(_applicationDataFolder, "BUtil");
#endif
    private static readonly string _settingsDir = Path.Combine(_userDataFolder, "Settings", "v1");

    public static readonly string TempFolder = System.IO.Path.GetTempPath();

    public static string UserDataFolder => _userDataFolder;

    public static string BinariesDir => _binariesDir;

    public static string SettingsDir => _settingsDir;

    private static void CreateDirectory(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    public static string LogsFolder { get; }

    static Directories()
    {
        CreateDirectory(_userDataFolder);
        LogsFolder = Path.Combine(Directories.UserDataFolder, "Logs", "v3");
        if (!Directory.Exists(LogsFolder))
            Directory.CreateDirectory(LogsFolder);
    }
}
