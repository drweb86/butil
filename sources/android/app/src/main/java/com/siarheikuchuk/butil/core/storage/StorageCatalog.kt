package com.siarheikuchuk.butil.core.storage

enum class StorageFieldKind { TEXT, PASSWORD, INTEGER, ENUM, FOLDER, FILE }

data class StorageChoice(val value: String, val label: String, val help: String = "")

data class StoragePatch(
    val target: String,
    val label: String? = null,
    val hidden: Boolean = false,
    val placeholder: String? = null,
    val value: String? = null,
)

data class StorageRule(val whenValue: String, val patches: List<StoragePatch>)

data class StorageFieldSpec(
    val key: String,
    val label: String,
    val kind: StorageFieldKind,
    val placeholder: String = "",
    val optional: Boolean = false,
    val defaultValue: String = "",
    val min: Long? = null,
    val max: Long? = null,
    val choices: List<StorageChoice> = emptyList(),
    val rules: List<StorageRule> = emptyList(),
)

data class StorageProviderSpec(
    val jsonType: String,
    val displayName: String,
    val fields: List<StorageFieldSpec>,
)

object StorageCatalog {
    val providers: List<StorageProviderSpec> = listOf(
        provider("Folder", "Directory", listOf(folderField())),
        provider("Sftp", "SFTP", sftpFields()),
        provider("Ftps", "FTPS", ftpsFields()),
        provider("WebDav", "WebDAV", webDavFields()),
        provider("S3", "S3", s3Fields()),
        provider("AzureBlob", "Azure Blob", azureFields()),
        provider("Samba", "SMB/CIFS", sambaFields()),
        provider("Nfs", "NFS", nfsFields()),
    )

    fun find(jsonType: String): StorageProviderSpec = providers.first { it.jsonType == jsonType }

    fun fieldValues(settings: StorageSettings): Map<String, String> = when (settings) {
        is FolderSettings -> mapOf("folder" to settings.destinationFolder)
        is FtpsSettings -> mapOf(
            "host" to settings.host,
            "encryption" to settings.encryption,
            "port" to settings.port.toString(),
            "user" to settings.user,
            "password" to settings.password,
            "folder" to settings.folder.orEmpty(),
            "certificate" to settings.trustedCertificate,
        )
        is SftpSettings -> mapOf(
            "host" to settings.host,
            "port" to settings.port.toString(),
            "user" to settings.user,
            "password" to settings.password.orEmpty(),
            "keyfile" to settings.keyFile.orEmpty(),
            "fingerprint" to settings.fingerPrintSha256,
            "folder" to settings.folder,
        )
        is WebDavSettings -> mapOf(
            "preset" to settings.preset,
            "host" to settings.host,
            "port" to settings.port.toString(),
            "basePath" to settings.basePath,
            "user" to settings.user,
            "password" to settings.password,
        )
        is S3Settings -> mapOf(
            "provider" to settings.provider,
            "serviceUrl" to settings.serviceUrl,
            "region" to settings.region,
            "accessKey" to settings.accessKey,
            "secretKey" to settings.secretKey,
            "bucket" to settings.bucketName,
            "pathPrefix" to settings.pathPrefix,
        )
        is AzureBlobSettings -> mapOf(
            "accountName" to settings.accountName,
            "accountKey" to settings.accountKey,
            "container" to settings.containerName,
            "pathPrefix" to settings.pathPrefix,
        )
        is SambaSettings -> mapOf(
            "url" to settings.url,
            "user" to settings.user.orEmpty(),
            "password" to settings.password.orEmpty(),
        )
        is NfsSettings -> mapOf(
            "host" to settings.host,
            "sharePath" to settings.sharePath,
            "mountPoint" to settings.mountPoint,
            "mountOptions" to settings.mountOptions.orEmpty(),
        )
    }

