using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace BUtil.Core.Storages;

public class FtpsStorageSettingsProvider : IStorageSettingsProvider
{
    public string StorageId => "Ftps";
    public string DisplayName => "FTPS";
    public int Order => 3;
    public bool IsSupported => true;

    private static string R(string key, string fallback) =>
        Resources.ResourceManager.GetString(key) ?? fallback;

    private static string F(string key, string fallbackFormat, params object?[] args) =>
        string.Format(R(key, fallbackFormat), args);

    public IReadOnlyList<StorageFieldDescriptor> Fields { get; } =
    [
        new StorageFieldDescriptor
        {
            Key = "host",
            Label = Resources.Server_Field_Address,
            Type = StorageFieldType.Text,
            Placeholder = "192.168.11.1",
        },
        new StorageFieldDescriptor
        {
            Key = "encryption",
            Label = Resources.Ftps_Field_Encryption,
            Type = StorageFieldType.Enum,
            DefaultValue = "Explicit",
            Options =
            [
                ("Explicit", Resources.Ftps_Encryption_Option_Explicit),
                ("Implicit", Resources.Ftps_Encryption_Option_Implicit),
            ],
        },
        new StorageFieldDescriptor
        {
            Key = "port",
            Label = Resources.Server_Field_Port,
            Type = StorageFieldType.Integer,
            Min = 0,
            Max = 65535,
            DefaultValue = 0L,
        },
        new StorageFieldDescriptor
        {
            Key = "user",
            Label = Resources.User_Field,
            Type = StorageFieldType.Text,
        },
        new StorageFieldDescriptor
        {
            Key = "password",
            Label = Resources.Password_Field,
            Type = StorageFieldType.Password,
        },
        new StorageFieldDescriptor
        {
            Key = "folder",
            Label = Resources.Field_Folder,
            Type = StorageFieldType.Text,
            IsOptional = true,
        },
        new StorageFieldDescriptor
        {
            Key = "certificate",
            Label = Resources.TrustedCertificate_Field,
            Type = StorageFieldType.Text,
            IsOptional = true,
        },
    ];

    public bool CanHandle(IStorageSettingsV2 settings) => settings is FtpsStorageSettingsV2;

    public IStorageSettingsV2 CreateSettings(
        IReadOnlyDictionary<string, string?> fieldValues,
        long quota,
        string? mountScript,
        string? unmountScript) =>
        new FtpsStorageSettingsV2
        {
            SingleBackupQuotaGb = quota,
            MountPowershellScript = mountScript,
            UnmountPowershellScript = unmountScript,
            Host = fieldValues.GetValueOrDefault("host") ?? string.Empty,
            Encryption = fieldValues.GetValueOrDefault("encryption") == "Implicit"
                ? FtpsStorageEncryptionV2.Implicit
                : FtpsStorageEncryptionV2.Explicit,
            Port = int.TryParse(fieldValues.GetValueOrDefault("port"), out var port) ? port : 0,
            User = fieldValues.GetValueOrDefault("user") ?? string.Empty,
            Password = fieldValues.GetValueOrDefault("password") ?? string.Empty,
            Folder = fieldValues.GetValueOrDefault("folder"),
            TrustedCertificate = fieldValues.GetValueOrDefault("certificate") ?? string.Empty,
        };

    public IReadOnlyDictionary<string, string?> ExtractValues(IStorageSettingsV2 settings)
    {
        var s = (FtpsStorageSettingsV2)settings;
        return new Dictionary<string, string?>
        {
            ["host"] = s.Host,
            ["encryption"] = s.Encryption == FtpsStorageEncryptionV2.Implicit ? "Implicit" : "Explicit",
            ["port"] = s.Port.ToString(),
            ["user"] = s.User,
            ["password"] = s.Password,
            ["folder"] = s.Folder,
            ["certificate"] = s.TrustedCertificate,
        };
    }

    public string? TryApplyDetectedTrust(
        IStorageSettingsV2 testedSettings,
        IReadOnlyDictionary<string, string?> currentValues,
        out IReadOnlyDictionary<string, string?>? updatedValues)
    {
        updatedValues = null;
        if (testedSettings is not FtpsStorageSettingsV2 ftps) return null;
        if (string.IsNullOrWhiteSpace(ftps.TrustedCertificate)) return null;

        var current = currentValues.GetValueOrDefault("certificate");
        if (ftps.TrustedCertificate.Equals(current, StringComparison.OrdinalIgnoreCase)) return null;

        updatedValues = new Dictionary<string, string?> { ["certificate"] = ftps.TrustedCertificate };
        return BuildCertInfo(ftps);
    }

    private static string BuildCertInfo(FtpsStorageSettingsV2 s)
    {
        var lines = new List<string>
        {
            R("TrustDetected_Ftps_Title", "FTPS server certificate detected during test."),
            R("TrustDetected_Ftps_Instruction", "Verify certificate identity out-of-band, then click Save again to store it."),
            string.Empty,
            F("TrustDetected_Label_Host", "Host: {0}", s.Host),
            F("TrustDetected_Label_Port", "Port: {0}", s.Port == 0 ? R("TrustDetected_Port_Default", "default") : s.Port.ToString()),
            F("TrustDetected_Label_Encryption", "Encryption: {0}", s.Encryption),
            F("TrustDetected_Label_Folder", "Folder: {0}", s.Folder),
            F("TrustDetected_Label_CertificateRawHex", "Certificate (raw hex): {0}", s.TrustedCertificate),
        };

        if (!string.IsNullOrWhiteSpace(s.TrustedCertificate))
        {
            try
            {
                var certBytes = Convert.FromHexString(s.TrustedCertificate);
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
        }

        return string.Join(Environment.NewLine, lines);
    }

    private static string GetPublicKeySizeText(X509Certificate2 cert)
    {
        using var rsa = cert.GetRSAPublicKey();
        if (rsa != null) return $"{rsa.KeySize} bits";
        using var ecdsa = cert.GetECDsaPublicKey();
        if (ecdsa != null) return $"{ecdsa.KeySize} bits";
        using var dsa = cert.GetDSAPublicKey();
        if (dsa != null) return $"{dsa.KeySize} bits";
        using var ecdh = cert.GetECDiffieHellmanPublicKey();
        if (ecdh != null) return $"{ecdh.KeySize} bits";
        return R("TrustDetected_PublicKeySize_Unknown", "unknown");
    }
}
