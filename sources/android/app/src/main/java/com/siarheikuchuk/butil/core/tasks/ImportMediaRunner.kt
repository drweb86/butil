package com.siarheikuchuk.butil.core.tasks

import android.content.Context
import com.siarheikuchuk.butil.BuildConfig
import com.siarheikuchuk.butil.core.storage.LocalFolder
import com.siarheikuchuk.butil.core.storage.StorageFactory
import org.json.JSONArray
import org.json.JSONObject
import java.io.File
import java.time.Instant
import java.time.ZoneId
import java.time.format.TextStyle
import java.util.Locale

object ImportMediaRules {
    val defaultExtensions = listOf(
        ".jpg", ".jpeg", ".jpe", ".jfif", ".png", ".gif", ".bmp", ".webp", ".tif", ".tiff",
        ".heic", ".heif", ".avif", ".raw", ".dng", ".cr2", ".cr3", ".nef", ".arw", ".orf", ".rw2", ".pef", ".srw", ".raf",
        ".mp3", ".wav", ".flac", ".aac", ".m4a", ".ogg", ".oga", ".wma", ".aiff", ".aif", ".opus", ".amr", ".m4b",
    )

    val presets = listOf(
        "{DATE:yyyy}/{DATE:yyyy'-'MM', 'MMMM}/{DATE:yyyy'-'MM'-'dd', 'dddd}/{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
        "{DATE:yyyy}/{DATE:MM}/{DATE:yyyy'-'MM'-'dd}/{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
        "{DATE:yyyy}/{DATE:MM}/{DATE:dd}/{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
        "{DATE:yyyy}/{DATE:yyyy'-'MM'-'dd}/{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
        "{DATE:yyyy'-'MM}/{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
        "{DATE:yyyy'-'MM'-'dd}/{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
        "{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
        "{DATE:yyyy}/{DATE:MMMM}/{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
        "{DATE:yyyy}/{DATE:MM'-'dd}/{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
        "{DATE:yyyy}/{DATE:MM}/{DATE:yyyyMMdd_HHmmss}",
        "{DATE:yyyyMMdd_HHmmss}",
        "{DATE:yyyy}/{DATE:MM}/{DATE:dd}/{DATE:HH'-'mm'-'ss}",
        "{DATE:yyyy'-'MM'-'dd}/{DATE:HH'-'mm'-'ss}",
    )

    fun parseExtensions(text: String): List<String> {
        if (text.isBlank()) return emptyList()
        return text.split('\r', '\n', ',', ';', ' ', '\t')
            .map { it.trim() }
            .filter { it.isNotEmpty() }
            .map { value ->
                val withDot = if (value.startsWith(".")) value else ".$value"
                withDot.lowercase(Locale.ROOT)
            }
            .filter { it.length > 1 }
            .distinct()
    }

    fun matches(fileName: String, extensions: List<String>?): Boolean {
        if (extensions.isNullOrEmpty()) return true
        val extension = fileName.substringAfterLast('.', "").let { if (it == fileName) "" else ".$it" }
        if (extension.isEmpty()) return false
        return extensions.any { it.equals(extension, ignoreCase = true) }
    }

    fun validateTransform(transform: String): String? {
        if (transform.isBlank()) return "Transform file name is empty."
        return try {
            val sample = DateTokens.apply(transform, System.currentTimeMillis())
            if (sample.contains("..") || sample.contains(':')) "Invalid transform file name string." else null
        } catch (_: Exception) {
            "Invalid transform file name string."
        }
    }

    fun example(transform: String, outputFolder: String): String? {
        return try {
            val modified = System.currentTimeMillis()
            val destination = outputFolder.trimEnd('/', '\\') + "/" +
                DateTokens.apply(transform, modified) + ".jpg"
            "Example:\nImagine you have file DCIM001.jpg (modified at ${BackupClock.format(modified)}).\n" +
                "With this directory and template string file will be stored as:\n$destination"
        } catch (_: Exception) {
            null
        }
    }

