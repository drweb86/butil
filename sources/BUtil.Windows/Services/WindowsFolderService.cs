﻿using BUtil.Core.Localization;
using BUtil.Core.Services;
using System.Diagnostics;

namespace BUtil.Windows.Services;

internal class WindowsFolderService : IFolderService
{
    public IEnumerable<string> GetDefaultBackupFolders()
    {
        return [
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),
            Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
            Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
        ];
    }

    public string GetDefaultSynchronizationFolder()
    {
        return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    }

    public void OpenFolderInShell(string folder)
    {
        Process.Start("explorer.exe", $"\"{folder}\"");
    }
    public void OpenFileInShell(string file)
    {
        Process.Start("explorer.exe", $"/select,\"{file}\"");
    }

    public string GetStorageItemExcludePatternHelp()
    {
        return Resources.StorageItem_ExcludePattern_Help;
    }
}
