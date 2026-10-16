package com.siarheikuchuk.butil.core.ftps

data class FtpsUserAccount(
    val login: String,
    val password: String,
    val folder: String,
    val canRead: Boolean = true,
    val canWrite: Boolean = true,
)

data class FtpsServerSettings(
    val ip: String = "0.0.0.0",
    val port: Int = 2121,
    val maxConnections: Int = 10,
    val certificatePath: String? = null,
    val certificatePassword: String? = null,
) {
    companion object {
        const val MAX_CONNECTIONS_UPPER_BOUND = 100

        fun effectiveMaxConnections(configured: Int): Int {
            if (configured < 1) return 1
            if (configured > MAX_CONNECTIONS_UPPER_BOUND) return MAX_CONNECTIONS_UPPER_BOUND
            return configured
        }
    }
}

data class FtpsServerConfig(
    val settings: FtpsServerSettings,
    val users: List<FtpsUserAccount>,
)

data class FileSystemEntry(
    val fileName: String,
    val lastWriteTimeMillis: Long,
    val length: Long,
    val isDirectory: Boolean,
)

interface FtpsLog {
    fun debug(message: String)
    fun info(message: String)
    fun warn(message: String)
    fun error(message: String, error: Throwable? = null)
}

interface FileSystemProvider {
    fun createDirectory(userFolder: String, parts: List<String>)
    fun directoryExists(userFolder: String, parts: List<String>): Boolean
    fun fileExists(userFolder: String, parts: List<String>): Boolean
    fun directoryDelete(userFolder: String, parts: List<String>)
    fun fileDelete(userFolder: String, parts: List<String>)
    fun directoryMove(userFolder: String, fromParts: List<String>, toParts: List<String>)
    fun fileMove(userFolder: String, fromParts: List<String>, toParts: List<String>)
    fun fileCreate(userFolder: String, parts: List<String>): java.io.OutputStream
    fun fileOpenRead(userFolder: String, parts: List<String>): java.io.InputStream
    fun getFileLastWriteTimeUtcMillis(userFolder: String, parts: List<String>): Long
    fun getFileLength(userFolder: String, parts: List<String>): Long
    fun directoryEntries(userFolder: String, parts: List<String>): List<FileSystemEntry>
}

enum class DataProtection { Clear, Protected }
