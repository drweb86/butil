package com.siarheikuchuk.butil.core.tasks

import android.content.Context
import com.siarheikuchuk.butil.BuildConfig
import com.siarheikuchuk.butil.core.services.EncryptionService
import com.siarheikuchuk.butil.core.storage.LocalFolder
import com.siarheikuchuk.butil.core.storage.StorageFactory
import org.json.JSONArray
import org.json.JSONObject
import java.io.File

private enum class SyncRelation { NotChanged, Created, Changed, Deleted }
private enum class SyncAction { DoNothing, Update, Delete }

private data class SyncFile(
    val relativeFileName: String,
    val modifiedAtUtcMillis: Long,
    val sha512: String,
    val size: Long,
) {
    fun sameContent(other: SyncFile?): Boolean {
        if (other == null) return false
        return size == other.size && sha512.equals(other.sha512, ignoreCase = true)
    }
}

private class SyncItem(val relativeFileName: String) {
    var actual: SyncFile? = null
    var local: SyncFile? = null
    var remote: SyncFile? = null
    var actualToLocal = SyncRelation.NotChanged
    var remoteToLocal = SyncRelation.NotChanged
    var actualAction = SyncAction.DoNothing
    var remoteAction = SyncAction.DoNothing
    var forceUpdateState = false
    val existsLocally: Boolean get() = actual != null
}

class SynchronizationRunner(private val context: Context) {
    fun run(task: SynchronizationTask, onProgress: (String) -> Unit, cancelled: () -> Boolean): String? {
        if (task.password.isBlank()) return "Password is not specified."
        if (!task.repositorySubfolder.isNullOrBlank()) {
            return "Repository subfolder is no longer supported. Edit the task and clear it."
        }
        if (!LocalFolder.isDirectory(context, task.localFolder)) return "Local folder does not exist."
        val work = File(context.cacheDir, "sync-" + System.nanoTime()).apply { mkdirs() }
        try {
            return StorageFactory.open(context, task.storage, autodetect = false).use { storage ->
                onProgress("Reading synchronization state.")
                val remoteBackup = BackupStateCodec.read(storage, task.password, work)
                if (task.synchronizationMode == SynchronizationTask.MODE_TWO_WAY) removeUnknownVersions(storage, remoteBackup)
                val localState = if (remoteBackup.versions.isEmpty()) emptyList() else loadLocal(task)
                val actual = scanLocal(task, work, cancelled) ?: return "Synchronization cancelled."
                val remoteFiles = remoteFiles(remoteBackup)
                val decisions = decide(task.synchronizationMode, localState, actual, remoteFiles)
                var changed = false
                decisions.filter { it.actualAction != SyncAction.DoNothing }.forEach { item ->
                    if (cancelled()) return "Synchronization cancelled."
                    changed = true
                    when (item.actualAction) {
                        SyncAction.Delete -> {
                            onProgress("Deleting local ${item.relativeFileName}")
                            LocalFolder.delete(context, task.localFolder, item.relativeFileName.replace('\\', '/'))
                        }
                        SyncAction.Update -> {
                            val remote = item.remote ?: return@forEach
                            val stored = storedFile(remoteBackup, remote) ?: return "Remote file is missing from backup state: ${item.relativeFileName}"
                            onProgress("Downloading ${item.relativeFileName}")
                            val plain = extract(storage, stored, task.password, work, "down")
                            LocalFolder.write(
                                context,
                                task.localFolder,
                                item.relativeFileName.replace('\\', '/'),
                                { plain.inputStream() },
                                remote.modifiedAtUtcMillis,
                            )
                            plain.delete()
                        }
                        SyncAction.DoNothing -> Unit
                    }
                }
                val uploads = decisions.filter { it.remoteAction == SyncAction.Update && it.actual != null }
                val deletes = decisions.filter { it.remoteAction == SyncAction.Delete }
                if (task.synchronizationMode == SynchronizationTask.MODE_TWO_WAY && (uploads.isNotEmpty() || deletes.isNotEmpty())) {
                    changed = true
                    publishRemote(task, storage, remoteBackup, uploads, deletes, work, onProgress, cancelled)?.let { return it }
                }
                if (changed || decisions.any { it.forceUpdateState }) {
                    val fresh = scanLocal(task, work) { false } ?: return "Synchronization cancelled."
                    saveLocal(task, syncedLocalState(task.synchronizationMode, localState, fresh, remoteFiles(BackupStateCodec.read(storage, task.password, work))))
                }
                onProgress("Synchronization finished.")
                null
            }
        } finally {
            work.deleteRecursively()
        }
    }

