package com.siarheikuchuk.butil.core.ftps

// Client-supplied text (command names, paths, user names) ends up in replies and in the log.
// ESC, BEL and other control characters there can drive a terminal that shows the log, so
// they are replaced before the text leaves the session.
internal object SafeText {
    const val REPLACEMENT = '?'

    fun sanitize(text: String): String {
        if (text.none { Character.isISOControl(it) }) return text
        return buildString(text.length) {
            for (ch in text) append(if (Character.isISOControl(ch)) REPLACEMENT else ch)
        }
    }
}

internal class SanitizingLog(private val inner: FtpsLog) : FtpsLog {
    override fun debug(message: String) = inner.debug(SafeText.sanitize(message))
    override fun info(message: String) = inner.info(SafeText.sanitize(message))
    override fun warn(message: String) = inner.warn(SafeText.sanitize(message))
    override fun error(message: String, error: Throwable?) = inner.error(SafeText.sanitize(message), error)
}
