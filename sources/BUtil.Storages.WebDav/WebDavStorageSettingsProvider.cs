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
            Key = "preset",
            Label = Resources.Storage_Field_Preset,
            Type = StorageFieldType.Enum,
            DefaultValue = "Custom",
            Options =
            [
                new("Custom", "Generic / Custom",
                    "Generic WebDAV server (Nextcloud, ownCloud, Synology DSM, Seafile, Apache, etc.)\n\n" +
                    "1. Enter the server Address, choose HTTPS (recommended), and set the port (0 = default: 80 for HTTP, 443 for HTTPS).\n" +
                    "2. Enter the base path for the WebDAV endpoint.\n" +
                    "   Nextcloud example:  /remote.php/dav/files/<username>/Backups\n" +
                    "   Synology DSM:       /\n" +
                    "3. Enter your username and password (or an app-specific password if your server requires it)."),
                new("YandexDisk", "Yandex Disk",
                    "Yandex Disk via WebDAV.\n\n" +
                    "1. Go to id.yandex.com → Security → App passwords → Create app password for 'WebDAV access'.\n" +
                    "   Copy the generated password — it is shown only once.\n" +
                    "2. Enter your Yandex login as the User.\n" +
                    "3. Enter the app password (not your Yandex account password) as the Password.\n" +
                    "   Server address, HTTPS, and port are configured automatically."),
            ],
            EnumSelectionUiRules =
            [
                // Yandex Disk: host, protocol, and port are all fixed — hide them
                new EnumSelectionUiRule("YandexDisk",
                [
                    new EnumUiPatch("host", Hidden: true),
                    new EnumUiPatch("useHttps", Hidden: true),
                    new EnumUiPatch("port", Hidden: true),
                    new EnumUiPatch("basePath", Hidden: true),
                ]),
            ],
        },
        new StorageFieldDescriptor
        {
            Key = "host",
            Label = Resources.Server_Field_Address,
            Type = StorageFieldType.Text,
            Placeholder = "nextcloud.example.com",
            IsOptional = true,
        },
        new StorageFieldDescriptor
        {
            Key = "useHttps",
            Label = Resources.Storage_Field_Https,
            Type = StorageFieldType.Enum,
            DefaultValue = "Yes",
            Options = [new("Yes", "Yes"), new("No", "No")],
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
            Label = Resources.Storage_Field_BasePath,
            Type = StorageFieldType.Text,
            Placeholder = "/remote.php/dav/files/user/Backups",
            IsOptional = true,
        },
        new StorageFieldDescriptor
        {
            Key = "user",
            Label = Resources.User_Field,
            Type = StorageFieldType.Text,
            IsOptional = true,
        },
        new StorageFieldDescriptor
        {
            Key = "password",
            Label = Resources.Storage_Field_PasswordOrToken,
            Type = StorageFieldType.Password,
        },
    ];

    public bool CanHandle(IStorageSettingsV2 settings) => settings is WebDavStorageSettingsV2;

    public IStorageSettingsV2 CreateSettings(
        IReadOnlyDictionary<string, string?> fieldValues,
        long quota,
        string? mountScript,
        string? unmountScript)
    {
        var preset = fieldValues.GetValueOrDefault("preset") ?? "Custom";
        var host = preset switch
        {
            "YandexDisk" => "webdav.yandex.ru",
            _ => fieldValues.GetValueOrDefault("host") ?? string.Empty,
        };
        var useHttps = preset == "Custom"
            ? fieldValues.GetValueOrDefault("useHttps") != "No"
            : true;

        return new WebDavStorageSettingsV2
        {
            SingleBackupQuotaGb = quota,
            MountPowershellScript = mountScript,
            UnmountPowershellScript = unmountScript,
            Preset = preset,
            Host = host,
            UseHttps = useHttps,
            Port = int.TryParse(fieldValues.GetValueOrDefault("port"), out var port) ? port : 0,
            BasePath = fieldValues.GetValueOrDefault("basePath") ?? string.Empty,
            User = fieldValues.GetValueOrDefault("user") ?? string.Empty,
            Password = fieldValues.GetValueOrDefault("password") ?? string.Empty,
        };
    }

    public IReadOnlyDictionary<string, string?> ExtractValues(IStorageSettingsV2 settings)
    {
        var s = (WebDavStorageSettingsV2)settings;
        return new Dictionary<string, string?>
        {
            ["preset"] = s.Preset,
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
