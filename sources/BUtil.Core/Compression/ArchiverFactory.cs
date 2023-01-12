using BUtil.Core.Logs;
using System.IO;

namespace BUtil.Core.Compression
{
    internal static class ArchiverFactory
    {
        public static IArchiver Create(ILog log)
        {
            return new SevenZipArchiver(log);
        }
    }
}