    fun toSettings(jsonType: String, values: Map<String, String>, quotaGb: Long, previous: StorageSettings?): StorageSettings {
        val settings = when (jsonType) {
            "Folder" -> FolderSettings(destinationFolder = values["folder"].orEmpty())
            "Ftps" -> FtpsSettings(
                host = values["host"].orEmpty(),
                encryption = if (values["encryption"] == "Implicit") "Implicit" else "Explicit",
                port = values["port"]?.toIntOrNull() ?: 0,
                user = values["user"].orEmpty(),
                password = values["password"].orEmpty(),
                folder = values["folder"],
                trustedCertificate = values["certificate"].orEmpty(),
            )
            "Sftp" -> SftpSettings(
                host = values["host"].orEmpty(),
                port = values["port"]?.toIntOrNull() ?: 22,
                user = values["user"].orEmpty(),
                password = values["password"],
                keyFile = values["keyfile"],
                fingerPrintSha256 = values["fingerprint"].orEmpty(),
                folder = values["folder"].orEmpty(),
            )
            "WebDav" -> webDav(values)
            "S3" -> s3(values)
            "AzureBlob" -> AzureBlobSettings(
                accountName = values["accountName"].orEmpty(),
                accountKey = values["accountKey"].orEmpty(),
                containerName = values["container"].orEmpty(),
                pathPrefix = values["pathPrefix"].orEmpty(),
            )
            "Samba" -> SambaSettings(
                url = values["url"].orEmpty(),
                user = values["user"],
                password = values["password"],
            )
            "Nfs" -> NfsSettings(
                host = values["host"].orEmpty(),
                sharePath = values["sharePath"].orEmpty(),
                mountPoint = values["mountPoint"].orEmpty(),
                mountOptions = values["mountOptions"],
            )
            else -> throw IllegalArgumentException("Unknown storage type '$jsonType'.")
        }
        settings.quotaGb = quotaGb
        settings.mountScript = previous?.takeIf { it.jsonType == jsonType }?.mountScript
        settings.unmountScript = previous?.takeIf { it.jsonType == jsonType }?.unmountScript
        return settings
    }

    private fun webDav(values: Map<String, String>): WebDavSettings {
        val draft = WebDavSettings(
            preset = values["preset"] ?: "Custom",
            host = values["host"].orEmpty(),
            useHttps = true,
            port = values["port"]?.toIntOrNull() ?: 0,
            basePath = values["basePath"].orEmpty(),
            user = values["user"].orEmpty(),
            password = values["password"].orEmpty(),
        )
        return WebDavStorage.resolve(draft)
    }

    private fun s3(values: Map<String, String>): S3Settings {
        val provider = values["provider"] ?: "Custom"
        var serviceUrl = values["serviceUrl"].orEmpty()
        if (serviceUrl.isBlank()) {
            serviceUrl = when (provider) {
                "Wasabi" -> "https://s3.wasabisys.com"
                "GoogleCloudStorage" -> "https://storage.googleapis.com"
                "YandexObjectStorage" -> "https://storage.yandexcloud.net"
                "VKCloudStorage" -> "https://hb.ru-msk.vkcloud-storage.ru"
                "CloudRuStorage" -> "https://obs.ru-moscow-1.hc.sbercloud.ru"
                "SelectelStorage" -> "https://s3.storage.selcloud.ru"
                else -> ""
            }
        }
        var region = values["region"].orEmpty()
        if (region.isBlank() && provider == "YandexObjectStorage") region = "ru-central1"
        return S3Settings(
            provider = provider,
            serviceUrl = serviceUrl,
            region = region,
            accessKey = values["accessKey"].orEmpty(),
            secretKey = values["secretKey"].orEmpty(),
            bucketName = values["bucket"].orEmpty(),
            pathPrefix = values["pathPrefix"].orEmpty(),
        )
    }

    private fun provider(type: String, name: String, fields: List<StorageFieldSpec>) =
        StorageProviderSpec(type, name, fields)

    private fun folderField() = StorageFieldSpec("folder", "Folder:", StorageFieldKind.FOLDER, "Folder is not specified.")

