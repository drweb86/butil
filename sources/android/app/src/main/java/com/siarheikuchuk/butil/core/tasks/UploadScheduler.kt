package com.siarheikuchuk.butil.core.tasks

import android.app.AlarmManager
import android.app.PendingIntent
import android.content.Context
import android.content.Intent
import android.os.Build
import java.util.Calendar

object UploadScheduler {
    const val ACTION_RUN = "com.siarheikuchuk.butil.RUN_UPLOAD"
    const val EXTRA_NAME = "taskName"

    fun rescheduleAll(context: Context) {
        val store = UploadFolderStore(context)
        for (name in store.names()) {
            reschedule(context, store.loadAny(name))
        }
    }

    fun reschedule(context: Context, task: AppTask) {
        val alarm = context.getSystemService(Context.ALARM_SERVICE) as AlarmManager
        val pending = pending(context, task.name)
        alarm.cancel(pending)
        val trigger = nextTrigger(task.schedule) ?: return
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
            try {
                alarm.setExactAndAllowWhileIdle(AlarmManager.RTC_WAKEUP, trigger, pending)
            } catch (_: SecurityException) {
                alarm.setAndAllowWhileIdle(AlarmManager.RTC_WAKEUP, trigger, pending)
            }
        } else {
            alarm.setExact(AlarmManager.RTC_WAKEUP, trigger, pending)
        }
    }

    fun cancel(context: Context, name: String) {
        val alarm = context.getSystemService(Context.ALARM_SERVICE) as AlarmManager
        alarm.cancel(pending(context, name))
    }

    fun runAtLogin(context: Context) {
        val store = UploadFolderStore(context)
        for (name in store.names()) {
            val task = store.loadAny(name)
            if (task.schedule.runAtLogin) {
                UploadFolderService.start(context, name)
            }
        }
    }

    private fun pending(context: Context, name: String): PendingIntent {
        val intent = Intent(context, UploadScheduleReceiver::class.java)
            .setAction(ACTION_RUN)
            .putExtra(EXTRA_NAME, name)
        val flags = PendingIntent.FLAG_UPDATE_CURRENT or
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) PendingIntent.FLAG_IMMUTABLE else 0
        return PendingIntent.getBroadcast(context, name.lowercase().hashCode(), intent, flags)
    }

    private fun nextTrigger(schedule: Schedule): Long? {
        if (schedule.days.isEmpty()) return null
        val now = Calendar.getInstance()
        for (offset in 0..7) {
            val candidate = Calendar.getInstance()
            candidate.add(Calendar.DAY_OF_YEAR, offset)
            val day = candidate.get(Calendar.DAY_OF_WEEK) - Calendar.SUNDAY
            if (day !in schedule.days) continue
            candidate.set(Calendar.HOUR_OF_DAY, schedule.hour)
            candidate.set(Calendar.MINUTE, schedule.minute)
            candidate.set(Calendar.SECOND, 0)
            candidate.set(Calendar.MILLISECOND, 0)
            if (candidate.timeInMillis > now.timeInMillis + 1_000) return candidate.timeInMillis
        }
        return null
    }
}
