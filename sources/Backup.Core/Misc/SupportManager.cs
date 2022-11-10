using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Globalization;
using BUtil.Core.PL;
using System.Threading;
using System.Diagnostics;
using BUtil.Core.Localization;

namespace BUtil.Core.Misc
{
	public static class SupportManager
	{
		internal struct Task
		{
			public string File;
			public bool UseShellExecute;
		}
		
		static readonly string[] _LINKS_UPDATED = new string[]
		{
			"https://github.com/drweb86/butil",
			"https://github.com/drweb86/butil/issues",
			"https://github.com/drweb86/butil/blob/master/help/TOC.md",
			"http://www.7-zip.org",
			"http://virtuawin.sourceforge.net/",
			"https://github.com/drweb86/butil/blob/master/help/Backup/Backup%20via%20Wizard/Backup%20Wizard.md",
            "https://github.com/drweb86/butil/blob/master/help/Manage%20Logs/Manage%20Logs.md",
            "https://github.com/drweb86/butil/blob/master/help/Restore/Restoration%20Wizard.md",
            "https://github.com/drweb86/butil/releases/latest",
            "https://raw.githubusercontent.com/drweb86/butil/master/CheckForUpdate.xml"
        };

		/// <summary>
		/// Performs support with providing information about errors. Does not throw exceptions
		/// </summary>
		/// <param name="kind">kind of support</param>
		public static void DoSupport(SupportRequest kind)
		{
			int index = (int)kind;

			if (index < _LINKS_UPDATED.Length)
			{
				string webLink = _LINKS_UPDATED[index];
				OpenWebLink(webLink);
			}
			else
				throw new NotImplementedException(kind.ToString());
		}

		public static void OpenWebLink(string webLink)
		{
			StartProcess(webLink, true);
		}
		
		public static void OpenWebLinkAsync(string webLink)
		{
			StartProcess(webLink, true, true);
		}
		
		public static string GetLink(SupportRequest kind)
		{
			int index = (int)kind;

			if (index < _LINKS_UPDATED.Length)
			{
				return _LINKS_UPDATED[index];
			}
			else
				throw new NotImplementedException(kind.ToString());
		}
		
		public static void StartProcess(string programName, bool useShellExecute)
		{
			StartProcess(programName, useShellExecute, false);
		}
		
		public static void StartProcess(string programName, bool useShellExecute, bool async)
		{
			if (string.IsNullOrEmpty(programName))
				throw new ArgumentNullException("programName");

			Task task = new Task();
			task.File = programName;
			task.UseShellExecute = useShellExecute;

			if (async)
			{
				thread_process(task);
			}
            else
            {
            	Thread thread = new Thread(thread_process);
				thread.Start(task);
            }
		}
		
		static void thread_process(object parameters)
        {
			string cannotRunFormatString = Resources.CoultNotRun0DueToN1;

			Task task = (Task) parameters;
			Process process = new Process();
			process.StartInfo.FileName = task.File;
			process.StartInfo.UseShellExecute = task.UseShellExecute;
			
			try
			{
				process.Start();
			}
			catch (System.ComponentModel.Win32Exception e)
			{
				Messages.ShowErrorBox(string.Format(CultureInfo.CurrentUICulture, cannotRunFormatString, process.StartInfo.FileName, e.Message));
			}
			catch (ObjectDisposedException e)
			{
				Messages.ShowErrorBox(string.Format(CultureInfo.CurrentUICulture, cannotRunFormatString, process.StartInfo.FileName, e.Message));
			}
			catch (InvalidOperationException e)
			{
				Messages.ShowErrorBox(string.Format(CultureInfo.CurrentUICulture, cannotRunFormatString, process.StartInfo.FileName, e.Message));
			}
			catch (Exception e)
			{
				Messages.ShowErrorBox(string.Format(CultureInfo.CurrentUICulture, cannotRunFormatString, process.StartInfo.FileName, e.Message));
			}
		}
	}
}
