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
        //Intent intent = context.getPackageManager().getLaunchIntentForPackage("com.sec.android.app.myfiles");
        //Uri uri = Uri.parse(rootPath);
        //if (intent != null)
        //{
        //    intent.setData(uri);
        //    startActivity(intent);
        //}

        //Process.Start("xdg-open", $"\"{folder}\"");
    }
    public void OpenFileInShell(string file)
    {
        //Process.Start("nautilus", $"--select \"{file}\"");
    }

    public string GetStorageItemExcludePatternHelp()
    {
        return Resources.StorageItem_ExcludePattern_Help
            .Replace("d:\\temp", "/**/.*")
            .Replace("d:", string.Empty)
            .Replace("\\", "/");
    }
}
