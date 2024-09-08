using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BUtil.Core.Services;

internal static class AES256EncryptionService
{
    static void EncryptFile(string inputFile, string outputFile, string password, string iv)
    {
        using (Aes aes = Aes.Create())
        {
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            byte[] key = DeriveKeyFromPassword(password, aes.KeySize);
            aes.Key = key;
            aes.IV = Encoding.UTF8.GetBytes(iv.PadRight(16, '0').Substring(0, 16));

            using (FileStream inputFileStream = new FileStream(inputFile, FileMode.Open))
            using (FileStream outputFileStream = new FileStream(outputFile, FileMode.Create))
            using (ICryptoTransform encryptor = aes.CreateEncryptor())
            using (CryptoStream cryptoStream = new CryptoStream(outputFileStream, encryptor, CryptoStreamMode.Write))
            {
                inputFileStream.CopyTo(cryptoStream);
            }
        }
    }

    static void DecryptFile(string inputFile, string outputFile, string password, string iv)
    {
        using (Aes aes = Aes.Create())
        {
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            byte[] key = DeriveKeyFromPassword(password, aes.KeySize);
            aes.Key = key;
            aes.IV = Encoding.UTF8.GetBytes(iv.PadRight(16, '0').Substring(0, 16));

            using (FileStream inputFileStream = new FileStream(inputFile, FileMode.Open))
            using (FileStream outputFileStream = new FileStream(outputFile, FileMode.Create))
            using (ICryptoTransform decryptor = aes.CreateDecryptor())
            using (CryptoStream cryptoStream = new CryptoStream(inputFileStream, decryptor, CryptoStreamMode.Read))
            {
                cryptoStream.CopyTo(outputFileStream);
            }
        }
    }

    static byte[] DeriveKeyFromPassword(string password, int keySizeInBits)
    {
        const int iterations = 10000;
        using (var deriveBytes = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes("saltysalt"), iterations, HashAlgorithmName.SHA256))
        {
            return deriveBytes.GetBytes(keySizeInBits / 8);
        }
    }
}
