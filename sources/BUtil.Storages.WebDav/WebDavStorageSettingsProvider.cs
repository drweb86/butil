using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Storages;
using System.Collections.Generic;

namespace BUtil.Storages.WebDav;

public class WebDavStorageSettingsProvider : IStorageSettingsProvider
{
    public string StorageId => "WebDav";
    public string DisplayName => "WebDAV";
    public int Order => 5;
    public bool IsSupported => true;

    public IReadOnlyList<StorageFieldDescriptor> Fields { get; } =
    [
        new StorageFieldDescriptor
        {
            Key = "host",
            Label = Resources.Server_Field_Address,
            Type = StorageFieldType.Text,
            Placeholder = "nextcloud.example.com",
        },
        new StorageFieldDescriptor
        {
            Key = "useHttps",
            Label = "HTTPS",
            Type = StorageFieldType.Enum,
            DefaultValue = "Yes",
            Options = [("Yes", "Yes"), ("No", "No")],
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
            Key = "basePath",
            Label = "Base Path",
            Type = StorageFieldType.Text,
            Placeholder = "/remote.php/dav/files/user/Backups",
            IsOptional = true,
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
    ];

    public bool CanHandle(IStorageSettingsV2 settings) => settings is WebDavStorageSettingsV2;

    public IStorageSettingsV2 CreateSettings(
        IReadOnlyDictionary<string, string?> fieldValues,
        long quota,
        string? mountScript,
        string? unmountScript) =>
        new WebDavStorageSettingsV2
        {
            SingleBackupQuotaGb = quota,
            MountPowershellScript = mountScript,
            UnmountPowershellScript = unmountScript,
            Host = fieldValues.GetValueOrDefault("host") ?? string.Empty,
            UseHttps = fieldValues.GetValueOrDefault("useHttps") != "No",
            Port = int.TryParse(fieldValues.GetValueOrDefault("port"), out var port) ? port : 0,
            BasePath = fieldValues.GetValueOrDefault("basePath") ?? string.Empty,
            User = fieldValues.GetValueOrDefault("user") ?? string.Empty,
            Password = fieldValues.GetValueOrDefault("password") ?? string.Empty,
        };

    public IReadOnlyDictionary<string, string?> ExtractValues(IStorageSettingsV2 settings)
    {
        var s = (WebDavStorageSettingsV2)settings;
        return new Dictionary<string, string?>
        {
            ["host"] = s.Host,
            ["useHttps"] = s.UseHttps ? "Yes" : "No",
            ["port"] = s.Port.ToString(),
            ["basePath"] = s.BasePath,
            ["user"] = s.User,
            ["password"] = s.Password,
        };
    }

    public string? TryApplyDetectedTrust(
        IStorageSettingsV2 testedSettings,
        IReadOnlyDictionary<string, string?> currentValues,
        out IReadOnlyDictionary<string, string?>? updatedValues)
    {
        updatedValues = null;
        return null;
    }
}
