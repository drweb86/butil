using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.Services;
using BUtil.Core.Storages;
using BUtil.Windows.Services;

namespace BUtil.Windows;

public class WindowsExperience : CrossPlatformExperience
{
    #region SMB/CIFS
    public override bool IsSmbCifsSupported { get => true; }
    public override IStorage GetSmbCifsStorage(ILog log, SambaStorageSettingsV2 settings)
    {
        return new WindowsSambaStorage(log, settings);
    }
    #endregion

    public override int MinimumListenerPort => 1;

    public override ISessionService SessionService => new WindowsSessionService();

    public override ISupportManager SupportManager => new WindowsSupportManager();

    public override IFolderService GetFolderService()
    {
        return new WindowsFolderService();
    }

    public override ITaskSchedulerService? GetTaskSchedulerService()
    {
        return new TaskSchedulerService();
    }

    public override IShowLogOnSystemLoginService? GetShowLogOnSystemLoginService()
    {
        return new ShowLogOnSystemLoginService();
    }

    public override IUiService UiService => new WindowsUiService();

    public override IOsSleepPreventionService OsSleepPreventionService => new WindowsOsSleepPreventionService();
}