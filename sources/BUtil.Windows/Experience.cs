using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.Storages;
using BUtil.Windows.Services;

namespace BUtil.Windows
{
    public class Experience : CrossPlatformExperience
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
    }
}