    fun tooBroad(folderNames: List<String>): String? {
        val known = setOf(
            "DCIM", "MISC", "PRIVATE", "MP_ROOT", "Pictures", "Download", "Downloads", "Movies", "Music",
            "Alarms", "Notifications", "Podcasts", "Ringtones", "Android", "Documents", "Photos",
            "PhotoData", "Photo Library", "Camera Roll", "Videos", "Desktop",
        )
        val matches = folderNames.map { it.substringAfterLast('/').substringAfterLast('\\') }
            .filter { known.any { knownName -> knownName.equals(it, ignoreCase = true) } }
            .distinctBy { it.lowercase(Locale.ROOT) }
            .sorted()
        if (matches.size < 2) return null
        val suggestion = matches.firstOrNull { it.equals("DCIM", ignoreCase = true) } ?: matches.first()
        return "The selected folder looks like a device or home root because it contains several known media folders (${matches.joinToString(", ")}). Please choose a more specific folder, for example \"$suggestion\"."
    }
}

object DateTokens {
    private val token = Regex("\\{(.*?)\\}")

    fun apply(input: String, millis: Long): String {
        return token.replace(input) { match ->
            val body = match.groupValues[1]
            if (!body.startsWith("DATE:")) return@replace ""
            format(body.removePrefix("DATE:"), millis)
        }
    }

    private fun format(pattern: String, millis: Long): String {
        val date = Instant.ofEpochMilli(millis).atZone(ZoneId.systemDefault())
        val locale = Locale.getDefault()
        val out = StringBuilder()
        var i = 0
        while (i < pattern.length) {
            if (pattern[i] == '\'') {
                i++
                if (i < pattern.length && pattern[i] == '\'') {
                    out.append('\'')
                    i++
                    continue
                }
                while (i < pattern.length && pattern[i] != '\'') {
                    out.append(pattern[i])
                    i++
                }
                if (i < pattern.length) i++
                continue
            }
            if (pattern[i] == '\\' && i + 1 < pattern.length) {
                out.append(pattern[i + 1])
                i += 2
                continue
            }
            val run = pattern.drop(i).takeWhile { it == pattern[i] }
            val replacement = when (run) {
                "yyyy" -> date.year.toString()
                "yy" -> (date.year % 100).toString().padStart(2, '0')
                "y" -> date.year.toString()
                "MMMM" -> date.month.getDisplayName(TextStyle.FULL, locale)
                "MMM" -> date.month.getDisplayName(TextStyle.SHORT, locale)
                "MM" -> date.monthValue.toString().padStart(2, '0')
                "M" -> date.monthValue.toString()
                "dddd" -> date.dayOfWeek.getDisplayName(TextStyle.FULL, locale)
                "ddd" -> date.dayOfWeek.getDisplayName(TextStyle.SHORT, locale)
                "dd" -> date.dayOfMonth.toString().padStart(2, '0')
                "d" -> date.dayOfMonth.toString()
                "HH" -> date.hour.toString().padStart(2, '0')
                "H" -> date.hour.toString()
                "hh" -> (((date.hour + 11) % 12) + 1).toString().padStart(2, '0')
                "h" -> (((date.hour + 11) % 12) + 1).toString()
                "mm" -> date.minute.toString().padStart(2, '0')
                "m" -> date.minute.toString()
                "ss" -> date.second.toString().padStart(2, '0')
                "s" -> date.second.toString()
                "tt" -> if (date.hour < 12) "AM" else "PM"
                else -> if (run.all { it == 'f' } && run.length <= 7) {
                    val nanos = date.nano.toString().padStart(9, '0').take(run.length)
                    nanos
                } else {
                    null
                }
            }
            if (replacement == null) {
                out.append(pattern[i])
                i++
            } else {
                out.append(replacement)
                i += run.length
            }
        }
        return out.toString().replace(Regex("[\\\\:*?\"<>|]"), "_")
    }
}

