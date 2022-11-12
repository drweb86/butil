using BUtil.Core.State;

namespace BUtil.Core.Storages
{
    public interface IStorage
    {
        IStorageUploadResult Upload(string sourceFile, string relativeFileName);
        string ReadAllText(string file);
        string Test();
    }
}
