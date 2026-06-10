
using System;
using System.IO;
using System.Reflection;

namespace BUtil.Core.FileSystem;

public static class Directories
{
    private static readonly string _assembly = Assembly.GetExecutingAssembly().Location;
    private static readonly string _binariesDir = Path.GetDirectoryName(_assembly) ?? throw new DirectoryNotFoundException("binaries");
    private static readonly string _applicationDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    private static readonly string _localApplicationDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

#if DEBUG
    private static readonly string _userDataFolder = Path.Combine(_applicationDataFolder, "BUtil-Development");
#else
    private static readonly string _userDataFolder = Path.Combine(_applicationDataFolder, "BUtil");
#endif
    private static readonly string _settingsDir = Path.Combine(_userDataFolder, "Settings", "v1");
    private static readonly string _userTaskPlugins = PluginTaskFolderUnder(_userDataFolder);
    private static readonly string _userTaskUiPlugins = PluginTaskUiFolderUnder(_userDataFolder);

    private static string PluginTaskFolderUnder(string root) =>
        Path.Combine(root, "plugins", "tasks");

    private static string PluginTaskUiFolderUnder(string root) =>
        Path.Combine(root, "plugins", "task-uis");

    public static readonly string TempFolder = System.IO.Path.GetTempPath();

    public static string UserDataFolder => _userDataFolder;
    public static string StateFolder => Path.Combine(_userDataFolder, "States");

    public static string BinariesDir => _binariesDir;
    public static string UserTaskPlugins => _userTaskPlugins;
    public static string ApplicationTaskPlugins => PluginTaskFolderUnder(_binariesDir);
    public static string UserTaskUiPlugins => _userTaskUiPlugins;
    public static string ApplicationTaskUiPlugins => PluginTaskUiFolderUnder(_binariesDir);

    public static string SettingsDir => _settingsDir;

    public static string LogsFolder { get; }

    /// <summary>Previous logs location under temp folder.</summary>
    public static string TempLogsFolder { get; }

    /// <summary>Previous logs location under application data (pre temp/task-folder layout).</summary>
    public static string LegacyLogsFolder { get; }

    public static string ImportStateFolder { get; }

    static Directories()
    {
        FileHelper.EnsureFolderCreated(_userDataFolder);
        LegacyLogsFolder = Path.Combine(_userDataFolder, "Logs", "v3");
#if DEBUG
        var logsAppFolder = "BUtil-Development";
#else
        var logsAppFolder = "BUtil";
#endif
        TempLogsFolder = Path.Combine(TempFolder, logsAppFolder, "logs");
        LogsFolder = Path.Combine(_localApplicationDataFolder, logsAppFolder, "logs", "v4");
        FileHelper.EnsureFolderCreated(LogsFolder);
        FileHelper.EnsureFolderCreated(StateFolder);
#if DEBUG
        ImportStateFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BUtil Backup Tasks - DEBUG - States");
#else
        ImportStateFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BUtil Backup Tasks - States");
#endif
        FileHelper.EnsureFolderCreated(ImportStateFolder);
    }
}
