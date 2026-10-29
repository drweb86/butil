package com.siarheikuchuk.butil.core.tasks

import android.app.AlarmManager
import android.app.PendingIntent
import android.content.Context
import android.content.Intent
import android.os.Build
import android.provider.Settings
import java.util.Calendar

object UploadScheduler {
    const val ACTION_RUN = "com.siarheikuchuk.butil.RUN_UPLOAD"
    const val EXTRA_NAME = "taskName"
    private const val APP_PREFS = "butil"
    private const val KEY_LAST_BOOT = "lastHandledBootCount"
    private const val RESTART_DELAY_MILLIS = 60_000L

    /** Without this, Android delivers alarms late and blocks the background start of the upload service. */
    fun canScheduleExact(context: Context): Boolean {
        if (Build.VERSION.SDK_INT < Build.VERSION_CODES.S) return true
        val alarm = context.getSystemService(Context.ALARM_SERVICE) as AlarmManager
        return alarm.canScheduleExactAlarms()
    }

    fun rescheduleAll(context: Context) {
        val store = UploadFolderStore(context)
        for (name in store.names()) {
            reschedule(context, store.loadAny(name))
        }
    }

    fun reschedule(context: Context, task: AppTask) {
        val alarm = context.getSystemService(Context.ALARM_SERVICE) as AlarmManager
        val pending = pending(context, task.name, restart = false)
        alarm.cancel(pending)
        val trigger = nextTrigger(task.schedule) ?: return
        setAlarm(alarm, trigger, pending)
    }

    fun cancel(context: Context, name: String) {
        val alarm = context.getSystemService(Context.ALARM_SERVICE) as AlarmManager
        alarm.cancel(pending(context, name, restart = false))
        alarm.cancel(pending(context, name, restart = true))
    }

    fun cancelAll(context: Context) {
        val store = UploadFolderStore(context)
        for (name in store.names()) {
            cancel(context, name)
        }
    }

    /**
     * Android 15+ forbids starting the upload service straight from BOOT_COMPLETED, so tasks
     * marked "run after device restart" go through a one-shot alarm a minute after boot.
     * Android 15+ also re-sends BOOT_COMPLETED when a force-stopped app is opened again;
     * the boot count tells a real restart apart from that.
     */
    fun scheduleAfterRestart(context: Context) {
        if (!isNewBoot(context)) return
        val alarm = context.getSystemService(Context.ALARM_SERVICE) as AlarmManager
        val store = UploadFolderStore(context)
        val trigger = System.currentTimeMillis() + RESTART_DELAY_MILLIS
        for (name in store.names()) {
            val task = store.loadAny(name)
            if (task.schedule.runAtLogin) {
                setAlarm(alarm, trigger, pending(context, name, restart = true))
            }
        }
    }

    private fun isNewBoot(context: Context): Boolean {
        if (Build.VERSION.SDK_INT < Build.VERSION_CODES.N) return true
        val boot = Settings.Global.getInt(context.contentResolver, Settings.Global.BOOT_COUNT, -1)
        if (boot < 0) return true
        val prefs = context.getSharedPreferences(APP_PREFS, Context.MODE_PRIVATE)
        if (prefs.getInt(KEY_LAST_BOOT, -1) == boot) return false
        prefs.edit().putInt(KEY_LAST_BOOT, boot).apply()
        return true
    }

    private fun setAlarm(alarm: AlarmManager, trigger: Long, pending: PendingIntent) {
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

    private fun pending(context: Context, name: String, restart: Boolean): PendingIntent {
        val intent = Intent(context, UploadScheduleReceiver::class.java)
            .setAction(ACTION_RUN)
            .putExtra(EXTRA_NAME, name)
        val flags = PendingIntent.FLAG_UPDATE_CURRENT or
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) PendingIntent.FLAG_IMMUTABLE else 0
        val key = name.lowercase()
        val requestCode = if (restart) "$key\u0000restart".hashCode() else key.hashCode()
        return PendingIntent.getBroadcast(context, requestCode, intent, flags)
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