    private fun sftpFields() = listOf(
        StorageFieldSpec("host", "Server name:", StorageFieldKind.TEXT, "192.168.11.1"),
        StorageFieldSpec("port", "Port:", StorageFieldKind.INTEGER, defaultValue = "22", min = 0, max = 65535),
        StorageFieldSpec("user", "User:", StorageFieldKind.TEXT),
        StorageFieldSpec("password", "Password:", StorageFieldKind.PASSWORD, optional = true),
        StorageFieldSpec("keyfile", "Key file:", StorageFieldKind.FILE, optional = true),
        StorageFieldSpec("fingerprint", "Fingerprint SHA-256:", StorageFieldKind.TEXT, optional = true),
        StorageFieldSpec("folder", "Folder:", StorageFieldKind.TEXT, "/home/sftpuser/ftpserver"),
    )

    private fun ftpsFields() = listOf(
        StorageFieldSpec("host", "Server name:", StorageFieldKind.TEXT, "192.168.11.1"),
        StorageFieldSpec(
            "encryption",
            "SSL/TTS encryption:",
            StorageFieldKind.ENUM,
            defaultValue = "Explicit",
            choices = listOf(StorageChoice("Explicit", "Explicit"), StorageChoice("Implicit", "Implicit")),
        ),
        StorageFieldSpec("port", "Port:", StorageFieldKind.INTEGER, defaultValue = "0", min = 0, max = 65535),
        StorageFieldSpec("user", "User:", StorageFieldKind.TEXT),
        StorageFieldSpec("password", "Password:", StorageFieldKind.PASSWORD),
        StorageFieldSpec("folder", "Folder:", StorageFieldKind.TEXT, optional = true),
        StorageFieldSpec("certificate", "Trusted certificate (hex):", StorageFieldKind.TEXT, optional = true),
    )

    private fun azureFields() = listOf(
        StorageFieldSpec("accountName", "Account Name:", StorageFieldKind.TEXT, "mystorageaccount"),
        StorageFieldSpec("accountKey", "Account Key:", StorageFieldKind.PASSWORD),
        StorageFieldSpec("container", "Container:", StorageFieldKind.TEXT, "backups"),
        StorageFieldSpec("pathPrefix", "Path Prefix:", StorageFieldKind.TEXT, "my-pc", optional = true),
    )

    private fun sambaFields() = listOf(
        StorageFieldSpec("url", "Url:", StorageFieldKind.TEXT, "\\\\192.168.11.1\\share\\folder"),
        StorageFieldSpec("user", "User:", StorageFieldKind.TEXT, optional = true),
        StorageFieldSpec("password", "Password:", StorageFieldKind.PASSWORD, optional = true),
    )

    private fun nfsFields() = listOf(
        StorageFieldSpec("host", "Server name:", StorageFieldKind.TEXT, "192.168.1.100"),
        StorageFieldSpec("sharePath", "Share Path:", StorageFieldKind.TEXT, "/export/backups"),
        StorageFieldSpec("mountPoint", "Mount Point:", StorageFieldKind.TEXT, "/mnt/nfs-backup"),
        StorageFieldSpec(
            "mountOptions",
            "Mount Options:",
            StorageFieldKind.TEXT,
            "vers=3,uid=1000,gid=1000",
            optional = true,
        ),
    )

