using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Globalization;
using BUtil.Core.PL;
using System.Threading;
using System.Diagnostics;
using BULocalization;

namespace BUtil.Core.Misc
{
	public static class SupportManager
	{
		internal struct Task
		{
			public string File;
			public bool UseShellExecute;
		}
		
		static readonly string[] _LINKS = new string[]
		{
			@"https://sourceforge.net/tracker/?func=add&group_id=195114&atid=952142",
			@"https://sourceforge.net/tracker/?func=add&group_id=195114&atid=952144",
			@"https://sourceforge.net/tracker/?func=add&group_id=195114&atid=952141",
			@"https://sourceforge.net/projects/butil/",
			@"https://sourceforge.net/projects/butil/",
			@"http://www.7-zip.org",
			@"http://www.codeplex.com/zipsolution",
			@"http://www.sourceforge.net/projects/bulocalization",
			@"http://sharpdevelop.net",
			@"http://virtuawin.sourceforge.net/"
		};

		/// <summary>
		/// Performs support with providing information about errors. Does not throw exceptions
		/// </summary>
		/// <param name="kind">kind of support</param>
		public static void DoSupport(SupportRequest kind)
		{
			int index = (int)kind;

			if (index < _LINKS.Length)
			{
				string webLink = _LINKS[index];
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

			if (index < _LINKS.Length)
			{
				return _LINKS[index];
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
			string cannotRunFormatString = Translation.Current[551];

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
				Messages.ShowErrorBox(string.Format(CultureInfo.CurrentCulture, cannotRunFormatString, process.StartInfo.FileName, e.Message));
			}
			catch (ObjectDisposedException e)
			{
				Messages.ShowErrorBox(string.Format(CultureInfo.CurrentCulture, cannotRunFormatString, process.StartInfo.FileName, e.Message));
			}
			catch (InvalidOperationException e)
			{
				Messages.ShowErrorBox(string.Format(CultureInfo.CurrentCulture, cannotRunFormatString, process.StartInfo.FileName, e.Message));
			}
			catch (Exception e)
			{
				Messages.ShowErrorBox(string.Format(CultureInfo.CurrentCulture, cannotRunFormatString, process.StartInfo.FileName, e.Message));
			}
		}
	}
}
