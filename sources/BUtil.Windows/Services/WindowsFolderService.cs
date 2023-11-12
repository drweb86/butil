using BUtil.Core.FileSystem;
using BUtil.Core.Services;
using System.Diagnostics;
using System.IO;

namespace BUtil.Windows.Services
{
    internal class WindowsFolderService: IFolderService
    {
        public WindowsFolderService()
        {
            LogsFolder = Path.Combine(Directories.UserDataFolder, "Logs", "v2");
            if (!Directory.Exists(LogsFolder))
                Directory.CreateDirectory(LogsFolder);
        }
        public string LogsFolder { get; }

        public void OpenFolderInShell(string folder)
        {
            Process.Start("explorer.exe", $"\"{folder}\"");
        }
        public void OpenFileInShell(string file)
        {
            Process.Start("explorer.exe", $"/select,\"{file}\"");
        }
    }
}
