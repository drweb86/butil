package com.siarheikuchuk.butil.core.legal

import android.content.Context
import com.siarheikuchuk.butil.BuildConfig

object PrivacyStore {
    const val PREFS_NAME = "privacy_consent"
    const val CONSENT_KEY = "china_pipl_policy_privacy_consent_accepted"
    private const val LANGUAGE_KEY = "privacy_policy_language"

    fun hasConsent(context: Context): Boolean =
        prefs(context).getBoolean(CONSENT_KEY, false)

    fun acceptConsent(context: Context) {
        prefs(context).edit().putBoolean(CONSENT_KEY, true).commit()
    }

    fun clearConsent(context: Context) {
        prefs(context).edit().putBoolean(CONSENT_KEY, false).commit()
    }

    fun languageCode(context: Context): String? =
        prefs(context).getString(LANGUAGE_KEY, null)?.takeIf { it.isNotBlank() }

    fun setLanguageCode(context: Context, code: String) {
        prefs(context).edit().putString(LANGUAGE_KEY, code).commit()
    }

    /**
     * China builds stay idle until both the privacy policy and the license
     * have been accepted. Other builds always run.
     */
    fun allowsBackgroundWork(context: Context): Boolean {
        if (!BuildConfig.CHINA_PIPL_POLICY) return true
        return hasConsent(context) && LicenseStore.hasConsent(context)
    }

    internal fun prefs(context: Context) =
        context.applicationContext.getSharedPreferences(PREFS_NAME, Context.MODE_PRIVATE)
}
