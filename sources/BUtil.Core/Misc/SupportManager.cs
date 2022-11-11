using System;

namespace BUtil.Core.Misc
{
	public static class SupportManager
	{
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