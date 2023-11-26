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
        private readonly string _workDir;
        private readonly string _uiApp;

        public LinuxSupportManager()
        {
            var isSnap = Directories.BinariesDir.StartsWith("/snap");
            _workDir = isSnap ? null : Directories.BinariesDir;
            _uiApp = isSnap ? "butil.ui" : "butil-ui.Desktop.dll";
        }

        private void LaunchUiAppInternal(bool preventSleep = false, string? arguments = null)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = preventSleep
                    ? "systemd-inhibit"
                    : _uiApp,
                WorkingDirectory = _workDir,
                Arguments = (preventSleep ? $"\"{_uiApp}\"" : "")
                    + (arguments != null ? $" {arguments}" : ""),
            });
        }

        public void LaunchTasksApp()
		{
            LaunchUiAppInternal();
        }

        public void LaunchTask(string taskName)
		{
            LaunchUiAppInternal(true, $"{TasksAppArguments.LaunchTask} \"{TasksAppArguments.RunTask}={taskName}\"");
        }

		public void OpenRestorationApp(string? taskName = null)
		{
			if (string.IsNullOrWhiteSpace(taskName))
			{
                LaunchUiAppInternal(true, TasksAppArguments.Restore);
                return;
			}

            var task = new TaskV2StoreService()
				.Load(taskName);

            if (task?.Model is IncrementalBackupModelOptionsV2)
            {
                LaunchUiAppInternal(true, $"{TasksAppArguments.Restore} \"{TasksAppArguments.RunTask}={taskName}\"");
            }
            else
            {
                LaunchUiAppInternal(true, TasksAppArguments.Restore);
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
