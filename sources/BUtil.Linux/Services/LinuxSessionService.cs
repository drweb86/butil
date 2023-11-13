using BUtil.Core.Misc;
using BUtil.Core.Services;
using System.Diagnostics;

namespace BUtil.Linux.Services
{
	public class LinuxSessionService: ISessionService
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
            Process.Start("systemctl", "reboot");
        }

		private static void Shutdown()
		{
            Process.Start("systemctl", "poweroff");
		}

		private static void LogOff()
		{
            Process.Start("gnome-session-quit", "--logout --no-prompt");
		}

		#endregion
	}
}
