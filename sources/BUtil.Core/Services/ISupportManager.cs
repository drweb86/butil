using BUtil.Core.Logs;
using BUtil.Core.Misc;

namespace BUtil.Core.Services
{
    public interface ISupportManager
    {
        string ScriptEngineName {  get; }
        bool LaunchScript(ILog log, string script, string forbiddenForLogs);

        void LaunchTasksApp();

        void LaunchTask(string taskName);

        void OpenRestorationApp(string? taskName = null);

        void OpenHomePage();

        void OpenLatestRelease();
    }
}
