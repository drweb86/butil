using BUtil.Core;
using BUtil.Core.Localization;
using BUtil.Core.Services;
using System.Diagnostics;

namespace BUtil.Linux.Services;

internal class AndroidFolderService : IFolderService
{
    public string GetDefaultSynchronizationFolder()
    {
        if (AndroidHack.Instance.RequestManageExternalStoragePermission())
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }
        return null;
    }

    public IEnumerable<string> GetDefaultBackupFolders()
    {
        if (AndroidHack.Instance.RequestManageExternalStoragePermission())
        {
            return [
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),
                Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
                Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
            ];
        }

        return [];
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
