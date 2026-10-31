package com.siarheikuchuk.butil.core.storage

import net.schmizz.sshj.SSHClient
import net.schmizz.sshj.sftp.OpenMode
import net.schmizz.sshj.sftp.SFTPClient
import net.schmizz.sshj.transport.verification.HostKeyVerifier
import net.schmizz.sshj.userauth.keyprovider.KeyProvider
import net.schmizz.sshj.userauth.method.AuthMethod
import net.schmizz.sshj.userauth.method.AuthPassword
import net.schmizz.sshj.userauth.method.AuthPublickey
import net.schmizz.sshj.userauth.password.PasswordFinder
import net.schmizz.sshj.userauth.password.Resource
import net.schmizz.sshj.common.Buffer
import org.bouncycastle.jce.provider.BouncyCastleProvider
import java.io.InputStream
import java.security.MessageDigest
import java.security.PublicKey
import java.security.Security
import java.util.EnumSet

internal class SftpStorage(
    private val settings: SftpSettings,
    private val testingConnection: Boolean,
) : Storage {
    private val ssh = SSHClient()
    private val sftp: SFTPClient

    init {
        validate()
        sftp = connect()
    }

    override fun test(): String? {
        return try {
            sftp.ls(settings.folder)
            null
        } catch (error: Exception) {
            if (!sftp.statExistence(settings.folder).let { it != null }) {
                "Folder does not exist."
            } else {
                error.message ?: error.toString()
            }
        }
    }

    override fun exists(relativeFileName: String): Boolean {
        return sftp.statExistence(remote(relativeFileName)) != null
    }

    override fun upload(relativeFileName: String, length: Long, open: () -> InputStream) {
        val path = remote(relativeFileName)
        val parent = path.substringBeforeLast('/', "")
        if (parent.isNotEmpty()) sftp.mkdirs(parent)
        if (exists(relativeFileName)) sftp.rm(path)
        sftp.open(path, EnumSet.of(OpenMode.WRITE, OpenMode.CREAT, OpenMode.TRUNC)).use { file ->
            open().use { input ->
                val buffer = ByteArray(1024 * 128)
                var offset = 0L
                while (true) {
                    val read = input.read(buffer)
                    if (read < 0) break
                    file.write(offset, buffer, 0, read)
                    offset += read
                }
            }
        }
    }

    override fun deleteFolder(relativeFolderName: String) {
        deleteRecursive(remote(relativeFolderName))
    }

    override fun download(relativeFileName: String): InputStream {
        val path = remote(relativeFileName)
        val file = sftp.open(path, EnumSet.of(OpenMode.READ))
        val size = sftp.stat(path).size
        return object : InputStream() {
            private var offset = 0L
            override fun read(): Int {
                val one = ByteArray(1)
                val read = read(one, 0, 1)
                return if (read < 0) -1 else one[0].toInt() and 0xFF
            }

            override fun read(b: ByteArray, off: Int, len: Int): Int {
                if (offset >= size) return -1
                val read = file.read(offset, b, off, len).toInt()
                if (read <= 0) return -1
                offset += read
                return read
            }

            override fun close() {
                file.close()
            }
        }
    }

    override fun delete(relativeFileName: String) {
        val path = remote(relativeFileName)
        if (sftp.statExistence(path) != null) sftp.rm(path)
    }

    override fun listFiles(): List<RemoteFile> {
        val root = settings.folder.trim().trimEnd('/').ifBlank { "/" }
        val files = mutableListOf<RemoteFile>()
        walk(root, "", files)
        return files.sortedBy { it.relativePath }
    }

    override fun listFolders(): List<String> {
        val root = settings.folder.trim().trimEnd('/').ifBlank { "/" }
        return sftp.ls(root)
            .filter { it.isDirectory && it.name != "." && it.name != ".." }
            .map { it.name }
            .sorted()
    }

    private fun walk(absolute: String, relative: String, into: MutableList<RemoteFile>) {
        val entries = try {
            sftp.ls(absolute)
        } catch (_: Exception) {
            return
        }
        for (entry in entries) {
            if (entry.name == "." || entry.name == "..") continue
            val childRelative = if (relative.isEmpty()) entry.name else "$relative/${entry.name}"
            val childAbsolute = if (absolute.endsWith("/")) absolute + entry.name else "$absolute/${entry.name}"
            if (entry.isDirectory) {
                walk(childAbsolute, childRelative, into)
            } else {
                val modified = entry.attributes.mtime * 1000L
                into += RemoteFile(childRelative, entry.attributes.size, modified)
            }
        }
    }

    override fun close() {
        try {
            sftp.close()
        } catch (_: Exception) {
        }
        try {
            ssh.disconnect()
        } catch (_: Exception) {
        }
    }

    private fun validate() {
        if (settings.host.isBlank()) throw IllegalArgumentException("Server name is not be specified.")
        if (settings.port < 0 || settings.port > 65535) throw IllegalArgumentException("Port is invalid.")
        if (settings.user.isBlank()) throw IllegalArgumentException("User is not specified.")
        if (settings.password.isNullOrBlank() && settings.keyFile.isNullOrBlank()) {
            throw IllegalArgumentException("Both password or key file are empty.")
        }
        if (!testingConnection && settings.fingerPrintSha256.isBlank()) {
            throw IllegalArgumentException("Fingerprint SHA-256 is not specified.")
        }
        if (settings.folder.isBlank()) throw IllegalArgumentException("Folder is not specified.")
        if (settings.folder == "/") {
            throw IllegalArgumentException("SFTP folder / is not allowed. Folder must point to more specific folder.")
        }
        if (!settings.folder.startsWith("/")) throw IllegalArgumentException("SFTP folder must start with /.")
        if (settings.folder.endsWith("/")) throw IllegalArgumentException("SFTP folder must not end on /.")
    }

    private fun connect(): SFTPClient {
        // Android's built-in provider named "BC" has no X25519. SSHJ keeps that name and
        // then fails the handshake. Replace it with the bundled provider before connecting.
        val current = Security.getProvider(BouncyCastleProvider.PROVIDER_NAME)
        if (current !is BouncyCastleProvider) {
            Security.removeProvider(BouncyCastleProvider.PROVIDER_NAME)
            Security.addProvider(BouncyCastleProvider())
        }
        ssh.connectTimeout = 30_000
        ssh.addHostKeyVerifier(fingerprintVerifier())
        val port = if (settings.port == 0) 22 else settings.port
        ssh.connect(settings.host, port)
        val methods = mutableListOf<AuthMethod>()
        if (!settings.keyFile.isNullOrBlank()) {
            methods += AuthPublickey(loadKey())
        }
        if (!settings.password.isNullOrBlank()) {
            val password = settings.password!!
            methods += AuthPassword(object : PasswordFinder {
                override fun reqPassword(resource: Resource<*>?): CharArray = password.toCharArray()
                override fun shouldRetry(resource: Resource<*>?): Boolean = false
            })
        }
        ssh.auth(settings.user, methods)
        return ssh.newSFTPClient()
    }

    private fun loadKey(): KeyProvider {
        val location = settings.keyFile!!
        if (location.startsWith("content:") || location.startsWith("file:")) {
            throw IllegalArgumentException("Key file must be readable as a local file path passed to the storage.")
        }
        return ssh.loadKeys(location)
    }

    private fun fingerprintVerifier(): HostKeyVerifier {
        return object : HostKeyVerifier {
            override fun verify(hostname: String?, port: Int, key: PublicKey?): Boolean {
                if (key == null) return false
                val fingerprint = fingerprint(key)
                val expected = settings.fingerPrintSha256
                if (testingConnection && expected.isBlank()) {
                    settings.fingerPrintSha256 = fingerprint
                }
                return fingerprintsMatch(expected, fingerprint)
            }

            override fun findExistingAlgorithms(hostname: String?, port: Int): MutableList<String> = mutableListOf()
        }
    }

    private fun remote(relative: String): String = RemotePaths.combine(settings.folder, relative)

    private fun deleteRecursive(path: String) {
        val entries = try {
            sftp.ls(path)
        } catch (_: Exception) {
            return
        }
        for (entry in entries) {
            if (entry.name == "." || entry.name == "..") continue
            val child = if (path.endsWith("/")) path + entry.name else "$path/${entry.name}"
            if (entry.isDirectory) deleteRecursive(child) else sftp.rm(child)
        }
        sftp.rmdir(path)
    }

    companion object {
        fun fingerprint(key: PublicKey): String {
            val buffer = Buffer.PlainBuffer()
            buffer.putPublicKey(key)
            val hash = MessageDigest.getInstance("SHA-256").digest(buffer.compactData)
            return java.util.Base64.getEncoder().encodeToString(hash).trimEnd('=')
        }

        fun fingerprintsMatch(expected: String, actual: String): Boolean {
            if (expected.isBlank()) return false
            return normalize(expected) == normalize(actual)
        }

        private fun normalize(value: String): String {
            return value.trim()
                .removePrefix("SHA256:")
                .removePrefix("sha256:")
                .trimEnd('=')
                .lowercase()
        }
    }
}
