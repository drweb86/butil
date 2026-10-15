package com.siarheikuchuk.butil.core.storage

import java.io.ByteArrayOutputStream
import java.io.DataInputStream
import java.io.InputStream
import java.net.InetSocketAddress
import java.net.Socket
import java.nio.ByteBuffer

/**
 * NFSv3 over TCP. Android has no kernel mount, so the same host, export, and mount options
 * are used to speak NFS directly. `vers=4` is rejected because this client is version 3.
 * `uid` and `gid` in the mount options become the AUTH_SYS identity (1000/1000 when omitted).
 */
internal class NfsStorage(private val settings: NfsSettings) : Storage {
    private val options = parseOptions(settings.mountOptions)
    private val client: Nfs3Client

    init {
        if (settings.host.isBlank()) throw IllegalArgumentException("NFS host is not specified.")
        if (settings.sharePath.isBlank()) throw IllegalArgumentException("NFS share path is not specified.")
        if (settings.mountPoint.isBlank()) throw IllegalArgumentException("NFS mount point is not specified.")
        val version = options["vers"] ?: options["nfsvers"]
        if (version != null && version != "3") {
            throw IllegalArgumentException("Android uploads with NFSv3. Set Mount Options to vers=3.")
        }
        client = Nfs3Client(
            host = settings.host,
            export = settings.sharePath,
            uid = options["uid"]?.toIntOrNull() ?: 1000,
            gid = options["gid"]?.toIntOrNull() ?: 1000,
            port = options["port"]?.toIntOrNull(),
            mountPort = options["mountport"]?.toIntOrNull(),
            writeSize = (options["wsize"]?.toIntOrNull() ?: 32768).coerceIn(1024, 1024 * 1024),
        )
    }

    override fun test(): String? = try {
        client.ping()
        null
    } catch (error: Exception) {
        error.message ?: error.toString()
    }

    override fun exists(relativeFileName: String): Boolean = client.exists(RemotePaths.normalizeRelative(relativeFileName))

    override fun upload(relativeFileName: String, length: Long, open: () -> InputStream) {
        client.upload(RemotePaths.normalizeRelative(relativeFileName), open)
    }

    override fun deleteFolder(relativeFolderName: String) {
        client.deleteTree(RemotePaths.normalizeRelative(relativeFolderName, allowEmpty = false))
    }

    override fun close() {
        client.close()
    }

    private fun parseOptions(text: String?): Map<String, String> {
        if (text.isNullOrBlank()) return emptyMap()
        return text.split(',').mapNotNull { item ->
            val parts = item.trim().split('=', limit = 2)
            val key = parts[0].trim().lowercase()
            if (key.isEmpty()) null else key to (parts.getOrNull(1)?.trim() ?: "")
        }.toMap()
    }
}

