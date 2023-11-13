using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.Services;
using BUtil.Core.Storages;
using BUtil.Windows.Services;

namespace BUtil.Windows
{
    public class WindowsExperience : CrossPlatformExperience
    {
        public override ISessionService SessionService => new WindowsSessionService();

        public override ISupportManager GetSupportManager()
        {
            return new WindowsSupportManager();
        }

        public override IFolderService GetFolderService()
        {
            return new WindowsFolderService();
        }

        public override IMtpService? GetMtpService()
        {
            return new MtpService();
        }

        public override IStorage? GetMtpStorage(ILog log, MtpStorageSettings storageSettings)
        {
            return new MtpStorage(log, storageSettings);
        }

        public override ITaskSchedulerService? GetTaskSchedulerService()
        {
            return new TaskSchedulerService();
        }

        public override IShowLogOnSystemLoginService? GetShowLogOnSystemLoginService()
        {
            return new ShowLogOnSystemLoginService();
        }

        public override IWindowBlinkerService? GetWindowBlinkerService()
        {
            return new WindowBlinkerService();
        }

        public override IOsSleepPreventionService OsSleepPreventionService => new WindowsOsSleepPreventionService();

        public override ISmbService? GetSmbService()
        {
            return new SmbService();
        }

        public override IArchiver GetArchiver(ILog log)
        {
            return new SevenZipArchiver(log);
        }
    }
}