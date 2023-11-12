using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.Services;
using BUtil.Core.Storages;

namespace BUtil.Core
{
    public abstract class CrossPlatformExperience
    {
        public abstract ISupportManager GetSupportManager();
        public abstract IArchiver GetArchiver(ILog log);
        public abstract IFolderService GetFolderService();

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

        public virtual ISmbService? GetSmbService()
        {
            return null;
        }
    }
}
