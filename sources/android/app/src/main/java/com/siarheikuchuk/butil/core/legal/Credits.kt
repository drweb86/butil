package com.siarheikuchuk.butil.core.legal

import android.content.Context

/**
 * Embedded credits, matching {@code BUtil.Core.Legal.CreditDocuments}.
 */
object Credits {
    const val ResourceName = "CREDITS.md"

    fun load(context: Context): String {
        try {
            return context.assets.open(ResourceName).bufferedReader(Charsets.UTF_8).use { it.readText() }
        } catch (ex: java.io.IOException) {
            throw IllegalStateException("Missing embedded credits.", ex)
        }
    }
}