    private fun publishRemote(
        task: SynchronizationTask,
        storage: com.siarheikuchuk.butil.core.storage.Storage,
        state: BackupState,
        uploads: List<SyncItem>,
        deletes: List<SyncItem>,
        work: File,
        onProgress: (String) -> Unit,
        cancelled: () -> Boolean,
    ): String? {
        val source = BackupSourceItem(BackupNames.EMPTY_ID, BackupNames.SYNC_TARGET, true)
        val versionMillis = System.currentTimeMillis()
        val created = mutableListOf<BackupStorageFile>()
        val updated = mutableListOf<BackupStorageFile>()
        val previous = state.lastStates.firstOrNull { it.sourceItem.id == BackupNames.EMPTY_ID }
        uploads.forEachIndexed { index, item ->
            if (cancelled()) return "Synchronization cancelled."
            val actual = item.actual ?: return@forEachIndexed
            val fullName = virtualName(actual.relativeFileName)
            val fileState = BackupFileState(fullName, actual.modifiedAtUtcMillis, actual.size, actual.sha512)
            val storageFile = BackupStorageFile(fileState)
            val existed = previous?.files?.any { it.fileName == fullName } == true
            if (existed) updated += storageFile else created += storageFile
            val existing = BackupStateCodec.findStored(state, fileState, ignoreLastVersion = false)
            if (existing != null) {
                storageFile.copyStorageFrom(existing)
                onProgress("Already stored (${index + 1}/${uploads.size}): ${item.relativeFileName}")
                return@forEachIndexed
            }
            onProgress("Uploading (${index + 1}/${uploads.size}): ${item.relativeFileName}")
            val plain = materialize(work, "up") {
                LocalFolder.open(context, task.localFolder, item.relativeFileName.replace('\\', '/'))
            }
            val encrypted = File(work, "up.aes")
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
            plain.delete()
            encrypted.delete()
        }
        val deletedNames = deletes.map { virtualName(it.relativeFileName) }
        if (created.isEmpty() && updated.isEmpty() && deletedNames.isEmpty()) return null
        val version = BackupVersion(
            versionMillis,
            mutableListOf(BackupSourceChanges(source, deletedNames.toMutableList(), updated, created)),
        )
        state.versions += version
        val replaced = (created + updated).map { it.file.fileName }.toSet()
        val nextFiles = (previous?.files.orEmpty().filter { old -> old.fileName !in deletedNames && old.fileName !in replaced } +
            (created + updated).map { it.file }).toMutableList()
        state.lastStates.removeAll { it.sourceItem.id == BackupNames.EMPTY_ID }
        state.lastStates += BackupSourceState(source, nextFiles)
        onProgress("Writing synchronization state.")
        BackupStateCodec.write(storage, task.password, state, work)
        return null
    }

    private fun storedFile(state: BackupState, remote: SyncFile): BackupStorageFile? {
        val source = BackupSourceItem(BackupNames.EMPTY_ID, BackupNames.SYNC_TARGET, true)
        val version = state.versions.maxByOrNull { it.backupDateUtcMillis } ?: return null
        return BackupStateCodec.filesAt(state, source, version).firstOrNull { stored ->
            val relative = BackupNames.relativeTo(BackupNames.SYNC_TARGET, stored.file.fileName)
            relative.equals(remote.relativeFileName, ignoreCase = true) ||
                (stored.file.size == remote.size && stored.file.sha512.equals(remote.sha512, ignoreCase = true))
        }
    }

    private fun virtualName(relative: String): String = "x:\\" + relative.replace('/', '\\').trimStart('\\')

    private fun scanLocal(task: SynchronizationTask, work: File, cancelled: () -> Boolean): List<SyncFile>? {
        val files = mutableListOf<SyncFile>()
        LocalFolder.listRemoteFiles(context, task.localFolder).forEach { remote ->
            if (cancelled()) return null
            val temp = materialize(work, "local") { LocalFolder.open(context, task.localFolder, remote.relativePath) }
            files += SyncFile(
                relativeFileName = remote.relativePath.replace('/', '\\'),
                modifiedAtUtcMillis = remote.modifiedUtcMillis,
                sha512 = FileHash.sha512(temp),
                size = temp.length(),
            )
            temp.delete()
        }
        return files
    }

