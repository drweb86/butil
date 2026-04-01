using Avalonia.Media.Imaging;
using Avalonia.Platform;
using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.UI;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace BUtil.UI.Controls;

public class WhereTaskViewModel : ObservableObject
{
    public WhereTaskViewModel(IStorageSettingsV2 storageSettings, string title, string iconUrl)
    {
        Title = title;
        IconSource = LoadFromResource(new Uri("avares://BUtil.UI" + iconUrl));
        TransportSource.Add(DirectoryStorage);

        if (PlatformSpecificExperience.Instance.IsSmbCifsSupported)
            TransportSource.Add(Smb);

        TransportSource.Add(Sftp);
        TransportSource.Add(Ftps);

        if (storageSettings is FolderStorageSettingsV2 folderStorageSettings)
        {
            Transport = DirectoryStorage;
         
            Quota = folderStorageSettings.SingleBackupQuotaGb;
            FolderConnectionScript = folderStorageSettings.MountPowershellScript;
            FolderDisconnectionScript = folderStorageSettings.UnmountPowershellScript;

            FolderFolder = folderStorageSettings.DestinationFolder;
        }

        if (storageSettings is SambaStorageSettingsV2 sambaStorageSettingsV2
            && PlatformSpecificExperience.Instance.IsSmbCifsSupported)
        {
            Transport = Smb;
            
            Quota = sambaStorageSettingsV2.SingleBackupQuotaGb;
            FolderConnectionScript = sambaStorageSettingsV2.MountPowershellScript;
            FolderDisconnectionScript = sambaStorageSettingsV2.UnmountPowershellScript;

            SmbUrl = sambaStorageSettingsV2.Url;
            SmbUser = sambaStorageSettingsV2.User;
            SmbPassword = sambaStorageSettingsV2.Password;
        }

        if (storageSettings is FtpsStorageSettingsV2 ftpsStorageSettingsV2)
        {
            Transport = Ftps;
            
            Quota = ftpsStorageSettingsV2.SingleBackupQuotaGb;
            FolderConnectionScript = ftpsStorageSettingsV2.MountPowershellScript;
            FolderDisconnectionScript = ftpsStorageSettingsV2.UnmountPowershellScript;

            FtpsServer = ftpsStorageSettingsV2.Host;
            FtpsEncryption = ftpsStorageSettingsV2.Encryption == FtpsStorageEncryptionV2.Explicit ?
                Ftps_Encryption_Option_Explicit : Ftps_Encryption_Option_Implicit;
            FtpsPort = ftpsStorageSettingsV2.Port;
            FtpsUser = ftpsStorageSettingsV2.User;
            FtpsPassword = ftpsStorageSettingsV2.Password;
            FtpsFolder = ftpsStorageSettingsV2.Folder;
            FtpsTrustedCertificate = ftpsStorageSettingsV2.TrustedCertificate;
        }

        if (storageSettings is SftpStorageSettingsV2 sftpStorageSettingsV2)
        {
            Transport = Sftp;

            Quota = sftpStorageSettingsV2.SingleBackupQuotaGb;
            FolderConnectionScript = sftpStorageSettingsV2.MountPowershellScript;
            FolderDisconnectionScript = sftpStorageSettingsV2.UnmountPowershellScript;

            FtpsServer = sftpStorageSettingsV2.Host;
            FtpsPort = sftpStorageSettingsV2.Port;
            FtpsUser = sftpStorageSettingsV2.User;
            FtpsPassword = sftpStorageSettingsV2.Password;
            SftpKeyFile = sftpStorageSettingsV2.KeyFile;
            SftpFingerPrintSHA256 = sftpStorageSettingsV2.FingerPrintSHA256;
            FtpsFolder = sftpStorageSettingsV2.Folder;
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
                MountPowershellScript = FolderConnectionScript,
                UnmountPowershellScript = FolderDisconnectionScript,

                DestinationFolder = FolderFolder,
            };
        }
        else if (Transport == Ftps)
        {
            return new FtpsStorageSettingsV2
            {
                SingleBackupQuotaGb = Quota,
                MountPowershellScript = FolderConnectionScript,
                UnmountPowershellScript = FolderDisconnectionScript,

                Host = FtpsServer!,
                Encryption = FtpsEncryption == Ftps_Encryption_Option_Explicit ?
                   FtpsStorageEncryptionV2.Explicit : FtpsStorageEncryptionV2.Implicit,
                Port = FtpsPort,
                User = FtpsUser!,
                Password = FtpsPassword!,
                Folder = FtpsFolder,
                TrustedCertificate = FtpsTrustedCertificate ?? string.Empty,
            };
        }
        else if (Transport == Smb)
        {
            return new SambaStorageSettingsV2
            {
                SingleBackupQuotaGb = Quota,
                MountPowershellScript = FolderConnectionScript,
                UnmountPowershellScript = FolderDisconnectionScript,

                Url = SmbUrl!,
                User = SmbUser,
                Password = SmbPassword,
            };
        }
        else if (Transport == Sftp)
        {
            return new SftpStorageSettingsV2
            {
                SingleBackupQuotaGb = Quota,
                MountPowershellScript = FolderConnectionScript,
                UnmountPowershellScript = FolderDisconnectionScript,

                Host = FtpsServer!,
                Port = FtpsPort,
                User = FtpsUser!,
                Password = FtpsPassword,
                KeyFile = SftpKeyFile,
                FingerPrintSHA256 = SftpFingerPrintSHA256!,
                Folder = FtpsFolder!,
            };
        }
        throw new System.ArgumentOutOfRangeException();
    }
    public string Title { get; }
    public Bitmap? IconSource { get; }
    public static string DirectoryStorage => Resources.DirectoryStorage;
    public static string Smb => "SMB/CIFS";
    public static string Ftps => "FTPS";
    public static string Sftp => "SFTP";
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
    public static string DataStorage_Scripts_Header => Resources.DataStorage_Scripts_Header;
    public static string DataStorage_Field_ConnectScript => Resources.DataStorage_Field_ConnectScript;
    public static string DataStorage_Field_DisconnectionScript => Resources.DataStorage_Field_DisconnectionScript;
    public static string Field_Folder => Resources.Field_Folder;

    public static string Url_Field => Resources.Url_Field;
    public static string User_Field => Resources.User_Field;
    public static string Password_Field => Resources.Password_Field;
    public static string Password_Field_Optional => Resources.Password_Field + " " + Resources.OptionalField_Hint;
    public static string Server_Field_Address => Resources.Server_Field_Address;
    public static string Server_Field_Port => Resources.Server_Field_Port;
    public static string Ftps_Field_Encryption => Resources.Ftps_Field_Encryption;
    public static string Field_Device => Resources.Field_Device;
    public static string Field_Folder_Browse => Resources.Field_Folder_Browse;
    public static string Field_File_Browse => Resources.Field_File_Browse;
    public static string Task_Launch => Resources.Task_Launch;
    public static string Field_TransportProtocol => Resources.Field_TransportProtocol;
    public static string KeyFile_Field_Optional => Resources.KeyFile_Field + " " + Resources.OptionalField_Hint;
    public static string FingerPrintSHA256_Field => Resources.FingerPrintSHA256_Field;
    public static string TrustedCertificate_Field => R("TrustedCertificate_Field", "Trusted certificate (hex):");
    public static string OptionalField_Hint => Resources.OptionalField_Hint;
    #endregion

    private static string R(string key, string fallback)
    {
        return Resources.ResourceManager.GetString(key) ?? fallback;
    }

    private static string F(string key, string fallbackFormat, params object?[] args)
    {
        return string.Format(R(key, fallbackFormat), args);
    }

    public string? ApplyDetectedConnectionTrustAndBuildInfo(IStorageSettingsV2 storageSettings)
    {
        if (storageSettings is FtpsStorageSettingsV2 ftpsSettings
            && !string.IsNullOrWhiteSpace(ftpsSettings.TrustedCertificate)
            && !ftpsSettings.TrustedCertificate.Cmp(FtpsTrustedCertificate))
        {
            FtpsTrustedCertificate = ftpsSettings.TrustedCertificate;
            return BuildFtpsTrustDetectedInfo(ftpsSettings);
        }

        if (storageSettings is SftpStorageSettingsV2 sftpSettings
            && !string.IsNullOrWhiteSpace(sftpSettings.FingerPrintSHA256)
            && !sftpSettings.FingerPrintSHA256.Cmp(SftpFingerPrintSHA256))
        {
            SftpFingerPrintSHA256 = sftpSettings.FingerPrintSHA256;
            return BuildSftpTrustDetectedInfo(sftpSettings);
        }

        return null;
    }

    private static string BuildSftpTrustDetectedInfo(SftpStorageSettingsV2 settings)
    {
        return string.Join(Environment.NewLine, new[]
        {
            R("TrustDetected_Sftp_Title", "SFTP server trust information detected during test."),
            R("TrustDetected_Sftp_Instruction", "Verify this fingerprint out-of-band, then click Save again to store it."),
            string.Empty,
            F("TrustDetected_Label_Host", "Host: {0}", settings.Host),
            F("TrustDetected_Label_Port", "Port: {0}", settings.Port == 0 ? R("TrustDetected_Port_Default22", "default (22)") : settings.Port.ToString()),
            F("TrustDetected_Label_User", "User: {0}", settings.User),
            F("TrustDetected_Label_Folder", "Folder: {0}", settings.Folder),
            F("TrustDetected_Label_FingerprintSha256", "Fingerprint (SHA-256): {0}", settings.FingerPrintSHA256),
        });
    }

    private static string BuildFtpsTrustDetectedInfo(FtpsStorageSettingsV2 settings)
    {
        var lines = new List<string>
        {
            R("TrustDetected_Ftps_Title", "FTPS server certificate detected during test."),
            R("TrustDetected_Ftps_Instruction", "Verify certificate identity out-of-band, then click Save again to store it."),
            string.Empty,
            F("TrustDetected_Label_Host", "Host: {0}", settings.Host),
            F("TrustDetected_Label_Port", "Port: {0}", settings.Port == 0 ? R("TrustDetected_Port_Default", "default") : settings.Port.ToString()),
            F("TrustDetected_Label_Encryption", "Encryption: {0}", settings.Encryption),
            F("TrustDetected_Label_Folder", "Folder: {0}", settings.Folder),
            F("TrustDetected_Label_CertificateRawHex", "Certificate (raw hex): {0}", settings.TrustedCertificate),
        };

        if (string.IsNullOrWhiteSpace(settings.TrustedCertificate))
            return string.Join(Environment.NewLine, lines);

        try
        {
            var certBytes = Convert.FromHexString(settings.TrustedCertificate);
            using var cert = X509CertificateLoader.LoadCertificate(certBytes);
            lines.Add(string.Empty);
            lines.Add(R("TrustDetected_ParsedCertificate_Header", "Parsed certificate details:"));
            lines.Add(F("TrustDetected_Label_Subject", "Subject: {0}", cert.Subject));
            lines.Add(F("TrustDetected_Label_Issuer", "Issuer: {0}", cert.Issuer));
            lines.Add(F("TrustDetected_Label_SerialNumber", "Serial number: {0}", cert.SerialNumber));
            lines.Add(F("TrustDetected_Label_Thumbprint", "Thumbprint: {0}", cert.Thumbprint));
            lines.Add(F("TrustDetected_Label_ValidFromUtc", "Valid from (UTC): {0}", cert.NotBefore.ToUniversalTime().ToString("O")));
            lines.Add(F("TrustDetected_Label_ValidUntilUtc", "Valid until (UTC): {0}", cert.NotAfter.ToUniversalTime().ToString("O")));
            lines.Add(F("TrustDetected_Label_SignatureAlgorithm", "Signature algorithm: {0}", cert.SignatureAlgorithm?.FriendlyName ?? cert.SignatureAlgorithm?.Value));
            lines.Add(F("TrustDetected_Label_PublicKeyAlgorithm", "Public key algorithm: {0}", cert.PublicKey?.Oid?.FriendlyName ?? cert.PublicKey?.Oid?.Value));
            lines.Add(F("TrustDetected_Label_PublicKeySize", "Public key size: {0}", GetPublicKeySizeText(cert)));

            var dnsName = cert.GetNameInfo(X509NameType.DnsName, false);
            if (!string.IsNullOrWhiteSpace(dnsName))
                lines.Add(F("TrustDetected_Label_DnsName", "DNS name: {0}", dnsName));

            var san = cert.Extensions
                .OfType<X509Extension>()
                .FirstOrDefault(x => x.Oid?.Value == "2.5.29.17")
                ?.Format(true);
            if (!string.IsNullOrWhiteSpace(san))
                lines.Add(F("TrustDetected_Label_SubjectAlternativeNames", "Subject alternative names: {0}", san));
        }
        catch (Exception ex)
        {
            lines.Add(string.Empty);
            lines.Add(F("TrustDetected_CertificateParseWarning", "Certificate parse warning: {0}", ex.Message));
        }

        return string.Join(Environment.NewLine, lines);
    }

    private static string GetPublicKeySizeText(X509Certificate2 cert)
    {
        using var rsa = cert.GetRSAPublicKey();
        if (rsa != null)
            return $"{rsa.KeySize} bits";

        using var ecdsa = cert.GetECDsaPublicKey();
        if (ecdsa != null)
            return $"{ecdsa.KeySize} bits";

        using var dsa = cert.GetDSAPublicKey();
        if (dsa != null)
            return $"{dsa.KeySize} bits";

        using var ecdh = cert.GetECDiffieHellmanPublicKey();
        if (ecdh != null)
            return $"{ecdh.KeySize} bits";

        return R("TrustDetected_PublicKeySize_Unknown", "unknown");
    }

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
            IsScriptsVisible = CanLaunchScripts;
            IsSmbTransport = value == Smb;
            IsFtpsTransport = value == Ftps;
            IsSftpTransport = value == Sftp;
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

    #region IsScriptsVisible

    private bool _isScriptsVisible = false;

    /// <summary>Scripts section is visible when CanLaunchScripts (for any storage type).</summary>
    public bool IsScriptsVisible
    {
        get
        {
            return _isScriptsVisible;
        }
        set
        {
            if (value == _isScriptsVisible)
                return;
            _isScriptsVisible = value;
            OnPropertyChanged(nameof(IsScriptsVisible));
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

    #region IsSftpTransport

    private bool _isSftpTransport = false;

    public bool IsSftpTransport
    {
        get
        {
            return _isSftpTransport;
        }
        set
        {
            if (value == _isSftpTransport)
                return;
            _isSftpTransport = value;
            OnPropertyChanged(nameof(IsSftpTransport));
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

    #region SftpKeyFile

    private string? _sftpKeyFile;

    public string? SftpKeyFile
    {
        get
        {
            return _sftpKeyFile;
        }
        set
        {
            if (value == _sftpKeyFile)
                return;
            _sftpKeyFile = value;
            OnPropertyChanged(nameof(SftpKeyFile));
        }
    }

    #endregion

    #region SftpFingerPrintSHA256

    private string? _sftpFingerPrintSHA256;

    public string? SftpFingerPrintSHA256
    {
        get
        {
            return _sftpFingerPrintSHA256;
        }
        set
        {
            if (value == _sftpFingerPrintSHA256)
                return;
            _sftpFingerPrintSHA256 = value;
            OnPropertyChanged(nameof(SftpFingerPrintSHA256));
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

    #region FtpsTrustedCertificate

    private string? _ftpsTrustedCertificate;

    public string? FtpsTrustedCertificate
    {
        get
        {
            return _ftpsTrustedCertificate;
        }
        set
        {
            if (value == _ftpsTrustedCertificate)
                return;
            _ftpsTrustedCertificate = value;
            OnPropertyChanged(nameof(FtpsTrustedCertificate));
        }
    }

    #endregion
}
