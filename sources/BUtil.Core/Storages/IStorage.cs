using BUtil.Core.State;

namespace BUtil.Core.Storages
{
    public interface IStorage
    {
        IStorageUploadResult Upload(string sourceFile, string relativeFileName);
        string ReadAllText(string file);
        byte[] ReadAllBytes(string file);
        void Delete(string file);
        void Download(StorageFile storageFile, string targetFileName);
        string Test();
    }
}
