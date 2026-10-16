package com.siarheikuchuk.butil.core.ftps

import android.content.Context
import android.net.Uri
import androidx.documentfile.provider.DocumentFile


import java.io.FileNotFoundException
import java.io.InputStream
import java.io.OutputStream

class SafFileSystemProvider(private val context: Context) : FileSystemProvider {

    // SAF createFile(mime, name) appends a MIME extension when name does not already
    // end with that extension ("test.js VERIFICATION" + javascript → "...VERIFICATION.js").
    private companion object {
        const val MIME_BINARY = "application/octet-stream"
    }

    private val liveUris = HashMap<String, String>()

    private fun root(uriString: String): DocumentFile {
        val live = liveUris[uriString]
            ?: PersistedFolderGrant.liveUri(context, uriString, needsWrite = false)?.also { liveUris[uriString] = it }
            ?: throw SecurityException("Folder access is no longer granted")
        return DocumentFile.fromTreeUri(context, Uri.parse(live))
            ?: throw SecurityException("Folder access is no longer granted")
    }

    private fun navigateFolder(uriString: String, parts: List<String>): DocumentFile {
        var current = root(uriString)
        for (part in parts) {
            if (part.isEmpty() || part == ".") continue
            if (part == "..") throw FileNotFoundException("Directory not found: $part")
            current = current.findFile(part)?.takeIf { it.isDirectory }
                ?: throw java.io.FileNotFoundException("Directory not found: $part")
        }
        return current
    }

    private fun navigateFile(uriString: String, parts: List<String>): DocumentFile {
        require(parts.isNotEmpty()) { "File path is empty" }
        val folder = navigateFolder(uriString, parts.dropLast(1))
        return folder.findFile(parts.last())?.takeIf { it.isFile }
            ?: throw FileNotFoundException("File not found: ${parts.last()}")
    }

    override fun createDirectory(userFolder: String, parts: List<String>) {
        if (parts.isEmpty()) return
        val parent = navigateFolder(userFolder, parts.dropLast(1))
        val name = parts.last()
        if (parent.findFile(name) == null) {
            parent.createDirectory(name) ?: error("Failed to create directory: $name")
        }
    }

    override fun directoryExists(userFolder: String, parts: List<String>): Boolean =
        try {
            navigateFolder(userFolder, parts)
            true
        } catch (_: FileNotFoundException) {
            false
        }

    override fun fileExists(userFolder: String, parts: List<String>): Boolean =
        try {
            navigateFile(userFolder, parts)
            true
        } catch (_: FileNotFoundException) {
            false
        }

    override fun directoryDelete(userFolder: String, parts: List<String>) {
        if (parts.isEmpty()) error("Cannot delete the shared folder")
        val folder = navigateFolder(userFolder, parts)
        if (!folder.delete()) error("Failed to delete directory")
    }

    override fun fileDelete(userFolder: String, parts: List<String>) {
        val file = navigateFile(userFolder, parts)
        if (!file.delete()) error("Failed to delete file")
    }

    override fun directoryMove(userFolder: String, fromParts: List<String>, toParts: List<String>) {
        if (fromParts.isEmpty()) error("Cannot move the shared folder")
        val from = navigateFolder(userFolder, fromParts)
        val destParent = navigateFolder(userFolder, toParts.dropLast(1))
        val newName = toParts.last()
        if (!from.renameTo(newName) || from.parentFile?.uri != destParent.uri) {
            val dest = destParent.createDirectory(newName) ?: error("Failed to create destination folder: $newName")
            copyTree(from, dest)
            from.delete()
        }
    }

    override fun fileMove(userFolder: String, fromParts: List<String>, toParts: List<String>) {
        val from = navigateFile(userFolder, fromParts)
        val destParent = navigateFolder(userFolder, toParts.dropLast(1))
        val newName = toParts.last()
        val sameFolder = fromParts.dropLast(1) == toParts.dropLast(1)
        destParent.findFile(newName)?.takeIf { it.uri != from.uri }?.delete()

        if (sameFolder && from.renameTo(newName) && from.name == newName) {
            return
        }

        val dest = createExactFile(destParent, newName)
        if (from.uri != dest.uri) {
            copyContents(from, dest)
            from.delete()
        }
    }

    override fun fileCreate(userFolder: String, parts: List<String>): OutputStream {
        require(parts.isNotEmpty()) { "File path is empty" }
        val folder = navigateFolder(userFolder, parts.dropLast(1))
        val name = parts.last()
        deleteFileIfPresent(folder, name)
        val file = createExactFile(folder, name)
        return context.contentResolver.openOutputStream(file.uri, "w")
            ?: error("Cannot open file for write: $name")
    }

    override fun fileOpenRead(userFolder: String, parts: List<String>): InputStream {
        val file = navigateFile(userFolder, parts)
        return context.contentResolver.openInputStream(file.uri)
            ?: error("Cannot open file for read")
    }

    override fun getFileLastWriteTimeUtcMillis(userFolder: String, parts: List<String>): Long =
        navigateFile(userFolder, parts).lastModified()

    override fun getFileLength(userFolder: String, parts: List<String>): Long =
        navigateFile(userFolder, parts).length()

    override fun directoryEntries(userFolder: String, parts: List<String>): List<FileSystemEntry> {
        val folder = navigateFolder(userFolder, parts)
        val result = mutableListOf<FileSystemEntry>()
        result += FileSystemEntry(".", folder.lastModified(), 0, true)
        val parent = folder.parentFile
        result += FileSystemEntry("..", parent?.lastModified() ?: folder.lastModified(), 0, true)
        for (item in folder.listFiles()) {
            val name = item.name ?: continue
            result += FileSystemEntry(
                fileName = name,
                lastWriteTimeMillis = item.lastModified(),
                length = if (item.isFile) item.length() else 0,
                isDirectory = item.isDirectory,
            )
        }
        return result
    }

    private fun copyTree(from: DocumentFile, to: DocumentFile) {
        for (item in from.listFiles()) {
            val name = item.name ?: continue
            if (item.isDirectory) {
                val child = to.createDirectory(name) ?: continue
                copyTree(item, child)
            } else {
                val dest = createExactFile(to, name)
                copyContents(item, dest)
            }
        }
    }

    private fun deleteFileIfPresent(parent: DocumentFile, name: String) {
        val existing = parent.findFile(name) ?: return
        if (existing.isDirectory) error("Cannot replace a directory with a file: $name")
        existing.delete()
    }

    private fun createExactFile(parent: DocumentFile, displayName: String): DocumentFile {
        deleteFileIfPresent(parent, displayName)
        val created = parent.createFile(MIME_BINARY, displayName)
            ?: error("Failed to create file: $displayName")
        if (created.name != null && created.name != displayName) {
            created.renameTo(displayName)
        }
        return created
    }

    private fun copyContents(from: DocumentFile, to: DocumentFile) {
        context.contentResolver.openInputStream(from.uri).use { input ->
            context.contentResolver.openOutputStream(to.uri, "w").use { output ->
                require(input != null && output != null)
                input.copyTo(output)
            }
        }
    }
}
