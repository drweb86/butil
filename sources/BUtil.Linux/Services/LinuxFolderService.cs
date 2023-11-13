using BUtil.Core.FileSystem;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using System.Diagnostics;

namespace BUtil.Linux.Services
{
    internal class LinuxFolderService : IFolderService
    {
        public LinuxFolderService()
        {
            ProcessHelper.Execute("xdg-user-dir", "DOWNLOAD", null, false, ProcessPriorityClass.Normal, out var stdOutput, out var stdError, out var returnCode);
            if (returnCode != 0)
                throw new Exception("Cannot locate Download folder at your system" + stdError);
            stdOutput = stdOutput.TrimEnd('\r', '\n');
            LogsFolder = Path.Combine(stdOutput, "BUtil", "Logs", "v2");
            if (!Directory.Exists(LogsFolder))
                Directory.CreateDirectory(LogsFolder);
        }
        public string LogsFolder { get; }

        public void OpenFolderInShell(string folder)
        {
            Process.Start("xdg-open", $"\"{folder}\"");
        }
        public void OpenFileInShell(string file)
        {
            Process.Start("nautilus", $"--select \"{file}\"");
        }
    }
}
