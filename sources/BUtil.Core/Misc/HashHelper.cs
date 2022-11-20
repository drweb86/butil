using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BUtil.Core.Misc
{
    internal static class HashHelper
    {
        public static string GetSha512(string file)
        {
            using var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 16 * 1024 * 1024);
            using var sha512Hash = SHA512.Create();

            var hash = sha512Hash.ComputeHash(fileStream);
            return HashToString(hash);
        }

        private static string HashToString(byte[] hash)
        {
            var sBuilder = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sBuilder.Append(hash[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