private class Nfs3Client(
    private val host: String,
    private val export: String,
    private val uid: Int,
    private val gid: Int,
    port: Int?,
    mountPort: Int?,
    private val writeSize: Int,
) : AutoCloseable {
    private val nfsPort = port ?: lookupPort(NFS_PROGRAM, NFS_VERSION) ?: 2049
    private val mntPort = mountPort ?: lookupPort(MOUNT_PROGRAM, MOUNT_VERSION) ?: 20048
    private val root: ByteArray

    init {
        root = mount()
    }

    fun ping() {
        val reply = call(nfsPort, NFS_PROGRAM, NFS_VERSION, GETATTR, xdr { opaqueVar(root) })
        if (reply.int() != NFS_OK) throw IllegalStateException("NFS getattr failed.")
    }

    fun exists(relative: String): Boolean = lookup(relative) != null

    fun upload(relative: String, open: () -> InputStream) {
        val parts = relative.split('/')
        var directory = root
        for (part in parts.dropLast(1)) {
            directory = ensureDirectory(directory, part)
        }
        val name = parts.last()
        lookupChild(directory, name)?.let { remove(directory, name) }
        val created = create(directory, name)
        open().use { input ->
            val buffer = ByteArray(writeSize)
            var offset = 0L
            while (true) {
                val read = input.read(buffer)
                if (read < 0) break
                write(created, offset, buffer, read)
                offset += read
            }
        }
    }

    fun deleteTree(relative: String) {
        val handle = lookup(relative) ?: return
        if (fileType(handle) == NF3DIR) {
            for (name in listNames(handle)) {
                val child = lookupChild(handle, name) ?: continue
                if (fileType(child) == NF3DIR) {
                    deleteTree(if (relative.isEmpty()) name else "$relative/$name")
                } else {
                    remove(handle, name)
                }
            }
            val parent = parentHandle(relative)
            rmdir(parent, relative.substringAfterLast('/'))
        } else {
            val parent = parentHandle(relative)
            remove(parent, relative.substringAfterLast('/'))
        }
    }

    override fun close() = Unit

    private fun parentHandle(relative: String): ByteArray {
        val parent = RemotePaths.parent(relative) ?: return root
        return lookup(parent) ?: throw IllegalStateException("NFS parent is missing: $parent")
    }

    private fun lookup(relative: String): ByteArray? {
        var current = root
        if (relative.isEmpty()) return current
        for (part in relative.split('/')) {
            current = lookupChild(current, part) ?: return null
        }
        return current
    }

    private fun lookupChild(directory: ByteArray, name: String): ByteArray? {
        val reply = call(nfsPort, NFS_PROGRAM, NFS_VERSION, LOOKUP, xdr {
            opaqueVar(directory)
            str(name)
        })
        val status = reply.int()
        if (status == NFSERR_NOENT) return null
        if (status != NFS_OK) throw IllegalStateException("NFS lookup failed ($status) for $name")
        return reply.opaqueVar()
    }

    private fun ensureDirectory(directory: ByteArray, name: String): ByteArray {
        lookupChild(directory, name)?.let { return it }
        val reply = call(nfsPort, NFS_PROGRAM, NFS_VERSION, MKDIR, xdr {
            opaqueVar(directory)
            str(name)
            sattr()
        })
        val status = reply.int()
        if (status != NFS_OK && status != NFSERR_EXIST) throw IllegalStateException("NFS mkdir failed ($status) for $name")
        return lookupChild(directory, name) ?: throw IllegalStateException("NFS mkdir did not create $name")
    }

    private fun create(directory: ByteArray, name: String): ByteArray {
        val reply = call(nfsPort, NFS_PROGRAM, NFS_VERSION, CREATE, xdr {
            opaqueVar(directory)
            str(name)
            int(0)
            sattr()
        })
        val status = reply.int()
        if (status != NFS_OK) throw IllegalStateException("NFS create failed ($status) for $name")
        val follows = reply.int()
        if (follows == 0) {
            return lookupChild(directory, name) ?: throw IllegalStateException("NFS create returned no handle for $name")
        }
        return reply.opaqueVar()
    }

    private fun write(handle: ByteArray, offset: Long, data: ByteArray, count: Int) {
        val reply = call(nfsPort, NFS_PROGRAM, NFS_VERSION, WRITE, xdr {
            opaqueVar(handle)
            long(offset)
            int(count)
            int(FILE_SYNC)
            opaqueVar(data.copyOf(count))
        })
        val status = reply.int()
        if (status != NFS_OK) throw IllegalStateException("NFS write failed ($status)")
    }

    private fun remove(directory: ByteArray, name: String) {
        val reply = call(nfsPort, NFS_PROGRAM, NFS_VERSION, REMOVE, xdr {
            opaqueVar(directory)
            str(name)
        })
        val status = reply.int()
        if (status != NFS_OK && status != NFSERR_NOENT) throw IllegalStateException("NFS remove failed ($status)")
    }

    private fun rmdir(directory: ByteArray, name: String) {
        val reply = call(nfsPort, NFS_PROGRAM, NFS_VERSION, RMDIR, xdr {
            opaqueVar(directory)
            str(name)
        })
        val status = reply.int()
        if (status != NFS_OK && status != NFSERR_NOENT) throw IllegalStateException("NFS rmdir failed ($status)")
    }

    private fun fileType(handle: ByteArray): Int {
        val reply = call(nfsPort, NFS_PROGRAM, NFS_VERSION, GETATTR, xdr { opaqueVar(handle) })
        val status = reply.int()
        if (status != NFS_OK) throw IllegalStateException("NFS getattr failed ($status)")
        return reply.int()
    }

    private fun listNames(directory: ByteArray): List<String> {
        val names = mutableListOf<String>()
        var cookie = 0L
        var verifier = ByteArray(8)
        while (true) {
            val reply = call(nfsPort, NFS_PROGRAM, NFS_VERSION, READDIR, xdr {
                opaqueVar(directory)
                long(cookie)
                opaqueFixed(verifier)
                int(8192)
            })
            val status = reply.int()
            if (status != NFS_OK) throw IllegalStateException("NFS readdir failed ($status)")
            skipPostOpAttr(reply)
            verifier = reply.opaqueFixed(8)
            while (reply.int() == 1) {
                reply.long()
                val name = reply.str()
                cookie = reply.long()
                if (name != "." && name != "..") names += name
            }
            if (reply.int() == 1) break
        }
        return names
    }

    private fun skipPostOpAttr(reply: XdrReader) {
        if (reply.int() != 1) return
        repeat(5) { reply.int() }
        repeat(8) { reply.long() }
    }

    private fun mount(): ByteArray {
        val reply = call(mntPort, MOUNT_PROGRAM, MOUNT_VERSION, MNT, xdr { str(export) })
        val status = reply.int()
        if (status != 0) throw IllegalStateException("NFS mount of $export failed ($status)")
        return reply.opaqueVar()
    }

    private fun lookupPort(program: Int, version: Int): Int? {
        return try {
            val reply = call(111, PORTMAP_PROGRAM, 2, PORTMAP_GETPORT, xdr {
                int(program)
                int(version)
                int(IPPROTO_TCP)
                int(0)
            }, authUnix = false)
            val port = reply.int()
            if (port == 0) null else port
        } catch (_: Exception) {
            null
        }
    }

    private fun call(port: Int, program: Int, version: Int, procedure: Int, body: ByteArray, authUnix: Boolean = true): XdrReader {
        Socket().use { socket ->
            socket.soTimeout = 60_000
            socket.connect(InetSocketAddress(host, port), 30_000)
            val xid = (System.nanoTime() and 0x7fffffff).toInt()
            val packet = xdr {
                int(xid)
                int(0)
                int(2)
                int(program)
                int(version)
                int(procedure)
                if (authUnix && procedure != 0) authSys() else {
                    int(0)
                    int(0)
                }
                int(0)
                int(0)
                raw(body)
            }
            val header = ByteBuffer.allocate(4).putInt(packet.size or LAST_FRAGMENT).array()
            socket.getOutputStream().write(header)
            socket.getOutputStream().write(packet)
            socket.getOutputStream().flush()
            val input = DataInputStream(socket.getInputStream())
            val record = readRecord(input)
            val reader = XdrReader(record)
            val replyXid = reader.int()
            if (replyXid != xid) throw IllegalStateException("NFS reply id did not match.")
            if (reader.int() != 1) throw IllegalStateException("NFS reply is not a reply.")
            if (reader.int() != 0) throw IllegalStateException("NFS call was rejected.")
            val verifierLength = reader.int().let { flavor ->
                if (flavor != 0) reader.opaqueVar() else ByteArray(0)
                reader
            }
            verifierLength.int()
            val accept = reader.int()
            if (accept != 0) throw IllegalStateException("NFS accept status $accept")
            return reader
        }
    }

    private fun XdrWriter.authSys() {
        val body = xdr {
            int(0)
            str("android")
            int(uid)
            int(gid)
            int(0)
        }
        int(AUTH_SYS)
        opaqueVar(body)
    }

    private fun XdrWriter.sattr() {
        int(1)
        int(420)
        int(0)
        int(0)
        int(0)
        int(0)
        int(0)
    }

    private fun readRecord(input: DataInputStream): ByteArray {
        val out = ByteArrayOutputStream()
        while (true) {
            val marker = input.readInt()
            val length = marker and 0x7fffffff
            val chunk = ByteArray(length)
            input.readFully(chunk)
            out.write(chunk)
            if (marker and LAST_FRAGMENT != 0) break
        }
        return out.toByteArray()
    }

    companion object {
        private const val LAST_FRAGMENT = 0x80000000.toInt()
        private const val AUTH_SYS = 1
        private const val IPPROTO_TCP = 6
        private const val PORTMAP_PROGRAM = 100000
        private const val PORTMAP_GETPORT = 3
        private const val NFS_PROGRAM = 100003
        private const val NFS_VERSION = 3
        private const val MOUNT_PROGRAM = 100005
        private const val MOUNT_VERSION = 3
        private const val MNT = 1
        private const val GETATTR = 1
        private const val LOOKUP = 3
        private const val READDIR = 16
        private const val WRITE = 7
        private const val CREATE = 8
        private const val MKDIR = 9
        private const val REMOVE = 12
        private const val RMDIR = 13
        private const val NFS_OK = 0
        private const val NFSERR_NOENT = 2
        private const val NFSERR_EXIST = 17
        private const val NF3DIR = 2
        private const val FILE_SYNC = 2
    }
}

