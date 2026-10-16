package com.siarheikuchuk.butil.core.tasks

import android.content.Context
import com.siarheikuchuk.butil.core.services.CompressionService
import com.siarheikuchuk.butil.core.services.EncryptionService
import com.siarheikuchuk.butil.core.storage.LocalFolder
import com.siarheikuchuk.butil.core.storage.Storage
import com.siarheikuchuk.butil.core.storage.StorageFactory
import java.io.File

class IncrementalBackupRunner(private val context: Context) {
    fun run(task: IncrementalBackupTask, onProgress: (String) -> Unit, cancelled: () -> Boolean): String? {
        if (task.password.isBlank()) return "Password is not specified."
        if (task.items.isEmpty()) return "Add at least one folder."
        task.items.forEach { item ->
            if (!LocalFolder.isDirectory(context, item.target)) return "Folder does not exist: ${item.target}"
        }
        val work = File(context.cacheDir, "incremental-" + System.nanoTime()).apply { mkdirs() }
        try {
            return StorageFactory.open(context, task.storage, autodetect = false).use { storage ->
                onProgress("Reading backup state.")
                val state = BackupStateCodec.read(storage, task.password, work)
                val scanned = scan(task, work, onProgress, cancelled) ?: return "Backup cancelled."
                val versionMillis = System.currentTimeMillis()
                val changes = diff(state, scanned)
                if (changes.none { it.createdFiles.isNotEmpty() || it.updatedFiles.isNotEmpty() || it.deletedFiles.isNotEmpty() }) {
                    onProgress("No changes since the last backup.")
                    return null
                }
                val version = BackupVersion(versionMillis, changes.toMutableList())
                state.versions += version
                val limit = if (task.storage.quotaGb > 0) task.storage.quotaGb * 1024L * 1024L * 1024L else Long.MAX_VALUE
                var sent = 0L
                var quotaReached = false
                val skipped = mutableSetOf<String>()
                val uploads = changes.flatMap { change ->
                    (change.createdFiles + change.updatedFiles).map { change.sourceItem to it }
                }
                uploads.forEachIndexed { index, (source, storageFile) ->
                    if (cancelled()) return "Backup cancelled."
                    val position = "${index + 1}/${uploads.size}"
                    val existing = BackupStateCodec.findStored(state, storageFile.file, ignoreLastVersion = true)
                    if (existing != null) {
                        storageFile.copyStorageFrom(existing)
                        onProgress("Already stored ($position): ${friendly(source, storageFile.file.fileName)}")
                        return@forEachIndexed
                    }
                    if (quotaReached || sent + storageFile.file.size > limit) {
                        quotaReached = true
                        skipped += storageFile.file.fileName
                        onProgress("Skipped because the upload limit was reached ($position): ${friendly(source, storageFile.file.fileName)}")
                        return@forEachIndexed
                    }
                    onProgress("Saving ($position): ${friendly(source, storageFile.file.fileName)}")
                    val plain = scanned.first { it.source.samePlace(source) }.temps.getValue(storageFile.file.fileName)
                    uploadEncrypted(storage, storageFile, plain, versionMillis, work)
                    sent += storageFile.file.size
                    if (sent >= limit) quotaReached = true
                }
                changes.forEach { change ->
                    change.createdFiles.removeAll { skipped.contains(it.file.fileName) }
                    change.updatedFiles.removeAll { skipped.contains(it.file.fileName) }
                }
                state.lastStates.clear()
                scanned.forEach { snapshot ->
                    val source = snapshot.source
                    val files = snapshot.files
                    state.lastStates += BackupSourceState(
                        source,
                        files.filter { !skipped.contains(it.fileName) }.toMutableList(),
                    )
                }
                if (changes.all { it.createdFiles.isEmpty() && it.updatedFiles.isEmpty() && it.deletedFiles.isEmpty() }) {
                    state.versions.remove(version)
                    onProgress("No files were saved.")
                    return null
                }
                onProgress("Writing backup state.")
                BackupStateCodec.write(storage, task.password, state, work)
                removeUnknownVersions(storage, state)
                if (quotaReached) "Finished. Some files were skipped because the upload limit was reached." else null
            }
        } finally {
            work.deleteRecursively()
        }
    }

