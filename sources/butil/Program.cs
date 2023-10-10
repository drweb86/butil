using System;
using System.Windows.Forms;
using BUtil.Configurator.Configurator;
using BUtil.Configurator.Configurator.Forms;
using BUtil.Core.Misc;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;

namespace BUtil.Configurator
{
    public static class Program
	{
		#region Private methods

		private static void ProcessArguments(string[] args)
		{
			args ??= Array.Empty<string>();
			if (args.Length == 0)
			{
                Application.Run(new TasksForm());
				return;
			}

			if (args.Length == 1)
			{
				if (string.Compare(args[0], TasksAppArguments.Restore, StringComparison.OrdinalIgnoreCase) == 0)
				{
					TasksController.OpenRestorationMaster(null, true, null);
				}
				else if (args[0].EndsWith(IncrementalBackupModelConstants.StorageIncrementalEncryptedCompressedStateFile))
				{
					TasksController.OpenRestorationMaster(args[0], true, null);
				}
				else
				{
					Messages.ShowErrorBox(Resources.CommandLineArguments_Invalid + string.Format(Resources.CommandLineArguments_Help, SupportManager.GetLink(SupportRequest.Homepage)));
				}
			}
            else if (args.Length > 1 && string.Compare(args[0], TasksAppArguments.Restore, StringComparison.OrdinalIgnoreCase) == 0)
            {
                string taskName = null;
                foreach (var argument in args)
                {
                    if (argument.StartsWith(TasksAppArguments.RunTask) && argument.Length > TasksAppArguments.RunTask.Length)
                    {
                        taskName = argument.Substring(TasksAppArguments.RunTask.Length + 1);
                    }
                }
                TasksController.OpenRestorationMaster(null, true, taskName);
            }
            else if (args.Length > 1)
			{
				Messages.ShowErrorBox(Resources.CommandLineArguments_Invalid + string.Format(Resources.CommandLineArguments_Help, SupportManager.GetLink(SupportRequest.Homepage)));
			}
			else
			{
				Application.Run(new TasksForm());
			}
		}

        #endregion

        [STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.SetCompatibleTextRenderingDefault(false);

			ImproveIt.HandleUiError = message => Messages.ShowErrorBox(message);
            ProcessArguments(args);
		}
	}
}
