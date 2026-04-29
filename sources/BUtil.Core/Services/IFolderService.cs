using System.Collections.Generic;

namespace BUtil.Core.Services;

public interface IFolderService
{
    string GetDefaultSynchronizationFolder();
    IEnumerable<string> GetDefaultBackupFolders();
    string GetDefaultMediaImportFolder();
    void OpenFolderInShell(string folder);
    void OpenFileInShell(string file);
    string GetStorageItemExcludePatternHelp();
}
