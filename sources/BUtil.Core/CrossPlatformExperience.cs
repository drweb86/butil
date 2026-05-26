using BUtil.Core.Options;
using BUtil.Core.Services;

namespace BUtil.Core;

public abstract class CrossPlatformExperience
{
    public abstract int MinimumListenerPort { get; }
    public abstract ISecretService SecretService { get; }
    public abstract ISessionService SessionService { get; }
    public abstract ISupportManager SupportManager { get; }
    public abstract IFolderService GetFolderService();

    /// <summary>
    /// Registers platform-specific storage plugins (e.g. SMB/CIFS) with StorageProviderRegistry.
    /// Called once at application startup.
    /// </summary>
    public abstract void RegisterPlatformStorages();

    /// <summary>
    /// Registers all built-in task plugins with TaskProviderRegistry.
    /// Called once at application startup.
    /// </summary>
    public abstract void RegisterPlatformTasks();

    public abstract ITaskSchedulerService GetTaskSchedulerService();

    public abstract ITaskShortcutService GetTaskShortcutService();

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
