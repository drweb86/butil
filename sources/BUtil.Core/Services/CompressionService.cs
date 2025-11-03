using System.IO;
using System.IO.Compression;

namespace BUtil.Core.Services;

public interface ICompressionService
{
    void CompressBrotliFile(string inputFile, string outputFile);
    void DecompressBrotliFile(string inputFile, string outputFile);
}

internal class CompressionService : ICompressionService
{
    public void CompressBrotliFile(string inputFile, string outputFile)
    {
        using var fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
        using var fsOutput = new FileStream(outputFile, FileMode.Create, FileAccess.Write);

        CompressBrotliStream(fsInput, fsOutput);
    }

    private void CompressBrotliStream(Stream inputStream, Stream outputStream)
    {
        using var brotliStream = new BrotliStream(outputStream, CompressionLevel.SmallestSize, true);
        inputStream.CopyTo(brotliStream);
    }

    public void DecompressBrotliFile(string inputFile, string outputFile)
    {
        using var inputStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
        using var brotliStream = new BrotliStream(inputStream, CompressionMode.Decompress);
        using var outputStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
        brotliStream.CopyTo(outputStream);
    }
}