class ImportMediaRunner(private val context: Context) {
    fun run(task: ImportMediaTask, onProgress: (String) -> Unit, cancelled: () -> Boolean): String? {
        val transformError = ImportMediaRules.validateTransform(task.transformFileName)
        if (transformError != null) return transformError
        if (!LocalFolder.isDirectory(context, task.destinationFolder)) return "Output folder does not exist."
        val work = File(context.cacheDir, "import-" + System.nanoTime()).apply { mkdirs() }
        try {
            return StorageFactory.open(context, task.from, autodetect = false).use { source ->
                val broad = ImportMediaRules.tooBroad(source.listFolders())
                if (broad != null) return broad
                val remembered = if (task.skipAlreadyImportedFiles) loadState(task.name) else mutableSetOf()
                val destinationFiles = LocalFolder.listRemoteFiles(context, task.destinationFolder)
                val destinationSignatures = destinationFiles.map { file ->
                    val temp = materialize(work, "dest") { LocalFolder.open(context, task.destinationFolder, file.relativePath) }
                    try {
                        "${temp.length()} ${FileHash.sha512(temp)}"
                    } finally {
                        temp.delete()
                    }
                }.toMutableSet()
                val sourceFiles = source.listFiles().filter { ImportMediaRules.matches(it.relativePath, task.fileExtensions) }
                var imported = 0
                sourceFiles.forEachIndexed { index, file ->
                    if (cancelled()) return "Import cancelled."
                    val position = "${index + 1}/${sourceFiles.size}"
                    if (task.skipAlreadyImportedFiles && remembered.contains(file.relativePath)) {
                        onProgress("Skipped already imported ($position): ${file.relativePath}")
                        return@forEachIndexed
                    }
                    val minimum = task.fileLastWriteTimeMinMillis
                    if (minimum != null && file.modifiedUtcMillis < minimum) {
                        onProgress("Skipped by date ($position): ${file.relativePath}")
                        remembered += file.relativePath
                        return@forEachIndexed
                    }
                    val downloaded = materialize(work, "source") { source.download(file.relativePath) }
                    val signature = "${downloaded.length()} ${FileHash.sha512(downloaded)}"
                    if (destinationSignatures.contains(signature)) {
                        onProgress("Already in the destination ($position): ${file.relativePath}")
                        remembered += file.relativePath
                        downloaded.delete()
                        return@forEachIndexed
                    }
                    val relative = uniqueDestination(task, file.relativePath, file.modifiedUtcMillis)
                    onProgress("Importing ($position): ${file.relativePath} => $relative")
                    LocalFolder.write(context, task.destinationFolder, relative, { downloaded.inputStream() }, file.modifiedUtcMillis)
                    destinationSignatures += signature
                    remembered += file.relativePath
                    imported++
                    if (task.deleteCopiedDataOnSourceMedia) source.delete(file.relativePath)
                    downloaded.delete()
                }
                if (task.skipAlreadyImportedFiles) saveState(task.name, remembered)
                onProgress("Imported $imported files.")
                null
            }
        } finally {
            work.deleteRecursively()
        }
    }

    private fun uniqueDestination(task: ImportMediaTask, sourceName: String, modified: Long): String {
        val transformed = DateTokens.apply(task.transformFileName, modified).replace('\\', '/').trim('/')
        val extension = sourceName.substringAfterLast('.', "").let { if (it == sourceName) "" else ".$it" }
        val folder = transformed.substringBeforeLast('/', "")
        val base = transformed.substringAfterLast('/')
        var candidate = listOf(folder, base + extension).filter { it.isNotEmpty() }.joinToString("/")
        var index = 0
        while (LocalFolder.exists(context, task.destinationFolder, candidate)) {
            index++
            val name = "${base}_$index$extension"
            candidate = if (folder.isEmpty()) name else "$folder/$name"
        }
        return candidate
    }

    private fun stateFile(name: String): File {
        val folderName = if (BuildConfig.DEBUG) "BUtil Backup Tasks - DEBUG - States" else "BUtil Backup Tasks - States"
        return File(context.filesDir, folderName).apply { mkdirs() }.resolve("$name.json")
    }

    private fun loadState(name: String): MutableSet<String> {
        val file = stateFile(name)
        if (!file.isFile) return mutableSetOf()
        val array = JSONObject(file.readText()).optJSONArray("Files") ?: return mutableSetOf()
        return (0 until array.length()).map { array.optString(it) }.toMutableSet()
    }

    private fun saveState(name: String, files: Set<String>) {
        val array = JSONArray()
        files.sorted().forEach { array.put(it) }
        stateFile(name).writeText(JSONObject().put("Files", array).toString(2))
    }
}
