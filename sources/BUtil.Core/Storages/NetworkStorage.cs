using System;
using BUtil.Core.Logs;

namespace BUtil.Core.Storages
{
    class SambaStorage: StorageBase<SambaStorageSettings>
	{
        const string _COPYING = "Copying '{0}' to '{1}'";
        const string _EncryptingFormatString = "Encrypting '{0}'";

        long SkipCopyingToNetworkStorageLimit
        {
            get { return Settings.SkipCopyingToNetworkStorageLimitMb * 1024 * 1024; }
        }

        internal SambaStorage(ILog log, SambaStorageSettings settings)
            :base(log, settings)
        {
        }

        public override string ReadAllText(string file)
        {
            throw new NotImplementedException();
        }

  //      public override void Put(string file, string directory = null)
		//{
		//	if (string.IsNullOrEmpty(file))
		//		throw new ArgumentNullException("file");

		//	if (_settings.SkipCopyingToNetworkStorageAndWriteWarningInLogIfBackupExceeds)
  //          {
  //              FileInfo info = new FileInfo(file);
  //              long size = info.Length;

  //              if (size > SkipCopyingToNetworkStorageLimit)
  //              {
  //                  Log.WriteLine(LoggingEvent.Warning,
  //                      string.Format(CultureInfo.CurrentCulture, Resources.CopyingOfBackupTo0NetworkStorageSkippedBecauseItsSize1ExceededSpecifiedLimit2, _settings.Name, size, SkipCopyingToNetworkStorageLimit));

  //                  return;
  //              }
  //          }

  //          var folder = string.IsNullOrEmpty(directory) ? _settings.DestinationFolder : Path.Combine(_settings.DestinationFolder, directory);
  //          string dest = folder + file.Substring(file.LastIndexOf('\\'));
		//	Log.WriteLine(LoggingEvent.Debug, String.Format(CultureInfo.CurrentCulture, _COPYING, file, dest));
  //          File.Copy(file, dest, true);
  //          if (_settings.EncryptUnderLocalSystemAccount)
  //          {
  //              Log.WriteLine(LoggingEvent.Debug, String.Format(CultureInfo.CurrentCulture, _EncryptingFormatString, dest));
  //              File.Encrypt(dest);
  //          }
		//}

        public override string Test()
        {
            return null;
        }

        public override IStorageUploadResult Upload(string sourceFile, string relativeFileName)
        {
            throw new NotImplementedException();
        }
    }
}
