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
            if (runningName == null) {
                startInForeground(getString(R.string.app_name))
                stopSelf()
            } else {
                startInForeground("Uploading \"$runningName\"")
            }
            return START_NOT_STICKY
        }
        if (runningName == name) {
            startInForeground("Uploading \"$name\"")
            return START_NOT_STICKY
        }
        runningName = name
        cancelled = false
        startInForeground("Uploading \"$name\"")
        UploadRunLog.begin()
        work = scope.launch {
            val store = UploadFolderStore(this@UploadFolderService)
            val runLog = TaskRunLog(this@UploadFolderService)
            try {
                runLog.begin(name)
                store.writeStarted(name, System.currentTimeMillis())
                val task = store.loadAny(name)
                note(runLog, statusTitle(task))
                val error = runTask(task) { line ->
                    note(runLog, line)
                    updateNotification(line)
                }
                if (error == null) {
                    store.writeStatus(name, "Finished successfully.")
                    note(runLog, "Upload finished.")
                    UploadRunLog.mark(false)
                } else {
                    store.writeStatus(name, error)
                    note(runLog, error)
                    UploadRunLog.mark(true)
                }
            } catch (error: Exception) {
                if (error is kotlinx.coroutines.CancellationException) throw error
                val message = error.message ?: error.toString()
                store.writeStatus(name, message)
                note(runLog, message)
                UploadRunLog.mark(true)
            } finally {
                runningName = null
                withContext(NonCancellable) {
                    runLog.close()
                    if (!suppressReschedule) {
                        try {
                            UploadScheduler.reschedule(this@UploadFolderService, store.loadAny(name))
                        } catch (_: Exception) {
                        }
                    }
                    withContext(Dispatchers.Main) {
                        UploadRunLog.finish()
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
            .setContentTitle(getString(R.string.app_name))
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

        @Volatile
        var suppressReschedule: Boolean = false

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

        fun stop(context: Context) {
            cancelled = true
            suppressReschedule = true
            context.stopService(Intent(context, UploadFolderService::class.java))
        }
    }

    private fun runTask(task: AppTask, onProgress: (String) -> Unit): String? {
        val cancelled = { UploadFolderService.cancelled }
        return when (task) {
            is UploadFolderTask -> UploadFolderRunner(this).run(task, { progress -> onProgress(progress.line) }, cancelled)
            is IncrementalBackupTask -> IncrementalBackupRunner(this).run(task, onProgress, cancelled)
            is ImportMediaTask -> ImportMediaRunner(this).run(task, onProgress, cancelled)
            is SynchronizationTask -> SynchronizationRunner(this).run(task, onProgress, cancelled)
            is FtpsServerTask -> FtpsServerRunner(this, task.folder).run(task, onProgress, cancelled)
        }
    }

    private fun note(runLog: TaskRunLog, line: String) {
        runLog.append(line)
        UploadRunLog.add(line)
    }

    private fun statusTitle(task: AppTask): String = when (task) {
        is UploadFolderTask -> "Uploading \"${task.name}\""
        is IncrementalBackupTask -> "Backing up \"${task.name}\""
        is ImportMediaTask -> "Importing \"${task.name}\""
        is SynchronizationTask -> "Synchronizing \"${task.name}\""
        is FtpsServerTask -> "Sharing \"${task.name}\""
    }
}

object UploadRunLog {
    val lines = mutableStateListOf<String>()
    var failed by mutableStateOf(false)
    var active by mutableStateOf(false)
    private val main = Handler(Looper.getMainLooper())

    fun begin() {
        onMain {
            lines.clear()
            failed = false
            active = true
        }
    }

    fun add(line: String) {
        onMain { lines.add(line) }
    }

    fun mark(failedRun: Boolean) {
        onMain { failed = failedRun }
    }

    fun finish() {
        onMain { active = false }
    }

    private fun onMain(block: () -> Unit) {
        if (Looper.myLooper() == Looper.getMainLooper()) block() else main.post(block)
    }
}
