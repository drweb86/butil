using BUtil.Core.FileSystem;
using BUtil.Core.TasksTree;
using BUtil.Interop.Logs;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BUtil.Core;

/// <summary>
/// Loads task plugins at runtime from two optional locations, both ending in <c>plugins/tasks</c>:
/// <list type="bullet">
/// <item><description>Next to the application binaries (portable installs): <c>…/plugins/tasks/</c></description></item>
/// <item><description>Under the BUtil application data folder: <c>…/BUtil/plugins/tasks/</c></description></item>
/// </list>
/// Drop a plugin DLL (plus its dependencies) into either folder.
/// Any public class implementing <see cref="ITaskPlugin"/> will be instantiated and <c>Register()</c> called.
/// </summary>
public static class TaskPluginLoader
{
    private static string PluginTaskFolderUnder(string root) =>
        Path.Combine(root, "plugins", "tasks");

    private static readonly string _userPluginFolder = PluginTaskFolderUnder(Directories.UserDataFolder);

    public static string PluginFolder => _userPluginFolder;

    public static string ApplicationPluginFolder => PluginTaskFolderUnder(Directories.BinariesDir);

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
                log?.WriteLine(LoggingEvent.Error, $"[TaskPluginLoader] Failed to load '{Path.GetFileName(dllPath)}': {ex.Message}");
            }
        }
    }

    private static void LoadFromAssembly(Assembly assembly, ILog? log)
    {
        var pluginTypes = assembly.GetExportedTypes()
            .Where(t => typeof(ITaskPlugin).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass);

        foreach (var type in pluginTypes)
        {
            try
            {
                var plugin = (ITaskPlugin)Activator.CreateInstance(type)!;
                plugin.Register();
                log?.WriteLine(LoggingEvent.Debug, $"[TaskPluginLoader] Registered plugin: {type.FullName}");
            }
            catch (Exception ex)
            {
                log?.WriteLine(LoggingEvent.Error, $"[TaskPluginLoader] Failed to register '{type.FullName}': {ex.Message}");
            }
        }
    }
}
