using BUtil.Core.FileSystem;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BUtil.Core;

public static class PlatformSpecificExperience
{
    public readonly static CrossPlatformExperience Instance;
    static PlatformSpecificExperience()
    {
        string assemblyFile = Files.WindowsExperience;
        if (OperatingSystem.IsWindows())
        {
            assemblyFile = Files.WindowsExperience;
        }
        else if (OperatingSystem.IsAndroid())
        {
            assemblyFile = Files.AndroidExperience;
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
        Instance = (CrossPlatformExperience)instance;
    }
}
