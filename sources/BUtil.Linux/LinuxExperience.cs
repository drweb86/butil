using BUtil.Core;
using BUtil.Core.Options;
using BUtil.Core.Services;
using BUtil.Core.Storages;
using BUtil.Storages.AzureBlob;
using BUtil.Storages.Ftps;
using BUtil.Storages.Nfs;
using BUtil.Storages.S3;
using BUtil.Storages.Samba;
using BUtil.Storages.Sftp;
using BUtil.Storages.WebDav;
using BUtil.Linux.Services;

namespace BUtil.Linux;

public class LinuxExperience : CrossPlatformExperience
{
    public override ISecretService SecretService => new LinuxSecretService();

    public override int MinimumListenerPort => 1025;
    public override ISessionService SessionService => new LinuxSessionService();

    public override ITaskSchedulerService GetTaskSchedulerService()
    {
        return new LinuxTaskSchedulerService();
    }

    public override IShowLogOnSystemLoginService? GetShowLogOnSystemLoginService()
    {
        return null;
    }

    public override IUiService UiService => new LinuxUiService();

    public override IOsSleepPreventionService OsSleepPreventionService => new LinuxOsSleepPreventionService();

    public override IFolderService GetFolderService()
    {
        return new LinuxFolderService();
    }

    public override ISupportManager SupportManager => new LinuxSupportManager();

    public override void RegisterPlatformStorages()
    {
        SftpStoragePlugin.Register();
        FtpsStoragePlugin.Register();
        WebDavStoragePlugin.Register();
        S3StoragePlugin.Register();
        AzureBlobStoragePlugin.Register();
        StorageProviderRegistry.Register(
            new SambaStorageSettingsProvider(),
            typeof(SambaStorageSettingsV2),
            (log, s, _) => new LinuxSambaStorage(log, (SambaStorageSettingsV2)s));
        StorageProviderRegistry.Register(
            new NfsStorageSettingsProvider(),
            typeof(NfsStorageSettingsV2),
            (log, s, _) => new LinuxNfsStorage(log, (NfsStorageSettingsV2)s));
    }
}
