using BUtil.Core.Misc;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BUtil.Core.Services;

public interface IEncryptionService
{
    void EncryptAes256File(string inputFile, string outputFile, string password);
    void DecryptAes256File(string inputFile, string outputFile, string password);

    string EncryptTextAes256Base64(string text, string password);
    string DecryptTextAes256Base64(string text, string password);

    void DecryptAes256(Stream inputStream, Stream outputStream, string password);
    void EncryptAes256(Stream inputStream, Stream outputStream, string password);
    byte[] EncryptAes256(byte[] data, string password);
    byte[] DecryptAes256(byte[] data, string password);
}

class EncryptionService: IEncryptionService
{
    public byte[] EncryptAes256(byte[] data, string password)
    {
        using var inputStream = new MemoryStream(data);
        inputStream.Position = 0;
        using var outputStream = new MemoryStream();
        EncryptAes256(inputStream, outputStream, password);
        return outputStream.ToArray();
    }

    public string EncryptTextAes256Base64(string text, string password)
    {
        using var inputStream = StringHelper.StringToMemoryStream(text);
        using var outputStream = new MemoryStream();
        EncryptAes256(inputStream, outputStream, password);
        return StringHelper.MemoryStreamToBase64(outputStream);
    }

    public void EncryptAes256File(string inputFile, string outputFile, string password)
    {
        using var inputStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
        using var outputStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
        EncryptAes256(inputStream, outputStream, password);
    }

    public void EncryptAes256(Stream inputStream, Stream outputStream, string password)
    {
        const int version = 1;
        const int keySize = 256;
        const int createKeyIterations = 1000;
        const string hashAlgorithm = "SHA512";

        using var aes = Aes.Create();
        aes.KeySize = keySize;
        var keySalt = CreateRandomBytes(aes.KeySize / 8);
        var iv = CreateRandomBytes(aes.BlockSize / 8);

        using (var binaryWriter = new BinaryWriter(outputStream, Encoding.ASCII, true))
        {
            aes.Key = CreateKey(password, keySalt, createKeyIterations, hashAlgorithm);
            aes.IV = iv;

            binaryWriter.Write(version);
            binaryWriter.Write(aes.KeySize);
            binaryWriter.Write(aes.BlockSize);
            binaryWriter.Write(createKeyIterations);
            binaryWriter.Write(hashAlgorithm);
        }

        outputStream.Write(keySalt);
        outputStream.Write(iv);
        using var cryptoStream = new CryptoStream(outputStream, aes.CreateEncryptor(), CryptoStreamMode.Write, true);
        inputStream.CopyTo(cryptoStream);
        cryptoStream.Flush();
    }


    public string DecryptTextAes256Base64(string text, string password)
    {
        var inputStream = StringHelper.MemoryStreamFromBase64(text);
        var outputStream = new MemoryStream();
        DecryptAes256(inputStream, outputStream, password);
        return StringHelper.StringFromMemoryStream(outputStream);
    }

    public byte[] DecryptAes256(byte[] data, string password)
    {
        var inputStream = new MemoryStream(data);
        inputStream.Position = 0;
        var outputStream = new MemoryStream();
        DecryptAes256(inputStream, outputStream, password);
        return outputStream.ToArray();
    }

    public void DecryptAes256(Stream inputStream, Stream outputStream, string password)
    {
        using var binaryReader = new BinaryReader(inputStream, Encoding.ASCII, true);
        var version = binaryReader.ReadInt32();

        if (version != 1)
            throw new InvalidOperationException("Unsupported version");

        using var aes = Aes.Create();
        aes.KeySize = binaryReader.ReadInt32();
        aes.BlockSize = binaryReader.ReadInt32();
        int iterations = binaryReader.ReadInt32();
        var hashAlgorithm = binaryReader.ReadString();

        var keySalt = new byte[aes.KeySize / 8];
        inputStream.ReadExactly(keySalt);
        aes.Key = CreateKey(password, keySalt, iterations, hashAlgorithm);

        var iv = new byte[aes.BlockSize / 8];
        inputStream.ReadExactly(iv);
        aes.IV = iv;

        using var cryptoStream = new CryptoStream(outputStream, aes.CreateDecryptor(), CryptoStreamMode.Write, true);
        inputStream.CopyTo(cryptoStream);
        cryptoStream.Flush();
    }

    public void DecryptAes256File(string inputFile, string outputFile, string password)
    {
        using var fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
        using var fsOutput = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
        DecryptAes256(fsInput, fsOutput, password);
    }

    private static byte[] CreateRandomBytes(int bytesLength)
    {
        return RandomNumberGenerator.GetBytes(bytesLength);
    }

    private static byte[] CreateKey(string password, byte[] salt, int iterations, string hashAlgorithm)
    {
        var algorithm = hashAlgorithm switch
        {
            "SHA512" => HashAlgorithmName.SHA512,
            // Compatibility: https://learn.microsoft.com/en-us/dotnet/standard/security/cross-platform-cryptography#sha-3
            _ => throw new ArgumentOutOfRangeException(nameof(hashAlgorithm)),
        };
        return Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, algorithm, salt.Length);
    }
}
