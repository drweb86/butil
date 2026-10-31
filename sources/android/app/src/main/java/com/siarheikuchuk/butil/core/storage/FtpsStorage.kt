package com.siarheikuchuk.butil.core.storage

import android.util.Log
import org.apache.commons.net.ftp.FTP
import org.apache.commons.net.ftp.FTPReply
import org.apache.commons.net.ftp.FTPSClient
import org.conscrypt.Conscrypt
import java.io.FileDescriptor
import java.io.InputStream
import java.io.OutputStream
import java.net.Socket
import java.security.Security
import java.security.cert.CertificateException
import java.security.cert.X509Certificate
import javax.net.ssl.SSLSocket
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
        // The platform engine socket closes TCP before close_notify is written. The server
        // then rejects the upload. This provider's file-descriptor sockets send the alert first.
        if (Security.getProvider("Conscrypt") == null) {
            Security.insertProviderAt(Conscrypt.newProvider(), 1)
        }
        Conscrypt.setUseEngineSocketByDefault(false)
        val implicit = settings.encryption == "Implicit"
        val ftp = SessionReuseFtpsClient(implicit)
        ftp.connectTimeout = 30_000
        ftp.defaultTimeout = 120_000
        ftp.controlEncoding = "UTF-8"
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
        if (FTPReply.isPositiveCompletion(ftp.sendCommand("OPTS", "UTF8 ON"))) {
            ftp.controlEncoding = "UTF-8"
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

/**
 * The data connection is a different port from the control connection. Conscrypt only
 * offers the cached TLS session when the host and port match, so a protected upload
 * otherwise stops after the first TLS record. Copy the control session onto the data port.
 */
private class SessionReuseFtpsClient(implicit: Boolean) : FTPSClient("TLS", implicit) {
    init {
        bufferSize = 64 * 1024
    }

    override fun _prepareDataSocket_(socket: Socket) {
        if (socket !is SSLSocket) return
        val control = _socket_ as? SSLSocket ?: return
        try {
            reuseConscryptSession(control, socket)
        } catch (error: Exception) {
            Log.w("BUtilFtps", "Could not reuse TLS session: ${error.javaClass.simpleName}: ${error.message}")
        }
    }

    override fun _openDataConnection_(command: String, arg: String): Socket? {
        val opened = super._openDataConnection_(command, arg) ?: return null
        return ClosingSslSocket(opened)
    }
}

/**
 * Commons Net closes the upload stream and then the socket. Conscrypt's
 * [SSLSocket.shutdownOutput] shuts the raw TCP write side and does not send
 * close_notify, so a later [SSLSocket.close] can no longer send it either.
 * Flush the buffered records here, and let [close] perform the TLS shutdown.
 */
private class ClosingSslSocket(private val inner: Socket) : Socket() {
    override fun getOutputStream(): OutputStream {
        val raw = inner.getOutputStream()
        return object : java.io.FilterOutputStream(raw) {
            override fun close() {
                flush()
            }
        }
    }

    override fun getInputStream(): InputStream = inner.getInputStream()
    override fun close() {
        if (inner is SSLSocket) sendCloseNotify(inner)
        inner.close()
    }

    override fun isConnected(): Boolean = inner.isConnected
    override fun isClosed(): Boolean = inner.isClosed
    override fun isInputShutdown(): Boolean = inner.isInputShutdown
    override fun isOutputShutdown(): Boolean = inner.isOutputShutdown
}

// File-descriptor Conscrypt interrupts the SSL object inside close() before it writes
// close_notify, which drops the alert when a write is still finishing. Send it first.
private fun sendCloseNotify(socket: SSLSocket) {
    try {
        val sslField = generateSequence(socket.javaClass as Class<*>?) { it.superclass }
            .flatMap { type -> type.declaredFields.asSequence() }
            .firstOrNull { it.name == "ssl" }
            ?: return
        sslField.isAccessible = true
        val ssl = sslField.get(socket) ?: return
        val shutdown = ssl.javaClass.declaredMethods.firstOrNull {
            it.name == "shutdown" && it.parameterTypes.size == 1 && it.parameterTypes[0] == FileDescriptor::class.java
        } ?: return
        shutdown.isAccessible = true
        val platform = Class.forName("org.conscrypt.Platform")
        val fd = platform.getDeclaredMethod("getFileDescriptor", Socket::class.java)
            .apply { isAccessible = true }
            .invoke(null, socket)
        shutdown.invoke(ssl, fd)
    } catch (error: Exception) {
        Log.w("BUtilFtps", "Could not send TLS close_notify: ${error.javaClass.simpleName}: ${error.message}")
    }
}

private fun reuseConscryptSession(control: SSLSocket, data: SSLSocket) {
    val session = control.session ?: return
    val context = session.sessionContext ?: return
    val mapField = context.javaClass.declaredFields.firstOrNull { it.name == "sessionsByHostAndPort" } ?: return
    mapField.isAccessible = true
    val map = mapField.get(context) ?: return
    val values = map.javaClass.getMethod("values").invoke(map) as Collection<*>
    val holder = values.firstOrNull() ?: return
    val keys = map.javaClass.getMethod("keySet").invoke(map) as Collection<*>
    val sampleKey = keys.firstOrNull() ?: return
    val ctor = sampleKey.javaClass.declaredConstructors.maxByOrNull { it.parameterTypes.size } ?: return
    ctor.isAccessible = true
    if (ctor.parameterTypes.size != 2) return
    val put = map.javaClass.methods.first { it.name == "put" && it.parameterTypes.size == 2 }
    val port = data.port
    val hosts = listOfNotNull(data.inetAddress?.hostAddress, data.inetAddress?.hostName, session.peerHost).distinct()
    for (host in hosts) {
        put.invoke(map, ctor.newInstance(host, port), holder)
    }
}
