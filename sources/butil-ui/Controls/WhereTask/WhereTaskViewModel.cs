using Avalonia.Media.Imaging;
using Avalonia.Platform;
using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace butil_ui.Controls;

public class WhereTaskViewModel : ObservableObject
{
    public WhereTaskViewModel(IStorageSettingsV2 storageSettings, string title, string iconUrl)
    {
        Title = title;
        IconSource = LoadFromResource(new Uri("avares://butil-ui" + iconUrl));
        TransportSource.Add(DirectoryStorage);
        TransportSource.Add(Smb);
        TransportSource.Add(Ftps);

        var mtp = PlatformSpecificExperience.Instance.GetMtpService();
        if (mtp != null)
            TransportSource.Add(Mtp);

        var folderStorageSettings = storageSettings as FolderStorageSettingsV2;
        if (folderStorageSettings != null)
        {
            Transport = DirectoryStorage;
            Quota = folderStorageSettings.SingleBackupQuotaGb;
            FolderFolder = folderStorageSettings.DestinationFolder;
            FolderConnectionScript = folderStorageSettings.MountPowershellScript;
            FolderDisconnectionScript = folderStorageSettings.UnmountPowershellScript;
        }

        var sambaStorageSettingsV2 = storageSettings as SambaStorageSettingsV2;
        if (sambaStorageSettingsV2 != null)
        {
            Transport = Smb;
            Quota = sambaStorageSettingsV2.SingleBackupQuotaGb;
            SmbUrl = sambaStorageSettingsV2.Url;
            SmbUser = sambaStorageSettingsV2.User;
            SmbPassword = sambaStorageSettingsV2.Password;
        }

        var ftpsStorageSettingsV2 = storageSettings as FtpsStorageSettingsV2;
        if (ftpsStorageSettingsV2 != null)
        {
            Transport = Ftps;
            Quota = ftpsStorageSettingsV2.SingleBackupQuotaGb;
            FtpsServer = ftpsStorageSettingsV2.Host;
            FtpsEncryption = ftpsStorageSettingsV2.Encryption == FtpsStorageEncryptionV2.Explicit ?
                Ftps_Encryption_Option_Explicit : Ftps_Encryption_Option_Implicit;
            FtpsPort = ftpsStorageSettingsV2.Port;
            FtpsUser = ftpsStorageSettingsV2.User;
            FtpsPassword = ftpsStorageSettingsV2.Password;
            FtpsFolder = ftpsStorageSettingsV2.Folder;
        }

        var mtpStorageSettings = storageSettings as MtpStorageSettings;
        if (mtpStorageSettings != null && mtp != null)
        {
            var mtpService = PlatformSpecificExperience.Instance.GetMtpService();
            if (mtpService != null)
            {
                MtpDevicesSource = mtpService.GetItems();
            }

            Transport = Mtp;
            Quota = mtpStorageSettings.SingleBackupQuotaGb;
            MtpDevice = mtpStorageSettings.Device;
            MtpFolder = mtpStorageSettings.Folder;
        }

        FtpsEncryptionSource.Add(Ftps_Encryption_Option_Implicit);
        FtpsEncryptionSource.Add(Ftps_Encryption_Option_Explicit);
    }

    public void UpdateListMtpDevices()
    {
        var mtpService = PlatformSpecificExperience.Instance.GetMtpService();
        if (mtpService != null)
        {
            MtpDevicesSource = mtpService.GetItems();
        }
    }

    public async Task MountTaskLaunchCommand()
    {
        var memoryLog = new MemoryLog();
        if (PlatformSpecificExperience.Instance.SupportManager.LaunchScript(memoryLog, this.FolderConnectionScript, "***"))
            await Messages.ShowInformationBox(Resources.DataStorage_Field_DisconnectionScript_Ok);
        else
            await Messages.ShowErrorBox(Resources.DataStorage_Field_DisconnectionScript_Bad + Environment.NewLine + Environment.NewLine + memoryLog);
    }

    public async Task UnmountTaskLaunchCommand()
    {
        var memoryLog = new MemoryLog();
        if (PlatformSpecificExperience.Instance.SupportManager.LaunchScript(memoryLog, this.FolderDisconnectionScript, "***"))
            await Messages.ShowInformationBox(Resources.DataStorage_Field_DisconnectionScript_Ok);
        else
            await Messages.ShowErrorBox(Resources.DataStorage_Field_DisconnectionScript_Bad + Environment.NewLine + Environment.NewLine + memoryLog);
    }

    private static Bitmap LoadFromResource(Uri resourceUri)
    {
        return new Bitmap(AssetLoader.Open(resourceUri));
    }

