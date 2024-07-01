using BUtil.Core.Services;
using System.Diagnostics;

namespace BUtil.Linux.Services;

internal class LinuxFolderService : IFolderService
{
    public string GetDefaultSynchronizationFolder()
    {
        return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    }

    public void OpenFolderInShell(string folder)
    {
        Process.Start("xdg-open", $"\"{folder}\"");
    }
    public void OpenFileInShell(string file)
    {
        Process.Start("nautilus", $"--select \"{file}\"");
    }
}
