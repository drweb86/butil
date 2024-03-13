using BUtil.Core.FileSystem;
using System;
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
        else if (OperatingSystem.IsLinux())
        {
            assemblyFile = Files.LinuxExperience;
        }
        else
        {
            throw new PlatformNotSupportedException();
        }

        var assembly = Assembly.LoadFrom(assemblyFile);
        var experienceType = assembly
            .GetTypes()
            .Where(x => x.BaseType == typeof(CrossPlatformExperience))
            .SingleOrDefault();

        if (experienceType == null)
            throw new Exception("Could not locate single type derrived from CrossPlatformExperience");

        var instance = Activator.CreateInstance(experienceType);

        if (instance == null)
            throw new Exception("Instance is null");

        Instance = (CrossPlatformExperience)instance;
    }
}
