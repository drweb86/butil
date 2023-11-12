using BUtil.Core.Misc;

namespace BUtil.Core.Services
{
    public interface ISupportManager
    {
        void LaunchTasksApp();

        void LaunchTask(string taskName);

        void OpenRestorationApp(string? taskName = null);

        void OpenHomePage();

        void OpenLatestRelease();
    }
}
