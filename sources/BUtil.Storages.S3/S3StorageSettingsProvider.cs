using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Storages;
using System.Collections.Generic;

namespace BUtil.Storages.S3;

public class S3StorageSettingsProvider : IStorageSettingsProvider
{
    public IReadOnlyList<StorageFieldDescriptor> Fields { get; } =
    [
        new StorageFieldDescriptor
        {
            Key = "provider",
            Label = Resources.Storage_Field_Provider,
            Type = StorageFieldType.Enum,
            DefaultValue = "Custom",
            Options =
            [
                new("AWSS3", "AWS S3", Resources.S3_Provider_AWSS3_Help),
                new("BackblazeB2", "Backblaze B2", Resources.S3_Provider_BackblazeB2_Help),
                new("Wasabi", "Wasabi", Resources.S3_Provider_Wasabi_Help),
                new("CloudflareR2", "Cloudflare R2", Resources.S3_Provider_CloudflareR2_Help),
                new("DigitalOceanSpaces", "DigitalOcean Spaces", Resources.S3_Provider_DigitalOceanSpaces_Help),
                new("LinodeObjectStorage", "Linode / Akamai Object Storage", Resources.S3_Provider_LinodeObjectStorage_Help),
                new("ScalewayObjectStorage", "Scaleway Object Storage", Resources.S3_Provider_ScalewayObjectStorage_Help),
                new("GoogleCloudStorage", "Google Cloud Storage", Resources.S3_Provider_GoogleCloudStorage_Help),
                new("YandexObjectStorage", "Yandex Object Storage (RU)", Resources.S3_Provider_YandexObjectStorage_Help),
                new("VKCloudStorage", "VK Cloud Object Storage (RU)", Resources.S3_Provider_VKCloudStorage_Help),
                new("CloudRuStorage", "Cloud.ru Object Storage (RU)", Resources.S3_Provider_CloudRuStorage_Help),
                new("SelectelStorage", "Selectel Object Storage (RU)", Resources.S3_Provider_SelectelStorage_Help),
                new("AlibabaCloudOSS", "Alibaba Cloud OSS (CN)", Resources.S3_Provider_AlibabaCloudOSS_Help),
                new("TencentCloudCOS", "Tencent Cloud COS (CN)", Resources.S3_Provider_TencentCloudCOS_Help),
                new("HuaweiCloudOBS", "Huawei Cloud OBS (CN)", Resources.S3_Provider_HuaweiCloudOBS_Help),
                new("BaiduCloudBOS", "Baidu Cloud BOS (CN)", Resources.S3_Provider_BaiduCloudBOS_Help),
                new("QiniuCloudKodo", "Qiniu Cloud Kodo (CN)", Resources.S3_Provider_QiniuCloudKodo_Help),
                new("VolcanoEngineTOS", "Volcano Engine TOS (CN)", Resources.S3_Provider_VolcanoEngineTOS_Help),
                new("Custom", "MinIO / Custom", Resources.S3_Provider_Custom_Help),
            ],
            EnumSelectionUiRules =
            [
                // AWS S3: SDK resolves endpoint from region; no service URL needed
                new EnumSelectionUiRule("AWSS3",
                [
                    new EnumUiPatch("serviceUrl", Hidden: true),
                    new EnumUiPatch("region", PlaceholderOverride: "us-east-1"),
                    new EnumUiPatch("accessKey", LabelOverride: "Access Key ID"),
                    new EnumUiPatch("secretKey", LabelOverride: "Secret Access Key"),
                    new EnumUiPatch("bucket", PlaceholderOverride: "my-backup-bucket"),
                ]),
                // Backblaze B2: keyID / applicationKey terminology
                new EnumSelectionUiRule("BackblazeB2",
                [
                    new EnumUiPatch("serviceUrl", PlaceholderOverride: "https://s3.us-west-004.backblazeb2.com"),
                    new EnumUiPatch("region", PlaceholderOverride: "us-west-004"),
                    new EnumUiPatch("accessKey", LabelOverride: "Key ID"),
                    new EnumUiPatch("secretKey", LabelOverride: "Application Key"),
                    new EnumUiPatch("bucket", PlaceholderOverride: "my-backup-bucket"),
                ]),
                // Wasabi: endpoint is auto-filled; standard region codes
                new EnumSelectionUiRule("Wasabi",
                [
                    new EnumUiPatch("serviceUrl", Hidden: true),
                    new EnumUiPatch("region", PlaceholderOverride: "us-east-1"),
                    new EnumUiPatch("accessKey", LabelOverride: "Access Key"),
                    new EnumUiPatch("secretKey", LabelOverride: "Secret Key"),
                    new EnumUiPatch("bucket", PlaceholderOverride: "my-backup-bucket"),
                ]),
                // Cloudflare R2: region must always be "auto"; account ID in service URL
                new EnumSelectionUiRule("CloudflareR2",
                [
                    new EnumUiPatch("region", ValueWhenSelected: "auto"),
                    new EnumUiPatch("serviceUrl", PlaceholderOverride: "https://<ACCOUNT_ID>.r2.cloudflarestorage.com"),
                    new EnumUiPatch("accessKey", LabelOverride: "Access Key ID"),
                    new EnumUiPatch("secretKey", LabelOverride: "Secret Access Key"),
                    new EnumUiPatch("bucket", PlaceholderOverride: "my-backup-bucket"),
                ]),
                // DigitalOcean Spaces: region-based endpoint
                new EnumSelectionUiRule("DigitalOceanSpaces",
                [
                    new EnumUiPatch("serviceUrl", PlaceholderOverride: "https://nyc3.digitaloceanspaces.com"),
                    new EnumUiPatch("region", PlaceholderOverride: "nyc3"),
                    new EnumUiPatch("accessKey", LabelOverride: "Access Key"),
                    new EnumUiPatch("secretKey", LabelOverride: "Secret Key"),
                    new EnumUiPatch("bucket", PlaceholderOverride: "my-backup-bucket"),
                ]),
                // Linode / Akamai Object Storage
                new EnumSelectionUiRule("LinodeObjectStorage",
                [
                    new EnumUiPatch("serviceUrl", PlaceholderOverride: "https://us-east-1.linodeobjects.com"),
                    new EnumUiPatch("region", PlaceholderOverride: "us-east-1"),
                    new EnumUiPatch("accessKey", LabelOverride: "Access Key"),
                    new EnumUiPatch("secretKey", LabelOverride: "Secret Key"),
                    new EnumUiPatch("bucket", PlaceholderOverride: "my-backup-bucket"),
                ]),
                // Scaleway Object Storage
                new EnumSelectionUiRule("ScalewayObjectStorage",
                [
                    new EnumUiPatch("serviceUrl", PlaceholderOverride: "https://s3.fr-par.scw.cloud"),
                    new EnumUiPatch("region", PlaceholderOverride: "fr-par"),
                    new EnumUiPatch("accessKey", LabelOverride: "Access Key ID"),
                    new EnumUiPatch("secretKey", LabelOverride: "Secret Access Key"),
                    new EnumUiPatch("bucket", PlaceholderOverride: "my-backup-bucket"),
                ]),
                // Google Cloud Storage: endpoint and region are both constant
                new EnumSelectionUiRule("GoogleCloudStorage",
                [
                    new EnumUiPatch("serviceUrl", Hidden: true),
                    new EnumUiPatch("region", Hidden: true),
                    new EnumUiPatch("accessKey", LabelOverride: "HMAC Access Key"),
                    new EnumUiPatch("secretKey", LabelOverride: "HMAC Secret"),
                    new EnumUiPatch("bucket", PlaceholderOverride: "my-backup-bucket"),
                ]),
                // Yandex Object Storage: endpoint and region are both auto-filled
                new EnumSelectionUiRule("YandexObjectStorage",
                [
                    new EnumUiPatch("serviceUrl", Hidden: true),
                    new EnumUiPatch("region", Hidden: true),
                    new EnumUiPatch("accessKey", LabelOverride: "Key ID"),
                    new EnumUiPatch("secretKey", LabelOverride: "Secret Key"),
                    new EnumUiPatch("bucket", PlaceholderOverride: "my-backup-bucket"),
                ]),
                // VK Cloud: endpoint auto-filled
                new EnumSelectionUiRule("VKCloudStorage",
                [
                    new EnumUiPatch("serviceUrl", Hidden: true),
                    new EnumUiPatch("region", PlaceholderOverride: "ru-msk"),
                    new EnumUiPatch("accessKey", LabelOverride: "Access Key"),
                    new EnumUiPatch("secretKey", LabelOverride: "Secret Key"),
                    new EnumUiPatch("bucket", PlaceholderOverride: "my-backup-bucket"),
                ]),
                // Cloud.ru (SberCloud): endpoint auto-filled
                new EnumSelectionUiRule("CloudRuStorage",
                [
                    new EnumUiPatch("serviceUrl", Hidden: true),
                    new EnumUiPatch("region", PlaceholderOverride: "ru-moscow-1"),
                    new EnumUiPatch("accessKey", LabelOverride: "Access Key"),
                    new EnumUiPatch("secretKey", LabelOverride: "Secret Access Key"),
                    new EnumUiPatch("bucket", PlaceholderOverride: "my-backup-bucket"),
                ]),
                // Selectel: endpoint auto-filled
                new EnumSelectionUiRule("SelectelStorage",
                [
                    new EnumUiPatch("serviceUrl", Hidden: true),
                    new EnumUiPatch("region", PlaceholderOverride: "ru-1"),
                    new EnumUiPatch("accessKey", LabelOverride: "Access Key"),
                    new EnumUiPatch("secretKey", LabelOverride: "Secret Key"),
                    new EnumUiPatch("bucket", PlaceholderOverride: "my-backup-bucket"),
                ]),
                // Alibaba Cloud OSS
                new EnumSelectionUiRule("AlibabaCloudOSS",
                [
                    new EnumUiPatch("serviceUrl", PlaceholderOverride: "https://oss-cn-hangzhou.aliyuncs.com"),
                    new EnumUiPatch("region", PlaceholderOverride: "oss-cn-hangzhou"),
                    new EnumUiPatch("accessKey", LabelOverride: "AccessKey ID"),
                    new EnumUiPatch("secretKey", LabelOverride: "AccessKey Secret"),
                    new EnumUiPatch("bucket", PlaceholderOverride: "my-backup-bucket"),
                ]),
                // Tencent Cloud COS: bucket name includes AppId
                new EnumSelectionUiRule("TencentCloudCOS",
                [
                    new EnumUiPatch("serviceUrl", PlaceholderOverride: "https://cos.ap-beijing.myqcloud.com"),
                    new EnumUiPatch("region", PlaceholderOverride: "ap-beijing"),
                    new EnumUiPatch("accessKey", LabelOverride: "SecretId"),
                    new EnumUiPatch("secretKey", LabelOverride: "SecretKey"),
                    new EnumUiPatch("bucket", PlaceholderOverride: "mybucket-1250000000"),
                ]),
                // Huawei Cloud OBS
                new EnumSelectionUiRule("HuaweiCloudOBS",
                [
                    new EnumUiPatch("serviceUrl", PlaceholderOverride: "https://obs.cn-north-4.myhuaweicloud.com"),
                    new EnumUiPatch("region", PlaceholderOverride: "cn-north-4"),
                    new EnumUiPatch("accessKey", LabelOverride: "AK (Access Key)"),
                    new EnumUiPatch("secretKey", LabelOverride: "SK (Secret Key)"),
                    new EnumUiPatch("bucket", PlaceholderOverride: "my-backup-bucket"),
                ]),
                // Baidu Cloud BOS
                new EnumSelectionUiRule("BaiduCloudBOS",
                [
                    new EnumUiPatch("serviceUrl", PlaceholderOverride: "https://s3.bj.bcebos.com"),
                    new EnumUiPatch("region", PlaceholderOverride: "bj"),
                    new EnumUiPatch("accessKey", LabelOverride: "Access Key ID"),
                    new EnumUiPatch("secretKey", LabelOverride: "Secret Access Key"),
                    new EnumUiPatch("bucket", PlaceholderOverride: "my-backup-bucket"),
                ]),
                // Qiniu Cloud Kodo
                new EnumSelectionUiRule("QiniuCloudKodo",
                [
                    new EnumUiPatch("serviceUrl", PlaceholderOverride: "https://s3-cn-east-1.qiniucs.com"),
                    new EnumUiPatch("region", PlaceholderOverride: "cn-east-1"),
                    new EnumUiPatch("accessKey", LabelOverride: "AccessKey"),
                    new EnumUiPatch("secretKey", LabelOverride: "SecretKey"),
                    new EnumUiPatch("bucket", PlaceholderOverride: "my-backup-bucket"),
                ]),
                // Volcano Engine TOS (ByteDance)
                new EnumSelectionUiRule("VolcanoEngineTOS",
                [
                    new EnumUiPatch("serviceUrl", PlaceholderOverride: "https://tos-s3-cn-beijing.volces.com"),
                    new EnumUiPatch("region", PlaceholderOverride: "cn-beijing"),
                    new EnumUiPatch("accessKey", LabelOverride: "Access Key ID"),
                    new EnumUiPatch("secretKey", LabelOverride: "Secret Access Key"),
                    new EnumUiPatch("bucket", PlaceholderOverride: "my-backup-bucket"),
                ]),
                // MinIO / Custom
                new EnumSelectionUiRule("Custom",
                [
                    new EnumUiPatch("serviceUrl", PlaceholderOverride: "http://192.168.1.10:9000"),
                    new EnumUiPatch("region", PlaceholderOverride: "us-east-1"),
                    new EnumUiPatch("accessKey", LabelOverride: "Access Key"),
                    new EnumUiPatch("secretKey", LabelOverride: "Secret Key"),
                    new EnumUiPatch("bucket", PlaceholderOverride: "my-backup-bucket"),
                ]),
            ],
        },
        new StorageFieldDescriptor
        {
            Key = "serviceUrl",
            Label = Resources.Storage_Field_ServiceUrl,
            Type = StorageFieldType.Text,
            Placeholder = "https://s3.example.com",
            IsOptional = true,
        },
        new StorageFieldDescriptor
        {
            Key = "region",
            Label = Resources.Storage_Field_Region,
            Type = StorageFieldType.Text,
            Placeholder = "us-east-1",
            IsOptional = true,
        },
        new StorageFieldDescriptor
        {
            Key = "bucket",
            Label = Resources.Storage_Field_Bucket,
            Type = StorageFieldType.Text,
        },
        new StorageFieldDescriptor
        {
            Key = "accessKey",
            Label = Resources.Storage_Field_AccessKey,
            Type = StorageFieldType.Text,
        },
        new StorageFieldDescriptor
        {
            Key = "secretKey",
            Label = Resources.Storage_Field_SecretKey,
            Type = StorageFieldType.Password,
        },
        new StorageFieldDescriptor
        {
            Key = "pathPrefix",
            Label = Resources.Storage_Field_PathPrefix,
            Type = StorageFieldType.Text,
            Placeholder = "backups/my-pc",
            IsOptional = true,
        },
    ];

    public IReadOnlyList<string> SecretSettingsProperties { get; } = ["secretKey"];


    public IStorageSettingsV2 GetSettings(
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

    public IReadOnlyDictionary<string, string?> GetFieldValues(IStorageSettingsV2 settings)
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
