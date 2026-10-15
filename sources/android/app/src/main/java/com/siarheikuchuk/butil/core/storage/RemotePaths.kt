package com.siarheikuchuk.butil.core.storage

/**
 * Relative names used by the desktop upload task: separators are normalized,
 * and a path may not climb out of the destination root.
 */
object RemotePaths {
    fun normalizeRelative(path: String, allowEmpty: Boolean = false): String {
        val parts = path.replace('\\', '/')
            .split('/')
            .filter { it.isNotEmpty() && it != "." }
        if (parts.any { it == ".." }) {
            throw IllegalArgumentException("Relative path escapes the destination folder.")
        }
        if (parts.isEmpty()) {
            if (allowEmpty) return ""
            throw IllegalArgumentException("Relative path is empty.")
        }
        return parts.joinToString("/")
    }

    fun parent(relative: String): String? {
        val normalized = normalizeRelative(relative)
        val slash = normalized.lastIndexOf('/')
        return if (slash < 0) null else normalized.substring(0, slash)
    }

    fun combine(root: String?, relative: String): String {
        val child = normalizeRelative(relative)
        val base = root?.replace('\\', '/')?.trim('/') ?: ""
        return if (base.isEmpty()) child else "$base/$child"
    }

    fun encodeSegments(path: String): String {
        return path.split('/').joinToString("/") { segment ->
            java.net.URLEncoder.encode(segment, Charsets.UTF_8.name()).replace("+", "%20")
        }
    }
}
