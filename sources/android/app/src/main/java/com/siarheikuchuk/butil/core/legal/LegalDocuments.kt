package com.siarheikuchuk.butil.core.legal

import android.content.Context

object LegalDocuments {
    fun loadPrivacy(context: Context, fileName: String): String =
        loadAsset(context, "privacy", fileName)

    fun loadLicense(context: Context, fileName: String, includeChinaAppendix: Boolean): String {
        val body = loadAsset(context, "license", fileName)
        if (!includeChinaAppendix) return body
        val extra = loadAsset(context, "license/china-appendix", fileName)
        return if (extra.isBlank()) body else "$body\n\n$extra"
    }

    private fun loadAsset(context: Context, folder: String, fileName: String): String {
        fun read(name: String): String? = try {
            context.assets.open("$folder/$name").bufferedReader(Charsets.UTF_8).use { it.readText() }
                .lineSequence()
                .filterNot { it.trim() == "[Languages](README.md)" }
                .joinToString("\n")
                .trim()
                .ifBlank { null }
        } catch (_: Exception) {
            null
        }
        if (fileName.equals("en.md", ignoreCase = true)) return read("en.md").orEmpty()
        return read(fileName) ?: read("en.md").orEmpty()
    }
}
