package com.siarheikuchuk.butil.core.tasks

import android.app.Notification
import android.app.NotificationChannel
import android.app.NotificationManager
import android.app.Service
import android.content.Context
import android.content.Intent
import android.content.pm.ServiceInfo
import android.os.Build
import android.os.IBinder
import androidx.core.app.NotificationCompat
import com.siarheikuchuk.butil.R
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.Job
import kotlinx.coroutines.SupervisorJob
import kotlinx.coroutines.NonCancellable
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import android.os.Handler
import android.os.Looper
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue

class UploadFolderService : Service() {
    private val job = SupervisorJob()
    private val scope = CoroutineScope(job + Dispatchers.IO)
    private var work: Job? = null

    override fun onBind(intent: Intent?): IBinder? = null

    override fun onStartCommand(intent: Intent?, flags: Int, startId: Int): Int {
        val name = intent?.getStringExtra(UploadScheduler.EXTRA_NAME)
        if (name.isNullOrBlank()) {
            stopSelf()
            return START_NOT_STICKY
        }
        if (runningName == name) return START_NOT_STICKY
        runningName = name
        cancelled = false
        startInForeground("Uploading \"$name\"")
        work = scope.launch {
            val store = UploadFolderStore(this@UploadFolderService)
            try {
                val task = store.load(name)
                UploadRunLog.clear()
                UploadRunLog.add("Uploading \"$name\"")
                val error = UploadFolderRunner(this@UploadFolderService).run(task, { progress ->
                    UploadRunLog.add(progress.line)
                    updateNotification(progress.line)
                }, { cancelled })
                if (error == null) {
                    store.writeStatus(name, "Finished successfully.")
                    UploadRunLog.add("Upload finished.")
                    UploadRunLog.mark(false)
                } else {
                    store.writeStatus(name, error)
                    UploadRunLog.add(error)
                    UploadRunLog.mark(true)
                }
            } catch (error: Exception) {
                if (error is kotlinx.coroutines.CancellationException) throw error
                val message = error.message ?: error.toString()
                store.writeStatus(name, message)
                UploadRunLog.add(message)
                UploadRunLog.mark(true)
            } finally {
                runningName = null
                withContext(NonCancellable) {
                    try {
                        UploadScheduler.reschedule(this@UploadFolderService, store.load(name))
                    } catch (_: Exception) {
                    }
                    withContext(Dispatchers.Main) {
                        UploadRunLog.active = false
                        @Suppress("DEPRECATION")
                        stopForeground(true)
                        stopSelf()
                    }
                }
            }
        }
        return START_NOT_STICKY
    }

    override fun onDestroy() {
        cancelled = true
        work?.cancel()
        job.cancel()
        runningName = null
        super.onDestroy()
    }

    private fun startInForeground(text: String) {
        val notification = notification(text)
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.Q) {
            startForeground(NOTIFICATION_ID, notification, ServiceInfo.FOREGROUND_SERVICE_TYPE_DATA_SYNC)
        } else {
            startForeground(NOTIFICATION_ID, notification)
        }
    }

    private fun updateNotification(text: String) {
        val manager = getSystemService(NotificationManager::class.java)
        manager.notify(NOTIFICATION_ID, notification(text))
    }

    private fun notification(text: String): Notification {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            val manager = getSystemService(NotificationManager::class.java)
            manager.createNotificationChannel(
                NotificationChannel(CHANNEL, "Uploads", NotificationManager.IMPORTANCE_LOW),
            )
        }
        return NotificationCompat.Builder(this, CHANNEL)
            .setSmallIcon(android.R.drawable.stat_sys_upload)
            .setContentTitle(getString(R.string.upload_folder_contents_task_create))
            .setContentText(text)
            .setOngoing(true)
            .build()
    }

    companion object {
        private const val CHANNEL = "uploads"
        private const val NOTIFICATION_ID = 42

        @Volatile
        var runningName: String? = null

        @Volatile
        var cancelled: Boolean = false

        fun start(context: Context, name: String) {
            val intent = Intent(context, UploadFolderService::class.java).putExtra(UploadScheduler.EXTRA_NAME, name)
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
                context.startForegroundService(intent)
            } else {
                context.startService(intent)
            }
        }

        fun cancel() {
            cancelled = true
        }
    }
}

object UploadRunLog {
    val lines = mutableStateListOf<String>()
    var failed by mutableStateOf(false)
    var active by mutableStateOf(false)
    private val main = Handler(Looper.getMainLooper())

    fun clear() {
        main.post {
            lines.clear()
            failed = false
            active = true
        }
    }

    fun add(line: String) {
        main.post { lines.add(line) }
    }

    fun mark(failedRun: Boolean) {
        main.post { failed = failedRun }
    }
}
