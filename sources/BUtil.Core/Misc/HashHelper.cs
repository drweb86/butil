using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BUtil.Core.Misc
{
    internal static class HashHelper
    {
        public static string GetSha512(string file)
        {
            using var fileStream = File.OpenRead(file);
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
