package com.siarheikuchuk.butil.core.tasks

import com.siarheikuchuk.butil.core.services.CompressionService
import com.siarheikuchuk.butil.core.services.EncryptionService
import com.siarheikuchuk.butil.core.storage.Storage
import org.json.JSONArray
import org.json.JSONObject
import java.io.File
import java.security.MessageDigest
import java.security.SecureRandom
import java.time.Instant
import java.time.ZoneOffset
import java.time.format.DateTimeFormatter
import java.util.UUID

object BackupClock {
    private val folderFormat = DateTimeFormatter.ofPattern("yyyy-MM-dd-'T'-HH-mm-ss").withZone(ZoneOffset.UTC)
    private val dateFormat = DateTimeFormatter.ofPattern("yyyy-MM-dd'T'HH:mm:ss").withZone(ZoneOffset.UTC)

    fun format(millis: Long): String {
        val fraction = (Math.floorMod(millis, 1000L)).toString().padStart(3, '0')
        return dateFormat.format(Instant.ofEpochMilli(millis)) + ".$fraction" + "0000Z"
    }

    fun parse(value: String): Long {
        val text = value.trim()
        if (text.isEmpty()) return 0L
        val zoned = if (text.endsWith("Z") || text.contains("+") || hasOffset(text)) text else text + "Z"
        return Instant.parse(zoned).toEpochMilli()
    }

    fun versionFolder(millis: Long): String = folderFormat.format(Instant.ofEpochMilli(millis))

    fun isVersionFolder(name: String): Boolean = VERSION_FOLDER.matches(name)

    private fun hasOffset(text: String): Boolean {
        val time = text.indexOf('T')
        if (time < 0) return false
        return text.lastIndexOf('-') > time
    }

    private val VERSION_FOLDER = Regex("\\d{4}-\\d{2}-\\d{2}-T-\\d{2}-\\d{2}-\\d{2}")
}

object BackupNames {
    const val STATE_FILE = "State.brotli.aes256v1"
    const val AES = "Encrypted AES256"
    const val BROTLI_AES = "Brotli Compressed, Encrypted AES256"
    const val SHA512 = "SHA512"
    const val EMPTY_ID = "00000000-0000-0000-0000-000000000000"
    const val SYNC_TARGET = "x:\\"

    fun storageFileName(versionMillis: Long): String {
        return BackupClock.versionFolder(versionMillis) + "/" + UUID.randomUUID().toString() + ".aes256v1"
    }

    fun join(folder: String, relative: String): String {
        val root = folder.trimEnd('/', '\\')
        val child = relative.trimStart('/', '\\')
        return if (child.isEmpty()) root else "$root/$child"
    }

    fun relativeTo(folder: String, fileName: String): String {
        val root = folder.trimEnd('\\', '/')
        val file = fileName.replace('/', '\\')
        val trimmed = file.trimStart('\\')
        val prefixed = listOf(file, trimmed, "\\$trimmed")
        for (candidate in prefixed) {
            val normalized = candidate.trimStart('\\')
            if (normalized.equals(root, ignoreCase = true)) return ""
            val prefix = root + "\\"
            if (normalized.startsWith(prefix, ignoreCase = true)) {
                return normalized.substring(prefix.length)
            }
        }
        if (file.length > root.length + 1 && file.startsWith(root, ignoreCase = true)) {
            return file.substring(root.length + 1).trimStart('\\')
        }
        return file.trimStart('\\')
    }
}

object FileHash {
    fun sha512(file: File): String {
        val digest = MessageDigest.getInstance("SHA-512")
        file.inputStream().use { input ->
            val buffer = ByteArray(1024 * 128)
            while (true) {
                val read = input.read(buffer)
                if (read < 0) break
                digest.update(buffer, 0, read)
            }
        }
        return digest.digest().joinToString("") { "%02x".format(it) }
    }

    fun randomPassword(): String {
        val alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789&?%$@"
        val bytes = ByteArray(255)
        SecureRandom().nextBytes(bytes)
        return buildString(255) {
            bytes.forEach { byte -> append(alphabet[(byte.toInt() and 0xFF) % alphabet.length]) }
        }
    }
}

