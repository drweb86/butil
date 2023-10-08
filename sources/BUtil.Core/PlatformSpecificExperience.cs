using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.FileSystem;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.Storages;
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
                var assembly = Assembly.LoadFile(Files.WindowsExperience);
                var experienceType = assembly
                    .GetTypes()
                    .Where(x => x.BaseType == typeof(CrossPlatformExperience))
                    .First();

                Instance = (CrossPlatformExperience)Activator.CreateInstance(experienceType);
            }
        }
    }

    public class CrossPlatformExperience
    {
        public virtual IMtpService? GetMtpService()
        {
            return null;
        }

        public virtual IStorage? GetMtpStorage(ILog log, MtpStorageSettings storageSettings)
        {
            return null;
        }

        public virtual ITaskSchedulerService? GetTaskSchedulerService()
        {
            return null;
        }
    }
}
