using System;
using System.IO;
using System.Linq;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using FluentFTP;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace BUtil.Core.Storages
{
    class FtpsStorage : StorageBase<FtpsStorageSettings>
    {
        internal FtpsStorage(ILog log, FtpsStorageSettings settings)
            : base(log, settings)
        {
            if (string.IsNullOrWhiteSpace(Settings.Host))
                throw new InvalidDataException(BUtil.Core.Localization.Resources.HostIsNotBeSpecified);
            if (Settings.Port < 0) // 0 - default.
                throw new InvalidDataException(BUtil.Core.Localization.Resources.PortIsInvalid);
            if (string.IsNullOrWhiteSpace(Settings.User))
                throw new InvalidDataException(BUtil.Core.Localization.Resources.UserIsNotSpecified);
            if (string.IsNullOrWhiteSpace(Settings.Password))
                throw new InvalidDataException(BUtil.Core.Localization.Resources.PasswordIsNotSpecified);

            Mount();
        }

        private readonly object _uploadLock = new();
        public override IStorageUploadResult Upload(string sourceFile, string relativeFileName)
        {
            //lock (_uploadLock) // because we're limited by upload speed and Samba has limit of 6 parallel uploads usually
            //{
            //    var destinationFile = Path.Combine(Settings.Url, relativeFileName);
            //    var destinationDirectory = Path.GetDirectoryName(destinationFile);

            //    Log.WriteLine(LoggingEvent.Debug, $"Copying \"{sourceFile}\" to \"{destinationFile}\"");

            //    if (!Directory.Exists(destinationDirectory))
            //        Directory.CreateDirectory(destinationDirectory);

            //    Copy(sourceFile, destinationFile);

            //    return new IStorageUploadResult
            //    {
            //        StorageFileName = destinationFile,
            //        StorageFileNameSize = new FileInfo(destinationFile).Length,
            //    };
            //}
            return null;
        }

        public override void DeleteFolder(string relativeFolderName)
        {
            //var fullPathName = string.IsNullOrWhiteSpace(relativeFolderName)
            //    ? Settings.Url
            //    : Path.Combine(Settings.Url, relativeFolderName);

            //if (Directory.Exists(fullPathName))
            //    Directory.Delete(fullPathName, true);
        }

        public override string[] GetFolders(string relativeFolderName, string mask = null)
        {
            //var fullPathName = string.IsNullOrWhiteSpace(relativeFolderName)
            //    ? Settings.Url
            //    : Path.Combine(Settings.Url, relativeFolderName);

            //return Directory
            //    .GetDirectories(fullPathName, mask)
            //    .Select(x => x.Substring(fullPathName.Length))
            //    .Select(x => x.Trim(new[] { '\\', '/' }))
            //    .ToArray();
            return new string[0];
        }

        private FtpClient client;
        private void Mount()
        {
            Log.WriteLine(LoggingEvent.Debug, $"Mount");
            client = new FtpClient(Settings.Host, Settings.User, Settings.Password, Settings.Port);
            client.Config.EncryptionMode = FtpEncryptionMode.Auto;
            client.Config.ValidateAnyCertificate = true;
            client.Config.SslProtocols = System.Security.Authentication.SslProtocols.None;
            client.AutoConnect();
        }

        private void Unmount()
        {
            Log.WriteLine(LoggingEvent.Debug, $"Unmount");
            if (client.IsConnected)
            {
                client.Disconnect();
            }
            client.Dispose();
        }

        public override string Test()
        {
            if (string.IsNullOrWhiteSpace(Settings.Folder))
                client.GetListing();
            else
                client.GetListing(Settings.Folder);

            return null;
        }

        public override bool Exists(string relativeFileName)
        {
            //var fullPathName = Path.Combine(Settings.Url, relativeFileName);

            //return File.Exists(fullPathName);
            return true;
        }

        public override void Delete(string relativeFileName)
        {
            //var fullPathName = Path.Combine(Settings.Url, relativeFileName);

            //if (File.Exists(fullPathName))
            //    File.Delete(fullPathName);
        }

        public override void Download(string relativeFileName, string targetFileName)
        {
            //var file = Path.Combine(Settings.Url, relativeFileName);
            //Copy(file, targetFileName);
        }

        public static void Copy(string inputFile, string outputFilePath)
        {
            //int bufferSize = 16 * 1024 * 1024;

            //using var inputFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, bufferSize);
            //using var outputFileStream = new FileStream(outputFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, bufferSize);
            //outputFileStream.SetLength(inputFileStream.Length);
            //int bytesRead = -1;
            //byte[] bytes = new byte[bufferSize];

            //while ((bytesRead = inputFileStream.Read(bytes, 0, bufferSize)) > 0)
            //{
            //    outputFileStream.Write(bytes, 0, bytesRead);
            //}
        }

        public override void Dispose()
        {
            Unmount();
        }
    }
}
