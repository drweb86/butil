namespace BUtil.Core.FileSystem;

public interface ILocalFileSystem
{
    void EnsureFolderCreated(string folder);
    bool FileExists(string path);
    string ReadAllText(string path);
    void WriteAllText(string path, string content);
    void DeleteFile(string path);
    string[] GetFiles(string folder, string pattern);
}
