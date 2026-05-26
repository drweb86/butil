using System;
using System.IO;
using System.Text;

namespace BUtil.Core.FileSystem;

public static class Files
{
    public static readonly string WindowsExperience =
        Path.Combine(Directories.BinariesDir, "BUtil.Windows.dll");

    public static readonly string LinuxExperience =
        Path.Combine(Directories.BinariesDir, "BUtil.Linux.dll");

    public static readonly string BugReportFile =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "BUtil BUG report.txt");

    public static string GetTaskShortcutName(string applicationName, string taskName) =>
        GetDeveloperPrefix() + applicationName + " - " + taskName;

    public static string GetSafeFileName(string name)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var result = new StringBuilder(name.Length);

        foreach (var ch in name)
            result.Append(Array.IndexOf(invalid, ch) >= 0 || char.IsControl(ch) ? '_' : ch);

        return result.ToString().Trim();
    }

    private static string GetDeveloperPrefix()
    {
#if DEBUG
        return "BUtil Developer - ";
#else
        return string.Empty;
#endif
    }
}
