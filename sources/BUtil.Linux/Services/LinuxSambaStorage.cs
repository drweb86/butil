using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Storages;

namespace BUtil.Linux.Services
{
    class LinuxSambaStorage : StorageBase<SambaStorageSettingsV2>
    {
        private IStorage _proxy;

        internal LinuxSambaStorage(ILog log, SambaStorageSettingsV2 settings)
            : base(log, settings)
        {
            if (string.IsNullOrWhiteSpace(Settings.Url))
            {
                throw new InvalidDataException(BUtil.Core.Localization.Resources.Url_Field_Validation);
            }

            ProcessHelper.Execute("id", "-u", null, false, System.Diagnostics.ProcessPriorityClass.Normal,
                out var stdOut, out var stdError, out var returnCode);
            if (returnCode != 0)
                throw new InvalidDataException("Cannot get user ID\n" + stdOut + "\n" + stdError);

            var userId = stdOut.Trim('\r', '\n');

            var uri = new Uri(Settings.Url);
            var host = uri.Host;
            var path = uri.AbsolutePath;
            var share = path.Split('/', '\\', StringSplitOptions.RemoveEmptyEntries)[0];
            var folderAtShare = string.Join(
                "/",
                path.Split('/', '\\', StringSplitOptions.RemoveEmptyEntries)
                    .Skip(1)
                    .ToArray());
            var splittedUser = Settings.User.Split('/', '\\');
            var userDomain = splittedUser.Length == 1 ? string.Empty : splittedUser[0];
            var userWithoutDomain = splittedUser.Length == 2 ? splittedUser[1] : Settings.User;

            var destinationFolder = string.IsNullOrEmpty(folderAtShare)
                ? $"/run/user/{userId}/gvfs/smb-share:server={host},share={share}"
                : $"/run/user/{userId}/gvfs/smb-share:server={host},share={share}/{folderAtShare}";

            _proxy = new FolderStorage(log, new FolderStorageSettingsV2
            {
                DestinationFolder = destinationFolder,
                SingleBackupQuotaGb = Settings.SingleBackupQuotaGb,
                MountPowershellScript = @$"
gio mount -u ""smb://{host}/{share}"" --force
killall gvfsd
echo ""{userWithoutDomain}
{userDomain}
{Settings.Password}
{userWithoutDomain}
{userDomain}
{Settings.Password}
{userWithoutDomain}
{userDomain}
{Settings.Password}"" | gio mount ""smb://{host}/{share}""
",
                UnmountPowershellScript = $"gio mount -u \"smb://{host}/{share}\" --force",
            });
        }

        public override string? Test()
        {
            return _proxy.Test();
        }

        public override IStorageUploadResult Upload(string sourceFile, string relativeFileName)
        {
            return _proxy.Upload(sourceFile, relativeFileName);
        }

        public override void DeleteFolder(string relativeFolderName)
        {
            _proxy.DeleteFolder(relativeFolderName);
        }

        public override string[] GetFolders(string relativeFolderName, string? mask = null)
        {
            return _proxy.GetFolders(relativeFolderName, mask);
        }

        public override bool Exists(string relativeFileName)
        {
            return _proxy.Exists(relativeFileName);
        }

        public override void Delete(string relativeFileName)
        {
            _proxy.Delete(relativeFileName);
        }

        public override void Download(string relativeFileName, string targetFileName)
        {
            _proxy.Download(relativeFileName, targetFileName);
        }
        
        public override string[] GetFiles(string? relativeFolderName = null, SearchOption option = SearchOption.TopDirectoryOnly)
        {
            return _proxy.GetFiles(relativeFolderName, option);
        }

        public override DateTime GetModifiedTime(string relativeFileName)
        {
            return _proxy.GetModifiedTime(relativeFileName);
        }

        public override void Dispose()
        {
            _proxy.Dispose();
        }
    }
}
