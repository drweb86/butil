package com.siarheikuchuk.butil.core.tasks

import android.content.Context
import java.io.BufferedWriter
import java.io.File
import java.time.LocalTime
import java.time.format.DateTimeFormatter

/**
 * One text log per task, stored in [Context.getNoBackupFilesDir] so Android backup
 * and device transfer never copy it. Starting a task replaces the previous log.
 */
internal class TaskRunLog(context: Context) {
    private val directory = File(context.applicationContext.noBackupFilesDir, DIRECTORY).apply { mkdirs() }
    private var writer: BufferedWriter? = null

    fun begin(taskName: String) {
        synchronized(fileLock) {
            closeWriter()
            try {
                directory.mkdirs()
                writer = file(taskName).bufferedWriter(Charsets.UTF_8)
            } catch (_: Exception) {
                writer = null
            }
        }
    }

    fun append(message: String) {
        synchronized(fileLock) {
            val output = writer ?: return
            try {
                output.append(time.format(LocalTime.now()))
                output.append(' ')
                output.append(message)
                output.newLine()
                output.flush()
            } catch (_: Exception) {
            }
        }
    }

    fun close() {
        synchronized(fileLock) {
            closeWriter()
        }
    }

    fun exists(taskName: String): Boolean = synchronized(fileLock) {
        file(taskName).isFile
    }

    fun readBytes(taskName: String): ByteArray? = synchronized(fileLock) {
        val log = file(taskName)
        if (!log.isFile) null else log.readBytes()
    }

    fun delete(taskName: String) {
        synchronized(fileLock) {
            file(taskName).delete()
        }
    }

    fun move(fromTaskName: String, toTaskName: String) {
        if (fromTaskName == toTaskName) return
        synchronized(fileLock) {
            val source = file(fromTaskName)
            val target = file(toTaskName)
            if (!source.isFile) return
            if (target.exists() && source.absolutePath != target.absolutePath) target.delete()
            if (!source.renameTo(target)) {
                source.copyTo(target, overwrite = true)
                if (source.absolutePath != target.absolutePath) source.delete()
            }
        }
    }

    private fun file(taskName: String) = File(directory, "$taskName.txt")

    private fun closeWriter() {
        try {
            writer?.flush()
            writer?.close()
        } catch (_: Exception) {
        } finally {
            writer = null
        }
    }

    companion object {
        private const val DIRECTORY = "logs"
        private val fileLock = Any()
        private val time = DateTimeFormatter.ofPattern("HH:mm")
    }
}
