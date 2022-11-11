namespace BUtil.Core.Storages
{
    public interface IStorage
    {
        string Upload(string sourceFile, string relativeFileName);
        string ReadAllText(string file);
    }
}
