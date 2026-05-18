using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Storages;
using System;
using System.Collections.Generic;

namespace BUtil.Storages.Sftp;

public class SftpStorageSettingsProvider : IStorageSettingsProvider
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
            Key = "port",
            Label = Resources.Server_Field_Port,
            Type = StorageFieldType.Integer,
            Min = 0,
            Max = 65535,
            DefaultValue = 22L,
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
            IsOptional = true,
        },
        new StorageFieldDescriptor
        {
            Key = "keyfile",
            Label = Resources.KeyFile_Field,
            Type = StorageFieldType.File,
            IsOptional = true,
        },
        new StorageFieldDescriptor
        {
            Key = "fingerprint",
            Label = Resources.FingerPrintSHA256_Field,
            Type = StorageFieldType.Text,
        },
        new StorageFieldDescriptor
        {
            Key = "folder",
            Label = Resources.Field_Folder,
            Type = StorageFieldType.Text,
            Placeholder = "/home/sftpuser/ftpserver",
        },
    ];

    public IReadOnlyList<string> SecretSettingsProperties { get; } = ["password"];


    public IStorageSettingsV2 GetSettings(
        IReadOnlyDictionary<string, string?> fieldValues,
        long quota,
        string? mountScript,
        string? unmountScript) =>
        new SftpStorageSettingsV2
        {
            SingleBackupQuotaGb = quota,
            MountPowershellScript = mountScript,
            UnmountPowershellScript = unmountScript,
            Host = fieldValues.GetValueOrDefault("host") ?? string.Empty,
            Port = int.TryParse(fieldValues.GetValueOrDefault("port"), out var port) ? port : 22,
            User = fieldValues.GetValueOrDefault("user") ?? string.Empty,
            Password = fieldValues.GetValueOrDefault("password"),
            KeyFile = fieldValues.GetValueOrDefault("keyfile"),
            FingerPrintSHA256 = fieldValues.GetValueOrDefault("fingerprint") ?? string.Empty,
            Folder = fieldValues.GetValueOrDefault("folder") ?? string.Empty,
        };

    public IReadOnlyDictionary<string, string?> GetFieldValues(IStorageSettingsV2 settings)
    {
        var s = (SftpStorageSettingsV2)settings;
        return new Dictionary<string, string?>
        {
            ["host"] = s.Host,
            ["port"] = s.Port.ToString(),
            ["user"] = s.User,
            ["password"] = s.Password,
            ["keyfile"] = s.KeyFile,
            ["fingerprint"] = s.FingerPrintSHA256,
            ["folder"] = s.Folder,
        };
    }

    public string? TryApplyDetectedTrust(
        IStorageSettingsV2 testedSettings,
        IReadOnlyDictionary<string, string?> currentValues,
        out IReadOnlyDictionary<string, string?>? updatedValues)
    {
        updatedValues = null;
        if (testedSettings is not SftpStorageSettingsV2 sftp) return null;
        if (string.IsNullOrWhiteSpace(sftp.FingerPrintSHA256)) return null;

        var current = currentValues.GetValueOrDefault("fingerprint");
        if (sftp.FingerPrintSHA256.Equals(current, StringComparison.OrdinalIgnoreCase)) return null;

        updatedValues = new Dictionary<string, string?> { ["fingerprint"] = sftp.FingerPrintSHA256 };
        return BuildFingerprintInfo(sftp);
    }

    private static string BuildFingerprintInfo(SftpStorageSettingsV2 s) =>
        string.Join(Environment.NewLine,
        [
            Resources.TrustDetected_Sftp_Title,
            Resources.TrustDetected_Sftp_Instruction,
            string.Empty,
            string.Format(Resources.TrustDetected_Label_Host, s.Host),
            string.Format(Resources.TrustDetected_Label_Port, s.Port == 0 ? Resources.TrustDetected_Port_Default22 : s.Port.ToString()),
            string.Format(Resources.TrustDetected_Label_User, s.User),
            string.Format(Resources.TrustDetected_Label_Folder, s.Folder),
            string.Format(Resources.TrustDetected_Label_FingerprintSha256, s.FingerPrintSHA256),
        ]);
}
