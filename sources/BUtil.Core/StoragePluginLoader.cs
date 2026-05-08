using BUtil.Core.Logs;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BUtil.Core.Storages;

/// <summary>
/// Loads storage plugins from the user plugin folder at runtime.
/// The folder is a sibling to the tasks folder:
///   %AppData%\BUtil Backup Plugins\   (or %AppData%\BUtil Backup Plugins - DEBUG\ in debug builds)
///
/// Drop a plugin DLL (plus its dependencies) into that folder.
/// Any public class implementing IStoragePlugin will be instantiated and Register() called.
/// </summary>
public static class StoragePluginLoader
{
    private static readonly string _pluginFolder;

    static StoragePluginLoader()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
#if DEBUG
        _pluginFolder = Path.Combine(appData, "BUtil Backup Plugins - DEBUG");
#else
        _pluginFolder = Path.Combine(appData, "BUtil Backup Plugins");
#endif
    }

    /// <summary>Returns the folder users should place plugin DLLs into.</summary>
    public static string PluginFolder => _pluginFolder;

    /// <summary>
    /// Scans PluginFolder for DLLs, loads each one, and calls Register() on every
    /// IStoragePlugin implementation found. Errors per-DLL are logged and skipped.
    /// </summary>
    public static void LoadFromPluginFolder(ILog? log = null)
    {
        if (!Directory.Exists(_pluginFolder))
            return;

        foreach (var dllPath in Directory.GetFiles(_pluginFolder, "*.dll"))
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
