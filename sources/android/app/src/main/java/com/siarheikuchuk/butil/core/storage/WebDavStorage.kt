package com.siarheikuchuk.butil.core.storage

import java.io.InputStream
import java.net.HttpURLConnection
import java.net.URL
import java.util.Base64

internal class WebDavStorage(settings: WebDavSettings) : Storage {
    private val resolved = resolve(settings)
    private val origin: String
    private val basePath: String

    init {
        if (resolved.host.isBlank()) throw IllegalArgumentException("Host is not specified.")
        if (resolved.preset != "YandexDisk" && resolved.user.isBlank()) {
            throw IllegalArgumentException("User is not specified.")
        }
        if (resolved.password.isBlank()) throw IllegalArgumentException("Password is not specified.")
        val scheme = if (resolved.useHttps) "https" else "http"
        val port = resolved.port
        val portPart = when {
            port <= 0 -> ""
            resolved.useHttps && port == 443 -> ""
            !resolved.useHttps && port == 80 -> ""
            else -> ":$port"
        }
        origin = "$scheme://${resolved.host}$portPart"
        basePath = "/" + resolved.basePath.trim('/')
    }

    override fun test(): String? {
        val code = propfind(uri(null))
        return if (code in listOf(200, 207)) null else "WebDAV test failed (HTTP $code)"
    }

    override fun exists(relativeFileName: String): Boolean {
        return propfind(uri(relativeFileName)) in listOf(200, 207)
    }

    override fun upload(relativeFileName: String, length: Long, open: () -> InputStream) {
        val relative = RemotePaths.normalizeRelative(relativeFileName)
        val parent = RemotePaths.parent(relative)
        if (parent != null) ensureDirectory(parent)
        if (exists(relative)) request("DELETE", uri(relative), null, null)
        val code = request("PUT", uri(relative), length, open)
        if (code !in listOf(200, 201, 204)) {
            throw IllegalStateException("Upload failed (HTTP $code): $relative")
        }
    }

    override fun deleteFolder(relativeFolderName: String) {
        request("DELETE", uri(relativeFolderName), null, null)
    }

    private fun ensureDirectory(relativePath: String) {
        val parts = relativePath.split('/')
        var current = ""
        for (part in parts) {
            current = if (current.isEmpty()) part else "$current/$part"
            if (propfind(uri(current)) !in listOf(200, 207)) {
                request("MKCOL", uri(current), null, null)
            }
        }
    }

    private fun uri(relative: String?): String {
        val prefix = basePath.trimEnd('/')
        if (relative.isNullOrBlank()) return "$origin$prefix/"
        val encoded = RemotePaths.encodeSegments(RemotePaths.normalizeRelative(relative))
        return "$origin$prefix/$encoded"
    }

    private fun propfind(url: String): Int {
        val body = """<?xml version="1.0" encoding="utf-8"?><d:propfind xmlns:d="DAV:"><d:prop><d:resourcetype/></d:prop></d:propfind>"""
            .toByteArray(Charsets.UTF_8)
        return request("PROPFIND", url, body.size.toLong(), { body.inputStream() }, mapOf("Depth" to "0", "Content-Type" to "application/xml"))
    }

    private fun request(
        method: String,
        url: String,
        length: Long?,
        open: (() -> InputStream)?,
        extra: Map<String, String> = emptyMap(),
    ): Int {
        val connection = (URL(url).openConnection() as HttpURLConnection).apply {
            requestMethod = method
            connectTimeout = 30_000
            readTimeout = 0
            instanceFollowRedirects = false
            setRequestProperty("Authorization", authorization())
            extra.forEach { (name, value) -> setRequestProperty(name, value) }
        }
        if (open != null && length != null) {
            connection.doOutput = true
            connection.setFixedLengthStreamingMode(length)
            connection.outputStream.use { output -> open().use { input -> StorageIo.copy(input, output) } }
        }
        val code = connection.responseCode
        connection.disconnect()
        return code
    }

    private fun authorization(): String {
        if (resolved.preset == "YandexDisk") {
            return "OAuth ${resolved.password}"
        }
        val token = Base64.getEncoder().encodeToString("${resolved.user}:${resolved.password}".toByteArray(Charsets.UTF_8))
        return "Basic $token"
    }

    companion object {
        fun resolve(settings: WebDavSettings): WebDavSettings {
            val preset = settings.preset.ifBlank { "Custom" }
            val host = when (preset) {
                "Koofr" -> "app.koofr.eu"
                "pCloud" -> "ewebdav.pcloud.com"
                "IONOSHiDrive" -> "webdav.hidrive.ionos.com"
                "Box" -> "dav.box.com"
                "YandexDisk" -> "webdav.yandex.ru"
                "MailRuCloud" -> "webdav.cloud.mail.ru"
                "Jianguoyun" -> "dav.jianguoyun.com"
                else -> settings.host
            }
            val basePath = when (preset) {
                "Box", "MailRuCloud" -> "/dav"
                "Jianguoyun" -> "/dav/"
                else -> settings.basePath
            }
            val https = if (preset == "Custom" || preset == "Nextcloud" || preset == "ownCloud") settings.useHttps else true
            return settings.copy(preset = preset, host = host, basePath = basePath, useHttps = https)
        }
    }
}
