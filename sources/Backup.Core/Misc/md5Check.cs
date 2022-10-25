using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using BUtil.Core.FileSystem;

namespace BUtil.Core.Misc
{
    /// <summary>
    /// Description of md5Class.
    /// </summary>
    public static class MD5Class
    {
        private const string _Exes = "*.exe";
        private const string _Dlls = "*.dll";
        private const string _Md5 = ".md5";

        /// <summary>
        /// Signes all files in specified folder
        /// </summary>
        /// <param name="path">Folder with located files here</param>
        /// <exception cref="others">Exceptions are not handled</exception>
        public static void MD5SignAll(string path)
        {
            ArrayList files = new ArrayList();
            files.AddRange(Directory.GetFiles(path, _Exes));
            files.AddRange(Directory.GetFiles(path, _Dlls));

            foreach (string file_name in files)
            {
				File.WriteAllText(file_name + _Md5, GetFileMD5(file_name));
            }
        }


		public static string GetFileMD5(string file)
		{
			if (string.IsNullOrEmpty(file))
				throw new ArgumentNullException("file");

			string md5hash;

            using (var md5 = MD5.Create())
            {
                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    var hashCode = md5.ComputeHash(fs);
                    md5hash = BitConverter.ToString(hashCode).Replace("-", "");
                }
            }

			return md5hash;
		}
    }
}
