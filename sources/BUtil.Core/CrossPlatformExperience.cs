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
    public abstract void RegisterPlatformStorages();
    public abstract void RegisterPlatformTasks();

    public abstract ITaskSchedulerService GetTaskSchedulerService();

    public abstract ITaskShortcutService GetTaskShortcutService();

    public virtual IShowLogOnSystemLoginService? GetShowLogOnSystemLoginService() => null;

    public virtual IConsoleWindowService? GetConsoleWindowService() => null;
    public abstract IUiService UiService { get; }

    public abstract IOsSleepPreventionService OsSleepPreventionService { get; }

    public virtual bool IsSandboxed => false;
    public virtual string UnsandboxPath(string path) => path;
}
