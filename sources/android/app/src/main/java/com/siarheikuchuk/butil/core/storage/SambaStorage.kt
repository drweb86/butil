package com.siarheikuchuk.butil.core.storage

import com.hierynomus.msdtyp.AccessMask
import com.hierynomus.msfscc.FileAttributes
import com.hierynomus.msfscc.fileinformation.FileIdBothDirectoryInformation
import com.hierynomus.mssmb2.SMB2CreateDisposition
import com.hierynomus.mssmb2.SMB2ShareAccess
import com.hierynomus.smbj.SMBClient
import com.hierynomus.smbj.auth.AuthenticationContext
import com.hierynomus.smbj.share.DiskShare
import java.io.InputStream
import java.util.EnumSet

internal class SambaStorage(private val settings: SambaSettings) : Storage {
    private val target = parse(settings.url)
    private val client = SMBClient()
    private val connection = client.connect(target.host)
    private val session = connection.authenticate(authentication())
    private val share: DiskShare = session.connectShare(target.share) as DiskShare

    init {
        if (settings.url.isBlank()) throw IllegalArgumentException("Url is not specified.")
    }

    override fun test(): String? {
        return try {
            if (target.folder.isNotEmpty() && !share.folderExists(target.folder)) {
                "Folder does not exist."
            } else {
                share.list(target.folder.ifEmpty { "" })
                null
            }
        } catch (error: Exception) {
            error.message ?: error.toString()
        }
    }

    override fun exists(relativeFileName: String): Boolean {
        return share.fileExists(path(relativeFileName))
    }

    override fun upload(relativeFileName: String, length: Long, open: () -> InputStream) {
        val destination = path(relativeFileName)
        ensureDirectories(destination.substringBeforeLast('\\', ""))
        share.openFile(
            destination,
            EnumSet.of(AccessMask.GENERIC_WRITE),
            EnumSet.of(FileAttributes.FILE_ATTRIBUTE_NORMAL),
            EnumSet.of(SMB2ShareAccess.FILE_SHARE_READ),
            SMB2CreateDisposition.FILE_OVERWRITE_IF,
            null,
        ).use { file ->
            file.outputStream.use { output -> open().use { input -> StorageIo.copy(input, output) } }
        }
    }

    override fun deleteFolder(relativeFolderName: String) {
        deleteRecursive(path(relativeFolderName))
    }

    override fun download(relativeFileName: String): InputStream {
        val file = share.openFile(
            path(relativeFileName),
            EnumSet.of(AccessMask.GENERIC_READ),
            EnumSet.of(FileAttributes.FILE_ATTRIBUTE_NORMAL),
            EnumSet.of(SMB2ShareAccess.FILE_SHARE_READ),
            SMB2CreateDisposition.FILE_OPEN,
            null,
        )
        val stream = file.inputStream
        return object : InputStream() {
            override fun read(): Int = stream.read()
            override fun read(b: ByteArray, off: Int, len: Int): Int = stream.read(b, off, len)
            override fun close() {
                stream.close()
                file.close()
            }
        }
    }

    override fun delete(relativeFileName: String) {
        val destination = path(relativeFileName)
        if (share.fileExists(destination)) share.rm(destination)
    }

    override fun listFiles(): List<RemoteFile> {
        val files = mutableListOf<RemoteFile>()
        walkFiles(target.folder, "", files)
        return files.sortedBy { it.relativePath }
    }

    override fun listFolders(): List<String> {
        return share.list(target.folder.ifEmpty { "" })
            .filter { it.fileName != "." && it.fileName != ".." && directory(it) }
            .map { it.fileName }
            .sorted()
    }

    private fun walkFiles(absolute: String, relative: String, into: MutableList<RemoteFile>) {
        val children = try {
            share.list(absolute.ifEmpty { "" })
        } catch (_: Exception) {
            return
        }
        for (child in children) {
            if (child.fileName == "." || child.fileName == "..") continue
            val childRelative = if (relative.isEmpty()) child.fileName else "$relative/${child.fileName}"
            val childAbsolute = if (absolute.isEmpty()) child.fileName else "$absolute\\${child.fileName}"
            if (directory(child)) {
                walkFiles(childAbsolute, childRelative, into)
            } else {
                into += RemoteFile(childRelative, child.endOfFile, child.lastWriteTime.toDate().time)
            }
        }
    }

    private fun directory(child: FileIdBothDirectoryInformation): Boolean {
        return child.fileAttributes and FileAttributes.FILE_ATTRIBUTE_DIRECTORY.value != 0L
    }

    override fun close() {
        try {
            share.close()
        } catch (_: Exception) {
        }
        try {
            session.close()
        } catch (_: Exception) {
        }
        try {
            connection.close()
        } catch (_: Exception) {
        }
        client.close()
    }

    private fun path(relative: String): String {
        val child = RemotePaths.normalizeRelative(relative).replace('/', '\\')
        return if (target.folder.isEmpty()) child else "${target.folder}\\$child"
    }

    private fun ensureDirectories(directory: String) {
        if (directory.isBlank()) return
        val parts = directory.split('\\').filter { it.isNotEmpty() }
        var current = ""
        for (part in parts) {
            current = if (current.isEmpty()) part else "$current\\$part"
            if (!share.folderExists(current)) share.mkdir(current)
        }
    }

    private fun deleteRecursive(directory: String) {
        val children: List<FileIdBothDirectoryInformation> = share.list(directory)
        for (child in children) {
            if (child.fileName == "." || child.fileName == "..") continue
            val childPath = "$directory\\${child.fileName}"
            val directoryBit = FileAttributes.FILE_ATTRIBUTE_DIRECTORY.value
            if (child.fileAttributes and directoryBit != 0L) deleteRecursive(childPath) else share.rm(childPath)
        }
        share.rmdir(directory, true)
    }

    private fun authentication(): AuthenticationContext {
        val user = settings.user?.trim().orEmpty()
        if (user.isEmpty()) return AuthenticationContext.guest()
        val split = user.split('\\', '/')
        val domain = if (split.size == 2) split[0] else ""
        val name = if (split.size == 2) split[1] else user
        return AuthenticationContext(name, (settings.password ?: "").toCharArray(), domain)
    }

    private data class Target(val host: String, val share: String, val folder: String)

    companion object {
        private fun parse(url: String): Target {
            val normalized = url.trim().replace('\\', '/').removePrefix("smb:").trim('/')
            val parts = normalized.split('/').filter { it.isNotEmpty() }
            if (parts.size < 2) throw IllegalArgumentException("Url is not specified.")
            return Target(parts[0], parts[1], parts.drop(2).joinToString("\\"))
        }
    }
}
