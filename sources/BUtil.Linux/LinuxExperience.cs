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
    #region SMB/CIFS
    public override bool IsSmbCifsSupported { get => true; }
    public override IStorage GetSmbCifsStorage(ILog log, SambaStorageSettingsV2 settings)
    {
        return new LinuxSambaStorage(log, settings);
    }
    #endregion

    public override ISessionService SessionService => new LinuxSessionService();

    #region MTP
    public override bool IsMtpSupported { get => false; }
    public override IMtpService GetMtpService()
    {
        throw new NotSupportedException("Linux does not support MTP protocol.");
    }
    public override IStorage GetMtpStorage(ILog log, MtpStorageSettings storageSettings)
    {
        throw new NotSupportedException("Linux does not support MTP protocol.");
    }
    #endregion

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

    public override IFolderService GetFolderService()
    {
        return new LinuxFolderService();
    }

    public override ISupportManager SupportManager => new LinuxSupportManager();
}