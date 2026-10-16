package com.siarheikuchuk.butil.core.ftps

import android.content.Context
import java.io.File
import java.io.FileNotFoundException
import java.io.InputStream
import java.io.OutputStream

/**
 * Storage Access Framework folders use the FTPS project's provider. A plain filesystem
 * path uses the same operations on [File], so a shared folder that is not a tree URI still works.
 */
internal class SharedFolderFileSystem(context: Context) : FileSystemProvider {
    private val saf = SafFileSystemProvider(context)
    private val files = LocalPathFileSystem()

    private fun provider(folder: String): FileSystemProvider =
        if (folder.startsWith("content:")) saf else files

    override fun createDirectory(userFolder: String, parts: List<String>) =
        provider(userFolder).createDirectory(userFolder, parts)

    override fun directoryExists(userFolder: String, parts: List<String>): Boolean =
        provider(userFolder).directoryExists(userFolder, parts)

    override fun fileExists(userFolder: String, parts: List<String>): Boolean =
        provider(userFolder).fileExists(userFolder, parts)

    override fun directoryDelete(userFolder: String, parts: List<String>) =
        provider(userFolder).directoryDelete(userFolder, parts)

    override fun fileDelete(userFolder: String, parts: List<String>) =
        provider(userFolder).fileDelete(userFolder, parts)

    override fun directoryMove(userFolder: String, fromParts: List<String>, toParts: List<String>) =
        provider(userFolder).directoryMove(userFolder, fromParts, toParts)

    override fun fileMove(userFolder: String, fromParts: List<String>, toParts: List<String>) =
        provider(userFolder).fileMove(userFolder, fromParts, toParts)

    override fun fileCreate(userFolder: String, parts: List<String>): OutputStream =
        provider(userFolder).fileCreate(userFolder, parts)

    override fun fileOpenRead(userFolder: String, parts: List<String>): InputStream =
        provider(userFolder).fileOpenRead(userFolder, parts)

    override fun getFileLastWriteTimeUtcMillis(userFolder: String, parts: List<String>): Long =
        provider(userFolder).getFileLastWriteTimeUtcMillis(userFolder, parts)

    override fun getFileLength(userFolder: String, parts: List<String>): Long =
        provider(userFolder).getFileLength(userFolder, parts)

    override fun directoryEntries(userFolder: String, parts: List<String>): List<FileSystemEntry> =
        provider(userFolder).directoryEntries(userFolder, parts)
}

private class LocalPathFileSystem : FileSystemProvider {
    override fun createDirectory(userFolder: String, parts: List<String>) {
        if (parts.isEmpty()) return
        val parent = directory(userFolder, parts.dropLast(1))
        val dir = child(parent, parts.last())
        if (dir.isDirectory) return
        if (dir.exists() || !dir.mkdir()) error("Failed to create directory: ${parts.last()}")
    }

    override fun directoryExists(userFolder: String, parts: List<String>): Boolean =
        try {
            directory(userFolder, parts)
            true
        } catch (_: FileNotFoundException) {
            false
        }

    override fun fileExists(userFolder: String, parts: List<String>): Boolean =
        try {
            file(userFolder, parts).isFile
        } catch (_: FileNotFoundException) {
            false
        }

    override fun directoryDelete(userFolder: String, parts: List<String>) {
        if (parts.isEmpty()) error("Cannot delete the shared folder")
        val dir = directory(userFolder, parts)
        if (!dir.deleteRecursively()) error("Failed to delete directory")
    }

    override fun fileDelete(userFolder: String, parts: List<String>) {
        val target = file(userFolder, parts)
        if (!target.delete()) error("Failed to delete file")
    }

    override fun directoryMove(userFolder: String, fromParts: List<String>, toParts: List<String>) {
        if (fromParts.isEmpty()) error("Cannot move the shared folder")
        val from = directory(userFolder, fromParts)
        val destParent = directory(userFolder, toParts.dropLast(1))
        val dest = child(destParent, toParts.last())
        if (from == dest) return
        if (dest.exists()) error("Destination already exists")
        if (!from.renameTo(dest)) {
            from.copyRecursively(dest, overwrite = false)
            if (!from.deleteRecursively()) error("Failed to remove source directory")
        }
    }

