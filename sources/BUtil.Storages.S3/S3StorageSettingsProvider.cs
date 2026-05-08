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
            Label = Resources.Storage_Field_Provider,
            Type = StorageFieldType.Enum,
            DefaultValue = "Custom",
            Options =
            [
                new("AWSS3", "AWS S3",
                    "AWS S3 — Amazon's original object storage.\n\n" +
                    "1. Open the S3 Console (https://s3.console.aws.amazon.com/) and create a bucket. Note its region (e.g. us-east-1).\n" +
                    "2. Open IAM → Users → create a user → attach the AmazonS3FullAccess policy.\n" +
                    "3. Under Security credentials, create an Access key — copy the Key ID and Secret.\n" +
                    "4. Fill in: Bucket, Access Key, Secret Key, and Region."),
                new("BackblazeB2", "Backblaze B2",
                    "Backblaze B2 — affordable S3-compatible storage.\n\n" +
                    "1. In Backblaze (https://www.backblaze.com/) → B2 Cloud Storage → Buckets, create a bucket.\n" +
                    "2. Go to App Keys → Add a New Application Key, scope it to the bucket.\n" +
                    "   Copy the keyID (= Access Key) and applicationKey (= Secret Key).\n" +
                    "3. Service URL: your bucket's endpoint shown in bucket details, e.g. https://s3.us-west-004.backblazeb2.com\n" +
                    "4. Region: the segment between 's3.' and '.backblazeb2.com', e.g. us-west-004."),
                new("Wasabi", "Wasabi",
                    "Wasabi — hot cloud storage with no egress fees.\n\n" +
                    "1. In Wasabi Console (https://console.wasabisys.com/) → Buckets → Create Bucket. Choose a region.\n" +
                    "2. Go to Access Keys → Create New Access Key. Copy the Access Key and Secret Key.\n" +
                    "3. Fill in: Bucket, Access Key, Secret Key, and Region (e.g. us-east-1, eu-central-1).\n" +
                    "   Service URL is filled in automatically."),
                new("CloudflareR2", "Cloudflare R2",
                    "Cloudflare R2 — zero-egress S3-compatible storage.\n\n" +
                    "1. In Cloudflare Dashboard (https://dash.cloudflare.com/) → R2 → create a bucket.\n" +
                    "2. Go to R2 → Manage R2 API Tokens → Create API Token with Object Read & Write.\n" +
                    "   Copy the Access Key ID and Secret Access Key.\n" +
                    "3. Service URL: https://<ACCOUNT_ID>.r2.cloudflarestorage.com\n" +
                    "   Your Account ID is shown on the R2 overview page.\n" +
                    "4. Fill in: Bucket, Access Key, Secret Key, and Service URL. Region is set to 'auto' automatically."),
                new("DigitalOceanSpaces", "DigitalOcean Spaces",
                    "DigitalOcean Spaces — S3-compatible object storage.\n\n" +
                    "1. In Spaces (https://cloud.digitalocean.com/spaces), create a Space. Note its region (e.g. nyc3).\n" +
                    "2. Go to API → Spaces Keys → Generate New Key. Copy the Key (= Access Key) and Secret.\n" +
                    "3. Service URL: https://<REGION>.digitaloceanspaces.com (e.g. https://nyc3.digitaloceanspaces.com).\n" +
                    "4. Region: same short code, e.g. nyc3."),
                new("LinodeObjectStorage", "Linode / Akamai Object Storage",
                    "Linode (Akamai) Object Storage — S3-compatible storage.\n\n" +
                    "1. In Linode Cloud Manager (https://cloud.linode.com/object-storage/buckets), create a bucket. Note its cluster.\n" +
                    "2. Go to Object Storage → Access Keys → Create an Access Key scoped to the bucket.\n" +
                    "   Copy the Access Key and Secret Key.\n" +
                    "3. Service URL: https://<CLUSTER>.linodeobjects.com (e.g. https://us-east-1.linodeobjects.com).\n" +
                    "4. Region: same cluster code, e.g. us-east-1."),
                new("ScalewayObjectStorage", "Scaleway Object Storage",
                    "Scaleway Object Storage — European S3-compatible storage.\n\n" +
                    "1. In Scaleway Console (https://console.scaleway.com/object-storage/buckets), create a bucket. Note its region (e.g. fr-par).\n" +
                    "2. Go to IAM → API Keys → Generate API Key with Object Storage write permissions.\n" +
                    "   Copy the Access Key and Secret Key.\n" +
                    "3. Service URL: https://s3.<REGION>.scw.cloud (e.g. https://s3.fr-par.scw.cloud).\n" +
                    "4. Region: e.g. fr-par, nl-ams, pl-waw."),
                new("GoogleCloudStorage", "Google Cloud Storage",
                    "Google Cloud Storage — via the S3 interoperability API.\n\n" +
                    "1. In Google Cloud Console (https://console.cloud.google.com/storage), create a bucket.\n" +
                    "2. Go to Cloud Storage → Settings → Interoperability → create an HMAC key for a service account.\n" +
                    "   Copy the Access Key and Secret.\n" +
                    "3. Fill in: Bucket, Access Key (HMAC key), Secret Key (HMAC secret).\n" +
                    "   Service URL and region are set automatically."),
                new("YandexObjectStorage", "Yandex Object Storage (RU)",
                    "Yandex Object Storage — S3-compatible Russian cloud storage.\n\n" +
                    "1. In Yandex Cloud Console (https://console.yandex.cloud/) → Object Storage → create a bucket.\n" +
                    "2. Go to Service Accounts → create one with the storage.editor role → create a Static access key.\n" +
                    "   Copy the Key ID (= Access Key) and Secret key.\n" +
                    "3. Fill in: Bucket, Access Key, Secret Key.\n" +
                    "   Service URL and region are set automatically."),
                new("VKCloudStorage", "VK Cloud Object Storage (RU)",
                    "VK Cloud Object Storage — S3-compatible storage from VK Cloud.\n\n" +
                    "1. In VK Cloud Portal (https://msk.cloud.vk.com/) → Cloud Storage → create a bucket.\n" +
                    "2. Under your account → Access Keys → create a new key pair.\n" +
                    "   Copy the Access Key and Secret Key.\n" +
                    "3. Fill in: Bucket, Access Key, Secret Key, and Region if required (e.g. ru-msk).\n" +
                    "   Service URL is filled in automatically."),
                new("CloudRuStorage", "Cloud.ru Object Storage (RU)",
                    "Cloud.ru (SberCloud) Object Storage — S3-compatible OBS storage.\n\n" +
                    "1. In Cloud.ru Console (https://cloud.ru/) → Object Storage Service (OBS) → create a bucket.\n" +
                    "2. Go to My Credentials → Access Keys → Add Access Key.\n" +
                    "   Copy the Access Key and Secret Access Key.\n" +
                    "3. Fill in: Bucket, Access Key, Secret Key, and Region (e.g. ru-moscow-1).\n" +
                    "   Service URL is filled in automatically."),
                new("SelectelStorage", "Selectel Object Storage (RU)",
                    "Selectel Object Storage — S3-compatible Russian storage.\n\n" +
                    "1. In Selectel Panel (https://my.selectel.ru/) → Cloud Storage → create a container (= bucket).\n" +
                    "2. Under Users & Roles, create a service user with storage access → S3 Keys → create a key pair.\n" +
                    "   Copy the Access Key and Secret Key.\n" +
                    "3. Fill in: Bucket, Access Key, Secret Key.\n" +
                    "   Service URL is filled in automatically."),
                new("AlibabaCloudOSS", "Alibaba Cloud OSS (CN)",
                    "Alibaba Cloud OSS — S3-compatible object storage from Alibaba.\n\n" +
                    "1. In OSS Console (https://oss.console.aliyun.com/), create a bucket. Note its region (e.g. oss-cn-hangzhou).\n" +
                    "2. In RAM Console → Users → create a user → attach AliyunOSSFullAccess → create an AccessKey.\n" +
                    "   Copy the AccessKey ID (= Access Key) and AccessKey Secret (= Secret Key).\n" +
                    "3. Service URL: https://oss-<REGION>.aliyuncs.com (e.g. https://oss-cn-hangzhou.aliyuncs.com).\n" +
                    "4. Region: e.g. oss-cn-hangzhou."),
                new("TencentCloudCOS", "Tencent Cloud COS (CN)",
                    "Tencent Cloud COS — S3-compatible object storage from Tencent.\n\n" +
                    "1. In COS Console (https://console.cloud.tencent.com/cos), create a bucket. Note the AppId and Region (e.g. ap-beijing).\n" +
                    "2. In Access Management → API Keys → Create Key.\n" +
                    "   Copy the SecretId (= Access Key) and SecretKey (= Secret Key).\n" +
                    "3. Service URL: https://cos.<REGION>.myqcloud.com (e.g. https://cos.ap-beijing.myqcloud.com).\n" +
                    "4. Bucket name format: <name>-<AppId> (e.g. mybucket-1250000000). Region: e.g. ap-beijing."),
                new("HuaweiCloudOBS", "Huawei Cloud OBS (CN)",
                    "Huawei Cloud OBS — S3-compatible object storage from Huawei.\n\n" +
                    "1. In OBS Console (https://console.huaweicloud.com/obs/), create a bucket. Note its region (e.g. cn-north-4).\n" +
                    "2. Go to My Credentials → Access Keys → Create Access Key.\n" +
                    "   Copy the AK (= Access Key) and SK (= Secret Key).\n" +
                    "3. Service URL: https://obs.<REGION>.myhuaweicloud.com (e.g. https://obs.cn-north-4.myhuaweicloud.com).\n" +
                    "4. Region: e.g. cn-north-4."),
                new("BaiduCloudBOS", "Baidu Cloud BOS (CN)",
                    "Baidu Cloud BOS — S3-compatible object storage from Baidu.\n\n" +
                    "1. In BOS Console (https://console.bce.baidu.com/bos/), create a bucket. Note its region (e.g. bj, gz, su).\n" +
                    "2. Go to Security → Access Keys — copy your Access Key ID and Secret Access Key.\n" +
                    "3. Service URL: https://s3.<REGION>.bcebos.com (e.g. https://s3.bj.bcebos.com).\n" +
                    "4. Region: e.g. bj."),
                new("QiniuCloudKodo", "Qiniu Cloud Kodo (CN)",
                    "Qiniu Cloud Kodo — S3-compatible storage from Qiniu.\n\n" +
                    "1. In Qiniu Portal (https://portal.qiniu.com/kodo/bucket), create a bucket. Note its region.\n" +
                    "2. Go to Personal Center → Key Management — copy your AccessKey and SecretKey.\n" +
                    "3. Service URL by region:\n" +
                    "   East China:    https://s3-cn-east-1.qiniucs.com\n" +
                    "   North China:   https://s3-cn-north-1.qiniucs.com\n" +
                    "   South China:   https://s3-cn-south-1.qiniucs.com\n" +
                    "   North America: https://s3-us-north-1.qiniucs.com\n" +
                    "4. Region: matching code, e.g. cn-east-1."),
                new("VolcanoEngineTOS", "Volcano Engine TOS (CN)",
                    "Volcano Engine TOS (ByteDance) — S3-compatible storage.\n\n" +
                    "1. In Volcano Engine Console (https://console.volcengine.com/tos), create a bucket. Note its region (e.g. cn-beijing).\n" +
                    "2. Go to IAM → Access Keys → create a key.\n" +
                    "   Copy the Access Key ID and Secret Access Key.\n" +
                    "3. Service URL: https://tos-s3-<REGION>.volces.com (e.g. https://tos-s3-cn-beijing.volces.com).\n" +
                    "4. Region: e.g. cn-beijing."),
                new("Custom", "MinIO / Custom",
                    "MinIO or any S3-compatible server.\n\n" +
                    "1. Start your MinIO (or other S3-compatible) server.\n" +
                    "2. Create a bucket via the MinIO Console or 'mc mb <alias>/<bucket>'.\n" +
                    "3. Create an access key in MinIO Console → Access Keys (avoid root credentials in production).\n" +
                    "4. Service URL: your server endpoint, e.g. http://192.168.1.10:9000 or https://minio.example.com.\n" +
                    "5. Region: any value (e.g. us-east-1) — or leave empty if the server does not enforce it."),
            ],
            EnumSelectionUiRules =
            [
                // AWS S3: the SDK resolves the endpoint from the region; no service URL needed
                new EnumSelectionUiRule("AWSS3",
                [
                    new EnumUiPatch("serviceUrl", Hidden: true),
                ]),
                // Wasabi: endpoint is auto-filled to https://s3.wasabisys.com
                new EnumSelectionUiRule("Wasabi",
                [
                    new EnumUiPatch("serviceUrl", Hidden: true),
                ]),
                // Cloudflare R2: region must always be the literal string "auto"
                new EnumSelectionUiRule("CloudflareR2",
                [
                    new EnumUiPatch("region", ValueWhenSelected: "auto"),
                ]),
                // Google Cloud Storage: endpoint and region are both constant
                new EnumSelectionUiRule("GoogleCloudStorage",
                [
                    new EnumUiPatch("serviceUrl", Hidden: true),
                    new EnumUiPatch("region", Hidden: true),
                ]),
                // Yandex Object Storage: endpoint and region are both auto-filled
                new EnumSelectionUiRule("YandexObjectStorage",
                [
                    new EnumUiPatch("serviceUrl", Hidden: true),
                    new EnumUiPatch("region", Hidden: true),
                ]),
                // VK Cloud: endpoint auto-filled
                new EnumSelectionUiRule("VKCloudStorage",
                [
                    new EnumUiPatch("serviceUrl", Hidden: true),
                ]),
                // Cloud.ru: endpoint auto-filled
                new EnumSelectionUiRule("CloudRuStorage",
                [
                    new EnumUiPatch("serviceUrl", Hidden: true),
                ]),
                // Selectel: endpoint auto-filled
                new EnumSelectionUiRule("SelectelStorage",
                [
                    new EnumUiPatch("serviceUrl", Hidden: true),
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
            Key = "bucket",
            Label = Resources.Storage_Field_Bucket,
            Type = StorageFieldType.Text,
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
