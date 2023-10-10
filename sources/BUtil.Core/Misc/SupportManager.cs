using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.FileSystem;
using BUtil.Core.Options;
using System;
using System.Diagnostics;

namespace BUtil.Core.Misc
{
	public static class SupportManager
	{
		public static void LaunchTask(string taskName)
		{
            Process.Start(Files.TasksAppV2, $"{TasksAppArguments.LaunchTask} \"{TasksAppArguments.RunTask}={taskName}\"");
        }

		public static void OpenRestorationApp(string? taskName = null)
		{
			if (string.IsNullOrWhiteSpace(taskName))
			{
				Process.Start(Files.TasksApp, TasksAppArguments.Restore);
				return;
			}

            var task = new TaskV2StoreService()
				.Load(taskName);

            if (task?.Model is IncrementalBackupModelOptionsV2)
            {
                Process.Start(Files.TasksApp, $"{TasksAppArguments.Restore} \"{TasksAppArguments.RunTask}={taskName}\"");
            }
            else
            {
                Process.Start(Files.TasksApp, TasksAppArguments.Restore);
            }
        }

        public static void OpenLogs()
		{
            ProcessHelper.ShellExecute(Directories.LogsFolder);
        }

		public static void OpenHomePage()
		{
            SupportManager.DoSupport(SupportRequest.Homepage);
        }

        public static void OpenLatestRelease()
        {
            SupportManager.DoSupport(SupportRequest.LatestRelease);
        }

        static readonly string[] _lINKS_UPDATED = new string[]
		{
			"https://github.com/drweb86/butil",
            "https://github.com/drweb86/butil/releases/latest",
        };

		public static void DoSupport(SupportRequest kind)
		{
			int index = (int)kind;

			if (index < _lINKS_UPDATED.Length)
			{
				string webLink = _lINKS_UPDATED[index];
				ProcessHelper.ShellExecute(webLink);
			}
			else
				throw new NotImplementedException(kind.ToString());
		}
		
		public static string GetLink(SupportRequest kind)
		{
			int index = (int)kind;

			if (index < _lINKS_UPDATED.Length)
				return _lINKS_UPDATED[index];
			else
				throw new NotImplementedException(kind.ToString());
		}
	}
}
