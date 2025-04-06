using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.Services;
using BUtil.Core.Storages;
using BUtil.Linux.Services;
using BUtil.Windows.Services;

namespace BUtil.Windows;

public class AndroidExperience : CrossPlatformExperience
{
    public override bool IsSmbCifsSupported { get => false; }
    public override IStorage GetSmbCifsStorage(ILog log, SambaStorageSettingsV2 settings)
    {
        throw new NotImplementedException("For this platform SMB/CIFS service is not implemented.");
    }

    public override ISessionService SessionService => new AndroidSessionService();

    #region MTP
    public override bool IsMtpSupported { get => false; }
    public override IMtpService GetMtpService()
    {
        throw new NotSupportedException("Android does not support MTP protocol.");
    }

    public override IStorage GetMtpStorage(ILog log, MtpStorageSettings storageSettings)
    {
        throw new NotSupportedException("Android does not support MTP protocol.");
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

    public override IUiService UiService => new AndroidLinuxUiService();

    public override IOsSleepPreventionService OsSleepPreventionService => new AndroidOsSleepPreventionService();

    public override IFolderService GetFolderService()
    {
        return new AndroidFolderService();
    }
    
    public override ISupportManager SupportManager => new AndroidSupportManager();
}