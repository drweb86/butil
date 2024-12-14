using BUtil.Core.Logs;

namespace BUtil.Core.Services;

public interface ISupportManager
{
    string ScriptEngineName { get; }
    bool LaunchScript(ILog log, string script, string forbiddenForLogs);

    void LaunchTasksApp();

    void OpenHomePage();

    void OpenLatestRelease();
}
