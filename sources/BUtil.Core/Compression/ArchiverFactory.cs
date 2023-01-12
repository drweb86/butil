using BUtil.Core.Logs;
using System.IO;

namespace BUtil.Core.Compression
{
    internal static class ArchiverFactory
    {
        public static IArchiver CreateByExtension(ILog log, string file = null)
        {
            var sevenZipFileArchiver = new SevenZipArchiver(log);
            if (sevenZipFileArchiver.IsAvailable())
                return sevenZipFileArchiver;

            return null; // Currently no other archive that does that job.
        }

        public static bool IsSevenZipAvailable(ILog log)
        {
            var sevenZipFileArchiver = new SevenZipArchiver(log);
            return sevenZipFileArchiver.IsAvailable();
        }
    }
}
