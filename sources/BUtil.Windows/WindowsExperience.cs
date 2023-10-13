using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.Services;
using BUtil.Core.Storages;
using BUtil.Windows.Services;
using System.Reflection;

namespace BUtil.Windows
{
    public class WindowsExperience : CrossPlatformExperience
    {
        public override IMtpService GetMtpService()
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

        public override IOsSleepPreventionService? GetIOsSleepPreventionService()
        {
            return new OsSleepPreventionService();
        }

        public override ISmbService? GetSmbService()
        {
            return new SmbService();
        }
    }
}