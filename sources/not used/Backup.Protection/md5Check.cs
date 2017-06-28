#region Copyright
/*
 * Copyright (c)Cuchuk Sergey Alexandrovich, 2007-2008. All rights reserved
 * Project: BUtil
 * Link: http://www.sourceforge.net/projects/butil
 * License: GNU GPL or SPL with limitations
 * E-mail:
 * Cuchuk.Sergey@gmail.com
 * toCuchukSergey@yandex.ru
 */
#endregion

using System;
using System.IO;
using System.Collections;
using System.Security.Cryptography;
using System.Globalization;
using System.Runtime.Serialization;

[assembly: CLSCompliant(true)]
namespace BUtil.Protection
{
    [Serializable]
    public class InvalidSignException : Exception
    {
        #region Constructors

        public InvalidSignException(string message)
            : base(message)
        {

        }

        public InvalidSignException()
            : base()
        { 
        
        }

        public InvalidSignException(string message, Exception innerException)
            : base(message, innerException)
        { 
        
        }

        protected InvalidSignException(SerializationInfo info, StreamingContext sc)
			: base(info, sc)
        {

        }

        #endregion
    }

    /// <summary>
    /// Description of md5Class.
    /// </summary>
    public static class MD5Class
    {
        private const string _EXES = "*.exe";
        private const string _DLLS = "*.dll";
        private const string _MD5 = ".md5";
        private const string _INVALIDSUMM = "File '{0}' has invalid checksumm '{1}', original is '{2}'. This file is damaged by virus!";
        private const string _MD5FILENOTFOUND = "MD5 summ file '{0}' wasn't found";

        /// <summary>
        /// Signes all files in specified folder
        /// </summary>
        /// <param name="path">Folder with located files here</param>
        /// <exception cref="others">Exceptions are not handled</exception>
        public static void MD5SignAll(string path)
        {
            MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();

            ArrayList files = new ArrayList();
            files.AddRange(Directory.GetFiles(path, _EXES));
            files.AddRange(Directory.GetFiles(path, _DLLS));

            foreach (string file_name in files)
            {
                string md5hash;
                
                using (FileStream fs = new FileStream(@file_name, FileMode.Open, FileAccess.Read))
                {
                    Byte[] hashCode = md5Provider.ComputeHash(fs);
                    md5hash = BitConverter.ToString(hashCode).Replace("-", "");
                }
              
                File.WriteAllText(file_name + _MD5, md5hash);
            }
        }

        /// <summary>
        /// Verifies all files in folder
        /// </summary>
        /// <param name="path">Folder with located files to check</param>
        /// <exception cref="InvalidSignException">Found unisgned binary or sign and binary are not equal</exception>
        /// <exception cref="others">Other exceptions are not handled</exception>
        public static void VerifyAll(string path)
        {
            MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();

            ArrayList files = new ArrayList();
            files.AddRange(Directory.GetFiles(path, _EXES));
            files.AddRange(Directory.GetFiles(path, _DLLS));

            foreach (string file_name in files)
            {
                FileStream fs = null;
                string md5hash, hashcmp;
                string md5File = file_name + _MD5;

                if (!File.Exists(md5File))
                    throw new InvalidSignException(string.Format(CultureInfo.InvariantCulture, _MD5FILENOTFOUND, md5File));

                try
                {
                    fs = new FileStream(@file_name, FileMode.Open, FileAccess.Read);
                    Byte[] hashCode = md5Provider.ComputeHash(fs);

                    md5hash = BitConverter.ToString(hashCode).Replace("-", "");
                    hashcmp = File.ReadAllText(md5File);
                }
                finally
                {
                    if (fs != null) fs.Close();
                }
                if (hashcmp != md5hash)
                    throw new InvalidSignException(string.Format( CultureInfo.InvariantCulture, _INVALIDSUMM, file_name, md5hash, hashcmp));
            }
        }
    }
}
