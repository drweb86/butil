using BUtil.Core.Logs;
using BUtil.Core.Misc;

namespace BUtil.Core.Services
{
    public interface ISupportManager
    {
        bool LaunchPowershell(ILog log, string script);

        void LaunchTasksApp();

        void LaunchTask(string taskName);

        void OpenRestorationApp(string? taskName = null);

        void OpenHomePage();

        void OpenLatestRelease();
    }
}