    private fun webDavFields(): List<StorageFieldSpec> {
        fun rule(value: String, vararg patches: StoragePatch) = StorageRule(value, patches.toList())
        val hideServer = listOf(
            StoragePatch("host", hidden = true),
            StoragePatch("port", hidden = true),
            StoragePatch("basePath", hidden = true),
        )
        return listOf(
            StorageFieldSpec(
                "preset",
                "Preset:",
                StorageFieldKind.ENUM,
                defaultValue = "Custom",
                choices = listOf(
                    StorageChoice("Custom", "Generic / Custom", "Generic WebDAV server. Enter the address, port, and base path. The connection is HTTPS."),
                    StorageChoice("Nextcloud", "Nextcloud", "Nextcloud. Base path looks like /remote.php/dav/files/<username>/."),
                    StorageChoice("ownCloud", "ownCloud", "ownCloud. Base path looks like /remote.php/dav/files/<username>/."),
                    StorageChoice("Koofr", "Koofr (EU)", "Koofr. Server details are filled in automatically. Use an app password."),
                    StorageChoice("pCloud", "pCloud (EU/US)", "pCloud EU (ewebdav.pcloud.com) is filled in automatically. Use an app password."),
                    StorageChoice("IONOSHiDrive", "IONOS HiDrive (DE)", "IONOS HiDrive. Server details are filled in automatically."),
                    StorageChoice("Box", "Box (US)", "Box. Server details are filled in automatically."),
                    StorageChoice("YandexDisk", "Yandex Disk (RU)", "Yandex Disk. Password is the WebDAV app password sent as an OAuth token."),
                    StorageChoice("MailRuCloud", "Mail.ru Cloud (RU)", "Mail.ru Cloud. Server details are filled in automatically. Use an app password."),
                    StorageChoice("Jianguoyun", "Jianguoyun / Nutstore (CN)", "Jianguoyun. Server details are filled in automatically. Use an app password."),
                ),
                rules = listOf(
                    rule(
                        "Nextcloud",
                        StoragePatch("host", placeholder = "nextcloud.example.com"),
                        StoragePatch("basePath", placeholder = "/remote.php/dav/files/<username>/"),
                        StoragePatch("user", placeholder = "your-nextcloud-username"),
                        StoragePatch("password", label = "Password / App Password"),
                    ),
                    rule(
                        "ownCloud",
                        StoragePatch("host", placeholder = "owncloud.example.com"),
                        StoragePatch("basePath", placeholder = "/remote.php/dav/files/<username>/"),
                        StoragePatch("user", placeholder = "your-owncloud-username"),
                        StoragePatch("password", label = "Password / App Password"),
                    ),
                    rule("Koofr", *hideServer.toTypedArray(), StoragePatch("user", placeholder = "your@email.com"), StoragePatch("password", label = "App Password")),
                    rule("pCloud", *hideServer.toTypedArray(), StoragePatch("user", placeholder = "your@email.com"), StoragePatch("password", label = "App Password")),
                    rule("IONOSHiDrive", *hideServer.toTypedArray(), StoragePatch("user", placeholder = "your-hidrive-username")),
                    rule("Box", *hideServer.toTypedArray(), StoragePatch("user", placeholder = "your@email.com")),
                    rule("YandexDisk", *hideServer.toTypedArray(), StoragePatch("user", placeholder = "your-yandex-login"), StoragePatch("password", label = "App Password")),
                    rule("MailRuCloud", *hideServer.toTypedArray(), StoragePatch("user", placeholder = "your@mail.ru"), StoragePatch("password", label = "App Password")),
                    rule("Jianguoyun", *hideServer.toTypedArray(), StoragePatch("user", placeholder = "your@email.com"), StoragePatch("password", label = "App Password")),
                ),
            ),
            StorageFieldSpec("host", "Server name:", StorageFieldKind.TEXT, "nextcloud.example.com", optional = true),
            StorageFieldSpec("port", "Port:", StorageFieldKind.INTEGER, defaultValue = "0", min = 0, max = 65535),
            StorageFieldSpec("basePath", "Base Path:", StorageFieldKind.TEXT, "/remote.php/dav/files/user/Backups", optional = true),
            StorageFieldSpec("user", "User:", StorageFieldKind.TEXT, optional = true),
            StorageFieldSpec("password", "Password / OAuth Token:", StorageFieldKind.PASSWORD),
        )
    }

