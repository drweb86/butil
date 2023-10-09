using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.Services;
using BUtil.Core.Storages;

namespace BUtil.Core
{
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

        public virtual IShowLogOnSystemLoginService? GetShowLogOnSystemLoginService()
        {
            return null;
        }

        public virtual IWindowBlinkerService? GetWindowBlinkerService()
        {
            return null;
        }

        public virtual IOsSleepPreventionService? GetIOsSleepPreventionService()
        {
            return null;
        }
    }
}
