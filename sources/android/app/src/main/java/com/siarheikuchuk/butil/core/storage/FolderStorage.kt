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
        return listRemoteFiles(context, location).map { file ->
            ListedFile(file.relativePath, file.length) { open(context, location, file.relativePath) }
        }
    }

    fun listRemoteFiles(context: Context, location: String): List<RemoteFile> {
        requireDirectory(context, location)
        val document = document(context, location)
        val files = mutableListOf<RemoteFile>()
        if (document != null) {
            walkDocumentFiles(document, "", files)
        } else {
            val root = File(location)
            walkFileEntries(root, root, files)
        }
        return files.sortedBy { it.relativePath }
    }

    fun listFolderNames(context: Context, location: String): List<String> {
        requireDirectory(context, location)
        val document = document(context, location)
        if (document != null) {
            return document.listFiles().filter { it.isDirectory }.mapNotNull { it.name }.sorted()
        }
        return File(location).listFiles()?.filter { it.isDirectory }?.map { it.name }?.sorted().orEmpty()
    }

    fun open(context: Context, location: String, relativePath: String): InputStream {
        val document = document(context, location)
        if (document != null) {
            val file = findDocument(document, relativePath) ?: throw IllegalStateException("Cannot read $relativePath")
            return context.contentResolver.openInputStream(file.uri)
                ?: throw IllegalStateException("Cannot read $relativePath")
        }
        return File(location, relativePath).inputStream()
    }

    fun write(context: Context, location: String, relativePath: String, open: () -> InputStream, modifiedUtcMillis: Long = 0L) {
        requireDirectory(context, location)
        val document = document(context, location)
        if (document != null) {
            val file = ensureDocumentFile(document, relativePath)
            context.contentResolver.openOutputStream(file.uri, "wt")?.use { output ->
                open().use { input -> StorageIo.copy(input, output) }
            } ?: throw IllegalStateException("Cannot write $relativePath")
            return
        }
        val target = File(location, relativePath)
        target.parentFile?.mkdirs()
        target.outputStream().use { output -> open().use { input -> StorageIo.copy(input, output) } }
        if (modifiedUtcMillis > 0) target.setLastModified(modifiedUtcMillis)
    }

    fun delete(context: Context, location: String, relativePath: String) {
        val document = document(context, location)
        if (document != null) {
            findDocument(document, relativePath)?.delete()
            return
        }
        File(location, relativePath).delete()
    }

    data class Child(val name: String, val directory: Boolean, val length: Long, val modifiedUtcMillis: Long)

    fun ensureDirectory(context: Context, location: String, relativePath: String) {
        if (relativePath.isBlank()) {
            requireDirectory(context, location)
            return
        }
        val rootDocument = document(context, location)
        if (rootDocument == null) {
            File(location, relativePath).mkdirs()
            return
        }
        var current: DocumentFile = rootDocument
        for (part in relativePath.split('/').filter { it.isNotEmpty() }) {
            val existing = current.findFile(part)
            current = when {
                existing == null -> current.createDirectory(part) ?: throw IllegalStateException("Cannot create $part")
                existing.isDirectory -> existing
                else -> throw IllegalStateException("$part is a file.")
            }
        }
    }

    fun describe(context: Context, location: String, relativePath: String): Child? {
        if (relativePath.isBlank()) return Child("", true, 0, 0)
        val parent = relativePath.substringBeforeLast('/', "")
        val name = relativePath.substringAfterLast('/')
        return children(context, location, parent).firstOrNull { it.name == name }
    }

    fun children(context: Context, location: String, relativePath: String): List<Child> {
        requireDirectory(context, location)
        val document = document(context, location)
        if (document != null) {
            val directory = if (relativePath.isBlank()) document else findDocument(document, relativePath)
            if (directory == null || !directory.isDirectory) return emptyList()
            return directory.listFiles().mapNotNull { child ->
                val name = child.name ?: return@mapNotNull null
                if (name == "." || name == "..") return@mapNotNull null
                Child(name, child.isDirectory, child.length().coerceAtLeast(0), child.lastModified())
            }
        }
        val directory = if (relativePath.isBlank()) File(location) else File(location, relativePath)
        return directory.listFiles().orEmpty().map { child ->
            Child(child.name, child.isDirectory, child.length(), child.lastModified())
        }
    }

    fun exists(context: Context, location: String, relativePath: String): Boolean {
        val document = document(context, location)
        if (document != null) return findDocument(document, relativePath)?.isFile == true
        return File(location, relativePath).isFile
    }

    private fun walkDocumentFiles(directory: DocumentFile, relative: String, into: MutableList<RemoteFile>) {
        for (child in directory.listFiles()) {
            val name = child.name ?: continue
            if (name == "." || name == "..") continue
            val childRelative = if (relative.isEmpty()) name else "$relative/$name"
            if (child.isDirectory) {
                walkDocumentFiles(child, childRelative, into)
            } else if (child.isFile) {
                into += RemoteFile(childRelative, child.length().coerceAtLeast(0), child.lastModified())
            }
        }
    }

    private fun walkFileEntries(root: File, current: File, into: MutableList<RemoteFile>) {
        for (child in current.listFiles() ?: return) {
            if (child.isDirectory) {
                walkFileEntries(root, child, into)
            } else if (child.isFile) {
                into += RemoteFile(child.relativeTo(root).invariantSeparatorsPath, child.length(), child.lastModified())
            }
        }
    }

    private fun findDocument(root: DocumentFile, relative: String): DocumentFile? {
        var current = root
        for (part in relative.split('/')) {
            if (part.isEmpty()) continue
            current = current.findFile(part) ?: return null
        }
        return current
    }

    private fun ensureDocumentFile(root: DocumentFile, relative: String): DocumentFile {
        val parts = relative.split('/').filter { it.isNotEmpty() }
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

    override fun download(relativeFileName: String): InputStream {
        val relative = RemotePaths.normalizeRelative(relativeFileName)
        val document = rootDocument()
        if (document != null) {
            val file = find(document, relative) ?: throw IllegalStateException("File does not exist: $relative")
            return context.contentResolver.openInputStream(file.uri)
                ?: throw IllegalStateException("Cannot read $relative")
        }
        return File(settings.destinationFolder, relative).inputStream()
    }

    override fun delete(relativeFileName: String) {
        val relative = RemotePaths.normalizeRelative(relativeFileName)
        val document = rootDocument()
        if (document != null) {
            find(document, relative)?.delete()
            return
        }
        File(settings.destinationFolder, relative).delete()
    }

    override fun listFiles(): List<RemoteFile> = LocalFolder.listRemoteFiles(context, settings.destinationFolder)

    override fun listFolders(): List<String> = LocalFolder.listFolderNames(context, settings.destinationFolder)

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
