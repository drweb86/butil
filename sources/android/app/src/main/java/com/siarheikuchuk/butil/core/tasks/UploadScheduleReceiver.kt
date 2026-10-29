package com.siarheikuchuk.butil.core.tasks

import android.app.AlarmManager
import android.content.BroadcastReceiver
import android.content.Context
import android.content.Intent
import com.siarheikuchuk.butil.core.legal.PrivacyStore

class UploadScheduleReceiver : BroadcastReceiver() {
    override fun onReceive(context: Context, intent: Intent?) {
        if (!PrivacyStore.allowsBackgroundWork(context)) return
        when (intent?.action) {
            Intent.ACTION_BOOT_COMPLETED -> {
                UploadScheduler.rescheduleAll(context)
                UploadScheduler.scheduleAfterRestart(context)
            }
            AlarmManager.ACTION_SCHEDULE_EXACT_ALARM_PERMISSION_STATE_CHANGED -> UploadScheduler.rescheduleAll(context)
            UploadScheduler.ACTION_RUN -> {
                val name = intent.getStringExtra(UploadScheduler.EXTRA_NAME) ?: return
                try {
                    UploadFolderService.start(context, name)
                } catch (error: IllegalStateException) {
                    // ForegroundServiceStartNotAllowedException: Android blocked the background start,
                    // usually because the alarm was inexact. Record it so the task does not look successful.
                    recordBlockedStart(context, name, error)
                }
            }
        }
    }

    private fun recordBlockedStart(context: Context, name: String, error: IllegalStateException) {
        val store = UploadFolderStore(context)
        try {
            store.writeStarted(name, System.currentTimeMillis())
            store.writeStatus(
                name,
                if (UploadScheduler.canScheduleExact(context)) {
                    "Android did not allow the scheduled run to start: ${error.message}"
                } else {
                    "Android did not allow the scheduled run to start. Allow \"Alarms & reminders\" for BUtil in the task's When? section."
                },
            )
            UploadScheduler.reschedule(context, store.loadAny(name))
        } catch (_: Exception) {
        }
    }
}
