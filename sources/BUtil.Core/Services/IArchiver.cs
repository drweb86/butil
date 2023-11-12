namespace BUtil.Core.Services
{
    public interface IArchiver
    {
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
