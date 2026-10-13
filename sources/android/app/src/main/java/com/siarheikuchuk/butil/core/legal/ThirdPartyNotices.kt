package com.siarheikuchuk.butil.core.legal

import android.content.Context

/**
 * Embedded third-party notices, matching {@code BUtil.Core.Legal.ThirdPartyNotices}.
 */
object ThirdPartyNotices {
    const val ResourceName = "THIRD-PARTY-NOTICES.md"

    fun load(context: Context): String {
        try {
            return context.assets.open(ResourceName).bufferedReader(Charsets.UTF_8).use { it.readText() }
        } catch (ex: java.io.IOException) {
            throw IllegalStateException("Missing embedded third-party notices.", ex)
        }
    }
}
