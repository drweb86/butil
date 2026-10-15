package com.siarheikuchuk.butil.core.storage

import android.content.Context
import android.net.Uri
import androidx.documentfile.provider.DocumentFile
import java.io.File
import java.io.InputStream

internal class ListedFile(
    val relativePath: String,
    val length: Long,
    val open: () -> InputStream,
)

/**
 * A local folder is either a Storage Access Framework tree or a filesystem path.
 * Uploaded names are relative to this folder, matching the desktop client.
 */
internal object LocalFolder {
    fun requireDirectory(context: Context, location: String) {
        if (location.isBlank()) throw IllegalArgumentException("Folder is not specified.")
        if (!isDirectory(context, location)) throw IllegalArgumentException("Folder does not exist.")
    }

    fun isDirectory(context: Context, location: String): Boolean {
        val document = document(context, location) ?: return File(location).isDirectory
        return document.isDirectory
    }

    fun listFiles(context: Context, location: String): List<ListedFile> {
        requireDirectory(context, location)
        val document = document(context, location)
        val files = mutableListOf<ListedFile>()
        if (document != null) {
            walkDocument(context, document, "", files)
        } else {
            val root = File(location)
            walkFile(context, root, root, files)
        }
        return files.sortedBy { it.relativePath }
    }

    private fun walkDocument(context: Context, directory: DocumentFile, relative: String, into: MutableList<ListedFile>) {
        for (child in directory.listFiles()) {
            val name = child.name ?: continue
            if (name == "." || name == "..") continue
            val childRelative = if (relative.isEmpty()) name else "$relative/$name"
            if (child.isDirectory) {
                walkDocument(context, child, childRelative, into)
            } else if (child.isFile) {
                val uri = child.uri
                into += ListedFile(childRelative, length(context, uri, child.length())) {
                    context.contentResolver.openInputStream(uri)
                        ?: throw IllegalStateException("Cannot read $childRelative")
                }
            }
        }
    }

    private fun walkFile(context: Context, root: File, current: File, into: MutableList<ListedFile>) {
        for (child in current.listFiles() ?: return) {
            if (child.isDirectory) {
                walkFile(context, root, child, into)
            } else if (child.isFile) {
                val relative = child.relativeTo(root).invariantSeparatorsPath
                into += ListedFile(relative, child.length()) { child.inputStream() }
            }
        }
    }

    fun storage(context: Context, settings: FolderSettings): Storage {
        if (settings.destinationFolder.isBlank()) {
            throw IllegalArgumentException("Folder is not specified.")
        }
        return FolderStorage(context, settings)
    }

    private fun document(context: Context, location: String): DocumentFile? {
        if (!location.startsWith("content:")) return null
        val uri = Uri.parse(location)
        return DocumentFile.fromTreeUri(context, uri) ?: DocumentFile.fromSingleUri(context, uri)
    }

    private fun length(context: Context, uri: Uri, fallback: Long): Long {
        context.contentResolver.openAssetFileDescriptor(uri, "r")?.use { descriptor ->
            if (descriptor.length >= 0) return descriptor.length
        }
        return fallback
    }
}

private class FolderStorage(
    private val context: Context,
    private val settings: FolderSettings,
) : Storage {
    override fun test(): String? {
        return if (LocalFolder.isDirectory(context, settings.destinationFolder)) {
            null
        } else {
            "Folder does not exist."
        }
    }

    override fun exists(relativeFileName: String): Boolean {
        val relative = RemotePaths.normalizeRelative(relativeFileName)
        val document = rootDocument()
        if (document != null) return find(document, relative)?.isFile == true
        return File(settings.destinationFolder, relative).isFile
    }

    override fun upload(relativeFileName: String, length: Long, open: () -> InputStream) {
        val relative = RemotePaths.normalizeRelative(relativeFileName)
        val document = rootDocument()
        if (document != null) {
            val file = ensureFile(document, relative)
            context.contentResolver.openOutputStream(file.uri, "wt")?.use { output ->
                open().use { input -> StorageIo.copy(input, output) }
            } ?: throw IllegalStateException("Cannot write $relative")
            return
        }
        val target = File(settings.destinationFolder, relative)
        target.parentFile?.mkdirs()
        val temporary = File(target.parentFile, target.name + ".tmp." + java.util.UUID.randomUUID().toString().replace("-", ""))
        try {
            temporary.outputStream().use { output -> open().use { input -> StorageIo.copy(input, output) } }
            if (target.exists() && !target.delete()) {
                throw IllegalStateException("Cannot replace $relative")
            }
            if (!temporary.renameTo(target)) {
                temporary.copyTo(target, overwrite = true)
                temporary.delete()
            }
        } catch (error: Exception) {
            temporary.delete()
            throw error
        }
    }

    override fun deleteFolder(relativeFolderName: String) {
        val relative = RemotePaths.normalizeRelative(relativeFolderName, allowEmpty = true)
        val document = rootDocument()
        if (document != null) {
            if (relative.isEmpty()) return
            find(document, relative)?.delete()
            return
        }
        File(settings.destinationFolder, relative).deleteRecursively()
    }

    private fun rootDocument(): DocumentFile? {
        val location = settings.destinationFolder
        if (!location.startsWith("content:")) return null
        val uri = Uri.parse(location)
        return DocumentFile.fromTreeUri(context, uri)
    }

    private fun find(root: DocumentFile, relative: String): DocumentFile? {
        var current = root
        for (part in relative.split('/')) {
            current = current.findFile(part) ?: return null
        }
        return current
    }

    private fun ensureFile(root: DocumentFile, relative: String): DocumentFile {
        val parts = relative.split('/')
        var directory = root
        for (part in parts.dropLast(1)) {
            val existing = directory.findFile(part)
            directory = when {
                existing == null -> directory.createDirectory(part)
                    ?: throw IllegalStateException("Cannot create folder $part")
                existing.isDirectory -> existing
                else -> throw IllegalStateException("$part is a file.")
            }
        }
        val name = parts.last()
        directory.findFile(name)?.delete()
        return directory.createFile("application/octet-stream", name)
            ?: throw IllegalStateException("Cannot create $relative")
    }
}

private val File.invariantSeparatorsPath: String
    get() = path.replace(File.separatorChar, '/')
