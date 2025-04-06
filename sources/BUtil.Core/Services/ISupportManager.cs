using BUtil.Core.Logs;

namespace BUtil.Core.Services;

public interface ISupportManager
{
    string ScriptEngineName { get; }
    bool CanLaunchScripts { get; }
    bool LaunchScript(ILog log, string script, string forbiddenForLogs);

    void LaunchTasksAppOrExit();

    void OpenHomePage();

    void OpenLatestRelease();
    void OpenIcons();
    bool CanOpenLink { get; }
}
