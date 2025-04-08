using System.Collections.Generic;

namespace BUtil.Core.Services;

public interface IFolderService
{
    string GetDefaultSynchronizationFolder();
    IEnumerable<string> GetDefaultBackupFolders();
    void OpenFolderInShell(string folder);
    void OpenFileInShell(string file);
    string GetStorageItemExcludePatternHelp();
}
