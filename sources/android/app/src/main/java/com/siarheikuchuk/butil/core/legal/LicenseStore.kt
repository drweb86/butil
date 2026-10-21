package com.siarheikuchuk.butil.core.legal

import android.content.Context

object LicenseStore {
    const val CONSENT_KEY = "china_pipl_policy_license_consent_accepted"

    fun hasConsent(context: Context): Boolean =
        PrivacyStore.prefs(context).getBoolean(CONSENT_KEY, false)

    fun acceptConsent(context: Context) {
        PrivacyStore.prefs(context).edit().putBoolean(CONSENT_KEY, true).commit()
    }

    fun clearConsent(context: Context) {
        PrivacyStore.prefs(context).edit().putBoolean(CONSENT_KEY, false).commit()
    }
}
