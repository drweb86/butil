
using System;
using System.Collections.Generic;
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
    public static string StateFolder => Path.Combine(_userDataFolder, "States");

    public static string BinariesDir => _binariesDir;

    public static string SettingsDir => _settingsDir;

    public static string LogsFolder { get; }

    public static string ImportStateFolder { get; }

    static Directories()
    {
        FileHelper.EnsureFolderCreated(_userDataFolder);
        LogsFolder = Path.Combine(Directories.UserDataFolder, "Logs", "v3");
        FileHelper.EnsureFolderCreated(LogsFolder);
        FileHelper.EnsureFolderCreated(StateFolder);
#if DEBUG
        ImportStateFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BUtil Backup Tasks - DEBUG - States");
#else
        ImportStateFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BUtil Backup Tasks - States");
#endif
        FileHelper.EnsureFolderCreated(ImportStateFolder);
    }

    public static IEnumerable<string> GetDefaultBackupFolders()
    {
        return [
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),
            Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
            Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
        ];
    }
}
