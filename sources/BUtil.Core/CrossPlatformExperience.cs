using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.Services;
using BUtil.Core.Storages;

namespace BUtil.Core;

public abstract class CrossPlatformExperience
{
    public abstract ISessionService SessionService { get; }
    public abstract ISupportManager SupportManager { get; }
    public abstract IFolderService GetFolderService();

    #region SMB/CIFS
    public abstract bool IsSmbCifsSupported { get; }
    public abstract IStorage GetSmbCifsStorage(ILog log, SambaStorageSettingsV2 settings);
    #endregion

    #region MTP
    public abstract bool IsMtpSupported { get; }
    public abstract IMtpService GetMtpService();
    public abstract IStorage GetMtpStorage(ILog log, MtpStorageSettings storageSettings);
    #endregion

    public virtual ITaskSchedulerService? GetTaskSchedulerService()
    {
        return null;
    }

    public virtual IShowLogOnSystemLoginService? GetShowLogOnSystemLoginService()
    {
        return null;
    }

    public abstract IUiService UiService { get; }

    public abstract IOsSleepPreventionService OsSleepPreventionService { get; }


}
