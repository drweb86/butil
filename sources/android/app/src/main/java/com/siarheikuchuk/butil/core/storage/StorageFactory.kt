package com.siarheikuchuk.butil.core.storage

import android.content.Context
import android.net.Uri
import java.io.File
import java.io.InputStream

object StorageFactory {
    fun open(context: Context, settings: StorageSettings, autodetect: Boolean): Storage {
        val prepared = when (settings) {
            is SftpSettings -> settings.copy(keyFile = materializeKey(context, settings.keyFile))
            else -> settings
        }
        return when (prepared) {
            is FolderSettings -> LocalFolder.storage(context, prepared)
            is FtpsSettings -> FtpsStorage(prepared, autodetect)
            is SftpSettings -> SftpStorage(prepared, autodetect)
            is WebDavSettings -> WebDavStorage(prepared)
            is S3Settings -> S3Storage(prepared)
            is AzureBlobSettings -> AzureBlobStorage(prepared)
            is SambaSettings -> SambaStorage(prepared)
            is NfsSettings -> NfsStorage(prepared)
        }
    }

    fun test(context: Context, settings: StorageSettings): String? {
        if (settings.quotaGb < 0) return "Quota should be 0 (do not apply) or value above 0."
        return try {
            open(context, settings, autodetect = true).use { storage ->
                val readError = storage.test()
                if (!readError.isNullOrBlank()) return readError
                writeProbe(storage)
                null
            }
        } catch (error: Exception) {
            error.message ?: error.toString()
        }
    }

    private fun writeProbe(storage: Storage) {
        val folder = "BUtil check " + java.util.UUID.randomUUID()
        val file = "$folder/" + java.util.UUID.randomUUID()
        val payload = "butil".toByteArray(Charsets.UTF_8)
        storage.upload(file, payload.size.toLong()) { payload.inputStream() }
        storage.deleteFolder(folder)
    }

    private fun materializeKey(context: Context, location: String?): String? {
        if (location.isNullOrBlank()) return null
        if (!location.startsWith("content:")) return location
        val uri = Uri.parse(location)
        val target = File(context.cacheDir, "sftp-" + uri.hashCode().toUInt().toString(16) + ".key")
        context.contentResolver.openInputStream(uri)?.use { input ->
            target.outputStream().use { output -> StorageIo.copy(input, output) }
        } ?: throw IllegalArgumentException("Key file cannot be read.")
        return target.absolutePath
    }
}

internal fun InputStream.copyToFile(file: File) {
    file.outputStream().use { output -> StorageIo.copy(this, output) }
}
