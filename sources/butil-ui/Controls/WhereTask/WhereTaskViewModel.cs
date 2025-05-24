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

        if (PlatformSpecificExperience.Instance.IsSmbCifsSupported)
            TransportSource.Add(Smb);

        TransportSource.Add(Ftps);

        if (storageSettings is FolderStorageSettingsV2 folderStorageSettings)
        {
            Transport = DirectoryStorage;
            Quota = folderStorageSettings.SingleBackupQuotaGb;
            FolderFolder = folderStorageSettings.DestinationFolder;
            FolderConnectionScript = folderStorageSettings.MountPowershellScript;
            FolderDisconnectionScript = folderStorageSettings.UnmountPowershellScript;
        }

        if (storageSettings is SambaStorageSettingsV2 sambaStorageSettingsV2
            && PlatformSpecificExperience.Instance.IsSmbCifsSupported)
        {
            Transport = Smb;
            Quota = sambaStorageSettingsV2.SingleBackupQuotaGb;
            SmbUrl = sambaStorageSettingsV2.Url;
            SmbUser = sambaStorageSettingsV2.User;
            SmbPassword = sambaStorageSettingsV2.Password;
        }

        if (storageSettings is FtpsStorageSettingsV2 ftpsStorageSettingsV2)
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

        FtpsEncryptionSource.Add(Ftps_Encryption_Option_Implicit);
        FtpsEncryptionSource.Add(Ftps_Encryption_Option_Explicit);
    }

    public async Task MountTaskLaunchCommand()
    {
        if (string.IsNullOrWhiteSpace(FolderConnectionScript))
            return;

        if (PlatformSpecificExperience.Instance.SupportManager.CanLaunchScripts)
        {
            var memoryLog = new MemoryLog();
            if (PlatformSpecificExperience.Instance.SupportManager.LaunchScript(memoryLog, this.FolderConnectionScript, "***"))
                await Messages.ShowInformationBox(Resources.DataStorage_Field_DisconnectionScript_Ok);
            else
                await Messages.ShowErrorBox(Resources.DataStorage_Field_DisconnectionScript_Bad + Environment.NewLine + Environment.NewLine + memoryLog);
        }
    }

    public async Task UnmountTaskLaunchCommand()
    {
        if (string.IsNullOrWhiteSpace(FolderDisconnectionScript))
            return;

        if (PlatformSpecificExperience.Instance.SupportManager.CanLaunchScripts)
        {
            var memoryLog = new MemoryLog();
            if (PlatformSpecificExperience.Instance.SupportManager.LaunchScript(memoryLog, FolderDisconnectionScript, "***"))
                await Messages.ShowInformationBox(Resources.DataStorage_Field_DisconnectionScript_Ok);
            else
                await Messages.ShowErrorBox(Resources.DataStorage_Field_DisconnectionScript_Bad + Environment.NewLine + Environment.NewLine + memoryLog);
        }
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

                Host = FtpsServer!,
                Encryption = FtpsEncryption == Ftps_Encryption_Option_Explicit ?
                   FtpsStorageEncryptionV2.Explicit : FtpsStorageEncryptionV2.Implicit,
                Port = FtpsPort,
                User = FtpsUser!,
                Password = FtpsPassword!,
                Folder = FtpsFolder,
            };
        }
        else if (Transport == Smb)
        {
            return new SambaStorageSettingsV2
            {
                SingleBackupQuotaGb = Quota,
                Url = SmbUrl!,
                User = SmbUser,
                Password = SmbPassword,
            };
        }
        throw new System.ArgumentOutOfRangeException();
    }
    public string Title { get; }
    public Bitmap? IconSource { get; }
    public static string DirectoryStorage => Resources.DirectoryStorage;
    public static string Smb => "SMB/CIFS";
    public static string Ftps => "FTPS";
    public bool CanLaunchScripts { get; } = PlatformSpecificExperience.Instance.SupportManager.CanLaunchScripts;

    public List<string> TransportSource { get; } = [];


    public string Ftps_Encryption_Option_Explicit = Resources.Ftps_Encryption_Option_Explicit;
    public string Ftps_Encryption_Option_Implicit = Resources.Ftps_Encryption_Option_Implicit;
    public List<string> FtpsEncryptionSource { get; } = [];

    #region Labels
    public static string LeftMenu_Where => Resources.LeftMenu_Where;
    public static string DataStorage_Field_UploadQuota => Resources.DataStorage_Field_UploadQuota;
    public static string DataStorage_Field_UploadQuota_Help => Resources.DataStorage_Field_UploadQuota_Help;
    public static string DataStorage_Script_Help => string.Format(Resources.DataStorage_Script_Help, PlatformSpecificExperience.Instance.SupportManager.ScriptEngineName);
    public static string DataStorage_Field_ConnectScript => Resources.DataStorage_Field_ConnectScript;
    public static string DataStorage_Field_DisconnectionScript => Resources.DataStorage_Field_DisconnectionScript;
    public static string Field_Folder => Resources.Field_Folder;

    public static string Url_Field => Resources.Url_Field;
    public static string User_Field => Resources.User_Field;
    public static string Password_Field => Resources.Password_Field;
    public static string Server_Field_Address => Resources.Server_Field_Address;
    public static string Server_Field_Port => Resources.Server_Field_Port;
    public static string Ftps_Field_Encryption => Resources.Ftps_Field_Encryption;
    public static string Field_Device => Resources.Field_Device;
    public static string Field_Folder_Browse => Resources.Field_Folder_Browse;
    public static string Task_Launch => Resources.Task_Launch;
    public static string Field_TransportProtocol => Resources.Field_TransportProtocol;
    #endregion

    #region Transport

    private string? _transport;

    public string? Transport
    {
        get
        {
            return _transport;
        }
        set
        {
            if (value == _transport)
                return;
            _transport = value;
            OnPropertyChanged(nameof(Transport));
            IsDirectoryTransport = value == DirectoryStorage;
            IsDirectoryScriptsVisible = IsDirectoryTransport && CanLaunchScripts;
            IsSmbTransport = value == Smb;
            IsFtpsTransport = value == Ftps;
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

    #region IsDirectoryScriptsVisible

    private bool _isDirectoryScriptsVisible = false;

    public bool IsDirectoryScriptsVisible
    {
        get
        {
            return _isDirectoryScriptsVisible;
        }
        set
        {
            if (value == _isDirectoryScriptsVisible)
                return;
            _isDirectoryScriptsVisible = value;
            OnPropertyChanged(nameof(IsDirectoryScriptsVisible));
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

    private string? _folderConnectionScript;

    public string? FolderConnectionScript
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

    private string? _folderDisconnectionScript;

    public string? FolderDisconnectionScript
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

    private string? _smbUrl;

    public string? SmbUrl
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

    private string? _ftpsServer;

    public string? FtpsServer
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

    private string _ftpsEncryption = Resources.Ftps_Encryption_Option_Explicit; // Most popular option

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

    private string? _ftpsUser;

    public string? FtpsUser
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

    private string? _ftpsPassword;

    public string? FtpsPassword
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
}