    private fun remoteFiles(state: BackupState): List<SyncFile> {
        val source = state.lastStates.firstOrNull { it.sourceItem.id == BackupNames.EMPTY_ID && it.sourceItem.isFolder }
            ?: return emptyList()
        return source.files.map { file ->
            SyncFile(
                BackupNames.relativeTo(source.sourceItem.target, file.fileName).replace('/', '\\'),
                file.lastWriteUtcMillis,
                file.sha512,
                file.size,
            )
        }
    }

    private fun syncedLocalState(
        mode: Int,
        previousLocal: List<SyncFile>,
        actual: List<SyncFile>,
        remote: List<SyncFile>,
    ): List<SyncFile> {
        return decide(mode, previousLocal, actual, remote)
            .filter { it.actualAction == SyncAction.DoNothing && it.remoteAction == SyncAction.DoNothing && it.remote != null && it.existsLocally }
            .mapNotNull { it.actual }
    }

    private fun decide(mode: Int, local: List<SyncFile>, actual: List<SyncFile>, remote: List<SyncFile>): List<SyncItem> {
        val items = linkedMapOf<String, SyncItem>()
        fun add(files: List<SyncFile>, assign: (SyncItem, SyncFile) -> Unit) {
            files.forEach { file ->
                val item = items.getOrPut(file.relativeFileName) { SyncItem(file.relativeFileName) }
                assign(item, file)
            }
        }
        add(actual) { item, file -> item.actual = file }
        add(local) { item, file -> item.local = file }
        add(remote) { item, file -> item.remote = file }
        items.values.forEach { item ->
            val actualFile = item.actual
            val remoteFile = item.remote
            item.actualToLocal = relation(actualFile, item.local)
            item.remoteToLocal = relation(remoteFile, item.local)
            if (actualFile != null && remoteFile != null && actualFile.sameContent(remoteFile)) {
                item.forceUpdateState = !remoteFile.sameContent(item.local)
                return@forEach
            }
            if (mode == SynchronizationTask.MODE_TWO_WAY) resolveTwoWay(item) else resolveRead(item)
        }
        return items.values.toList()
    }

    private fun relation(primary: SyncFile?, secondary: SyncFile?): SyncRelation {
        if (primary == null) return if (secondary == null) SyncRelation.NotChanged else SyncRelation.Deleted
        if (secondary == null) return SyncRelation.Created
        return if (primary.sameContent(secondary)) SyncRelation.NotChanged else SyncRelation.Changed
    }

    private fun resolveRead(item: SyncItem) {
        fun apply(relation: SyncRelation, changed: SyncAction, deleted: SyncAction = SyncAction.DoNothing, created: SyncAction = SyncAction.Update) {
            item.actualAction = when (relation) {
                SyncRelation.NotChanged -> SyncAction.DoNothing
                SyncRelation.Created, SyncRelation.Changed -> created
                SyncRelation.Deleted -> deleted
            }
        }
        when (item.actualToLocal) {
            SyncRelation.NotChanged -> apply(item.remoteToLocal, SyncAction.Update, SyncAction.Delete)
            SyncRelation.Created -> when (item.remoteToLocal) {
                SyncRelation.NotChanged -> item.actualAction = SyncAction.DoNothing
                SyncRelation.Created -> item.actualAction = SyncAction.Update
                else -> Unit
            }
            SyncRelation.Changed -> when (item.remoteToLocal) {
                SyncRelation.NotChanged, SyncRelation.Changed -> item.actualAction = SyncAction.Update
                SyncRelation.Deleted -> item.actualAction = SyncAction.Delete
                SyncRelation.Created -> Unit
            }
            SyncRelation.Deleted -> when (item.remoteToLocal) {
                SyncRelation.NotChanged, SyncRelation.Changed -> item.actualAction = SyncAction.Update
                SyncRelation.Deleted -> item.actualAction = SyncAction.DoNothing
                SyncRelation.Created -> Unit
            }
        }
    }

