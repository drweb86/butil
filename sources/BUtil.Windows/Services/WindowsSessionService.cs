using BUtil.Core.Misc;
using BUtil.Core.Services;
using System.Diagnostics;

namespace BUtil.Windows.Services;

public class WindowsSessionService : ISessionService
{
    public void DoTask(PowerTask task)
    {
        switch (task)
        {
            case PowerTask.LogOff:
                LogOff();
                break;
            case PowerTask.Shutdown:
                Shutdown();
                break;
            case PowerTask.Reboot:
                Restart();
                break;
            case PowerTask.None:
                break;
            default:
                throw new NotImplementedException(task.ToString());
        }
    }

    #region Power operations

    private static void Restart()
    {
        Process.Start("shutdown", "-r -t 0");
    }

    private static void Shutdown()
    {
        Process.Start("shutdown", "-s -t 0");
    }

    private static void LogOff()
    {
        Process.Start("shutdown", "-I -t 0");
    }

    #endregion
}
