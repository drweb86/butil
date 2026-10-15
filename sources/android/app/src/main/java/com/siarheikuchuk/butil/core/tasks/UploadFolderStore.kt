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

    fun names(): List<String> {
        return root.listFiles()
            .orEmpty()
            .filter { it.isFile && it.name.endsWith(TASK_EXTENSION) }
            .map { it.name.removeSuffix(TASK_EXTENSION) }
            .sortedWith(String.CASE_INSENSITIVE_ORDER)
    }

    fun load(name: String): UploadFolderTask {
        val task = TaskCodec.fromTaskJson(taskFile(name).readText())
        task.storage = cipher.unprotectSettings(task.storage) as StorageSettings
        val scheduleFile = scheduleFile(name)
        if (scheduleFile.isFile) task.schedule = TaskCodec.fromScheduleJson(scheduleFile.readText())
        task.name = name
        return task
    }

    fun save(task: UploadFolderTask, previousName: String?) {
        val name = task.name.trimEnd()
        task.name = name
        if (previousName != null && !previousName.equals(name, ignoreCase = true)) {
            delete(previousName)
        } else if (previousName != null) {
            taskFile(previousName).delete()
            scheduleFile(previousName).delete()
        }
        val protected = task.copy(storage = cipher.protectSettings(task.storage))
        taskFile(name).writeText(TaskCodec.toTaskJson(protected))
        scheduleFile(name).writeText(TaskCodec.toScheduleJson(task.schedule))
    }

    fun delete(name: String) {
        taskFile(name).delete()
        scheduleFile(name).delete()
        statusFile(name).delete()
    }

    fun readStatus(name: String): String? {
        val file = statusFile(name)
        return if (file.isFile) file.readText() else null
    }

    fun writeStatus(name: String, text: String) {
        statusFile(name).writeText(text)
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

    companion object {
        private const val TASK_EXTENSION = ".v3.json"
        private const val ILLEGAL = "\\/:*?\"<>|"
    }
}
