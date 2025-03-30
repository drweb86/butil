using BUtil.Core.Localization;
using BUtil.Core.Services;
using System.Diagnostics;

namespace BUtil.Linux.Services;

internal class AndroidFolderService : IFolderService
{
    public string GetDefaultSynchronizationFolder()
    {
        return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    }

    public void OpenFolderInShell(string folder)
    {
        // TODO:
    }

    public void OpenFileInShell(string file)
    {
        // TODO:
    }

    public string GetStorageItemExcludePatternHelp()
    {
        return Resources.StorageItem_ExcludePattern_Help
            .Replace("d:\\temp", "/**/.*")
            .Replace("d:", string.Empty)
            .Replace("\\", "/");
    }
}