private class XdrWriter {
    private val out = ByteArrayOutputStream()
    fun int(value: Int) {
        out.write((value ushr 24) and 0xFF)
        out.write((value ushr 16) and 0xFF)
        out.write((value ushr 8) and 0xFF)
        out.write(value and 0xFF)
    }

    fun long(value: Long) {
        int((value ushr 32).toInt())
        int(value.toInt())
    }

    fun raw(bytes: ByteArray) {
        out.write(bytes)
    }

    fun opaqueVar(bytes: ByteArray) {
        int(bytes.size)
        raw(bytes)
        pad(bytes.size)
    }

    fun opaqueFixed(bytes: ByteArray) {
        raw(bytes)
        pad(bytes.size)
    }

    fun str(value: String) = opaqueVar(value.toByteArray(Charsets.UTF_8))

    fun bytes(): ByteArray = out.toByteArray()

    private fun pad(size: Int) {
        val extra = (4 - size % 4) % 4
        repeat(extra) { out.write(0) }
    }
}

private class XdrReader(private val data: ByteArray) {
    private var offset = 0

    fun int(): Int {
        if (offset + 4 > data.size) throw IllegalStateException("Truncated NFS reply.")
        val value = ((data[offset].toInt() and 0xFF) shl 24) or
            ((data[offset + 1].toInt() and 0xFF) shl 16) or
            ((data[offset + 2].toInt() and 0xFF) shl 8) or
            (data[offset + 3].toInt() and 0xFF)
        offset += 4
        return value
    }

    fun long(): Long = (int().toLong() shl 32) or (int().toLong() and 0xffffffffL)

    fun opaqueVar(): ByteArray {
        val size = int()
        return opaqueFixed(size)
    }

    fun opaqueFixed(size: Int): ByteArray {
        if (size < 0 || offset + size > data.size) throw IllegalStateException("Truncated NFS opaque.")
        val bytes = data.copyOfRange(offset, offset + size)
        offset += size
        offset += (4 - size % 4) % 4
        return bytes
    }

    fun str(): String = opaqueVar().toString(Charsets.UTF_8)
}

private fun xdr(block: XdrWriter.() -> Unit): ByteArray = XdrWriter().apply(block).bytes()
