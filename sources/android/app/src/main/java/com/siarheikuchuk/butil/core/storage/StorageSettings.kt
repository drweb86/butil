package com.siarheikuchuk.butil.core.storage

/**
 * Storage settings stored with the same property names and `$type` values as the desktop app.
 * Secrets are replaced with `enc::` values before the task file is written.
 */
sealed interface StorageSettings {
    val jsonType: String
    var quotaGb: Long
    var mountScript: String?
    var unmountScript: String?

    fun secretFields(): Map<String, String>
    fun withSecrets(updated: Map<String, String>): StorageSettings
}

data class FolderSettings(
    var destinationFolder: String = "",
    override var quotaGb: Long = 0,
    override var mountScript: String? = null,
    override var unmountScript: String? = null,
) : StorageSettings {
    override val jsonType: String = "Folder"
    override fun secretFields(): Map<String, String> = emptyMap()
    override fun withSecrets(updated: Map<String, String>) = this
}

data class FtpsSettings(
    var host: String = "",
    var encryption: String = "Explicit",
    var port: Int = 0,
    var folder: String? = null,
    var user: String = "",
    var password: String = "",
    var trustedCertificate: String = "",
    override var quotaGb: Long = 0,
    override var mountScript: String? = null,
    override var unmountScript: String? = null,
) : StorageSettings {
    override val jsonType: String = "Ftps"
    override fun secretFields() = mapOf("Password" to password)
    override fun withSecrets(updated: Map<String, String>) = copy(password = updated["Password"] ?: password)
}

data class SftpSettings(
    var host: String = "",
    var port: Int = 22,
    var folder: String = "",
    var user: String = "",
    var password: String? = null,
    var keyFile: String? = null,
    var fingerPrintSha256: String = "",
    override var quotaGb: Long = 0,
    override var mountScript: String? = null,
    override var unmountScript: String? = null,
) : StorageSettings {
    override val jsonType: String = "Sftp"
    override fun secretFields() = mapOf("Password" to (password ?: ""))
    override fun withSecrets(updated: Map<String, String>) = copy(password = updated["Password"] ?: password)
}

data class WebDavSettings(
    var preset: String = "Custom",
    var host: String = "",
    var port: Int = 0,
    var useHttps: Boolean = true,
    var basePath: String = "",
    var user: String = "",
    var password: String = "",
    override var quotaGb: Long = 0,
    override var mountScript: String? = null,
    override var unmountScript: String? = null,
) : StorageSettings {
    override val jsonType: String = "WebDav"
    override fun secretFields() = mapOf("Password" to password)
    override fun withSecrets(updated: Map<String, String>) = copy(password = updated["Password"] ?: password)
}

data class S3Settings(
    var provider: String = "Custom",
    var serviceUrl: String = "",
    var region: String = "",
    var accessKey: String = "",
    var secretKey: String = "",
    var bucketName: String = "",
    var pathPrefix: String = "",
    override var quotaGb: Long = 0,
    override var mountScript: String? = null,
    override var unmountScript: String? = null,
) : StorageSettings {
    override val jsonType: String = "S3"
    override fun secretFields() = mapOf("SecretKey" to secretKey)
    override fun withSecrets(updated: Map<String, String>) = copy(secretKey = updated["SecretKey"] ?: secretKey)
}

data class AzureBlobSettings(
    var accountName: String = "",
    var accountKey: String = "",
    var containerName: String = "",
    var pathPrefix: String = "",
    override var quotaGb: Long = 0,
    override var mountScript: String? = null,
    override var unmountScript: String? = null,
) : StorageSettings {
    override val jsonType: String = "AzureBlob"
    override fun secretFields() = mapOf("AccountKey" to accountKey)
    override fun withSecrets(updated: Map<String, String>) = copy(accountKey = updated["AccountKey"] ?: accountKey)
}

data class SambaSettings(
    var url: String = "",
    var user: String? = null,
    var password: String? = null,
    override var quotaGb: Long = 0,
    override var mountScript: String? = null,
    override var unmountScript: String? = null,
) : StorageSettings {
    override val jsonType: String = "Samba"
    override fun secretFields() = mapOf("Password" to (password ?: ""))
    override fun withSecrets(updated: Map<String, String>) = copy(password = updated["Password"] ?: password)
}

data class NfsSettings(
    var host: String = "",
    var sharePath: String = "",
    var mountPoint: String = "",
    var mountOptions: String? = null,
    override var quotaGb: Long = 0,
    override var mountScript: String? = null,
    override var unmountScript: String? = null,
) : StorageSettings {
    override val jsonType: String = "Nfs"
    override fun secretFields(): Map<String, String> = emptyMap()
    override fun withSecrets(updated: Map<String, String>) = this
}

fun blankStorage(jsonType: String): StorageSettings = when (jsonType) {
    "Folder" -> FolderSettings()
    "Ftps" -> FtpsSettings()
    "Sftp" -> SftpSettings()
    "WebDav" -> WebDavSettings()
    "S3" -> S3Settings()
    "AzureBlob" -> AzureBlobSettings()
    "Samba" -> SambaSettings()
    "Nfs" -> NfsSettings()
    else -> throw IllegalArgumentException("Unknown storage type '$jsonType'.")
}