    private fun resolveTwoWay(item: SyncItem) {
        val actualFile = item.actual
        val remoteFile = item.remote
        val actualRelation = item.actualToLocal
        val remoteRelation = item.remoteToLocal
        if (item.existsLocally && item.remote == null && item.local == null &&
            actualRelation == SyncRelation.Created && remoteRelation == SyncRelation.NotChanged
        ) {
            item.remoteAction = SyncAction.Update
            return
        }
        if (item.existsLocally && item.remote != null && item.local != null &&
            actualRelation == SyncRelation.NotChanged && remoteRelation == SyncRelation.NotChanged
        ) {
            return
        }
        if (item.existsLocally && item.remote != null && item.local != null &&
            actualRelation == SyncRelation.NotChanged && remoteRelation == SyncRelation.Changed
        ) {
            item.actualAction = SyncAction.Update
            return
        }
        if (!item.existsLocally && item.remote != null &&
            actualRelation == SyncRelation.Deleted && remoteRelation == SyncRelation.NotChanged
        ) {
            item.remoteAction = SyncAction.Delete
            return
        }
        if (item.existsLocally && actualRelation == SyncRelation.NotChanged && remoteRelation == SyncRelation.Deleted) {
            item.actualAction = SyncAction.Delete
            return
        }
        if (remoteFile != null && actualFile != null && item.existsLocally && actualRelation == SyncRelation.Created &&
            (remoteRelation == SyncRelation.Created || remoteRelation == SyncRelation.Changed)
        ) {
            if (remoteFile.modifiedAtUtcMillis > actualFile.modifiedAtUtcMillis) item.actualAction = SyncAction.Update
            else item.remoteAction = SyncAction.Update
            return
        }
        if (item.existsLocally && actualRelation == SyncRelation.Created && remoteRelation == SyncRelation.Deleted) {
            item.remoteAction = SyncAction.Update
        }
        if (item.existsLocally && actualRelation == SyncRelation.Changed && remoteRelation == SyncRelation.NotChanged) {
            item.remoteAction = SyncAction.Update
            return
        }
        if (remoteFile != null && actualFile != null && item.existsLocally &&
            actualRelation == SyncRelation.Changed && remoteRelation == SyncRelation.Changed
        ) {
            if (remoteFile.modifiedAtUtcMillis > actualFile.modifiedAtUtcMillis) item.actualAction = SyncAction.Update
            else item.remoteAction = SyncAction.Update
            return
        }
        if (item.existsLocally && actualRelation == SyncRelation.Changed && remoteRelation == SyncRelation.Deleted) {
            item.remoteAction = SyncAction.Update
            return
        }
        if (!item.existsLocally && actualRelation == SyncRelation.Deleted && remoteRelation == SyncRelation.Changed) {
            item.actualAction = SyncAction.Update
            return
        }
        if (!item.existsLocally && actualRelation == SyncRelation.Deleted && remoteRelation == SyncRelation.Deleted) {
            item.forceUpdateState = true
            return
        }
        if (!item.existsLocally && actualRelation == SyncRelation.NotChanged && remoteRelation == SyncRelation.Created) {
            item.actualAction = SyncAction.Update
        }
    }

    private fun stateFolder(): File {
        val name = if (BuildConfig.DEBUG) "BUtil-Development/States" else "BUtil/States"
        return File(context.filesDir, name).apply { mkdirs() }
    }

    private fun loadLocal(task: SynchronizationTask): List<SyncFile> {
        val file = File(stateFolder(), "${task.name}-SynchronizationLocalState.v1.json")
        if (!file.isFile) return emptyList()
        val root = JSONObject(file.readText())
        if (root.optString("LocalFolder") != task.localFolder) return emptyList()
        if (root.optString("RepositorySubfolder") != task.repositorySubfolder.orEmpty() &&
            !(root.isNull("RepositorySubfolder") && task.repositorySubfolder.isNullOrBlank())
        ) {
            return emptyList()
        }
        val entries = root.optJSONObject("SynchronizationState")?.optJSONArray("FileSystemEntries") ?: return emptyList()
        return (0 until entries.length()).map { index ->
            val item = entries.getJSONObject(index)
            SyncFile(
                item.optString("RelativeFileName"),
                BackupClock.parse(item.optString("ModifiedAtUtc")),
                item.optString("Sha512"),
                item.optLong("Size"),
            )
        }
    }

    private fun saveLocal(task: SynchronizationTask, files: List<SyncFile>) {
        val entries = JSONArray()
        files.forEach { file ->
            entries.put(
                JSONObject()
                    .put("RelativeFileName", file.relativeFileName.replace('/', '\\'))
                    .put("ModifiedAtUtc", BackupClock.format(file.modifiedAtUtcMillis))
                    .put("Sha512", file.sha512)
                    .put("Size", file.size),
            )
        }
        val root = JSONObject()
            .put("LocalFolder", task.localFolder)
            .put("RepositorySubfolder", task.repositorySubfolder ?: JSONObject.NULL)
            .put("SynchronizationState", JSONObject().put("FileSystemEntries", entries))
        File(stateFolder(), "${task.name}-SynchronizationLocalState.v1.json").writeText(root.toString(2))
    }
}
