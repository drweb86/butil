using BUtil.Core.State;

namespace BUtil.Core.Storages
{
    public interface IStorage
    {
        IStorageUploadResult Upload(string sourceFile, string relativeFileName);
        void Delete(string relativeFileName);
        void Download(string relativeFileName, string targetFileName);
        bool Exists(string relativeFileName);
        string Test();
    }
}
