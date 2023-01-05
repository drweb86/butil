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

            if (file == null)
                return null;

            var extension = Path.GetExtension(file)?.ToLowerInvariant();
            if (extension == null || extension == ".7z")
                return null;

            // TODO: create ZipArchive
            return null;
        }
    }
}
