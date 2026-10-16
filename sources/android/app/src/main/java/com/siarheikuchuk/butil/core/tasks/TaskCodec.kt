package com.siarheikuchuk.butil.core.tasks

import com.siarheikuchuk.butil.core.storage.AzureBlobSettings
import com.siarheikuchuk.butil.core.storage.FolderSettings
import com.siarheikuchuk.butil.core.storage.FtpsSettings
import com.siarheikuchuk.butil.core.storage.NfsSettings
import com.siarheikuchuk.butil.core.storage.S3Settings
import com.siarheikuchuk.butil.core.storage.SambaSettings
import com.siarheikuchuk.butil.core.storage.SftpSettings
import com.siarheikuchuk.butil.core.storage.StorageSettings
import com.siarheikuchuk.butil.core.storage.WebDavSettings
import org.json.JSONArray
import org.json.JSONObject

/**
 * Desktop [System.Text.Json] task document: PascalCase properties, numeric enums, and a `$type`
 * discriminator. Schedule is kept beside the task because the desktop stores it in the OS scheduler.
 */
data class UploadFolderTask(
    override var name: String = "",
    var folder: String = "",
    var skipExistingFiles: Boolean = false,
    var storage: StorageSettings = FolderSettings(),
    override var schedule: Schedule = Schedule(),
) : AppTask() {
    override val jsonType: String get() = "BUtilClient"
}

data class Schedule(
    val days: MutableSet<Int> = mutableSetOf(),
    var hour: Int = 23,
    var minute: Int = 50,
    var runAtLogin: Boolean = false,
) {
    fun isArmed(): Boolean = days.isNotEmpty()
}

object TaskCodec {
    const val TASK_TYPE = "BUtilClient"
    private const val DIRECTION_TO_SERVER = 0

    fun toTaskJson(task: UploadFolderTask): String {
        val root = JSONObject()
        root.put("Model", modelJson(task))
        root.put("Name", task.name)
        return root.toString(2)
    }

    fun fromTaskJson(json: String): UploadFolderTask {
        val root = JSONObject(json)
        val model = root.getJSONObject("Model")
        val type = model.optString("\$type")
        if (type != TASK_TYPE) {
            throw IllegalArgumentException("This file is not an upload-folder task.")
        }
        return UploadFolderTask(
            name = root.optString("Name"),
            folder = model.optString("Folder"),
            skipExistingFiles = model.optBoolean("SkipExistingFiles", false),
            storage = storageFromJson(model.getJSONObject("To")),
        )
    }

    fun toScheduleJson(schedule: Schedule): String {
        val days = JSONArray()
        schedule.days.sorted().forEach { days.put(it) }
        return JSONObject()
            .put("Days", days)
            .put("Time", "%02d:%02d:00".format(schedule.hour, schedule.minute))
            .put("RunAtLogin", schedule.runAtLogin)
            .toString(2)
    }

    fun fromScheduleJson(json: String): Schedule {
        val root = JSONObject(json)
        val schedule = Schedule(runAtLogin = root.optBoolean("RunAtLogin", false))
        val days = root.optJSONArray("Days")
        if (days != null) {
            for (i in 0 until days.length()) schedule.days.add(days.getInt(i))
        }
        val time = root.optString("Time", "23:50:00")
        val parts = time.split(":")
        schedule.hour = parts.getOrNull(0)?.toIntOrNull()?.coerceIn(0, 23) ?: 23
        schedule.minute = parts.getOrNull(1)?.toIntOrNull()?.coerceIn(0, 59) ?: 50
        return schedule
    }

    private fun modelJson(task: UploadFolderTask): JSONObject {
        return JSONObject()
            .put("\$type", TASK_TYPE)
            .put("To", storageToJson(task.storage))
            .put("Folder", task.folder)
            .put("Direction", DIRECTION_TO_SERVER)
            .put("SkipExistingFiles", task.skipExistingFiles)
    }

