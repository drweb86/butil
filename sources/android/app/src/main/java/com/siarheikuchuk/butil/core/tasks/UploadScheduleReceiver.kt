package com.siarheikuchuk.butil.core.tasks

import android.content.BroadcastReceiver
import android.content.Context
import android.content.Intent

class UploadScheduleReceiver : BroadcastReceiver() {
    override fun onReceive(context: Context, intent: Intent?) {
        when (intent?.action) {
            Intent.ACTION_BOOT_COMPLETED -> {
                UploadScheduler.rescheduleAll(context)
                UploadScheduler.runAtLogin(context)
            }
            UploadScheduler.ACTION_RUN -> {
                val name = intent.getStringExtra(UploadScheduler.EXTRA_NAME) ?: return
                UploadFolderService.start(context, name)
            }
        }
    }
}
