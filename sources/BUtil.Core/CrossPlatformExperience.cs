using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.Services;
using BUtil.Core.Storages;

namespace BUtil.Core;

public abstract class CrossPlatformExperience
{
    public abstract int MinimumListenerPort { get; }
    public abstract ISecretService SecretService { get; }
    public abstract ISessionService SessionService { get; }
    public abstract ISupportManager SupportManager { get; }
    public abstract IFolderService GetFolderService();

    #region SMB/CIFS
    public abstract bool IsSmbCifsSupported { get; }
    public abstract IStorage GetSmbCifsStorage(ILog log, SambaStorageSettingsV2 settings);
    #endregion

    public abstract ITaskSchedulerService GetTaskSchedulerService();

    public virtual IShowLogOnSystemLoginService? GetShowLogOnSystemLoginService()
    {
        return null;
    }

    /// <summary>
    /// Returns a service that can hide the console window. Non-Windows platforms return null (no-op).
    /// </summary>
    public virtual IConsoleWindowService? GetConsoleWindowService()
    {
        return null;
    }

    public abstract IUiService UiService { get; }

    public abstract IOsSleepPreventionService OsSleepPreventionService { get; }


}