    fun storageToJson(settings: StorageSettings): JSONObject {
        val json = JSONObject().put("\$type", settings.jsonType)
        when (settings) {
            is FolderSettings -> {
                json.put("DestinationFolder", settings.destinationFolder)
            }
            is FtpsSettings -> {
                json.put("Host", settings.host)
                json.put("Encryption", if (settings.encryption == "Implicit") 1 else 0)
                json.put("Port", settings.port)
                json.put("Folder", settings.folder ?: JSONObject.NULL)
                json.put("User", settings.user)
                json.put("Password", settings.password)
                json.put("TrustedCertificate", settings.trustedCertificate)
            }
            is SftpSettings -> {
                json.put("Host", settings.host)
                json.put("Port", settings.port)
                json.put("Folder", settings.folder)
                json.put("User", settings.user)
                json.put("Password", settings.password ?: JSONObject.NULL)
                json.put("KeyFile", settings.keyFile ?: JSONObject.NULL)
                json.put("FingerPrintSHA256", settings.fingerPrintSha256)
            }
            is WebDavSettings -> {
                json.put("Preset", settings.preset)
                json.put("Host", settings.host)
                json.put("Port", settings.port)
                json.put("UseHttps", settings.useHttps)
                json.put("BasePath", settings.basePath)
                json.put("User", settings.user)
                json.put("Password", settings.password)
            }
            is S3Settings -> {
                json.put("Provider", settings.provider)
                json.put("ServiceUrl", settings.serviceUrl)
                json.put("Region", settings.region)
                json.put("AccessKey", settings.accessKey)
                json.put("SecretKey", settings.secretKey)
                json.put("BucketName", settings.bucketName)
                json.put("PathPrefix", settings.pathPrefix)
            }
            is AzureBlobSettings -> {
                json.put("AccountName", settings.accountName)
                json.put("AccountKey", settings.accountKey)
                json.put("ContainerName", settings.containerName)
                json.put("PathPrefix", settings.pathPrefix)
            }
            is SambaSettings -> {
                json.put("Url", settings.url)
                json.put("User", settings.user ?: JSONObject.NULL)
                json.put("Password", settings.password ?: JSONObject.NULL)
            }
            is NfsSettings -> {
                json.put("Host", settings.host)
                json.put("SharePath", settings.sharePath)
                json.put("MountPoint", settings.mountPoint)
                json.put("MountOptions", settings.mountOptions ?: JSONObject.NULL)
            }
        }
        json.put("SingleBackupQuotaGb", settings.quotaGb)
        json.put("MountPowershellScript", settings.mountScript ?: JSONObject.NULL)
        json.put("UnmountPowershellScript", settings.unmountScript ?: JSONObject.NULL)
        return json
    }

    fun storageFromJson(json: JSONObject): StorageSettings {
        val settings = when (val type = json.optString("\$type")) {
            "Folder" -> FolderSettings(destinationFolder = json.optString("DestinationFolder"))
            "Ftps" -> FtpsSettings(
                host = json.optString("Host"),
                encryption = encryptionName(json.opt("Encryption")),
                port = json.optInt("Port"),
                folder = json.nullableString("Folder"),
                user = json.optString("User"),
                password = json.optString("Password"),
                trustedCertificate = json.optString("TrustedCertificate"),
            )
            "Sftp" -> SftpSettings(
                host = json.optString("Host"),
                port = json.optInt("Port", 22),
                folder = json.optString("Folder"),
                user = json.optString("User"),
                password = json.nullableString("Password"),
                keyFile = json.nullableString("KeyFile"),
                fingerPrintSha256 = json.optString("FingerPrintSHA256"),
            )
            "WebDav" -> WebDavSettings(
                preset = json.optString("Preset", "Custom").ifBlank { "Custom" },
                host = json.optString("Host"),
                port = json.optInt("Port"),
                useHttps = json.optBoolean("UseHttps", true),
                basePath = json.optString("BasePath"),
                user = json.optString("User"),
                password = json.optString("Password"),
            )
            "S3" -> S3Settings(
                provider = json.optString("Provider", "Custom").ifBlank { "Custom" },
                serviceUrl = json.optString("ServiceUrl"),
                region = json.optString("Region"),
                accessKey = json.optString("AccessKey"),
                secretKey = json.optString("SecretKey"),
                bucketName = json.optString("BucketName"),
                pathPrefix = json.optString("PathPrefix"),
            )
            "AzureBlob" -> AzureBlobSettings(
                accountName = json.optString("AccountName"),
                accountKey = json.optString("AccountKey"),
                containerName = json.optString("ContainerName"),
                pathPrefix = json.optString("PathPrefix"),
            )
            "Samba" -> SambaSettings(
                url = json.optString("Url"),
                user = json.nullableString("User"),
                password = json.nullableString("Password"),
            )
            "Nfs" -> NfsSettings(
                host = json.optString("Host"),
                sharePath = json.optString("SharePath"),
                mountPoint = json.optString("MountPoint"),
                mountOptions = json.nullableString("MountOptions"),
            )
            else -> throw IllegalArgumentException("Unknown storage type '$type'.")
        }
        settings.quotaGb = json.optLong("SingleBackupQuotaGb")
        settings.mountScript = json.nullableString("MountPowershellScript")
        settings.unmountScript = json.nullableString("UnmountPowershellScript")
        return settings
    }

    private fun encryptionName(value: Any?): String = when (value) {
        is Number -> if (value.toInt() == 1) "Implicit" else "Explicit"
        is String -> if (value.equals("Implicit", ignoreCase = true)) "Implicit" else "Explicit"
        else -> "Explicit"
    }

    fun toJson(task: AppTask): String {
        val root = JSONObject()
        root.put("Model", modelJson(task))
        root.put("Name", task.name)
        return root.toString(2)
    }

