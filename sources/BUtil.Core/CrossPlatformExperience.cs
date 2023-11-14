using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.Services;
using BUtil.Core.Storages;

namespace BUtil.Core
{
    public abstract class CrossPlatformExperience
    {
        public abstract ISessionService SessionService { get; }
        public abstract ISupportManager SupportManager { get; }
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

        public abstract IUiService UiService { get; }

        public abstract IOsSleepPreventionService OsSleepPreventionService { get; }

        public virtual ISmbService? GetSmbService()
        {
            return null;
        }
    }
}