    public IStorageSettingsV2 GetStorageSettings()
    {
        if (Transport == DirectoryStorage)
        {
            return new FolderStorageSettingsV2
            {
                SingleBackupQuotaGb = Quota,
                DestinationFolder = FolderFolder,
                MountPowershellScript = FolderConnectionScript,
                UnmountPowershellScript = FolderDisconnectionScript,
            };
        }
        else if (Transport == Ftps)
        {
            return new FtpsStorageSettingsV2
            {
                SingleBackupQuotaGb = Quota,

                Host = FtpsServer,
                Encryption = FtpsEncryption == Ftps_Encryption_Option_Explicit ?
                   FtpsStorageEncryptionV2.Explicit : FtpsStorageEncryptionV2.Implicit,
                Port = FtpsPort,
                User = FtpsUser,
                Password = FtpsPassword,
                Folder = FtpsFolder,
            };
        }
        else if (Transport == Smb)
        {
            return new SambaStorageSettingsV2
            {
                SingleBackupQuotaGb = Quota,
                Url = SmbUrl,
                User = SmbUser,
                Password = SmbPassword,
            };
        }
        else if (Transport == Mtp)
        {
            return new MtpStorageSettings
            {
                SingleBackupQuotaGb = Quota,
                Device = MtpDevice,
                Folder = MtpFolder
            };
        }
        throw new System.ArgumentOutOfRangeException();
    }
    public string Title { get; }
    public Bitmap? IconSource { get; }
    public string DirectoryStorage => Resources.DirectoryStorage;
    public string Smb => "SMB/CIFS";
    public string Ftps => "FTPS";
    public string Mtp => "MTP";

    public List<string> TransportSource { get; } = new List<string>();


    public string Ftps_Encryption_Option_Explicit = Resources.Ftps_Encryption_Option_Explicit;
    public string Ftps_Encryption_Option_Implicit = Resources.Ftps_Encryption_Option_Implicit;
    public List<string> FtpsEncryptionSource { get; } = new List<string>();

    #region Labels
    public string LeftMenu_Where => Resources.LeftMenu_Where;
    public string DataStorage_Field_UploadQuota => Resources.DataStorage_Field_UploadQuota;
    public string DataStorage_Field_UploadQuota_Help => Resources.DataStorage_Field_UploadQuota_Help;
    public string DataStorage_Script_Help => string.Format(Resources.DataStorage_Script_Help, PlatformSpecificExperience.Instance.SupportManager.ScriptEngineName);
    public string DataStorage_Field_ConnectScript => Resources.DataStorage_Field_ConnectScript;
    public string DataStorage_Field_DisconnectionScript => Resources.DataStorage_Field_DisconnectionScript;
    public string Field_Folder => Resources.Field_Folder;

    public string Url_Field => Resources.Url_Field;
    public string User_Field => Resources.User_Field;
    public string Password_Field => Resources.Password_Field;
    public string Server_Field_Address => Resources.Server_Field_Address;
    public string Server_Field_Port => Resources.Server_Field_Port;
    public string Ftps_Field_Encryption => Resources.Ftps_Field_Encryption;
    public string Field_Device => Resources.Field_Device;
    public string Field_Folder_Browse => Resources.Field_Folder_Browse;
    public string Task_Launch => Resources.Task_Launch;
    public string Field_TransportProtocol => Resources.Field_TransportProtocol;
    #endregion

    #region Transport

    private string _Transport;

    public string Transport
    {
        get
        {
            return _Transport;
        }
        set
        {
            if (value == _Transport)
                return;
            _Transport = value;
            OnPropertyChanged(nameof(Transport));
            IsDirectoryTransport = value == this.DirectoryStorage;
            IsMtpTransport = value == this.Mtp;
            IsSmbTransport = value == this.Smb;
            IsFtpsTransport = value == this.Ftps;
        }
    }

    #endregion

    #region IsDirectoryTransport

    private bool _isDirectoryTransport = false;

    public bool IsDirectoryTransport
    {
        get
        {
            return _isDirectoryTransport;
        }
        set
        {
            if (value == _isDirectoryTransport)
                return;
            _isDirectoryTransport = value;
            OnPropertyChanged(nameof(IsDirectoryTransport));
        }
    }

    #endregion

    #region IsFtpsTransport

    private bool _isFtpsTransport = false;

    public bool IsFtpsTransport
    {
        get
        {
            return _isFtpsTransport;
        }
        set
        {
            if (value == _isFtpsTransport)
                return;
            _isFtpsTransport = value;
            OnPropertyChanged(nameof(IsFtpsTransport));
        }
    }

    #endregion

    #region IsSmbTransport

    private bool _isSmbTransport = false;

    public bool IsSmbTransport
    {
        get
        {
            return _isSmbTransport;
        }
        set
        {
            if (value == _isSmbTransport)
                return;
            _isSmbTransport = value;
            OnPropertyChanged(nameof(IsSmbTransport));
        }
    }

    #endregion

    #region IsMtpTransport

    private bool _isMtpTransport = false;

    public bool IsMtpTransport
    {
        get
        {
            return _isMtpTransport;
        }
        set
        {
            if (value == _isMtpTransport)
                return;
            _isMtpTransport = value;
            OnPropertyChanged(nameof(IsMtpTransport));
        }
    }

    #endregion

    #region Quota

    private long _quota = 0;

    public long Quota
    {
        get
        {
            return _quota;
        }
        set
        {
            if (value == _quota)
                return;
            _quota = value;
            OnPropertyChanged(nameof(Quota));
        }
    }

