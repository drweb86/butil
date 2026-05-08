# S3-Compatible Storage Setup

BUtil supports any S3-compatible object storage. One protocol covers a large ecosystem: **AWS S3**, **Backblaze B2**, **Wasabi**, **Cloudflare R2**, **DigitalOcean Spaces**, **MinIO**, and many others.

## Fields

| Field | Description |
|---|---|
| **Provider** | Select your provider. Choosing a known preset auto-fills the Service URL where possible. |
| **Service URL** | The HTTPS endpoint of the S3-compatible API. Leave empty for AWS S3 (uses Region instead). |
| **Region** | AWS region (e.g. `us-east-1`) or equivalent for other providers. Required for AWS; optional but recommended for others. |
| **Access Key** | Your access key ID / key ID. |
| **Secret Key** | Your secret access key / application key. |
| **Bucket** | The bucket name. Create it in advance in your provider's console. |
| **Path Prefix** | Optional prefix inside the bucket (e.g. `backups/laptop`). Useful when sharing one bucket across multiple machines. |

---

## Provider-by-Provider Setup

### AWS S3

1. Open the [AWS Console → IAM](https://console.aws.amazon.com/iam/).
2. Create a user (or use an existing one) with **AmazonS3FullAccess** on the target bucket (use a least-privilege inline policy in production).
3. Generate an **Access Key** for that user.
4. Create an S3 bucket in the region of your choice.
5. In BUtil:
   - **Provider**: AWS S3
   - **Region**: e.g. `eu-central-1`
   - **Service URL**: leave empty
   - **Access Key / Secret Key**: from step 3

> **Tip:** Restrict the IAM policy to a single bucket and optionally a key prefix to limit blast radius.

---

### Backblaze B2

1. Log into [backblaze.com](https://www.backblaze.com/) and create a **B2 bucket** (set *Private*).
2. Go to **Application Keys** and create a new key with read/write access to that bucket.
3. Note the **keyID**, **applicationKey**, and the **endpoint** shown in the bucket's details (e.g. `s3.us-west-004.backblazeb2.com`).
4. In BUtil:
   - **Provider**: Backblaze B2
   - **Service URL**: your bucket's endpoint, e.g. `https://s3.us-west-004.backblazeb2.com`
   - **Region**: the region shown in the endpoint (e.g. `us-west-004`)
   - **Access Key**: your keyID
   - **Secret Key**: your applicationKey

---

### Wasabi

1. Log into [wasabi.com](https://wasabi.com/) and create a bucket. Note the bucket's region.
2. Go to **Access Keys** and create a new key pair.
3. In BUtil:
   - **Provider**: Wasabi
   - **Service URL**: leave empty — auto-filled as `https://s3.wasabisys.com`. If your bucket is in a specific regional endpoint (e.g. `s3.eu-central-1.wasabisys.com`), enter it manually.
   - **Region**: e.g. `eu-central-1`
   - **Access Key / Secret Key**: from step 2

---

### Cloudflare R2

1. In the [Cloudflare dashboard](https://dash.cloudflare.com/) open **R2** and create a bucket.
2. Go to **R2 → Manage API Tokens** and create a token with *Object Read & Write* on the bucket.
3. Note your **Account ID** (shown in the right sidebar on the R2 page).
4. In BUtil:
   - **Provider**: Cloudflare R2
   - **Service URL**: `https://<accountId>.r2.cloudflarestorage.com`
   - **Region**: `auto`
   - **Access Key**: the token's Access Key ID
   - **Secret Key**: the token's Secret Access Key

---

### DigitalOcean Spaces

1. In the [DigitalOcean control panel](https://cloud.digitalocean.com/) create a **Space** (bucket). Note the region.
2. Go to **API → Spaces Keys** and create a new key pair.
3. In BUtil:
   - **Provider**: DigitalOcean Spaces
   - **Service URL**: `https://<region>.digitaloceanspaces.com` (e.g. `https://ams3.digitaloceanspaces.com`)
   - **Region**: the region code (e.g. `ams3`)
   - **Access Key / Secret Key**: from step 2

---

### Linode / Akamai Object Storage

Linode Object Storage (now part of Akamai Cloud) is an S3-compatible service available across multiple global regions.

Service URL pattern: `https://<cluster>.linodeobjects.com`
Common clusters: `us-east-1`, `eu-central-1`, `ap-south-1`, `us-southeast-1`, `us-ord-1`, `fr-par-1`
Example: `https://eu-central-1.linodeobjects.com`

1. Log into [Akamai Cloud Manager](https://cloud.linode.com/) and go to **Object Storage → Buckets**. Create a bucket and note its cluster/region.
2. Go to **Object Storage → Access Keys** and create a new key pair (limit it to the specific bucket for safety).
3. In BUtil:
   - **Provider**: Linode / Akamai Object Storage
   - **Service URL**: `https://<cluster>.linodeobjects.com`
   - **Region**: the cluster name (e.g. `eu-central-1`)
   - **Access Key / Secret Key**: from step 2
   - **Bucket**: your bucket name

---

### Scaleway Object Storage

Scaleway is a French cloud provider with data centres in Paris, Amsterdam, and Warsaw — a popular S3 option in Europe.

Service URL pattern: `https://s3.<region>.scw.cloud`
Available regions: `fr-par` (Paris), `nl-ams` (Amsterdam), `pl-waw` (Warsaw)
Example: `https://s3.fr-par.scw.cloud`

1. Log into the [Scaleway Console](https://console.scaleway.com/) and open **Object Storage**. Create a bucket in the region of your choice.
2. Go to **IAM → API Keys** and create a new key pair, or use an existing one with *ObjectStorageFullAccess*.
3. In BUtil:
   - **Provider**: Scaleway Object Storage
   - **Service URL**: `https://s3.<region>.scw.cloud`
   - **Region**: e.g. `fr-par`
   - **Access Key / Secret Key**: from step 2
   - **Bucket**: your bucket name

> **GDPR note:** All three Scaleway regions are inside the EU, making Scaleway a straightforward choice for storing personal data of EU residents without additional transfer safeguards.

---

### Google Cloud Storage

Google Cloud Storage supports the S3-compatible XML API using HMAC keys — no separate plugin needed.

1. In the [Google Cloud Console](https://console.cloud.google.com/) open **Cloud Storage** and create a bucket.
2. Go to **Settings → Interoperability** in Cloud Storage.
3. Under **Service account HMAC keys**, create a new key for the service account that has *Storage Object Admin* on the bucket.
4. Copy the **Access Key** and **Secret**.
5. In BUtil:
   - **Provider**: Google Cloud Storage
   - **Service URL**: auto-filled as `https://storage.googleapis.com`
   - **Region**: your bucket's region (e.g. `europe-west1`). Optional — GCS accepts requests without it.
   - **Access Key / Secret Key**: from step 4
   - **Bucket**: your bucket name

> **Tip:** HMAC keys are created per service account. Grant only *Storage Object Admin* on the specific bucket to limit permissions.

---

---

## Russia

> **FZ-152 (Personal Data) note:** Russian law requires personal data of Russian citizens to be stored on servers physically located in Russia. All providers in this section operate data centres in Russia. To remain compliant, verify that the bucket region maps to a Russian location (all default regions listed below do).

### Yandex Object Storage

1. Log into [Yandex Cloud Console](https://console.yandex.cloud/) and create a **bucket** under **Object Storage**. Note the bucket name and region (default: `ru-central1`).
2. Go to **Service Accounts**, create an account with the `storage.editor` role on the bucket (or `storage.uploader` for write-only).
3. Under the service account, create **static access keys** — copy the **Key ID** and **Secret Key**.
4. In BUtil:
   - **Provider**: Yandex Object Storage (RU)
   - **Service URL**: auto-filled as `https://storage.yandexcloud.net`
   - **Region**: auto-filled as `ru-central1` (change only if your bucket is in another zone)
   - **Access Key / Secret Key**: from step 3
   - **Bucket**: your bucket name

> **Tip:** Yandex Object Storage supports S3 Lifecycle rules — configure automatic deletion of old backups directly in the bucket settings.

---

### VK Cloud Object Storage

1. Log into [VK Cloud](https://mcs.mail.ru/) and open **Object Storage**.
2. Create a bucket (set *Private*).
3. Go to **Access Keys** in the Object Storage section and generate a new key pair.
4. In BUtil:
   - **Provider**: VK Cloud Object Storage (RU)
   - **Service URL**: auto-filled as `https://hb.ru-msk.vkcloud-storage.ru`
   - **Access Key / Secret Key**: from step 3
   - **Bucket**: your bucket name

---

### Cloud.ru Object Storage (SberCloud)

1. Log into [Cloud.ru Console](https://console.cloud.ru/) and open **Object Storage Service (OBS)**.
2. Create a bucket in the `ru-moscow-1` region.
3. Go to **My Credentials → Access Keys** and create a new access key pair.
4. In BUtil:
   - **Provider**: Cloud.ru Object Storage (RU)
   - **Service URL**: auto-filled as `https://obs.ru-moscow-1.hc.sbercloud.ru`
   - **Access Key / Secret Key**: from step 3
   - **Bucket**: your bucket name

---

### Selectel Object Storage

1. Log into the [Selectel Control Panel](https://my.selectel.ru/) and open **Cloud Storage**.
2. Create a container (set *Private*).
3. Go to **Profile → API access** and generate S3 credentials (key ID and secret).
4. In BUtil:
   - **Provider**: Selectel Object Storage (RU)
   - **Service URL**: auto-filled as `https://s3.storage.selcloud.ru`
   - **Access Key / Secret Key**: from step 3
   - **Bucket**: your container name

---

## China

> **PIPL (Personal Information Protection Law) note:** Chinese law requires personal information of Chinese residents to be stored within China and prohibits cross-border transfer without a legal basis (consent, necessity assessment, or a standard contract). All providers in this section offer mainland China regions. To stay compliant, choose a region physically inside mainland China (e.g. `cn-east-1`, `ap-beijing`, `cn-north-4`) and do not enable cross-region replication to overseas endpoints.

For Chinese providers, the Service URL contains the region code. Enter it manually after choosing your provider and region.

### Alibaba Cloud OSS

Service URL pattern: `https://oss-cn-<region>.aliyuncs.com`
Common regions: `shanghai`, `beijing`, `hangzhou`, `shenzhen`
Example: `https://oss-cn-shanghai.aliyuncs.com`

1. Log into [Alibaba Cloud Console](https://www.aliyun.com/) and create an **OSS bucket** in the desired region.
2. Go to **RAM Console → Users**, create a user with `AliyunOSSFullAccess` on the bucket, and create an **AccessKey** for that user.
3. In BUtil:
   - **Provider**: Alibaba Cloud OSS (CN)
   - **Service URL**: `https://oss-cn-<region>.aliyuncs.com`
   - **Region**: e.g. `cn-shanghai`
   - **Access Key / Secret Key**: from step 2
   - **Bucket**: your bucket name

---

### Tencent Cloud COS

Service URL pattern: `https://cos.<region>.myqcloud.com`
Common regions: `ap-beijing`, `ap-shanghai`, `ap-guangzhou`, `ap-chengdu`
Example: `https://cos.ap-beijing.myqcloud.com`

1. Log into [Tencent Cloud Console](https://www.tencentcloud.com/) and create a **COS bucket** (set *Private*).
2. Go to **CAM → API Keys** and create a new key pair.
3. In BUtil:
   - **Provider**: Tencent Cloud COS (CN)
   - **Service URL**: `https://cos.<region>.myqcloud.com`
   - **Region**: e.g. `ap-beijing`
   - **Access Key / Secret Key**: from step 2
   - **Bucket**: your bucket name

---

### Huawei Cloud OBS

Service URL pattern: `https://obs.<region>.myhuaweicloud.com`
Common regions: `cn-north-4` (Beijing), `cn-east-3` (Shanghai), `cn-south-1` (Guangzhou)
Example: `https://obs.cn-north-4.myhuaweicloud.com`

1. Log into [Huawei Cloud Console](https://www.huaweicloud.com/) and create an **OBS bucket**.
2. Go to **IAM → My Credentials → Access Keys** and create a new access key pair.
3. In BUtil:
   - **Provider**: Huawei Cloud OBS (CN)
   - **Service URL**: `https://obs.<region>.myhuaweicloud.com`
   - **Region**: e.g. `cn-north-4`
   - **Access Key / Secret Key**: from step 2
   - **Bucket**: your bucket name

---

### Baidu Cloud BOS

Service URL pattern: `https://s3.<region>.bcebos.com`
Common regions: `bj` (Beijing), `gz` (Guangzhou), `su` (Suzhou)
Example: `https://s3.bj.bcebos.com`

1. Log into [Baidu Cloud Console](https://cloud.baidu.com/) and create a **BOS bucket**.
2. Go to **Security → Access Key** and create an access key pair.
3. In BUtil:
   - **Provider**: Baidu Cloud BOS (CN)
   - **Service URL**: `https://s3.<region>.bcebos.com`
   - **Region**: e.g. `bj`
   - **Access Key / Secret Key**: from step 2
   - **Bucket**: your bucket name

---

### Qiniu Cloud Kodo

Service URL pattern: `https://s3-<region>.qiniucs.com`
Common regions: `cn-east-1` (Shanghai), `cn-north-1` (Beijing), `cn-south-1` (Guangdong)
Example: `https://s3-cn-east-1.qiniucs.com`

1. Log into [Qiniu Cloud Console](https://portal.qiniu.com/) and create a **Kodo bucket** (set *Private*).
2. Go to **Keys** in the personal center and copy your **AK** (Access Key) and **SK** (Secret Key).
3. In BUtil:
   - **Provider**: Qiniu Cloud Kodo (CN)
   - **Service URL**: `https://s3-<region>.qiniucs.com`
   - **Region**: e.g. `cn-east-1`
   - **Access Key / Secret Key**: AK and SK from step 2
   - **Bucket**: your bucket name

---

### Volcano Engine TOS — ByteDance

Service URL pattern: `https://tos-s3-<region>.volces.com`
Common regions: `cn-beijing`, `cn-shanghai`, `cn-guangzhou`
Example: `https://tos-s3-cn-beijing.volces.com`

1. Log into [Volcano Engine Console](https://www.volcengine.com/) and open **TOS (Tinder Object Storage)**. Create a bucket.
2. Go to **IAM → Access Key** and create a new access key pair.
3. In BUtil:
   - **Provider**: Volcano Engine TOS (CN)
   - **Service URL**: `https://tos-s3-<region>.volces.com`
   - **Region**: e.g. `cn-beijing`
   - **Access Key / Secret Key**: from step 2
   - **Bucket**: your bucket name

---

### MinIO (Self-hosted) or Other S3-Compatible

1. Create a bucket and an access key in your MinIO console (or equivalent).
2. In BUtil:
   - **Provider**: MinIO / Custom
   - **Service URL**: your MinIO endpoint, e.g. `https://minio.example.com`
   - **Region**: `us-east-1` (MinIO accepts any value; some tools default to this)
   - **Access Key / Secret Key**: your MinIO credentials

---

## Troubleshooting

- **Access Denied** — the key lacks permissions on the bucket, or the bucket name is wrong.
- **InvalidAccessKeyId / SignatureDoesNotMatch** — wrong access key or secret key.
- **NoSuchBucket** — the bucket name is misspelled or the bucket doesn't exist yet.
- **Connection refused / timeout** — the Service URL is wrong or the server is unreachable. Verify you can reach it from a browser.
- **Region mismatch** (AWS only) — the bucket region and the Region field must match.