    private fun s3Fields(): List<StorageFieldSpec> {
        fun rule(value: String, vararg patches: StoragePatch) = StorageRule(value, patches.toList())
        fun common(service: String? = null, region: String? = null, access: String, secret: String, bucket: String, hideService: Boolean = false, hideRegion: Boolean = false, regionValue: String? = null) =
            listOf(
                StoragePatch("serviceUrl", hidden = hideService, placeholder = service),
                StoragePatch("region", hidden = hideRegion, placeholder = region, value = regionValue),
                StoragePatch("accessKey", label = access),
                StoragePatch("secretKey", label = secret),
                StoragePatch("bucket", placeholder = bucket),
            )
        return listOf(
            StorageFieldSpec(
                "provider",
                "Provider:",
                StorageFieldKind.ENUM,
                defaultValue = "Custom",
                choices = listOf(
                    StorageChoice("AWSS3", "AWS S3", "AWS S3. The endpoint is chosen from the region."),
                    StorageChoice("BackblazeB2", "Backblaze B2", "Backblaze B2. Key ID and application key."),
                    StorageChoice("Wasabi", "Wasabi", "Wasabi. The service URL is filled in from the region."),
                    StorageChoice("CloudflareR2", "Cloudflare R2", "Cloudflare R2. Region is auto. Service URL contains the account id."),
                    StorageChoice("DigitalOceanSpaces", "DigitalOcean Spaces", "DigitalOcean Spaces."),
                    StorageChoice("LinodeObjectStorage", "Linode / Akamai Object Storage", "Linode / Akamai Object Storage."),
                    StorageChoice("ScalewayObjectStorage", "Scaleway Object Storage", "Scaleway Object Storage."),
                    StorageChoice("GoogleCloudStorage", "Google Cloud Storage", "Google Cloud Storage HMAC keys. Endpoint and region are fixed."),
                    StorageChoice("YandexObjectStorage", "Yandex Object Storage (RU)", "Yandex Object Storage. Endpoint and region are filled in."),
                    StorageChoice("VKCloudStorage", "VK Cloud Object Storage (RU)", "VK Cloud. Endpoint is filled in."),
                    StorageChoice("CloudRuStorage", "Cloud.ru Object Storage (RU)", "Cloud.ru. Endpoint is filled in."),
                    StorageChoice("SelectelStorage", "Selectel Object Storage (RU)", "Selectel. Endpoint is filled in."),
                    StorageChoice("AlibabaCloudOSS", "Alibaba Cloud OSS (CN)", "Alibaba Cloud OSS uses virtual-hosted addresses."),
                    StorageChoice("TencentCloudCOS", "Tencent Cloud COS (CN)", "Tencent Cloud COS. Bucket name includes the AppId."),
                    StorageChoice("HuaweiCloudOBS", "Huawei Cloud OBS (CN)", "Huawei Cloud OBS."),
                    StorageChoice("BaiduCloudBOS", "Baidu Cloud BOS (CN)", "Baidu Cloud BOS."),
                    StorageChoice("QiniuCloudKodo", "Qiniu Cloud Kodo (CN)", "Qiniu Cloud Kodo."),
                    StorageChoice("VolcanoEngineTOS", "Volcano Engine TOS (CN)", "Volcano Engine TOS."),
                    StorageChoice("Custom", "MinIO / Custom", "MinIO or any other S3-compatible endpoint."),
                ),
                rules = listOf(
                    rule("AWSS3", *common(hideService = true, region = "us-east-1", access = "Access Key ID", secret = "Secret Access Key", bucket = "my-backup-bucket").toTypedArray()),
                    rule("BackblazeB2", *common(service = "https://s3.us-west-004.backblazeb2.com", region = "us-west-004", access = "Key ID", secret = "Application Key", bucket = "my-backup-bucket").toTypedArray()),
                    rule("Wasabi", *common(hideService = true, region = "us-east-1", access = "Access Key", secret = "Secret Key", bucket = "my-backup-bucket").toTypedArray()),
                    rule("CloudflareR2", *common(service = "https://<ACCOUNT_ID>.r2.cloudflarestorage.com", access = "Access Key ID", secret = "Secret Access Key", bucket = "my-backup-bucket", regionValue = "auto").toTypedArray()),
                    rule("DigitalOceanSpaces", *common(service = "https://nyc3.digitaloceanspaces.com", region = "nyc3", access = "Access Key", secret = "Secret Key", bucket = "my-backup-bucket").toTypedArray()),
                    rule("LinodeObjectStorage", *common(service = "https://us-east-1.linodeobjects.com", region = "us-east-1", access = "Access Key", secret = "Secret Key", bucket = "my-backup-bucket").toTypedArray()),
                    rule("ScalewayObjectStorage", *common(service = "https://s3.fr-par.scw.cloud", region = "fr-par", access = "Access Key ID", secret = "Secret Access Key", bucket = "my-backup-bucket").toTypedArray()),
                    rule("GoogleCloudStorage", *common(hideService = true, hideRegion = true, access = "HMAC Access Key", secret = "HMAC Secret", bucket = "my-backup-bucket").toTypedArray()),
                    rule("YandexObjectStorage", *common(hideService = true, hideRegion = true, access = "Key ID", secret = "Secret Key", bucket = "my-backup-bucket").toTypedArray()),
                    rule("VKCloudStorage", *common(hideService = true, region = "ru-msk", access = "Access Key", secret = "Secret Key", bucket = "my-backup-bucket").toTypedArray()),
                    rule("CloudRuStorage", *common(hideService = true, region = "ru-moscow-1", access = "Access Key", secret = "Secret Access Key", bucket = "my-backup-bucket").toTypedArray()),
                    rule("SelectelStorage", *common(hideService = true, region = "ru-1", access = "Access Key", secret = "Secret Key", bucket = "my-backup-bucket").toTypedArray()),
                    rule("AlibabaCloudOSS", *common(service = "https://oss-cn-hangzhou.aliyuncs.com", region = "oss-cn-hangzhou", access = "AccessKey ID", secret = "AccessKey Secret", bucket = "my-backup-bucket").toTypedArray()),
                    rule("TencentCloudCOS", *common(service = "https://cos.ap-beijing.myqcloud.com", region = "ap-beijing", access = "SecretId", secret = "SecretKey", bucket = "mybucket-1250000000").toTypedArray()),
                    rule("HuaweiCloudOBS", *common(service = "https://obs.cn-north-4.myhuaweicloud.com", region = "cn-north-4", access = "AK (Access Key)", secret = "SK (Secret Key)", bucket = "my-backup-bucket").toTypedArray()),
                    rule("BaiduCloudBOS", *common(service = "https://s3.bj.bcebos.com", region = "bj", access = "Access Key ID", secret = "Secret Access Key", bucket = "my-backup-bucket").toTypedArray()),
                    rule("QiniuCloudKodo", *common(service = "https://s3-cn-east-1.qiniucs.com", region = "cn-east-1", access = "AccessKey", secret = "SecretKey", bucket = "my-backup-bucket").toTypedArray()),
                    rule("VolcanoEngineTOS", *common(service = "https://tos-s3-cn-beijing.volces.com", region = "cn-beijing", access = "Access Key ID", secret = "Secret Access Key", bucket = "my-backup-bucket").toTypedArray()),
                    rule("Custom", *common(service = "http://192.168.1.10:9000", region = "us-east-1", access = "Access Key", secret = "Secret Key", bucket = "my-backup-bucket").toTypedArray()),
                ),
            ),
            StorageFieldSpec("serviceUrl", "Service URL:", StorageFieldKind.TEXT, "https://s3.example.com", optional = true),
            StorageFieldSpec("region", "Region:", StorageFieldKind.TEXT, "us-east-1", optional = true),
            StorageFieldSpec("bucket", "Bucket:", StorageFieldKind.TEXT),
            StorageFieldSpec("accessKey", "Access Key:", StorageFieldKind.TEXT),
            StorageFieldSpec("secretKey", "Secret Key:", StorageFieldKind.PASSWORD),
            StorageFieldSpec("pathPrefix", "Path Prefix:", StorageFieldKind.TEXT, "backups/my-pc", optional = true),
        )
    }
}
