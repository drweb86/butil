using System.IO;
using System.IO.Compression;

namespace BUtil.Core.Services;

internal interface ICompressionService
{
    void CompressFile(string inputFile, string outputFile);
    void CompressStream(Stream inputStream, Stream outputStream);
    void DecompressFile(string inputFile, string outputFile);
    void DecompressStream(Stream inputStream, Stream outputStream);
}

internal class BrotliCompressionService : ICompressionService
{
    public void CompressFile(string inputFile, string outputFile)
    {
        using var fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
        using var fsOutput = new FileStream(outputFile, FileMode.Create, FileAccess.Write);

        CompressStream(fsInput, fsOutput);
    }

    public void CompressStream(Stream inputStream, Stream outputStream)
    {
        using var brotliStream = new BrotliStream(outputStream, CompressionLevel.SmallestSize, true);
        inputStream.CopyTo(brotliStream);
    }

    public void DecompressStream(Stream inputStream, Stream outputStream)
    {
        using var brotliStream = new BrotliStream(outputStream, CompressionMode.Decompress);
        inputStream.CopyTo(brotliStream);
    }

    public void DecompressFile(string inputFile, string outputFile)
    {
        using var fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
        using var fsOutput = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
        DecompressStream(fsInput, fsOutput);
    }
}
