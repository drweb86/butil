using BUtil.Core.FileSystem;
using BUtil.Core.Logs;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BUtil.Core.Storages;

/// <summary>
/// Loads storage plugins at runtime from two optional locations, both ending in <c>plugins/storages</c>:
/// <list type="bullet">
/// <item><description>Next to the application binaries (portable installs): <c>…/plugins/storages/</c></description></item>
/// <item><description>Under the BUtil application data folder (see <see cref="Directories.UserDataFolder"/>): <c>…/BUtil/plugins/storages/</c> (debug builds use <c>BUtil-Development</c>)</description></item>
/// </list>
/// Drop a plugin DLL (plus its dependencies) into either folder.
/// Any public class implementing <see cref="IStoragePlugin"/> will be instantiated and <c>Register()</c> called.
/// The application folder is scanned first, then the user folder; the first registration wins if the same storage type is registered twice.
/// </summary>
public static class StoragePluginLoader
{
    private static string PluginStorageFolderUnder(string root) =>
        Path.Combine(root, "plugins", "storages");

    private static readonly string _userPluginFolder = PluginStorageFolderUnder(Directories.UserDataFolder);

    /// <summary>Folder under <see cref="Directories.UserDataFolder"/> where plugin DLLs can be placed.</summary>
    public static string PluginFolder => _userPluginFolder;

    /// <summary>
    /// Folder next to the application binaries for plugin DLLs (optional). Useful for portable deployments on a USB drive or self-contained directory.
    /// </summary>
    public static string ApplicationPluginFolder => PluginStorageFolderUnder(Directories.BinariesDir);

    /// <summary>
    /// Scans <see cref="ApplicationPluginFolder"/> then <see cref="PluginFolder"/> for DLLs, loads each assembly,
    /// and calls <c>Register()</c> on every <see cref="IStoragePlugin"/> implementation found.
    /// Errors per DLL are logged and skipped.
    /// </summary>
    public static void LoadFromPluginFolder(ILog? log = null)
    {
        LoadDllsFromDirectory(ApplicationPluginFolder, log);
        LoadDllsFromDirectory(_userPluginFolder, log);
    }

    private static void LoadDllsFromDirectory(string directory, ILog? log)
    {
        if (!Directory.Exists(directory))
            return;

        foreach (var dllPath in Directory.GetFiles(directory, "*.dll"))
        {
            try
            {
                var assembly = Assembly.LoadFrom(dllPath);
                LoadFromAssembly(assembly, log);
            }
            catch (Exception ex)
            {
                log?.WriteLine(LoggingEvent.Error, $"[StoragePluginLoader] Failed to load '{Path.GetFileName(dllPath)}': {ex.Message}");
            }
        }
    }

    private static void LoadFromAssembly(Assembly assembly, ILog? log)
    {
        var pluginTypes = assembly.GetExportedTypes()
            .Where(t => typeof(IStoragePlugin).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass);

        foreach (var type in pluginTypes)
        {
            try
            {
                var plugin = (IStoragePlugin)Activator.CreateInstance(type)!;
                plugin.Register();
                log?.WriteLine(LoggingEvent.Debug, $"[StoragePluginLoader] Registered plugin: {type.FullName}");
            }
            catch (Exception ex)
            {
                log?.WriteLine(LoggingEvent.Error, $"[StoragePluginLoader] Failed to register '{type.FullName}': {ex.Message}");
            }
        }
    }
}
