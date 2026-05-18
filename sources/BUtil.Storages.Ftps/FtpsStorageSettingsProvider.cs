using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Storages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace BUtil.Storages.Ftps;

public class FtpsStorageSettingsProvider : IStorageSettingsProvider
{
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
                new("Explicit", Resources.Ftps_Encryption_Option_Explicit),
                new("Implicit", Resources.Ftps_Encryption_Option_Implicit),
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

    public IReadOnlyList<string> SecretSettingsProperties { get; } = ["password"];


    public IStorageSettingsV2 GetSettings(
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

    public IReadOnlyDictionary<string, string?> GetFieldValues(IStorageSettingsV2 settings)
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
            Resources.TrustDetected_Ftps_Title,
            Resources.TrustDetected_Ftps_Instruction,
            string.Empty,
            string.Format(Resources.TrustDetected_Label_Host, s.Host),
            string.Format(Resources.TrustDetected_Label_Port, s.Port == 0 ? Resources.TrustDetected_Port_Default : s.Port.ToString()),
            string.Format(Resources.TrustDetected_Label_Encryption, s.Encryption),
            string.Format(Resources.TrustDetected_Label_Folder, s.Folder),
            string.Format(Resources.TrustDetected_Label_CertificateRawHex, s.TrustedCertificate),
        };

        if (!string.IsNullOrWhiteSpace(s.TrustedCertificate))
        {
            try
            {
                var certBytes = Convert.FromHexString(s.TrustedCertificate);
                using var cert = X509CertificateLoader.LoadCertificate(certBytes);
                lines.Add(string.Empty);
                lines.Add(Resources.TrustDetected_ParsedCertificate_Header);
                lines.Add(string.Format(Resources.TrustDetected_Label_Subject, cert.Subject));
                lines.Add(string.Format(Resources.TrustDetected_Label_Issuer, cert.Issuer));
                lines.Add(string.Format(Resources.TrustDetected_Label_SerialNumber, cert.SerialNumber));
                lines.Add(string.Format(Resources.TrustDetected_Label_Thumbprint, cert.Thumbprint));
                lines.Add(string.Format(Resources.TrustDetected_Label_ValidFromUtc, cert.NotBefore.ToUniversalTime().ToString("O")));
                lines.Add(string.Format(Resources.TrustDetected_Label_ValidUntilUtc, cert.NotAfter.ToUniversalTime().ToString("O")));
                lines.Add(string.Format(Resources.TrustDetected_Label_SignatureAlgorithm, cert.SignatureAlgorithm?.FriendlyName ?? cert.SignatureAlgorithm?.Value));
                lines.Add(string.Format(Resources.TrustDetected_Label_PublicKeyAlgorithm, cert.PublicKey?.Oid?.FriendlyName ?? cert.PublicKey?.Oid?.Value));
                lines.Add(string.Format(Resources.TrustDetected_Label_PublicKeySize, GetPublicKeySizeText(cert)));

                var dnsName = cert.GetNameInfo(X509NameType.DnsName, false);
                if (!string.IsNullOrWhiteSpace(dnsName))
                    lines.Add(string.Format(Resources.TrustDetected_Label_DnsName, dnsName));

                var san = cert.Extensions
                    .OfType<X509Extension>()
                    .FirstOrDefault(x => x.Oid?.Value == "2.5.29.17")
                    ?.Format(true);
                if (!string.IsNullOrWhiteSpace(san))
                    lines.Add(string.Format(Resources.TrustDetected_Label_SubjectAlternativeNames, san));
            }
            catch (Exception ex)
            {
                lines.Add(string.Empty);
                lines.Add(string.Format(Resources.TrustDetected_CertificateParseWarning, ex.Message));
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
        return Resources.TrustDetected_PublicKeySize_Unknown;
    }
}
