package com.siarheikuchuk.butil.core.tasks

import android.content.Context
import com.siarheikuchuk.butil.core.ftps.Certificates
import com.siarheikuchuk.butil.core.ftps.FtpsLog
import com.siarheikuchuk.butil.core.ftps.FtpsServer
import com.siarheikuchuk.butil.core.ftps.FtpsServerConfig
import com.siarheikuchuk.butil.core.ftps.FtpsServerSettings
import com.siarheikuchuk.butil.core.ftps.FtpsUserAccount
import com.siarheikuchuk.butil.core.ftps.SharedFolderFileSystem
import com.siarheikuchuk.butil.core.storage.LocalFolder

class FtpsServerRunner(private val context: Context, private val location: String) {
    fun run(task: FtpsServerTask, onProgress: (String) -> Unit, cancelled: () -> Boolean): String? {
        if (task.username.isBlank()) return "User is not specified."
        if (task.password.isBlank()) return "Password is not specified."
        if (task.port !in 1..65535) return "Port is invalid."
        if (task.durationMinutes !in 1..FtpsServerTask.MAX_DURATION_MINUTES) return "Duration is invalid."
        if (!LocalFolder.isDirectory(context, location)) return "Folder does not exist."
        val readOnly = task.folderAccess == FtpsServerTask.ACCESS_READ_ONLY
        val log = TaskLog(onProgress)
        val settings = FtpsServerSettings(port = task.port)
        val certificate = try {
            Certificates.loadOrCreate(context, context.filesDir, settings, log)
        } catch (error: Exception) {
            return error.message ?: "Cannot create the certificate."
        }
        val user = FtpsUserAccount(
            login = task.username,
            password = task.password,
            folder = location,
            canRead = true,
            canWrite = !readOnly,
        )
        val server = FtpsServer(log, FtpsServerConfig(settings, listOf(user)), SharedFolderFileSystem(context), certificate)
        try {
            server.start()
        } catch (error: Exception) {
            return error.message ?: "Cannot start the FTPS server."
        }
        try {
            onProgress(
                "User: ${task.username}. Encryption: explicit TLS. Access: ${if (readOnly) "read only" else "read and write"}.",
            )
            onProgress("Certificate SHA-256: ${certificate.fingerprintSha256}")
            val deadline = System.currentTimeMillis() + task.durationMinutes * 60_000L
            while (!cancelled() && System.currentTimeMillis() < deadline) {
                try {
                    Thread.sleep(500)
                } catch (_: InterruptedException) {
                    break
                }
            }
        } finally {
            server.stop()
        }
        return if (cancelled()) "FTPS server stopped." else null
    }

    private class TaskLog(private val onProgress: (String) -> Unit) : FtpsLog {
        override fun debug(message: String) = Unit
        override fun info(message: String) = onProgress(message)
        override fun warn(message: String) = onProgress(message)
        override fun error(message: String, error: Throwable?) = onProgress(error?.message?.let { "$message: $it" } ?: message)
    }
}
