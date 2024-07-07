namespace BUtil.Core.Services;

public interface IFolderService
{
    string GetDefaultSynchronizationFolder();
    void OpenFolderInShell(string folder);
    void OpenFileInShell(string file);
    string GetStorageItemExcludePatternHelp();
}
