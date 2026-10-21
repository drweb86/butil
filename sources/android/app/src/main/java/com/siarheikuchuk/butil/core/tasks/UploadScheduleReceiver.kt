package com.siarheikuchuk.butil.core.tasks

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
                UploadScheduler.runAtLogin(context)
            }
            UploadScheduler.ACTION_RUN -> {
                val name = intent.getStringExtra(UploadScheduler.EXTRA_NAME) ?: return
                UploadFolderService.start(context, name)
            }
        }
    }
}