data class BackupFileState(
    val fileName: String,
    val lastWriteUtcMillis: Long,
    val size: Long,
    val sha512: String,
) {
    fun sameContent(other: BackupFileState): Boolean = size == other.size && sha512.equals(other.sha512, ignoreCase = true)
}

data class BackupStorageFile(
    val file: BackupFileState,
    var storageRelativeFileName: String = "",
    var storageFileName: String = "",
    var storageFileNameSize: Long = 0,
    var storageMethod: String = "",
    var storageIntegrityMethod: String = "",
    var storageIntegrityMethodInfo: String = "",
    var storagePassword: String = "",
) {
    fun copyStorageFrom(other: BackupStorageFile) {
        storageRelativeFileName = other.storageRelativeFileName
        storageFileName = other.storageFileName
        storageFileNameSize = other.storageFileNameSize
        storageMethod = other.storageMethod
        storageIntegrityMethod = other.storageIntegrityMethod
        storageIntegrityMethodInfo = other.storageIntegrityMethodInfo
        storagePassword = other.storagePassword
    }

    fun locatedName(): String = storageRelativeFileName.ifBlank { storageFileName }
}

data class BackupSourceItem(val id: String, val target: String, val isFolder: Boolean) {
    fun samePlace(other: BackupSourceItem): Boolean = target == other.target && isFolder == other.isFolder
}

data class BackupSourceChanges(
    val sourceItem: BackupSourceItem,
    val deletedFiles: MutableList<String>,
    val updatedFiles: MutableList<BackupStorageFile>,
    val createdFiles: MutableList<BackupStorageFile>,
)

data class BackupVersion(
    val backupDateUtcMillis: Long,
    val changes: MutableList<BackupSourceChanges>,
)

data class BackupSourceState(
    val sourceItem: BackupSourceItem,
    val files: MutableList<BackupFileState>,
)

data class BackupState(
    val versions: MutableList<BackupVersion> = mutableListOf(),
    val lastStates: MutableList<BackupSourceState> = mutableListOf(),
)

object BackupStateCodec {
    const val AES_EXTENSION = "aes256v1"

    fun read(storage: Storage, password: String, directory: File): BackupState {
        if (!storage.exists(BackupNames.STATE_FILE)) return BackupState()
        val encrypted = File(directory, "state.aes")
        val brotli = File(directory, "state.brotli")
        val json = File(directory, "state.json")
        storage.download(BackupNames.STATE_FILE).use { input -> encrypted.outputStream().use { output -> input.copyTo(output) } }
        EncryptionService().decryptAes256File(encrypted.absolutePath, brotli.absolutePath, password)
        CompressionService().decompressBrotliFile(brotli.absolutePath, json.absolutePath)
        return parse(json.readText())
    }

    fun write(storage: Storage, password: String, state: BackupState, directory: File): BackupStorageFile {
        if (storage.exists(BackupNames.STATE_FILE)) storage.delete(BackupNames.STATE_FILE)
        val json = File(directory, "state.json")
        val brotli = File(directory, "state.brotli")
        val encrypted = File(directory, "state.aes")
        json.writeText(toJson(state))
        CompressionService().compressBrotliFile(json.absolutePath, brotli.absolutePath)
        EncryptionService().encryptAes256File(brotli.absolutePath, encrypted.absolutePath, password)
        storage.upload(BackupNames.STATE_FILE, encrypted.length()) { encrypted.inputStream() }
        return BackupStorageFile(
            file = BackupFileState(BackupNames.STATE_FILE, 0, json.length(), ""),
            storageRelativeFileName = BackupNames.STATE_FILE,
            storageFileName = BackupNames.STATE_FILE,
            storageFileNameSize = encrypted.length(),
            storageMethod = BackupNames.BROTLI_AES,
            storageIntegrityMethod = BackupNames.SHA512,
            storageIntegrityMethodInfo = FileHash.sha512(encrypted),
        )
    }

    fun toJson(state: BackupState): String {
        val versions = JSONArray()
        state.versions.forEach { version ->
            val changes = JSONArray()
            version.changes.forEach { change -> changes.put(changeJson(change)) }
            versions.put(
                JSONObject()
                    .put("BackupDateUtc", BackupClock.format(version.backupDateUtcMillis))
                    .put("SourceItemChanges", changes),
            )
        }
        val last = JSONArray()
        state.lastStates.forEach { item ->
            val files = JSONArray()
            item.files.forEach { files.put(fileJson(it)) }
            last.put(JSONObject().put("SourceItem", sourceJson(item.sourceItem)).put("FileStates", files))
        }
        return JSONObject().put("VersionStates", versions).put("LastSourceItemStates", last).toString(2)
    }

