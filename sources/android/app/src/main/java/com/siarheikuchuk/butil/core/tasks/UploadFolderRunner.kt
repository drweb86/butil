package com.siarheikuchuk.butil.core.tasks

import android.content.Context
import com.siarheikuchuk.butil.core.storage.LocalFolder
import com.siarheikuchuk.butil.core.storage.StorageFactory

data class UploadProgress(val index: Int, val total: Int, val relativePath: String, val line: String)

class UploadFolderRunner(private val context: Context) {
    fun run(task: UploadFolderTask, onProgress: (UploadProgress) -> Unit, cancelled: () -> Boolean): String? {
        LocalFolder.requireDirectory(context, task.folder)
        val files = LocalFolder.listFiles(context, task.folder)
        if (files.isEmpty()) {
            onProgress(UploadProgress(0, 0, "", "No files to upload."))
            return null
        }
        val limit = if (task.storage.quotaGb > 0) task.storage.quotaGb * 1024L * 1024L * 1024L else Long.MAX_VALUE
        var sent = 0L
        var quotaReached = false
        StorageFactory.open(context, task.storage, autodetect = false).use { storage ->
            files.forEachIndexed { index, file ->
                if (cancelled()) return "Upload cancelled."
                val position = index + 1
                if (quotaReached) {
                    onProgress(UploadProgress(position, files.size, file.relativePath, "Skipped because the upload limit was reached: ${file.relativePath}"))
                    return@forEachIndexed
                }
                if (task.skipExistingFiles && storage.exists(file.relativePath)) {
                    onProgress(UploadProgress(position, files.size, file.relativePath, "Skipped (already exists): ${file.relativePath}"))
                    return@forEachIndexed
                }
                if (file.length >= 0 && sent + file.length > limit) {
                    quotaReached = true
                    onProgress(UploadProgress(position, files.size, file.relativePath, "Skipped because the upload limit was reached: ${file.relativePath}"))
                    return@forEachIndexed
                }
                onProgress(UploadProgress(position, files.size, file.relativePath, "Uploading \"${file.relativePath}\""))
                storage.upload(file.relativePath, file.length, file.open)
                sent += if (file.length >= 0) file.length else 0
                if (sent >= limit) quotaReached = true
            }
        }
        return null
    }
}
