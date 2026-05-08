using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Storages;
using System.Collections.Generic;

namespace BUtil.Storages.S3;

public class S3StorageSettingsProvider : IStorageSettingsProvider
{
    public string StorageId => "S3";
    public string DisplayName => "S3";
    public int Order => 6;
    public bool IsSupported => true;

    public IReadOnlyList<StorageFieldDescriptor> Fields { get; } =
    [
        new StorageFieldDescriptor
        {
            Key = "provider",
            Label = "Provider",
            Type = StorageFieldType.Enum,
            DefaultValue = "Custom",
            Options =
            [
                ("AWSS3",              "AWS S3"),
                ("BackblazeB2",        "Backblaze B2"),
                ("Wasabi",             "Wasabi"),
                ("CloudflareR2",       "Cloudflare R2"),
                ("DigitalOceanSpaces", "DigitalOcean Spaces"),
                ("GoogleCloudStorage", "Google Cloud Storage"),
                ("YandexObjectStorage","Yandex Object Storage (RU)"),
                ("VKCloudStorage",     "VK Cloud Object Storage (RU)"),
                ("CloudRuStorage",     "Cloud.ru Object Storage (RU)"),
                ("SelectelStorage",    "Selectel Object Storage (RU)"),
                ("AlibabaCloudOSS",   "Alibaba Cloud OSS (CN)"),
                ("TencentCloudCOS",   "Tencent Cloud COS (CN)"),
                ("HuaweiCloudOBS",    "Huawei Cloud OBS (CN)"),
                ("BaiduCloudBOS",     "Baidu Cloud BOS (CN)"),
                ("QiniuCloudKodo",    "Qiniu Cloud Kodo (CN)"),
                ("VolcanoEngineTOS",  "Volcano Engine TOS (CN)"),
                ("Custom",             "MinIO / Custom"),
            ],
        },
        new StorageFieldDescriptor
        {
            Key = "serviceUrl",
            Label = "Service URL",
            Type = StorageFieldType.Text,
            // Leave empty for AWS S3 (uses region) or Wasabi (auto-filled).
            // Examples: https://s3.us-west-004.backblazeb2.com  |  https://<id>.r2.cloudflarestorage.com
            Placeholder = "https://s3.example.com",
            IsOptional = true,
        },
        new StorageFieldDescriptor
        {
            Key = "region",
            Label = "Region",
            Type = StorageFieldType.Text,
            Placeholder = "us-east-1",
            IsOptional = true,
        },
        new StorageFieldDescriptor
        {
            Key = "accessKey",
            Label = "Access Key",
            Type = StorageFieldType.Text,
        },
        new StorageFieldDescriptor
        {
            Key = "secretKey",
            Label = "Secret Key",
            Type = StorageFieldType.Password,
        },
        new StorageFieldDescriptor
        {
            Key = "bucket",
            Label = "Bucket",
            Type = StorageFieldType.Text,
        },
        new StorageFieldDescriptor
        {
            Key = "pathPrefix",
            Label = "Path Prefix",
            Type = StorageFieldType.Text,
            Placeholder = "backups/my-pc",
            IsOptional = true,
        },
    ];

    public bool CanHandle(IStorageSettingsV2 settings) => settings is S3StorageSettingsV2;

    public IStorageSettingsV2 CreateSettings(
        IReadOnlyDictionary<string, string?> fieldValues,
        long quota,
        string? mountScript,
        string? unmountScript)
    {
        var provider = fieldValues.GetValueOrDefault("provider") ?? "Custom";
        var serviceUrl = fieldValues.GetValueOrDefault("serviceUrl") ?? string.Empty;

        if (string.IsNullOrWhiteSpace(serviceUrl))
        {
            serviceUrl = provider switch
            {
                "Wasabi"              => "https://s3.wasabisys.com",
                "GoogleCloudStorage"  => "https://storage.googleapis.com",
                "YandexObjectStorage" => "https://storage.yandexcloud.net",
                "VKCloudStorage"      => "https://hb.ru-msk.vkcloud-storage.ru",
                "CloudRuStorage"      => "https://obs.ru-moscow-1.hc.sbercloud.ru",
                "SelectelStorage"     => "https://s3.storage.selcloud.ru",
                _ => string.Empty,
            };
        }

        var region = fieldValues.GetValueOrDefault("region") ?? string.Empty;
        if (string.IsNullOrWhiteSpace(region))
        {
            region = provider switch
            {
                "YandexObjectStorage" => "ru-central1",
                _ => string.Empty,
            };
        }

        return new S3StorageSettingsV2
        {
            SingleBackupQuotaGb = quota,
            MountPowershellScript = mountScript,
            UnmountPowershellScript = unmountScript,
            Provider = provider,
            ServiceUrl = serviceUrl,
            Region = region,
            AccessKey = fieldValues.GetValueOrDefault("accessKey") ?? string.Empty,
            SecretKey = fieldValues.GetValueOrDefault("secretKey") ?? string.Empty,
            BucketName = fieldValues.GetValueOrDefault("bucket") ?? string.Empty,
            PathPrefix = fieldValues.GetValueOrDefault("pathPrefix") ?? string.Empty,
        };
    }

    public IReadOnlyDictionary<string, string?> ExtractValues(IStorageSettingsV2 settings)
    {
        var s = (S3StorageSettingsV2)settings;
        return new Dictionary<string, string?>
        {
            ["provider"] = s.Provider,
            ["serviceUrl"] = s.ServiceUrl,
            ["region"] = s.Region,
            ["accessKey"] = s.AccessKey,
            ["secretKey"] = s.SecretKey,
            ["bucket"] = s.BucketName,
            ["pathPrefix"] = s.PathPrefix,
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