    fun parse(json: String): BackupState {
        val root = JSONObject(json)
        val state = BackupState()
        val versions = root.optJSONArray("VersionStates")
        if (versions != null) {
            for (i in 0 until versions.length()) {
                val version = versions.getJSONObject(i)
                val changes = mutableListOf<BackupSourceChanges>()
                val array = version.optJSONArray("SourceItemChanges")
                if (array != null) {
                    for (c in 0 until array.length()) changes += change(array.getJSONObject(c))
                }
                state.versions += BackupVersion(BackupClock.parse(version.optString("BackupDateUtc")), changes)
            }
        }
        val last = root.optJSONArray("LastSourceItemStates")
        if (last != null) {
            for (i in 0 until last.length()) {
                val item = last.getJSONObject(i)
                val files = mutableListOf<BackupFileState>()
                val array = item.optJSONArray("FileStates")
                if (array != null) {
                    for (f in 0 until array.length()) files += fileState(array.getJSONObject(f))
                }
                state.lastStates += BackupSourceState(sourceItem(item.getJSONObject("SourceItem")), files)
            }
        }
        return state
    }

    fun filesAt(state: BackupState, sourceItem: BackupSourceItem, version: BackupVersion): List<BackupStorageFile> {
        var result: MutableList<BackupStorageFile>? = null
        for (item in state.versions) {
            val change = item.changes.firstOrNull { it.sourceItem.samePlace(sourceItem) }
            if (change == null) {
                result = null
            } else if (result == null) {
                result = change.createdFiles.map { copyStorage(it) }.toMutableList()
            } else {
                result.addAll(change.createdFiles.map { copyStorage(it) })
                change.deletedFiles.forEach { deleted ->
                    result.removeAll { it.file.fileName == deleted }
                }
                change.updatedFiles.forEach { updated ->
                    result.removeAll { it.file.fileName == updated.file.fileName }
                    result += copyStorage(updated)
                }
            }
            if (item == version) break
        }
        return result.orEmpty().sortedBy { it.file.fileName }
    }

    fun findStored(state: BackupState, file: BackupFileState, ignoreLastVersion: Boolean): BackupStorageFile? {
        val versions = if (ignoreLastVersion && state.versions.isNotEmpty()) {
            state.versions.dropLast(1)
        } else {
            state.versions
        }
        for (version in versions) {
            for (change in version.changes) {
                val match = (change.createdFiles + change.updatedFiles).firstOrNull { it.file.sameContent(file) }
                if (match != null) return match
            }
        }
        return null
    }

    private fun copyStorage(file: BackupStorageFile): BackupStorageFile = file.copy(file = file.file)

    private fun changeJson(change: BackupSourceChanges): JSONObject {
        return JSONObject()
            .put("SourceItem", sourceJson(change.sourceItem))
            .put("DeletedFiles", JSONArray(change.deletedFiles))
            .put("UpdatedFiles", JSONArray().apply { change.updatedFiles.forEach { put(storageJson(it)) } })
            .put("CreatedFiles", JSONArray().apply { change.createdFiles.forEach { put(storageJson(it)) } })
    }

    private fun storageJson(file: BackupStorageFile): JSONObject {
        return JSONObject()
            .put("FileState", fileJson(file.file))
            .put("StorageRelativeFileName", file.storageRelativeFileName)
            .put("StorageFileName", file.storageFileName)
            .put("StorageFileNameSize", file.storageFileNameSize)
            .put("StorageMethod", file.storageMethod)
            .put("StorageIntegrityMethod", file.storageIntegrityMethod)
            .put("StorageIntegrityMethodInfo", file.storageIntegrityMethodInfo)
            .put("StoragePassword", file.storagePassword)
    }

    private fun fileJson(file: BackupFileState): JSONObject {
        return JSONObject()
            .put("FileName", file.fileName)
            .put("LastWriteTimeUtc", BackupClock.format(file.lastWriteUtcMillis))
            .put("Size", file.size)
            .put("Sha512", file.sha512)
    }

