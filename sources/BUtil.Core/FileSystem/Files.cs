using System;
using System.IO;

namespace BUtil.Core.FileSystem
{
    public static class Files
    {
        public static readonly string WindowsExperience =
            Path.Combine(Directories.BinariesDir, "BUtil.Windows.dll");

        public static readonly string ConsoleBackupTool = 
			Path.Combine(Directories.BinariesDir, "butilc.exe");

        public static readonly string UIApp =
            Path.Combine(Directories.BinariesDir, "butil-ui.Desktop.exe");

        public static readonly string BugReportFile = 
        	Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "BUtil BUG report.txt");
    }
}
