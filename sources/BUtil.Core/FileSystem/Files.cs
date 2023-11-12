using System;
using System.IO;

namespace BUtil.Core.FileSystem
{
    public static class Files
    {
        public static readonly string WindowsExperience =
            Path.Combine(Directories.BinariesDir, "BUtil.Windows.dll");
        public static readonly string LinuxExperience =
            Path.Combine(Directories.BinariesDir, "BUtil.Linux.dll");

        public static readonly string BugReportFile = 
        	Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "BUtil BUG report.txt");
    }
}
