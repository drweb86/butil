﻿using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.Services;
using BUtil.Core.Storages;
using BUtil.Linux.Services;

namespace BUtil.Windows
{
    public class LinuxExperience : CrossPlatformExperience
    {
        public override ISessionService SessionService => new LinuxSessionService();

        public override IMtpService? GetMtpService()
        {
            return null;
        }

        public override IStorage? GetMtpStorage(ILog log, MtpStorageSettings storageSettings)
        {
            return null;
        }

        public override ITaskSchedulerService? GetTaskSchedulerService()
        {
            return null;
        }

        public override IShowLogOnSystemLoginService? GetShowLogOnSystemLoginService()
        {
            return null;
        }

        public override IWindowBlinkerService? GetWindowBlinkerService()
        {
            return null;
        }

        public override IOsSleepPreventionService? GetIOsSleepPreventionService()
        {
            return null;
        }

        public override ISmbService? GetSmbService()
        {
            return null;
        }

        public override IArchiver GetArchiver(ILog log)
        {
            return new AptGetSevenZipArchiver(log);
        }

        public override IFolderService GetFolderService()
        {
            return new LinuxFolderService();
        }

        public override ISupportManager GetSupportManager()
        {
            return new LinuxSupportManager();
        }
    }
}