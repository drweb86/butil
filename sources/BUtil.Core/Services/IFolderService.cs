namespace BUtil.Core.Services
{
    public interface IFolderService
    {
        void OpenFolderInShell(string folder);
        void OpenFileInShell(string file);
    }
}
