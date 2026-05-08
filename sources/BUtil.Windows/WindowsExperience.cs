using BUtil.Core;
using BUtil.Core.Options;
using BUtil.Core.Services;
using BUtil.Core.Storages;
using BUtil.Storages.Ftps;
using BUtil.Storages.Nfs;
using BUtil.Storages.S3;
using BUtil.Storages.Samba;
using BUtil.Storages.Sftp;
using BUtil.Storages.WebDav;
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
        WebDavStoragePlugin.Register();
        S3StoragePlugin.Register();
        StorageProviderRegistry.Register(
            new SambaStorageSettingsProvider(),
            typeof(SambaStorageSettingsV2),
            (log, s, _) => new WindowsSambaStorage(log, (SambaStorageSettingsV2)s));
        StorageProviderRegistry.Register(
            new NfsStorageSettingsProvider(),
            typeof(NfsStorageSettingsV2),
            (log, s, _) => new WindowsNfsStorage(log, (NfsStorageSettingsV2)s));
    }
}
