package com.siarheikuchuk.butil.core.storage

import org.apache.commons.net.ftp.FTP
import org.apache.commons.net.ftp.FTPSClient
import java.io.InputStream
import java.security.cert.CertificateException
import java.security.cert.X509Certificate
import javax.net.ssl.TrustManagerFactory
import javax.net.ssl.X509TrustManager

internal class FtpsStorage(
    private val settings: FtpsSettings,
    private val autodetect: Boolean,
) : Storage {
    private val client: FTPSClient = connect()

    override fun test(): String? {
        val folder = settings.folder?.trim().orEmpty()
        if (folder.isNotEmpty()) {
            val current = client.printWorkingDirectory()
            val exists = client.changeWorkingDirectory(folder)
            if (!current.isNullOrBlank()) client.changeWorkingDirectory(current)
            if (!exists) return "Folder does not exist."
        }
        return null
    }

    override fun exists(relativeFileName: String): Boolean {
        val path = remote(relativeFileName)
        val listed = client.mlistFile(path)
        if (listed != null) return listed.isFile
        return client.listNames(path)?.isNotEmpty() == true
    }

    override fun upload(relativeFileName: String, length: Long, open: () -> InputStream) {
        val path = remote(relativeFileName)
        val parent = path.substringBeforeLast('/', "")
        if (parent.isNotEmpty()) makeDirectories(parent)
        open().use { input ->
            if (!client.storeFile(path, input)) {
                throw IllegalStateException("Failed to upload $path. ${client.replyString}")
            }
        }
    }

    override fun deleteFolder(relativeFolderName: String) {
        deleteRecursive(remote(relativeFolderName))
    }

    override fun download(relativeFileName: String): InputStream {
        val stream = client.retrieveFileStream(remote(relativeFileName))
            ?: throw IllegalStateException("Failed to download $relativeFileName. ${client.replyString}")
        return object : InputStream() {
            override fun read(): Int = stream.read()
            override fun read(b: ByteArray, off: Int, len: Int): Int = stream.read(b, off, len)
            override fun close() {
                stream.close()
                client.completePendingCommand()
            }
        }
    }

    override fun delete(relativeFileName: String) {
        client.deleteFile(remote(relativeFileName))
    }

    override fun listFiles(): List<RemoteFile> {
        val files = mutableListOf<RemoteFile>()
        walk(settings.folder?.trim()?.trim('/').orEmpty(), "", files)
        return files.sortedBy { it.relativePath }
    }

    override fun listFolders(): List<String> {
        val root = settings.folder?.trim()?.trim('/').orEmpty()
        return client.listFiles(root.ifEmpty { "." })
            ?.filter { it.isDirectory && it.name != "." && it.name != ".." }
            ?.map { it.name }
            ?.sorted()
            .orEmpty()
    }

    private fun walk(absolute: String, relative: String, into: MutableList<RemoteFile>) {
        val listed = client.listFiles(absolute.ifEmpty { "." }) ?: return
        for (child in listed) {
            if (child.name == "." || child.name == "..") continue
            val childRelative = if (relative.isEmpty()) child.name else "$relative/${child.name}"
            val childAbsolute = if (absolute.isEmpty()) child.name else "$absolute/${child.name}"
            if (child.isDirectory) {
                walk(childAbsolute, childRelative, into)
            } else if (child.isFile) {
                val modified = child.timestamp?.timeInMillis ?: 0L
                into += RemoteFile(childRelative, child.size, modified)
            }
        }
    }

    override fun close() {
        try {
            if (client.isConnected) client.logout()
        } catch (_: Exception) {
        }
        try {
            if (client.isConnected) client.disconnect()
        } catch (_: Exception) {
        }
    }

    private fun connect(): FTPSClient {
        if (settings.host.isBlank()) throw IllegalArgumentException("Server name is not be specified.")
        if (settings.port < 0 || settings.port > 65535) throw IllegalArgumentException("Port is invalid.")
        if (settings.user.isBlank()) throw IllegalArgumentException("User is not specified.")
        if (settings.password.isBlank()) throw IllegalArgumentException("Password is not specified.")
        val implicit = settings.encryption == "Implicit"
        val ftp = FTPSClient("TLS", implicit)
        ftp.connectTimeout = 30_000
        ftp.defaultTimeout = 120_000
        ftp.trustManager = PinningTrustManager(settings, autodetect)
        val port = when {
            settings.port > 0 -> settings.port
            implicit -> 990
            else -> 21
        }
        ftp.connect(settings.host, port)
        if (!ftp.login(settings.user, settings.password)) {
            throw IllegalStateException("FTPS login failed. ${ftp.replyString}")
        }
        ftp.execPBSZ(0)
        ftp.execPROT("P")
        ftp.enterLocalPassiveMode()
        ftp.setFileType(FTP.BINARY_FILE_TYPE)
        return ftp
    }

    private fun remote(relative: String): String {
        val child = RemotePaths.normalizeRelative(relative)
        val folder = settings.folder?.trim()?.trim('/') ?: ""
        return if (folder.isEmpty()) child else "$folder/$child"
    }

    private fun makeDirectories(directory: String) {
        val absolute = directory.startsWith("/")
        val parts = directory.trim('/').split('/').filter { it.isNotEmpty() }
        var current = ""
        for (part in parts) {
            current = when {
                current.isEmpty() && absolute -> "/$part"
                current.isEmpty() -> part
                else -> "$current/$part"
            }
            client.makeDirectory(current)
        }
    }

    private fun deleteRecursive(path: String) {
        val children = client.listFiles(path) ?: return
        for (child in children) {
            if (child.name == "." || child.name == "..") continue
            val childPath = "$path/${child.name}"
            if (child.isDirectory) deleteRecursive(childPath) else client.deleteFile(childPath)
        }
        client.removeDirectory(path)
    }
}

private class PinningTrustManager(
    private val settings: FtpsSettings,
    private val autodetect: Boolean,
) : X509TrustManager {
    private val system = systemTrust()

    override fun checkClientTrusted(chain: Array<out X509Certificate>?, authType: String?) = Unit

    override fun checkServerTrusted(chain: Array<out X509Certificate>?, authType: String?) {
        val cert = chain?.firstOrNull() ?: throw CertificateException("FTPS server presented no certificate.")
        val hex = cert.encoded.joinToString("") { "%02X".format(it) }
        if (trustedBySystem(chain, authType)) return
        if (settings.trustedCertificate.equals(hex, ignoreCase = true)) return
        if (autodetect) settings.trustedCertificate = hex
        throw CertificateException("FTPS certificate is not trusted.")
    }

    override fun getAcceptedIssuers(): Array<X509Certificate> = system?.acceptedIssuers ?: emptyArray()

    private fun trustedBySystem(chain: Array<out X509Certificate>, authType: String?): Boolean {
        return try {
            system?.checkServerTrusted(chain, authType)
            system != null
        } catch (_: CertificateException) {
            false
        }
    }

    private fun systemTrust(): X509TrustManager? {
        val factory = TrustManagerFactory.getInstance(TrustManagerFactory.getDefaultAlgorithm())
        factory.init(null as java.security.KeyStore?)
        return factory.trustManagers.filterIsInstance<X509TrustManager>().firstOrNull()
    }
}
