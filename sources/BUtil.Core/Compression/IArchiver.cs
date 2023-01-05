namespace BUtil.Core.Compression
{
    interface IArchiver
    {
        bool IsAvailable();

        bool Extract(
            string archive,
            string password,
            string outputDirectory);

        bool CompressFile(
            string file,
            string password,
            string archive);
    }
}
