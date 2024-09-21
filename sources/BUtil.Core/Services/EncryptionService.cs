using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BUtil.Core.Services;

internal interface IEncryptionService
{
    void EncryptAes256File(string inputFile, string outputFile, string password);
    void DecryptAes256File(string inputFile, string outputFile, string password);
}

internal class EncryptionService: IEncryptionService
{
    public void EncryptAes256File(string inputFile, string outputFile, string password)
    {
        using var fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
        using var fsOutput = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
        EncryptAes256Stream(fsInput, fsOutput, password);
    }

    private void EncryptAes256Stream(Stream inputStream, Stream outputStream, string password)
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
        using var cryptoStream = new CryptoStream(outputStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
        inputStream.CopyTo(cryptoStream);
    }

    private void DecryptAes256Stream(Stream inputStream, Stream outputStream, string password)
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

        using var cryptoStream = new CryptoStream(outputStream, aes.CreateDecryptor(), CryptoStreamMode.Write);
        inputStream.CopyTo(cryptoStream);
    }

    public void DecryptAes256File(string inputFile, string outputFile, string password)
    {
        using var fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
        using var fsOutput = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
        DecryptAes256Stream(fsInput, fsOutput, password);
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
        using var keyGenerator = new Rfc2898DeriveBytes(password, salt, iterations, algorithm);
        return keyGenerator.GetBytes(salt.Length);
    }
}