    fun versions(task: IncrementalBackupTask): List<BackupVersion> {
        val work = File(context.cacheDir, "restore-list-" + System.nanoTime()).apply { mkdirs() }
        try {
            return StorageFactory.open(context, task.storage, autodetect = false).use { storage ->
                BackupStateCodec.read(storage, task.password, work).versions
            }
        } finally {
            work.deleteRecursively()
        }
    }

    fun restore(
        storageSettings: com.siarheikuchuk.butil.core.storage.StorageSettings,
        password: String,
        versionMillis: Long,
        destination: String,
        onProgress: (String) -> Unit,
        cancelled: () -> Boolean,
    ): String? {
        if (password.isBlank()) return "Password is not specified."
        if (!LocalFolder.isDirectory(context, destination)) return "Destination folder does not exist."
        val work = File(context.cacheDir, "restore-" + System.nanoTime()).apply { mkdirs() }
        try {
            return StorageFactory.open(context, storageSettings, autodetect = false).use { storage ->
                val state = BackupStateCodec.read(storage, password, work)
                val version = state.versions.firstOrNull { it.backupDateUtcMillis == versionMillis }
                    ?: return "The selected version is not in this storage."
                val sourceItems = version.changes.map { it.sourceItem }.ifEmpty {
                    state.versions.flatMap { it.changes.map { change -> change.sourceItem } }.distinctBy { it.target }
                }
                var count = 0
                sourceItems.distinctBy { it.target to it.isFolder }.forEach { source ->
                    val files = BackupStateCodec.filesAt(state, source, version)
                    files.forEach { storageFile ->
                        if (cancelled()) return "Restore cancelled."
                        val relative = BackupNames.relativeTo(source.target, storageFile.file.fileName).replace('\\', '/')
                        if (relative.isBlank()) return@forEach
                        count++
                        onProgress("Restoring $relative")
                        val plain = extract(storage, storageFile, password, work, "restored-$count")
                        val actual = FileHash.sha512(plain)
                        if (storageFile.file.sha512.isNotBlank() && !actual.equals(storageFile.file.sha512, ignoreCase = true)) {
                            return "Restored content for \"$relative\" does not match its SHA-512."
                        }
                        if (storageFile.file.size != 0L && plain.length() != storageFile.file.size) {
                            return "Restored content for \"$relative\" has an unexpected size."
                        }
                        LocalFolder.write(context, destination, relative, { plain.inputStream() }, storageFile.file.lastWriteUtcMillis)
                    }
                }
                onProgress(if (count == 0) "This version has no files." else "Restored $count files.")
                null
            }
        } finally {
            work.deleteRecursively()
        }
    }

    private data class Snapshot(
        val source: BackupSourceItem,
        val files: List<BackupFileState>,
        val temps: Map<String, File>,
    )

    private fun scan(
        task: IncrementalBackupTask,
        work: File,
        onProgress: (String) -> Unit,
        cancelled: () -> Boolean,
    ): List<Snapshot>? {
        val result = mutableListOf<Snapshot>()
        task.items.forEach { item ->
            if (cancelled()) return null
            onProgress("Scanning ${item.target}")
            val source = BackupSourceItem(item.id, item.target, true)
            val files = mutableListOf<BackupFileState>()
            val temps = mutableMapOf<String, File>()
            LocalFolder.listRemoteFiles(context, item.target).forEach { remote ->
                if (cancelled()) return null
                if (ExcludePatterns.excluded(item.target, remote.relativePath, task.fileExcludePatterns)) return@forEach
                val local = materialize(work, "plain-${temps.size}") {
                    LocalFolder.open(context, item.target, remote.relativePath)
                }
                val state = BackupFileState(
                    fileName = BackupNames.join(item.target, remote.relativePath),
                    lastWriteUtcMillis = remote.modifiedUtcMillis,
                    size = local.length(),
                    sha512 = FileHash.sha512(local),
                )
                files += state
                temps[state.fileName] = local
            }
            result += Snapshot(source, files, temps)
        }
        return result
    }

