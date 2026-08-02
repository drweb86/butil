using BUtil.Interop.Logs;

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
    void OpenLink(string url);
    bool CanOpenLink { get; }
    bool SupportsSmileIcons { get; }
}
