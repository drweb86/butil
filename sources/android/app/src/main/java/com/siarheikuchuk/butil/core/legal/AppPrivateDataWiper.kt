package com.siarheikuchuk.butil.core.legal

import android.content.Context
import android.content.Intent
import com.siarheikuchuk.butil.core.storage.SecretCipher
import com.siarheikuchuk.butil.core.tasks.UploadFolderService
import com.siarheikuchuk.butil.core.tasks.UploadScheduler
import java.io.File

object AppPrivateDataWiper {
    private const val APP_PREFS = "butil"

    /**
     * Deletes tasks, settings, logs, sealed secrets, and folder grants stored by the app.
     * Does not delete files in folders the user selected. Keeps the language chosen
     * for the privacy policy and the license so the next launch can use it.
     */
    fun wipePrivateDataAndClearConsent(context: Context) {
        val app = context.applicationContext
        UploadFolderService.stop(app)
        UploadScheduler.cancelAll(app)
        revokeSafGrants(app)
        deleteChildren(app.filesDir)
        deleteChildren(app.cacheDir)
        app.noBackupFilesDir?.let { deleteChildren(it) }
        app.getSharedPreferences(APP_PREFS, Context.MODE_PRIVATE).edit().clear().commit()
        SecretCipher().deleteKey()
        PrivacyStore.clearConsent(app)
        LicenseStore.clearConsent(app)
    }

    private fun revokeSafGrants(context: Context) {
        val resolver = context.contentResolver
        for (perm in resolver.persistedUriPermissions.toList()) {
            var flags = 0
            if (perm.isReadPermission) flags = flags or Intent.FLAG_GRANT_READ_URI_PERMISSION
            if (perm.isWritePermission) flags = flags or Intent.FLAG_GRANT_WRITE_URI_PERMISSION
            try {
                resolver.releasePersistableUriPermission(perm.uri, flags)
            } catch (_: Exception) {
            }
        }
    }

    private fun deleteChildren(dir: File?) {
        if (dir == null || !dir.exists()) return
        dir.listFiles()?.forEach { child ->
            child.deleteRecursively()
        }
    }
}
