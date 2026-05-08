using BUtil.Core;
using BUtil.Core.Options;
using BUtil.Core.Services;
using BUtil.Core.Storages;
using BUtil.Storages.Ftps;
using BUtil.Storages.Samba;
using BUtil.Storages.Sftp;
using BUtil.Windows.Services;

namespace BUtil.Windows;

public class WindowsExperience : CrossPlatformExperience
{
    public override ISecretService SecretService => new WindowsSecretService();

    public override int MinimumListenerPort => 1;

    public override ISessionService SessionService => new WindowsSessionService();

    public override ISupportManager SupportManager => new WindowsSupportManager();

    public override IFolderService GetFolderService()
    {
        return new WindowsFolderService();
    }

    public override ITaskSchedulerService GetTaskSchedulerService()
    {
        return new TaskSchedulerService();
    }

    public override IShowLogOnSystemLoginService? GetShowLogOnSystemLoginService()
    {
        return new ShowLogOnSystemLoginService();
    }

    public override IConsoleWindowService? GetConsoleWindowService()
    {
        return new WindowsConsoleWindowService();
    }

    public override IUiService UiService => new WindowsUiService();

    public override IOsSleepPreventionService OsSleepPreventionService => new WindowsOsSleepPreventionService();

    public override void RegisterPlatformStorages()
    {
        SftpStoragePlugin.Register();
        FtpsStoragePlugin.Register();
        StorageProviderRegistry.Register(
            new SambaStorageSettingsProvider(),
            typeof(SambaStorageSettingsV2),
            (log, s, _) => new WindowsSambaStorage(log, (SambaStorageSettingsV2)s));
    }
}
