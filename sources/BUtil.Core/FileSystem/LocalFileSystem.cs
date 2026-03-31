using System.IO;

namespace BUtil.Core.FileSystem;

public class LocalFileSystem : ILocalFileSystem
{
    public void EnsureFolderCreated(string folder) => FileHelper.EnsureFolderCreated(folder);
    public bool FileExists(string path) => File.Exists(path);
    public string ReadAllText(string path) => File.ReadAllText(path);
    public void WriteAllText(string path, string content) => File.WriteAllText(path, content);
    public void DeleteFile(string path) => File.Delete(path);
    public string[] GetFiles(string folder, string pattern) => Directory.GetFiles(folder, pattern);
}
