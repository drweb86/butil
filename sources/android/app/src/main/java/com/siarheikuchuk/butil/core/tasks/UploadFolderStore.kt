package com.siarheikuchuk.butil.core.tasks

import android.content.Context
import com.siarheikuchuk.butil.BuildConfig
import com.siarheikuchuk.butil.core.storage.SecretCipher
import com.siarheikuchuk.butil.core.storage.StorageSettings
import java.io.File

class UploadFolderStore(context: Context) {
    private val root = File(
        context.filesDir,
        if (BuildConfig.DEBUG) "BUtil Backup Tasks - DEBUG" else "BUtil Backup Tasks",
    ).apply { mkdirs() }
    private val cipher = SecretCipher()
    private val logs = TaskRunLog(context)

    fun names(): List<String> {
        return root.listFiles()
            .orEmpty()
            .filter { it.isFile && it.name.endsWith(TASK_EXTENSION) }
            .map { it.name.removeSuffix(TASK_EXTENSION) }
            .sortedWith(String.CASE_INSENSITIVE_ORDER)
    }

    fun load(name: String): UploadFolderTask = loadAny(name) as UploadFolderTask

    fun kind(name: String): String {
        val root = org.json.JSONObject(taskFile(name).readText())
        return root.getJSONObject("Model").optString("\$type")
    }

    fun loadAny(name: String): AppTask {
        val task = unprotect(TaskCodec.fromJson(taskFile(name).readText()))
        val scheduleFile = scheduleFile(name)
        if (scheduleFile.isFile) task.schedule = TaskCodec.fromScheduleJson(scheduleFile.readText())
        task.name = name
        return task
    }

    fun save(task: UploadFolderTask, previousName: String?) = saveAny(task, previousName)

    fun saveAny(task: AppTask, previousName: String?) {
        val name = task.name.trimEnd()
        task.name = name
        if (previousName != null && previousName != name) {
            logs.move(previousName, name)
            moveFile(startedFile(previousName), startedFile(name))
        }
        if (previousName != null && !previousName.equals(name, ignoreCase = true)) {
            delete(previousName)
        } else if (previousName != null) {
            taskFile(previousName).delete()
            scheduleFile(previousName).delete()
        }
        taskFile(name).writeText(TaskCodec.toJson(protect(task)))
        scheduleFile(name).writeText(TaskCodec.toScheduleJson(task.schedule))
    }

    private fun protect(task: AppTask): AppTask = when (task) {
        is UploadFolderTask -> task.copy(storage = cipher.protectSettings(task.storage))
        is IncrementalBackupTask -> task.copy(password = cipher.protect(task.password), storage = cipher.protectSettings(task.storage))
        is ImportMediaTask -> task.copy(from = cipher.protectSettings(task.from))
        is SynchronizationTask -> task.copy(password = cipher.protect(task.password), storage = cipher.protectSettings(task.storage))
        is FtpsServerTask -> task.copy(password = cipher.protect(task.password))
    }

    private fun unprotect(task: AppTask): AppTask = when (task) {
        is UploadFolderTask -> task.copy(storage = cipher.unprotectSettings(task.storage) as StorageSettings)
        is IncrementalBackupTask -> task.copy(password = cipher.unprotect(task.password), storage = cipher.unprotectSettings(task.storage) as StorageSettings)
        is ImportMediaTask -> task.copy(from = cipher.unprotectSettings(task.from) as StorageSettings)
        is SynchronizationTask -> task.copy(password = cipher.unprotect(task.password), storage = cipher.unprotectSettings(task.storage) as StorageSettings)
        is FtpsServerTask -> task.copy(password = cipher.unprotect(task.password))
    }

    fun duplicate(name: String): String {
        val task = loadAny(name)
        val taken = names().map { it.lowercase() }.toMutableSet()
        var counter = 1
        var newName: String
        do {
            newName = "$name ($counter)"
            counter++
        } while (!taken.add(newName.lowercase()))
        task.name = newName
        saveAny(task, null)
        return newName
    }

    fun delete(name: String) {
        taskFile(name).delete()
        scheduleFile(name).delete()
        statusFile(name).delete()
        startedFile(name).delete()
        logs.delete(name)
    }

    fun readStatus(name: String): String? {
        val file = statusFile(name)
        return if (file.isFile) file.readText() else null
    }

    fun writeStatus(name: String, text: String) {
        statusFile(name).writeText(text)
    }

    fun readStarted(name: String): Long? {
        val file = startedFile(name)
        if (!file.isFile) return null
        return file.readText().trim().toLongOrNull()
    }

    fun writeStarted(name: String, startedAtMillis: Long) {
        startedFile(name).writeText(startedAtMillis.toString())
    }

    fun validateName(name: String, originalName: String?): String? {
        val trimmed = name.trimEnd()
        if (trimmed.isBlank() || ILLEGAL.any { trimmed.contains(it) } || trimmed.contains("..")) {
            return "Name is empty or contains not supported path characters."
        }
        val taken = names().any { existing ->
            existing.equals(trimmed, ignoreCase = true) && !existing.equals(originalName, ignoreCase = true)
        }
        if (taken) return "A task with this name already exists."
        return null
    }

    private fun taskFile(name: String) = File(root, name + TASK_EXTENSION)
    private fun scheduleFile(name: String) = File(root, "$name.schedule.json")
    private fun statusFile(name: String) = File(root, "$name.status.txt")
    private fun startedFile(name: String) = File(root, "$name.started.txt")

    private fun moveFile(from: File, to: File) {
        if (from.absolutePath == to.absolutePath || !from.isFile) return
        if (to.exists()) to.delete()
        if (!from.renameTo(to)) {
            from.copyTo(to, overwrite = true)
            from.delete()
        }
    }

    companion object {
        private const val TASK_EXTENSION = ".v3.json"
        private const val ILLEGAL = "\\/:*?\"<>|"
    }
}