    private fun sourceJson(item: BackupSourceItem): JSONObject {
        return JSONObject().put("Id", item.id).put("Target", item.target).put("IsFolder", item.isFolder)
    }

    private fun change(json: JSONObject): BackupSourceChanges {
        return BackupSourceChanges(
            sourceItem = sourceItem(json.getJSONObject("SourceItem")),
            deletedFiles = strings(json.optJSONArray("DeletedFiles")),
            updatedFiles = storageFiles(json.optJSONArray("UpdatedFiles")),
            createdFiles = storageFiles(json.optJSONArray("CreatedFiles")),
        )
    }

    private fun storageFiles(array: JSONArray?): MutableList<BackupStorageFile> {
        val files = mutableListOf<BackupStorageFile>()
        if (array == null) return files
        for (i in 0 until array.length()) files += storageFile(array.getJSONObject(i))
        return files
    }

    private fun storageFile(json: JSONObject): BackupStorageFile {
        return BackupStorageFile(
            file = fileState(json.getJSONObject("FileState")),
            storageRelativeFileName = json.optString("StorageRelativeFileName"),
            storageFileName = json.optString("StorageFileName"),
            storageFileNameSize = json.optLong("StorageFileNameSize"),
            storageMethod = json.optString("StorageMethod"),
            storageIntegrityMethod = json.optString("StorageIntegrityMethod"),
            storageIntegrityMethodInfo = json.optString("StorageIntegrityMethodInfo"),
            storagePassword = json.optString("StoragePassword"),
        )
    }

    private fun fileState(json: JSONObject): BackupFileState {
        return BackupFileState(
            fileName = json.optString("FileName"),
            lastWriteUtcMillis = BackupClock.parse(json.optString("LastWriteTimeUtc")),
            size = json.optLong("Size"),
            sha512 = json.optString("Sha512"),
        )
    }

    private fun sourceItem(json: JSONObject): BackupSourceItem {
        return BackupSourceItem(
            id = json.optString("Id").ifBlank { BackupNames.EMPTY_ID },
            target = json.optString("Target"),
            isFolder = json.optBoolean("IsFolder", true),
        )
    }

    private fun strings(array: JSONArray?): MutableList<String> {
        val values = mutableListOf<String>()
        if (array == null) return values
        for (i in 0 until array.length()) values += array.optString(i)
        return values
    }
}

object ExcludePatterns {
    fun excluded(folder: String, relative: String, patterns: List<String>): Boolean {
        val path = relative.replace('\\', '/')
        val full = BackupNames.join(folder, path).replace('\\', '/')
        return patterns.any { pattern ->
            val raw = pattern.trim().replace('\\', '/')
            if (raw.isEmpty()) return@any false
            val stripped = when {
                raw.startsWith(folder.trimEnd('/', '\\') + "/") -> raw.removePrefix(folder.trimEnd('/', '\\') + "/")
                else -> raw
            }
            matches(path, stripped) || matches(full, raw) || matches(path, raw)
        }
    }

    private fun matches(path: String, pattern: String): Boolean {
        if (!pattern.contains('*') && !pattern.contains('?') && !pattern.contains('/')) {
            return path.split('/').any { it == pattern }
        }
        val regex = buildString {
            append("^")
            var i = 0
            while (i < pattern.length) {
                when {
                    pattern.startsWith("**", i) -> {
                        append(".*")
                        i += 2
                        if (i < pattern.length && pattern[i] == '/') i++
                    }
                    pattern[i] == '*' -> {
                        append("[^/]*")
                        i++
                    }
                    pattern[i] == '?' -> {
                        append("[^/]")
                        i++
                    }
                    else -> {
                        append(Regex.escape(pattern[i].toString()))
                        i++
                    }
                }
            }
            append("$")
        }
        return Regex(regex).matches(path)
    }
}

data class ScannedFile(val relativePath: String, val file: File, val modifiedUtcMillis: Long, val open: () -> java.io.InputStream)

fun materialize(directory: File, name: String, open: () -> java.io.InputStream): File {
    val target = File(directory, name)
    target.parentFile?.mkdirs()
    open().use { input -> target.outputStream().use { output -> input.copyTo(output) } }
    return target
}
