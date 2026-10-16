package com.siarheikuchuk.butil.core.tasks

import com.siarheikuchuk.butil.core.storage.FolderSettings
import com.siarheikuchuk.butil.core.storage.StorageSettings
import java.util.UUID

sealed class AppTask {
    abstract var name: String
    abstract var schedule: Schedule
    abstract val jsonType: String
}

data class SourceFolder(
    var id: String = UUID.randomUUID().toString(),
    var target: String = "",
)

data class IncrementalBackupTask(
    override var name: String = "",
    var items: MutableList<SourceFolder> = mutableListOf(),
    var fileExcludePatterns: MutableList<String> = mutableListOf(),
    var password: String = "",
    var storage: StorageSettings = FolderSettings(),
    override var schedule: Schedule = Schedule(),
) : AppTask() {
    override val jsonType: String get() = "Incremental"
}

data class ImportMediaTask(
    override var name: String = "",
    var transformFileName: String = "{DATE:yyyy}/{DATE:yyyy'-'MM', 'MMMM}/{DATE:yyyy'-'MM'-'dd', 'dddd}/{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
    var from: StorageSettings = FolderSettings(),
    var destinationFolder: String = "",
    var skipAlreadyImportedFiles: Boolean = true,
    var deleteCopiedDataOnSourceMedia: Boolean = false,
    var fileExtensions: MutableList<String>? = ImportMediaRules.defaultExtensions.toMutableList(),
    var fileLastWriteTimeMinMillis: Long? = null,
    override var schedule: Schedule = Schedule(),
) : AppTask() {
    override val jsonType: String get() = "ImportMedia"
}

data class SynchronizationTask(
    override var name: String = "",
    var localFolder: String = "",
    var repositorySubfolder: String? = null,
    var synchronizationMode: Int = MODE_TWO_WAY,
    var password: String = "",
    var storage: StorageSettings = FolderSettings(),
    override var schedule: Schedule = Schedule(),
) : AppTask() {
    override val jsonType: String get() = "Synchronization"

    companion object {
        const val MODE_TWO_WAY = 0
        const val MODE_READ = 1
    }
}

data class FtpsServerTask(
    override var name: String = "",
    var port: Int = 10999,
    var username: String = "user",
    var password: String = "",
    var folder: String = "",
    var folderAccess: Int = ACCESS_READ_WRITE,
    var durationMinutes: Long = 30,
    override var schedule: Schedule = Schedule(),
) : AppTask() {
    override val jsonType: String get() = "BUtilServer"

    companion object {
        const val ACCESS_READ_WRITE = 0
        const val ACCESS_READ_ONLY = 1
        val MAX_DURATION_MINUTES = (UInt.MAX_VALUE.toLong() - 1L) / 60_000L
    }
}