    private fun diff(
        state: BackupState,
        scanned: List<Snapshot>,
    ): List<BackupSourceChanges> {
        return scanned.map { (source, files, _) ->
            val previous = state.lastStates.firstOrNull { it.sourceItem.samePlace(source) }
            if (previous == null) {
                BackupSourceChanges(source, mutableListOf(), mutableListOf(), files.map { BackupStorageFile(it) }.toMutableList())
            } else {
                val created = files.toMutableList()
                val updated = mutableListOf<BackupFileState>()
                val deleted = previous.files.toMutableList()
                for (current in files) {
                    val old = previous.files.firstOrNull { it.fileName == current.fileName } ?: continue
                    deleted.remove(old)
                    created.remove(current)
                    if (!old.sameContent(current) || old.lastWriteUtcMillis != current.lastWriteUtcMillis) updated += current
                }
                BackupSourceChanges(
                    source,
                    deleted.map { it.fileName }.toMutableList(),
                    updated.map { BackupStorageFile(it) }.toMutableList(),
                    created.map { BackupStorageFile(it) }.toMutableList(),
                )
            }
        }
    }

    private fun uploadEncrypted(storage: Storage, storageFile: BackupStorageFile, plain: File, versionMillis: Long, work: File) {
        val encrypted = File(work, "upload.aes")
        val password = FileHash.randomPassword()
        EncryptionService().encryptAes256File(plain.absolutePath, encrypted.absolutePath, password)
        val relative = BackupNames.storageFileName(versionMillis)
        storage.upload(relative, encrypted.length()) { encrypted.inputStream() }
        storageFile.storagePassword = password
        storageFile.storageMethod = BackupNames.AES
        storageFile.storageRelativeFileName = relative
        storageFile.storageFileName = relative
        storageFile.storageFileNameSize = encrypted.length()
        storageFile.storageIntegrityMethod = BackupNames.SHA512
        storageFile.storageIntegrityMethodInfo = FileHash.sha512(encrypted)
        encrypted.delete()
    }

    private fun friendly(source: BackupSourceItem, fileName: String): String {
        return BackupNames.relativeTo(source.target, fileName).ifBlank { fileName }
    }
}

fun extract(
    storage: Storage,
    storageFile: BackupStorageFile,
    fallbackPassword: String,
    directory: File,
    name: String,
): File {
    val downloaded = File(directory, "$name.download")
    val decrypted = File(directory, "$name.dec")
    val plain = File(directory, name)
    val remoteName = storageFile.locatedName()
    if (remoteName.isBlank()) throw IllegalStateException("Stored file has no remote name.")
    storage.download(remoteName).use { input -> downloaded.outputStream().use { output -> input.copyTo(output) } }
    val password = storageFile.storagePassword.ifBlank { fallbackPassword }
    when (storageFile.storageMethod) {
        BackupNames.BROTLI_AES -> {
            EncryptionService().decryptAes256File(downloaded.absolutePath, decrypted.absolutePath, password)
            CompressionService().decompressBrotliFile(decrypted.absolutePath, plain.absolutePath)
        }
        BackupNames.AES, "" -> EncryptionService().decryptAes256File(downloaded.absolutePath, plain.absolutePath, password)
        else -> throw IllegalArgumentException("Unsupported storage method ${storageFile.storageMethod}")
    }
    downloaded.delete()
    decrypted.delete()
    return plain
}

fun removeUnknownVersions(storage: Storage, state: BackupState) {
    val allowed = state.versions.map { BackupClock.versionFolder(it.backupDateUtcMillis) }.toSet()
    storage.listFolders().filter { BackupClock.isVersionFolder(it) && it !in allowed }.forEach { folder ->
        storage.deleteFolder(folder)
    }
}