    #endregion

    #region FolderFolder

    private string _folderFolder = string.Empty;

    public string FolderFolder
    {
        get
        {
            return _folderFolder;
        }
        set
        {
            if (value == _folderFolder)
                return;
            _folderFolder = value;
            OnPropertyChanged(nameof(FolderFolder));
        }
    }

    #endregion

    #region FolderConnectionScript

    private string _folderConnectionScript;

    public string FolderConnectionScript
    {
        get
        {
            return _folderConnectionScript;
        }
        set
        {
            if (value == _folderConnectionScript)
                return;
            _folderConnectionScript = value;
            OnPropertyChanged(nameof(FolderConnectionScript));
        }
    }

    #endregion

    #region FolderDisconnectionScript

    private string _folderDisconnectionScript;

    public string FolderDisconnectionScript
    {
        get
        {
            return _folderDisconnectionScript;
        }
        set
        {
            if (value == _folderDisconnectionScript)
                return;
            _folderDisconnectionScript = value;
            OnPropertyChanged(nameof(FolderDisconnectionScript));
        }
    }

    #endregion

    #region SmbUrl

    private string _smbUrl;

    public string SmbUrl
    {
        get
        {
            return _smbUrl;
        }
        set
        {
            if (value == _smbUrl)
                return;
            _smbUrl = value;
            OnPropertyChanged(nameof(SmbUrl));
        }
    }

    #endregion

    #region SmbUser

    private string? _smbUser;

    public string? SmbUser
    {
        get
        {
            return _smbUser;
        }
        set
        {
            if (value == _smbUser)
                return;
            _smbUser = value;
            OnPropertyChanged(nameof(SmbUser));
        }
    }

    #endregion

    #region SmbPassword

    private string? _smbPassword;

    public string? SmbPassword
    {
        get
        {
            return _smbPassword;
        }
        set
        {
            if (value == _smbPassword)
                return;
            _smbPassword = value;
            OnPropertyChanged(nameof(SmbPassword));
        }
    }

    #endregion

    #region FtpsServer

    private string _ftpsServer;

    public string FtpsServer
    {
        get
        {
            return _ftpsServer;
        }
        set
        {
            if (value == _ftpsServer)
                return;
            _ftpsServer = value;
            OnPropertyChanged(nameof(FtpsServer));
        }
    }

    #endregion

    #region FtpsEncryption

    private string _ftpsEncryption;

    public string FtpsEncryption
    {
        get
        {
            return _ftpsEncryption;
        }
        set
        {
            if (value == _ftpsEncryption)
                return;
            _ftpsEncryption = value;
            OnPropertyChanged(nameof(FtpsEncryption));
        }
    }

    #endregion

    #region FtpsPort

    private int _ftpsPort;

    public int FtpsPort
    {
        get
        {
            return _ftpsPort;
        }
        set
        {
            if (value == _ftpsPort)
                return;
            _ftpsPort = value;
            OnPropertyChanged(nameof(FtpsPort));
        }
    }

    #endregion

    #region FtpsUser

    private string _ftpsUser;

    public string FtpsUser
    {
        get
        {
            return _ftpsUser;
        }
        set
        {
            if (value == _ftpsUser)
                return;
            _ftpsUser = value;
            OnPropertyChanged(nameof(FtpsUser));
        }
    }

    #endregion

    #region FtpsPassword

    private string _ftpsPassword;

    public string FtpsPassword
    {
        get
        {
            return _ftpsPassword;
        }
        set
        {
            if (value == _ftpsPassword)
                return;
            _ftpsPassword = value;
            OnPropertyChanged(nameof(FtpsPassword));
        }
    }

    #endregion

    #region FtpsFolder

    private string? _ftpsFolder;

    public string? FtpsFolder
    {
        get
        {
            return _ftpsFolder;
        }
        set
        {
            if (value == _ftpsFolder)
                return;
            _ftpsFolder = value;
            OnPropertyChanged(nameof(FtpsFolder));
        }
    }

    #endregion

    #region MtpDevice

    private string _mtpDevice;

    public string MtpDevice
    {
        get
        {
            return _mtpDevice;
        }
        set
        {
            if (value == _mtpDevice)
                return;
            _mtpDevice = value;
            OnPropertyChanged(nameof(MtpDevice));
        }
    }

    #endregion

    #region MtpFolder

    private string _mtpFolder;

    public string MtpFolder
    {
        get
        {
            return _mtpFolder;
        }
        set
        {
            if (value == _mtpFolder)
                return;
            _mtpFolder = value;
            OnPropertyChanged(nameof(MtpFolder));
        }
    }

    #endregion

    #region MtpDevicesSource

    private IEnumerable<string> _mtpDevicesSource;

    public IEnumerable<string> MtpDevicesSource
    {
        get
        {
            return _mtpDevicesSource;
        }
        set
        {
            if (value == _mtpDevicesSource)
                return;
            _mtpDevicesSource = value;
            OnPropertyChanged(nameof(MtpDevicesSource));
        }
    }

    #endregion

}
