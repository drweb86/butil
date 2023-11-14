using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.FileSystem;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Options;
using BUtil.Core.Services;
using System.Diagnostics;

namespace BUtil.Linux.Services
{
	public class LinuxSupportManager: ISupportManager
    {
        public static readonly string UIApp =
            Path.Combine(Directories.BinariesDir, "butil-ui.Desktop.dll");
        public static readonly string ConsoleBackupTool =
            Path.Combine(Directories.BinariesDir, "butilc.dll");

        public void LaunchTasksApp()
		{
			Process.Start("dotnet", $"\"{UIApp}\"");
        }

        public void LaunchTask(string taskName)
		{
            Process.Start("systemd-inhibit", $"dotnet \"{UIApp}\" {TasksAppArguments.LaunchTask} \"{TasksAppArguments.RunTask}={taskName}\"");
        }

		public void OpenRestorationApp(string? taskName = null)
		{
			if (string.IsNullOrWhiteSpace(taskName))
			{
				Process.Start("systemd-inhibit", $"dotnet \"{UIApp}\" {TasksAppArguments.Restore}");
                return;
			}

            var task = new TaskV2StoreService()
				.Load(taskName);

            if (task?.Model is IncrementalBackupModelOptionsV2)
            {
                Process.Start("systemd-inhibit", $"dotnet \"{UIApp}\" {TasksAppArguments.Restore} \"{TasksAppArguments.RunTask}={taskName}\"");
            }
            else
            {
                Process.Start("systemd-inhibit", $"dotnet \"{UIApp}\" {TasksAppArguments.Restore}");
            }
        }

        public void OpenHomePage()
        {
            ProcessHelper.ShellExecute(ApplicationLinks.HomePage);
        }

        public void OpenLatestRelease()
        {
            ProcessHelper.ShellExecute(ApplicationLinks.LatestRelease);
        }

        public bool LaunchPowershell(ILog log, string script)
        {
            using var tempDir = new TempFolder();
            var scriptFile = Path.Combine(tempDir.Folder, "script.ps1");
            File.WriteAllText(scriptFile, script);

            log.WriteLine(LoggingEvent.Debug, $"Executing powershell script");

            ProcessHelper.Execute("pwsh",
                $"-File \"{scriptFile}\" -NoLogo -NonInteractive",
                null,
                false,
                ProcessPriorityClass.Idle,

                out var stdOutput,
                out var stdError,
                out var returnCode);

            var isSuccess = returnCode == 0;
            if (!string.IsNullOrWhiteSpace(stdOutput))
                log.LogProcessOutput(stdOutput, isSuccess);
            if (!string.IsNullOrWhiteSpace(stdError))
                log.LogProcessOutput(stdError, isSuccess);
            if (isSuccess)
                log.WriteLine(LoggingEvent.Debug, "Executing successfull.");
            if (!isSuccess)
                log.WriteLine(LoggingEvent.Error, "Executing failed.");
            return isSuccess;
        }
    }
}
