using BUtil.Core.Logs;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using System;
using ICSharpCode.SharpZipLib.Core;

namespace BUtil.Core.Compression
{
    public class SharpZipLibArchiver : IArchiver
    {
        private readonly ILog _log;

        public SharpZipLibArchiver(ILog log)
        {
            _log = log;
        }

        public bool CompressFile(string file, string password, string archive)
        {
            var compressionLevel = CompressionUtil.GetCompressionLevel(Path.GetExtension(file).ToLowerInvariant());
            var passwordIsSet = !string.IsNullOrWhiteSpace(password);
            if (passwordIsSet)
                _log.WriteLine(LoggingEvent.Debug, $"Compressing \"{file}\" to \"{archive}\"");
            else
                _log.WriteLine(LoggingEvent.Debug, $"Compressing \"{file}\" to \"{archive}\" with password");

            try
            {
                using (var archiveStream = File.Create(archive))
                using (var archiveZipOutputStream = new ZipOutputStream(archiveStream))
                {
                    archiveZipOutputStream.SetLevel(compressionLevel);

                    if (passwordIsSet)
                        archiveZipOutputStream.Password = password;

                    var zipEntry = new ZipEntry(file);
                    zipEntry.IsUnicodeText = true;
                    zipEntry.ForceZip64();

                    if (!string.IsNullOrEmpty(password))
                        zipEntry.AESKeySize = 256;

                    archiveZipOutputStream.PutNextEntry(zipEntry);

                    var buffer = new byte[4096 * 1024];
                    using (var readFileStream = File.OpenRead(file))
                        StreamUtils.Copy(readFileStream, archiveZipOutputStream, buffer);

                    archiveZipOutputStream.CloseEntry();
                }
                _log.WriteLine(LoggingEvent.Debug, "Compression successfull.");
                return true;
            }
            catch (Exception e)
            {
                _log.WriteLine(LoggingEvent.Error, e.ToString());
                _log.WriteLine(LoggingEvent.Error, "Compression failed.");
                return false;
            }
        }

        public bool Extract(string archive, string password, string outputDirectory)
        {
            var passwordIsSet = !string.IsNullOrWhiteSpace(password);
            if (!passwordIsSet)
                _log.WriteLine(LoggingEvent.Debug, $"Extracting \"{archive}\" to \"{outputDirectory}\"");
            else
                _log.WriteLine(LoggingEvent.Debug, $"Extracting \"{archive}\" to \"{outputDirectory}\" with password");

            try
            {
                using (var archiveStream = File.OpenRead(archive))
                using (var zipInputStream = new ZipInputStream(archiveStream))
                {
                    var buffer = new byte[4096 * 1024];

                    if (passwordIsSet)
                        zipInputStream.Password = password;

                    var zipEntry = zipInputStream.GetNextEntry();
                    while (zipEntry != null)
                    {
                        var outputFile = Path.Combine(outputDirectory, zipEntry.Name);
                        var outputDirectoryName = Path.GetDirectoryName(outputFile);
                        if (!string.IsNullOrEmpty(outputDirectoryName))
                            Directory.CreateDirectory(outputDirectoryName);

                        if (zipEntry.IsFile)
                            using (var outputFileStream = File.Create(outputFile))
                                StreamUtils.Copy(zipInputStream, outputFileStream, buffer);

                        zipEntry = zipInputStream.GetNextEntry();
                    }
                }
                _log.WriteLine(LoggingEvent.Debug, "Extract successfull.");
                return true;
            }
            catch (Exception e)
            {
                _log.WriteLine(LoggingEvent.Error, e.ToString());
                _log.WriteLine(LoggingEvent.Error, "Extract failed.");
                return false;
            }
        }

        public bool IsAvailable() => true;
    }
}
