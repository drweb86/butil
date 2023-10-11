using BUtil.Core.FileSystem;
using System;
using System.Linq;
using System.Reflection;

namespace BUtil.Core
{
    public static class PlatformSpecificExperience
    {
        public readonly static CrossPlatformExperience Instance;
        static PlatformSpecificExperience()
        {
            Instance = new CrossPlatformExperience();
            if (OperatingSystem.IsWindows())
            {
                var assembly = Assembly.LoadFrom(Files.WindowsExperience);
                var experienceType = assembly
                    .GetTypes()
                    .Where(x => x.BaseType == typeof(CrossPlatformExperience))
                    .First();

                var instance = Activator.CreateInstance(experienceType);
                if (instance != null)
                    Instance = (CrossPlatformExperience)instance;
            }
        }
    }
}
