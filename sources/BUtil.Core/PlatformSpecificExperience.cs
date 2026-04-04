using BUtil.Core.FileSystem;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BUtil.Core;

public static class PlatformSpecificExperience
{
    private static CrossPlatformExperience? _instanceForTests;
    private static CrossPlatformExperience? _loadedInstance;

    public static CrossPlatformExperience Instance =>
        _instanceForTests ?? (_loadedInstance ??= LoadFromPlatformAssembly());

    /// <summary>
    /// Replaces the platform-specific implementation (e.g. in unit tests that do not reference BUtil.Windows).
    /// Pass <c>null</c> to restore default loading from the platform assembly.
    /// </summary>
    internal static void SetInstanceForTests(CrossPlatformExperience? instance)
    {
        _instanceForTests = instance;
        _loadedInstance = null;
    }

    private static CrossPlatformExperience LoadFromPlatformAssembly()
    {
        string assemblyFile = Files.WindowsExperience;
        if (OperatingSystem.IsWindows())
        {
            assemblyFile = Files.WindowsExperience;
        }
        else if (OperatingSystem.IsLinux())
        {
            assemblyFile = Files.LinuxExperience;
        }
        else
        {
            throw new PlatformNotSupportedException();
        }

        if (!File.Exists(assemblyFile))
        {
            var allFiles = string.Join(",", Directory.GetFiles(Directories.BinariesDir));
            throw new FileNotFoundException($"Cannot find platform experience library {assemblyFile}. Files in the directory are following: {allFiles}");
        }

        var assembly = Assembly.LoadFrom(assemblyFile);
        var experienceType = assembly
            .GetTypes()
            .Where(x => x.BaseType == typeof(CrossPlatformExperience))
            .SingleOrDefault() ?? throw new Exception("Could not locate single type derrived from CrossPlatformExperience");
        var instance = Activator.CreateInstance(experienceType) ?? throw new Exception("Instance is null");
        return (CrossPlatformExperience)instance;
    }
}