    fun fromJson(json: String): AppTask {
        val root = JSONObject(json)
        val model = root.getJSONObject("Model")
        val name = root.optString("Name")
        return when (val type = model.optString("\$type")) {
            TASK_TYPE -> fromTaskJson(json)
            "Incremental" -> IncrementalBackupTask(
                name = name,
                items = sourceFolders(model.optJSONArray("Items")),
                fileExcludePatterns = stringList(model.optJSONArray("FileExcludePatterns")),
                password = model.optString("Password"),
                storage = storageFromJson(model.getJSONObject("To")),
            )
            "ImportMedia" -> ImportMediaTask(
                name = name,
                transformFileName = model.optString("TransformFileName"),
                from = storageFromJson(model.getJSONObject("From")),
                destinationFolder = model.optString("DestinationFolder"),
                skipAlreadyImportedFiles = model.optBoolean("SkipAlreadyImportedFiles", true),
                deleteCopiedDataOnSourceMedia = model.optBoolean("DeleteCopiedDataOnSourceMedia", false),
                fileExtensions = if (!model.has("FileExtensions") || model.isNull("FileExtensions")) {
                    null
                } else {
                    stringList(model.optJSONArray("FileExtensions"))
                },
                fileLastWriteTimeMinMillis = model.optString("FileLastWriteTimeMin").takeIf { model.has("FileLastWriteTimeMin") && !model.isNull("FileLastWriteTimeMin") }
                    ?.let(BackupClock::parse),
            )
            "Synchronization" -> SynchronizationTask(
                name = name,
                localFolder = model.optString("LocalFolder"),
                repositorySubfolder = model.nullableString("RepositorySubfolder"),
                synchronizationMode = enumNumber(model.opt("SynchronizationMode"), SynchronizationTask.MODE_TWO_WAY),
                password = model.optString("Password"),
                storage = storageFromJson(model.getJSONObject("To")),
            )
            "BUtilServer" -> FtpsServerTask(
                name = name,
                port = model.optInt("Port", 10999),
                username = model.optString("Username", "user"),
                password = model.optString("Password"),
                folder = model.optString("Folder"),
                folderAccess = enumNumber(model.opt("FolderAccess"), FtpsServerTask.ACCESS_READ_WRITE),
                durationMinutes = model.optLong("DurationMinutes", 30),
            )
            else -> throw IllegalArgumentException("Unknown task type '$type'.")
        }
    }

    private fun modelJson(task: AppTask): JSONObject = when (task) {
        is UploadFolderTask -> modelJson(task)
        is IncrementalBackupTask -> JSONObject()
            .put("\$type", task.jsonType)
            .put("To", storageToJson(task.storage))
            .put("Items", JSONArray().apply {
                task.items.forEach { item ->
                    put(JSONObject().put("Id", item.id).put("Target", item.target).put("IsFolder", true))
                }
            })
            .put("FileExcludePatterns", JSONArray(task.fileExcludePatterns))
            .put("Password", task.password)
        is ImportMediaTask -> JSONObject()
            .put("\$type", task.jsonType)
            .put("TransformFileName", task.transformFileName)
            .put("From", storageToJson(task.from))
            .put("DestinationFolder", task.destinationFolder)
            .put("SkipAlreadyImportedFiles", task.skipAlreadyImportedFiles)
            .put("DeleteCopiedDataOnSourceMedia", task.deleteCopiedDataOnSourceMedia)
            .put("FileExtensions", task.fileExtensions?.let { JSONArray(it) } ?: JSONObject.NULL)
            .put(
                "FileLastWriteTimeMin",
                task.fileLastWriteTimeMinMillis?.let(BackupClock::format) ?: JSONObject.NULL,
            )
        is SynchronizationTask -> JSONObject()
            .put("\$type", task.jsonType)
            .put("To", storageToJson(task.storage))
            .put("LocalFolder", task.localFolder)
            .put("RepositorySubfolder", task.repositorySubfolder ?: JSONObject.NULL)
            .put("SynchronizationMode", task.synchronizationMode)
            .put("Password", task.password)
        is FtpsServerTask -> JSONObject()
            .put("\$type", task.jsonType)
            .put("Port", task.port)
            .put("Username", task.username)
            .put("Password", task.password)
            .put("Folder", task.folder)
            .put("FolderAccess", task.folderAccess)
            .put("DurationMinutes", task.durationMinutes)
    }

    private fun sourceFolders(array: JSONArray?): MutableList<SourceFolder> {
        val items = mutableListOf<SourceFolder>()
        if (array == null) return items
        for (i in 0 until array.length()) {
            val item = array.getJSONObject(i)
            if (!item.optBoolean("IsFolder", true)) continue
            items += SourceFolder(
                id = item.optString("Id").ifBlank { java.util.UUID.randomUUID().toString() },
                target = item.optString("Target"),
            )
        }
        return items
    }

    private fun stringList(array: JSONArray?): MutableList<String> {
        val items = mutableListOf<String>()
        if (array == null) return items
        for (i in 0 until array.length()) items += array.optString(i)
        return items
    }

    private fun enumNumber(value: Any?, fallback: Int): Int = when (value) {
        is Number -> value.toInt()
        is String -> value.toIntOrNull() ?: fallback
        else -> fallback
    }

    private fun JSONObject.nullableString(name: String): String? {
        if (!has(name) || isNull(name)) return null
        return optString(name)
    }
}
