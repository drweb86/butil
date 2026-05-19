using BUtil.Core.FileSystem;
using BUtil.Core.Logs;
using BUtil.Interop.Logs;
using BUtil.Interop.Tasks.UI;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BUtil.UI;

/// <summary>
/// Loads task UI plugins from built-in task UI assemblies and optional <c>plugins/task-uis</c> folders.
/// </summary>
public static class TaskUIPluginLoader
{
    public static void LoadAll() => LoadAll(new MemoryLog());

    public static void LoadAll(ILog log)
    {
        ArgumentNullException.ThrowIfNull(log);

        LoadDllsFromDirectory(Directories.BinariesDir, "BUtil.Tasks.*.UI.dll", log);
        LoadDllsFromDirectory(Directories.ApplicationTaskUiPlugins, "*.dll", log);
        LoadDllsFromDirectory(Directories.UserTaskUiPlugins, "*.dll", log);
    }

    private static void LoadDllsFromDirectory(string directory, string searchPattern, ILog log)
    {
        if (!Directory.Exists(directory))
            return;

        foreach (var dllPath in Directory.GetFiles(directory, searchPattern))
        {
            try
            {
                var assembly = Assembly.LoadFrom(dllPath);
                LoadFromAssembly(assembly, log);
            }
            catch (Exception ex)
            {
                log.WriteLine(LoggingEvent.Error, $"[TaskUIPluginLoader] Failed to load '{Path.GetFileName(dllPath)}': {ex.Message}");
            }
        }
    }

    private static void LoadFromAssembly(Assembly assembly, ILog log)
    {
        var pluginTypes = assembly.GetExportedTypes()
            .Where(t => typeof(ITaskUIPlugin).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass);

        foreach (var type in pluginTypes)
        {
            try
            {
                var plugin = (ITaskUIPlugin)Activator.CreateInstance(type)!;
                plugin.Register();
                log.WriteLine(LoggingEvent.Debug, $"[TaskUIPluginLoader] Registered plugin: {type.FullName}");
            }
            catch (Exception ex)
            {
                log.WriteLine(LoggingEvent.Error, $"[TaskUIPluginLoader] Failed to register '{type.FullName}': {ex.Message}");
            }
        }
    }

}
