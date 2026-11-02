package com.siarheikuchuk.butil.core.storage

import android.content.Context
import android.util.Log
import com.siarheikuchuk.butil.R
import org.apache.commons.net.ftp.FTP
import org.apache.commons.net.ftp.FTPReply
import org.apache.commons.net.ftp.FTPSClient
import org.conscrypt.Conscrypt
import java.io.InputStream
import java.io.OutputStream
import java.net.Socket
import java.net.SocketTimeoutException
import java.nio.ByteBuffer
import java.security.Security
import java.security.cert.CertificateException
import java.security.cert.X509Certificate
import javax.net.ssl.SSLEngine
import javax.net.ssl.SSLEngineResult
import javax.net.ssl.SSLSocket
import javax.net.ssl.TrustManagerFactory
import javax.net.ssl.X509ExtendedTrustManager
import javax.net.ssl.X509TrustManager

internal class FtpsStorage(
    private val context: Context,
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
            if (!exists) return context.getString(R.string.error_folder_missing)
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
        if (settings.host.isBlank()) throw IllegalArgumentException(context.getString(R.string.error_server_name))
        if (settings.port < 0 || settings.port > 65535) throw IllegalArgumentException(context.getString(R.string.error_port_invalid))
        if (settings.user.isBlank()) throw IllegalArgumentException(context.getString(R.string.error_user_not_specified))
        if (settings.password.isBlank()) throw IllegalArgumentException(context.getString(R.string.error_password_not_specified))
        // Conscrypt's socket close frees the TLS engine while close_notify is still queued.
        // The data socket writes that alert itself before TCP is closed.
        if (Security.getProvider("Conscrypt") == null) {
            Security.insertProviderAt(Conscrypt.newProvider(), 1)
        }
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
) : X509ExtendedTrustManager() {
    private val system = systemTrust()

    override fun checkClientTrusted(chain: Array<out X509Certificate>?, authType: String?) = Unit

    override fun checkClientTrusted(chain: Array<out X509Certificate>?, authType: String?, socket: Socket?) = Unit

    override fun checkClientTrusted(chain: Array<out X509Certificate>?, authType: String?, engine: SSLEngine?) = Unit

    override fun checkServerTrusted(chain: Array<out X509Certificate>?, authType: String?) = check(chain, authType)

    override fun checkServerTrusted(chain: Array<out X509Certificate>?, authType: String?, socket: Socket?) = check(chain, authType)

    override fun checkServerTrusted(chain: Array<out X509Certificate>?, authType: String?, engine: SSLEngine?) = check(chain, authType)

    private fun check(chain: Array<out X509Certificate>?, authType: String?) {
        val cert = chain?.firstOrNull() ?: throw CertificateException("FTPS server presented no certificate.")
        val hex = cert.encoded.joinToString("") { "%02X".format(it) }
        if (trustedBySystem(chain, authType)) return
        if (settings.trustedCertificate.equals(hex, ignoreCase = true)) return
        // First sight of this certificate. Remember it for the form, and reject
        // this attempt. The next connection presents the same hex and is accepted.
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
            // A small upload plus the close alert must leave the host before TCP is finished.
            socket.tcpNoDelay = true
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
 * Commons Net closes the upload stream and then the socket. [SSLSocket.close] marks
 * the engine finished and frees it while close_notify is still queued, so the server
 * sees TCP close first and rejects the upload. Write the alert before that close.
 */
private class ClosingSslSocket(private val inner: Socket) : Socket() {
    override fun getOutputStream(): OutputStream {
        val raw = inner.getOutputStream()
        return object : java.io.FilterOutputStream(raw) {
            override fun close() {
                // The buffered stream already flushed the file bytes into this stream.
                // Send the alert before the socket close frees the TLS engine.
                if (inner is SSLSocket) sendCloseNotify(inner)
            }
        }
    }

    override fun getInputStream(): InputStream {
        val raw = inner.getInputStream()
        return object : java.io.FilterInputStream(raw) {
            override fun close() {
                // SSLInputStream.close closes the socket and drops close_notify.
                // Finish the alert first, then close the socket once.
                try {
                    if (inner is SSLSocket) sendCloseNotify(inner)
                } finally {
                    inner.close()
                }
            }
        }
    }

    override fun close() {
        if (inner is SSLSocket) sendCloseNotify(inner)
        inner.close()
    }

    override fun isConnected(): Boolean = inner.isConnected
    override fun isClosed(): Boolean = inner.isClosed
    override fun isInputShutdown(): Boolean = inner.isInputShutdown
    override fun isOutputShutdown(): Boolean = inner.isOutputShutdown
}

private fun declaredField(target: Any, name: String): java.lang.reflect.Field? {
    return generateSequence(target.javaClass as Class<*>?) { it.superclass }
        .flatMap { type -> type.declaredFields.asSequence() }
        .firstOrNull { it.name == name }
        ?.apply { isAccessible = true }
}

// One alert per data socket. A second close, after the engine is freed, must not
// try to write again: that throws "Socket is closed" and hides the first result.
private val closeNotifySent = java.util.Collections.newSetFromMap(java.util.WeakHashMap<SSLSocket, Boolean>())

// closeOutbound queues close_notify inside the engine. A TLS 1.3 NewSessionTicket can
// arrive first and block that shutdown until it is read. Copy the alert to the raw
// socket before SSLSocket.close frees the engine and closes TCP.
private fun sendCloseNotify(socket: SSLSocket) {
    if (socket in closeNotifySent) return
    try {
        val engine = declaredField(socket, "engine")?.get(socket) as? SSLEngine
        if (engine == null) {
            Log.w("BUtilFtps", "Could not send TLS close_notify: data socket has no TLS engine")
            return
        }
        val sslOut = declaredField(socket, "out")?.get(socket) as? OutputStream
        val raw = sslOut?.let { declaredField(it, "socketOutputStream")?.get(it) } as? OutputStream
        if (raw == null) {
            Log.w("BUtilFtps", "Could not send TLS close_notify: data socket has no output stream")
            return
        }
        readPostHandshake(socket)
        if (!engine.isOutboundDone) engine.closeOutbound()
        val empty = ByteBuffer.allocate(0)
        var packet = ByteBuffer.allocate(maxOf(engine.session.packetBufferSize, 18_432))
        var sent = 0
        var guard = 0
        while (guard++ < 8) {
            packet.clear()
            val result = engine.wrap(empty, packet)
            if (result.status == SSLEngineResult.Status.BUFFER_OVERFLOW) {
                packet = ByteBuffer.allocate(packet.capacity() * 2)
                continue
            }
            packet.flip()
            if (packet.hasRemaining()) {
                val bytes = ByteArray(packet.remaining())
                packet.get(bytes)
                raw.write(bytes)
                sent += bytes.size
            }
            if (result.handshakeStatus == SSLEngineResult.HandshakeStatus.NEED_UNWRAP) {
                readPostHandshake(socket)
            }
            if (result.handshakeStatus == SSLEngineResult.HandshakeStatus.NEED_TASK) {
                while (true) {
                    val task = engine.delegatedTask ?: break
                    task.run()
                }
                continue
            }
            if (result.bytesProduced() == 0) break
        }
        raw.flush()
        if (sent > 0) closeNotifySent.add(socket) else Log.w("BUtilFtps", "TLS close_notify was not produced")
    } catch (error: Exception) {
        Log.w("BUtilFtps", "Could not send TLS close_notify", error)
    }
}

// The server may send a session ticket before it is ready for our close alert.
// Read it, without treating a quiet socket as a failed upload.
private fun readPostHandshake(socket: SSLSocket) {
    val previous = try {
        socket.soTimeout
    } catch (_: Exception) {
        return
    }
    try {
        socket.soTimeout = 50
        val buffer = ByteArray(1)
        socket.getInputStream().read(buffer)
    } catch (_: SocketTimeoutException) {
    } finally {
        try {
            socket.soTimeout = previous
        } catch (_: Exception) {
        }
    }
}

private fun reuseConscryptSession(control: SSLSocket, data: SSLSocket) {
    val session = control.session ?: return
    if (!session.isValid) return
    val context = session.sessionContext ?: return
    val mapField = context.javaClass.declaredFields.firstOrNull { it.name == "sessionsByHostAndPort" } ?: return
    mapField.isAccessible = true
    val map = mapField.get(context) ?: return
    // The cache is keyed by host and port, and each value is a list of sessions.
    // A data connection is a different port, so it will not see the control session.
    // Copying the first list in the map shares that list: the next connection then
    // resumes the previous data-port session, the server rejects it, and the attempt
    // after that works again. Offer only the current control session, in a new list.
    synchronized(map) {
        val keySet = map.javaClass.getMethod("keySet").invoke(map) as Collection<*>
        val sampleKey = keySet.firstOrNull() ?: return
        val hostField = sampleKey.javaClass.declaredFields.firstOrNull { it.name == "host" } ?: return
        val portField = sampleKey.javaClass.declaredFields.firstOrNull { it.name == "port" } ?: return
        hostField.isAccessible = true
        portField.isAccessible = true
        val ctor = sampleKey.javaClass.getDeclaredConstructor(String::class.java, Int::class.javaPrimitiveType)
        ctor.isAccessible = true
        val get = map.javaClass.methods.first { it.name == "get" && it.parameterTypes.size == 1 }
        val controlPort = control.port
        val controlHosts = listOfNotNull(session.peerHost, control.inetAddress?.hostAddress, control.inetAddress?.hostName)
        var controlSessions: List<*>? = null
        for (key in keySet) {
            val keyHost = hostField.get(key) as? String ?: continue
            val keyPort = portField.getInt(key)
            if (keyPort == controlPort && controlHosts.any { it.equals(keyHost, ignoreCase = true) }) {
                controlSessions = get.invoke(map, key) as? List<*>
                break
            }
        }
        val current = controlSessions?.lastOrNull { it != null && sessionValid(it) } ?: return
        val put = map.javaClass.methods.first { it.name == "put" && it.parameterTypes.size == 2 }
        val dataHosts = listOfNotNull(data.inetAddress?.hostAddress, data.inetAddress?.hostName, session.peerHost).distinct()
        for (host in dataHosts) {
            put.invoke(map, ctor.newInstance(host, data.port), arrayListOf(current))
        }
    }
}

private fun sessionValid(session: Any): Boolean {
    val valid = session.javaClass.methods.firstOrNull { it.name == "isValid" && it.parameterTypes.isEmpty() }
        ?: return true
    return valid.invoke(session) == true
}
