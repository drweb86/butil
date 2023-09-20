using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.FileSystem;
using BUtil.Core.Options;
using System;
using System.Diagnostics;

namespace BUtil.Core.Misc
{
	public static class SupportManager
	{
		public static void OpenRestorationApp(string taskName = null)
		{
			if (string.IsNullOrWhiteSpace(taskName))
			{
				Process.Start(Files.TasksApp, TasksAppArguments.Restore);
				return;
			}

            var task = new TaskV2StoreService()
				.Load(taskName);

            if (task.Model is IncrementalBackupModelOptionsV2)
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

		static readonly string[] _LINKS_UPDATED = new string[]
		{
			"https://github.com/drweb86/butil",
			"https://github.com/drweb86/butil/blob/master/help/Backup/Backup%20via%20Wizard/Backup%20Wizard.md",
            "https://github.com/drweb86/butil/blob/master/help/Restore/Restoration%20Wizard.md",
            "https://github.com/drweb86/butil/releases/latest",
            "https://raw.githubusercontent.com/drweb86/butil/master/LastVersion.txt"
        };

		public static void DoSupport(SupportRequest kind)
		{
			int index = (int)kind;

			if (index < _LINKS_UPDATED.Length)
			{
				string webLink = _LINKS_UPDATED[index];
				ProcessHelper.ShellExecute(webLink);
			}
			else
				throw new NotImplementedException(kind.ToString());
		}
		
		public static string GetLink(SupportRequest kind)
		{
			int index = (int)kind;

			if (index < _LINKS_UPDATED.Length)
				return _LINKS_UPDATED[index];
			else
				throw new NotImplementedException(kind.ToString());
		}
	}
}
