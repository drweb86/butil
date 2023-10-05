using System;
using System.IO;

namespace BUtil.Core.FileSystem
{
    public static class Files
    {
		public static readonly string ConsoleBackupTool = 
			Path.Combine(Directories.BinariesDir, "butilc.exe");

        public static readonly string TasksApp =
            Path.Combine(Directories.BinariesDir, "butil.exe");

        public static readonly string TasksAppV2 =
            Path.Combine(Directories.BinariesDir, "butil-ui.Desktop.exe");

        public static readonly string BugReportFile = 
        	Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "BUtil BUG report.txt");
    }
}
