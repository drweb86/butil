using System;
using System.IO;

namespace BUtil.Core.FileSystem;

public static class Files
{
    public static readonly string WindowsExperience =
#if DEBUG
        Path.Combine(Directories.BinariesDir, @"..\..\..\..\BUtil.Windows\bin\windows\Debug\net10.0-windows7.0\BUtil.Windows.dll");
#else
        Path.Combine(Directories.BinariesDir, "BUtil.Windows.dll");
#endif

    public static readonly string LinuxExperience =
        Path.Combine(Directories.BinariesDir, "BUtil.Linux.dll");

    public static readonly string BugReportFile =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "BUtil BUG report.txt");
}
