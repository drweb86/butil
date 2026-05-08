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
