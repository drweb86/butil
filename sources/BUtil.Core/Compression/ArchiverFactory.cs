using BUtil.Core.Logs;

namespace BUtil.Core.Compression
{
    static class ArchiverFactory
    {
        public static IArchiver Create(ILog log)
        {
            return new SevenZipArchiver(log);
        }
    }
}
