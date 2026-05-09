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
                new("Custom", "Generic / Custom", Resources.WebDav_Preset_Custom_Help),
                new("Nextcloud", "Nextcloud", Resources.WebDav_Preset_Nextcloud_Help),
                new("ownCloud", "ownCloud", Resources.WebDav_Preset_ownCloud_Help),
                new("Koofr", "Koofr (EU)", Resources.WebDav_Preset_Koofr_Help),
                new("pCloud", "pCloud (EU/US)", Resources.WebDav_Preset_pCloud_Help),
                new("IONOSHiDrive", "IONOS HiDrive (DE)", Resources.WebDav_Preset_IONOSHiDrive_Help),
                new("Box", "Box (US)", Resources.WebDav_Preset_Box_Help),
                new("YandexDisk", "Yandex Disk (RU)", Resources.WebDav_Preset_YandexDisk_Help),
                new("MailRuCloud", "Mail.ru Cloud (RU)", Resources.WebDav_Preset_MailRuCloud_Help),
                new("Jianguoyun", "Jianguoyun / Nutstore (CN)", Resources.WebDav_Preset_Jianguoyun_Help),
            ],
            EnumSelectionUiRules =
            [
                // Nextcloud: host and credentials are user-supplied; pre-fill the base path
                new EnumSelectionUiRule("Nextcloud",
                [
                    new EnumUiPatch("host", PlaceholderOverride: "nextcloud.example.com"),
                    new EnumUiPatch("basePath", PlaceholderOverride: "/remote.php/dav/files/<username>/"),
                    new EnumUiPatch("user", PlaceholderOverride: "your-nextcloud-username"),
                    new EnumUiPatch("password", LabelOverride: "Password / App Password"),
                ]),
                // ownCloud: same structure as Nextcloud
                new EnumSelectionUiRule("ownCloud",
                [
                    new EnumUiPatch("host", PlaceholderOverride: "owncloud.example.com"),
                    new EnumUiPatch("basePath", PlaceholderOverride: "/remote.php/dav/files/<username>/"),
                    new EnumUiPatch("user", PlaceholderOverride: "your-owncloud-username"),
                    new EnumUiPatch("password", LabelOverride: "Password / App Password"),
                ]),
                // Koofr: all server details are fixed
                new EnumSelectionUiRule("Koofr",
                [
                    new EnumUiPatch("host", Hidden: true),
                    new EnumUiPatch("useHttps", Hidden: true),
                    new EnumUiPatch("port", Hidden: true),
                    new EnumUiPatch("basePath", Hidden: true),
                    new EnumUiPatch("user", PlaceholderOverride: "your@email.com"),
                    new EnumUiPatch("password", LabelOverride: "App Password"),
                ]),
                // pCloud EU: server details fixed for EU DC
                new EnumSelectionUiRule("pCloud",
                [
                    new EnumUiPatch("host", Hidden: true),
                    new EnumUiPatch("useHttps", Hidden: true),
                    new EnumUiPatch("port", Hidden: true),
                    new EnumUiPatch("basePath", Hidden: true),
                    new EnumUiPatch("user", PlaceholderOverride: "your@email.com"),
                    new EnumUiPatch("password", LabelOverride: "App Password"),
                ]),
                // IONOS HiDrive: all server details fixed
                new EnumSelectionUiRule("IONOSHiDrive",
                [
                    new EnumUiPatch("host", Hidden: true),
                    new EnumUiPatch("useHttps", Hidden: true),
                    new EnumUiPatch("port", Hidden: true),
                    new EnumUiPatch("basePath", Hidden: true),
                    new EnumUiPatch("user", PlaceholderOverride: "your-hidrive-username"),
                ]),
                // Box: all server details fixed
                new EnumSelectionUiRule("Box",
                [
                    new EnumUiPatch("host", Hidden: true),
                    new EnumUiPatch("useHttps", Hidden: true),
                    new EnumUiPatch("port", Hidden: true),
                    new EnumUiPatch("basePath", Hidden: true),
                    new EnumUiPatch("user", PlaceholderOverride: "your@email.com"),
                ]),
                // Yandex Disk: host, protocol, and port are all fixed — hide them
                new EnumSelectionUiRule("YandexDisk",
                [
                    new EnumUiPatch("host", Hidden: true),
                    new EnumUiPatch("useHttps", Hidden: true),
                    new EnumUiPatch("port", Hidden: true),
                    new EnumUiPatch("basePath", Hidden: true),
                    new EnumUiPatch("user", PlaceholderOverride: "your-yandex-login"),
                    new EnumUiPatch("password", LabelOverride: "App Password"),
                ]),
                // Mail.ru Cloud: all server details fixed
                new EnumSelectionUiRule("MailRuCloud",
                [
                    new EnumUiPatch("host", Hidden: true),
                    new EnumUiPatch("useHttps", Hidden: true),
                    new EnumUiPatch("port", Hidden: true),
                    new EnumUiPatch("basePath", Hidden: true),
                    new EnumUiPatch("user", PlaceholderOverride: "your@mail.ru"),
                    new EnumUiPatch("password", LabelOverride: "App Password"),
                ]),
                // Jianguoyun: all server details fixed
                new EnumSelectionUiRule("Jianguoyun",
                [
                    new EnumUiPatch("host", Hidden: true),
                    new EnumUiPatch("useHttps", Hidden: true),
                    new EnumUiPatch("port", Hidden: true),
                    new EnumUiPatch("basePath", Hidden: true),
                    new EnumUiPatch("user", PlaceholderOverride: "your@email.com"),
                    new EnumUiPatch("password", LabelOverride: "App Password"),
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
            "Koofr"        => "app.koofr.eu",
            "pCloud"       => "ewebdav.pcloud.com",
            "IONOSHiDrive" => "webdav.hidrive.ionos.com",
            "Box"          => "dav.box.com",
            "YandexDisk"   => "webdav.yandex.ru",
            "MailRuCloud"  => "webdav.cloud.mail.ru",
            "Jianguoyun"   => "dav.jianguoyun.com",
            _ => fieldValues.GetValueOrDefault("host") ?? string.Empty,
        };
        var basePath = preset switch
        {
            "Box"         => "/dav",
            "MailRuCloud" => "/dav",
            "Jianguoyun"  => "/dav/",
            _ => fieldValues.GetValueOrDefault("basePath") ?? string.Empty,
        };
        var useHttps = preset is "Custom" or "Nextcloud" or "ownCloud"
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
            BasePath = basePath,
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
