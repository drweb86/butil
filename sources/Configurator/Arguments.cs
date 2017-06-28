
using System;

namespace BUtil.Configurator
{
	/// <summary>
	/// Arguments in upper-case that can  be passed to program
	/// </summary>
	internal static class Arguments
	{
		public const string RunBackupMaster = "JUSTBACKUPMASTER";
        public const string RunTask = "Task";
		public const string RunRestorationMaster = "JUSTRESTORATIONMASTER";
		public const string RunJournals = "JUSTJOURNALS";
		public const string RemoveLocalSettings = "REMOVELOCALSETTINGS";
	}
}
