using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.Services;
using BUtil.Core.Storages;
using BUtil.Linux.Services;
using BUtil.Windows.Services;

namespace BUtil.Windows;

public class LinuxExperience : CrossPlatformExperience
{
    public override IStorage GetSmbStorage(ILog log, SambaStorageSettingsV2 settings)
    {
        return new LinuxSambaStorage(log, settings);
    }

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

    public override IUiService UiService => new LinuxUiService();

    public override IOsSleepPreventionService OsSleepPreventionService => new LinuxOsSleepPreventionService();

    public override ILegacyObsoleteArchiver GetArchiver(ILog log)
    {
        return new LinuxAptGetArchiver(log);
    }

    public override IFolderService GetFolderService()
    {
        return new LinuxFolderService();
    }

    public override ISupportManager SupportManager => new LinuxSupportManager();
}