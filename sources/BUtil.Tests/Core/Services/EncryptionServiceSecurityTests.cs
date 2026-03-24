using BUtil.Core.Services;
using System.Security.Cryptography;
using System.Text;

namespace BUtil.Tests.Core.Services;

[TestClass]
public class EncryptionServiceSecurityTests
{
    [TestMethod]
    public void EncryptV2_RoundTrip_Works()
    {
        var service = new EncryptionService();
        var plain = Encoding.UTF8.GetBytes("hello-security");
        var password = "strong-password";

        var encrypted = service.EncryptAes256(plain, password);
        var decrypted = service.DecryptAes256(encrypted, password);

        CollectionAssert.AreEqual(plain, decrypted);
    }

    [TestMethod]
    public void Decrypt_LegacyV1Payload_IsSupported()
    {
        var service = new EncryptionService();
        var plain = Encoding.UTF8.GetBytes("legacy-content");
        var password = "legacy-password";

        var encrypted = EncryptLegacyV1(plain, password);
        var decrypted = service.DecryptAes256(encrypted, password);

        CollectionAssert.AreEqual(plain, decrypted);
    }

    [TestMethod]
    public void DecryptV2_TamperedCiphertext_Throws()
    {
        var service = new EncryptionService();
        var plain = Encoding.UTF8.GetBytes("sensitive-data");
        var password = "tamper-test";

        var encrypted = service.EncryptAes256(plain, password);
        // Header is fixed-size 88 bytes (version + iterations + salts + iv).
        encrypted[90] ^= 0x7F;

        _ = ExpectThrows<CryptographicException>(() => _ = service.DecryptAes256(encrypted, password));
    }

    [TestMethod]
    public void DecryptV2_TooLargeIterations_Throws()
    {
        var service = new EncryptionService();
        using var stream = new MemoryStream();
        using (var writer = new BinaryWriter(stream, Encoding.ASCII, true))
        {
            writer.Write(2); // v2
            writer.Write(1_000_001); // unsupported iterations
        }

        stream.Write(new byte[32], 0, 32); // key salt
        stream.Write(new byte[32], 0, 32); // mac salt
        stream.Write(new byte[16], 0, 16); // iv
        stream.Write(new byte[32], 0, 32); // mac
        stream.Position = 0;

        using var output = new MemoryStream();
        _ = ExpectThrows<InvalidOperationException>(() => service.DecryptAes256(stream, output, "pw"));
    }

    private static byte[] EncryptLegacyV1(byte[] plain, string password)
    {
        const int version = 1;
        const int keySize = 256;
        const int createKeyIterations = 1000;
        const string hashAlgorithm = "SHA512";

        using var inputStream = new MemoryStream(plain);
        using var outputStream = new MemoryStream();
        using var aes = Aes.Create();
        aes.KeySize = keySize;

        var keySalt = RandomNumberGenerator.GetBytes(aes.KeySize / 8);
        var iv = RandomNumberGenerator.GetBytes(aes.BlockSize / 8);
        aes.Key = Rfc2898DeriveBytes.Pbkdf2(password, keySalt, createKeyIterations, HashAlgorithmName.SHA512, keySalt.Length);
        aes.IV = iv;

        using (var binaryWriter = new BinaryWriter(outputStream, Encoding.ASCII, true))
        {
            binaryWriter.Write(version);
            binaryWriter.Write(aes.KeySize);
            binaryWriter.Write(aes.BlockSize);
            binaryWriter.Write(createKeyIterations);
            binaryWriter.Write(hashAlgorithm);
        }

        outputStream.Write(keySalt, 0, keySalt.Length);
        outputStream.Write(iv, 0, iv.Length);
        using (var cryptoStream = new CryptoStream(outputStream, aes.CreateEncryptor(), CryptoStreamMode.Write, true))
        {
            inputStream.CopyTo(cryptoStream);
            cryptoStream.FlushFinalBlock();
        }

        return outputStream.ToArray();
    }

    private static TException ExpectThrows<TException>(Action action)
        where TException : Exception
    {
        try
        {
            action();
            Assert.Fail("Exception expected.");
        }
        catch (TException ex)
        {
            return ex;
        }

        throw new InvalidOperationException("Exception expected.");
    }
}