    override fun fileMove(userFolder: String, fromParts: List<String>, toParts: List<String>) {
        val from = file(userFolder, fromParts)
        val destParent = directory(userFolder, toParts.dropLast(1))
        val dest = child(destParent, toParts.last())
        if (from == dest) return
        if (dest.isDirectory) error("Cannot replace a directory with a file: ${dest.name}")
        if (dest.exists() && !dest.delete()) error("Failed to replace ${dest.name}")
        if (!from.renameTo(dest)) {
            from.copyTo(dest, overwrite = true)
            if (!from.delete()) error("Failed to remove source file")
        }
    }

    override fun fileCreate(userFolder: String, parts: List<String>): OutputStream {
        require(parts.isNotEmpty()) { "File path is empty" }
        val parent = directory(userFolder, parts.dropLast(1))
        val dest = child(parent, parts.last())
        if (dest.isDirectory) error("Cannot replace a directory with a file: ${dest.name}")
        dest.parentFile?.mkdirs()
        return dest.outputStream()
    }

    override fun fileOpenRead(userFolder: String, parts: List<String>): InputStream =
        file(userFolder, parts).inputStream()

    override fun getFileLastWriteTimeUtcMillis(userFolder: String, parts: List<String>): Long =
        file(userFolder, parts).lastModified()

    override fun getFileLength(userFolder: String, parts: List<String>): Long =
        file(userFolder, parts).length()

    override fun directoryEntries(userFolder: String, parts: List<String>): List<FileSystemEntry> {
        val folder = directory(userFolder, parts)
        val result = mutableListOf<FileSystemEntry>()
        result += FileSystemEntry(".", folder.lastModified(), 0, true)
        result += FileSystemEntry("..", folder.parentFile?.lastModified() ?: folder.lastModified(), 0, true)
        for (item in folder.listFiles().orEmpty()) {
            if (!inside(folder, item)) continue
            result += FileSystemEntry(
                fileName = item.name,
                lastWriteTimeMillis = item.lastModified(),
                length = if (item.isFile) item.length() else 0,
                isDirectory = item.isDirectory,
            )
        }
        return result
    }

    private fun directory(folder: String, parts: List<String>): File {
        val dir = resolve(folder, parts)
        if (!dir.isDirectory) throw FileNotFoundException("Directory not found")
        return dir
    }

    private fun file(folder: String, parts: List<String>): File {
        val target = resolve(folder, parts)
        if (!target.isFile) throw FileNotFoundException("File not found")
        return target
    }

    private fun resolve(folder: String, parts: List<String>): File {
        val base = File(folder)
        if (!base.isDirectory) throw SecurityException("Folder access is no longer granted")
        var current = base
        for (part in parts) {
            if (part.isEmpty() || part == ".") continue
            if (part == ".." || part.contains('/') || part.contains('\\') || '\u0000' in part) {
                throw FileNotFoundException("Directory not found: $part")
            }
            current = child(current, part)
        }
        if (!inside(base, current)) throw SecurityException("Path escapes the shared folder")
        return current
    }

    private fun child(parent: File, name: String): File {
        if (name.isEmpty() || name == "." || name == ".." || name.contains('/') || name.contains('\\') || '\u0000' in name) {
            throw FileNotFoundException("Directory not found: $name")
        }
        return File(parent, name)
    }

    private fun inside(base: File, target: File): Boolean {
        val root = base.canonicalFile
        val existing = if (target.exists()) target.canonicalFile else target.parentFile?.canonicalFile ?: return false
        val rootPath = root.path
        val prefix = if (rootPath.endsWith(File.separator)) rootPath else rootPath + File.separator
        val path = existing.path
        return path == rootPath || path.startsWith(prefix)
    }
}
