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

    override fun download(relativeFileName: String): InputStream {
        return openStream("GET", uri(relativeFileName), emptyMap())
    }

    override fun delete(relativeFileName: String) {
        request("DELETE", uri(relativeFileName), null, null)
    }

    override fun listFiles(): List<RemoteFile> {
        val files = mutableListOf<RemoteFile>()
        walk("", files)
        return files.sortedBy { it.relativePath }
    }

    override fun listFolders(): List<String> {
        return children("").filter { it.directory }.map { it.name }.sorted()
    }

    private fun walk(relative: String, into: MutableList<RemoteFile>) {
        for (child in children(relative)) {
            val childRelative = if (relative.isEmpty()) child.name else "$relative/${child.name}"
            if (child.directory) walk(childRelative, into) else into += RemoteFile(childRelative, child.length, child.modifiedUtcMillis)
        }
    }

    private fun children(relative: String): List<DavEntry> {
        val body = """<?xml version="1.0" encoding="utf-8"?><d:propfind xmlns:d="DAV:"><d:prop><d:resourcetype/><d:getcontentlength/><d:getlastmodified/></d:prop></d:propfind>"""
            .toByteArray(Charsets.UTF_8)
        val xml = openText(
            "PROPFIND",
            uri(relative.ifBlank { null }),
            body,
            mapOf("Depth" to "1", "Content-Type" to "application/xml"),
        )
        val self = uri(relative.ifBlank { null }).trimEnd('/')
        val entries = mutableListOf<DavEntry>()
        val responses = Regex("(?is)<(?:\\w+:)?response\\b.*?</(?:\\w+:)?response>").findAll(xml)
        for (response in responses) {
            val block = response.value
            val href = Regex("(?is)<(?:\\w+:)?href>(.*?)</(?:\\w+:)?href>").find(block)?.groupValues?.get(1) ?: continue
            val decoded = java.net.URLDecoder.decode(href.replace("&amp;", "&"), Charsets.UTF_8)
            val absolute = if (decoded.startsWith("http")) decoded else origin + if (decoded.startsWith("/")) decoded else "/$decoded"
            if (absolute.trimEnd('/') == self) continue
            val name = absolute.trimEnd('/').substringAfterLast('/')
            if (name.isBlank() || name == "." || name == "..") continue
            val directory = Regex("(?is)<(?:\\w+:)?collection\\s*/>").containsMatchIn(block) || absolute.endsWith("/")
            val length = Regex("(?is)<(?:\\w+:)?getcontentlength>(\\d+)</(?:\\w+:)?getcontentlength>")
                .find(block)?.groupValues?.get(1)?.toLongOrNull() ?: 0L
            val modifiedText = Regex("(?is)<(?:\\w+:)?getlastmodified>(.*?)</(?:\\w+:)?getlastmodified>")
                .find(block)?.groupValues?.get(1).orEmpty()
            val modified = httpDate(modifiedText)
            entries += DavEntry(name, directory, length, modified)
        }
        return entries
    }

    private fun httpDate(value: String): Long {
        if (value.isBlank()) return 0L
        return try {
            java.time.ZonedDateTime.parse(value, java.time.format.DateTimeFormatter.RFC_1123_DATE_TIME).toInstant().toEpochMilli()
        } catch (_: Exception) {
            0L
        }
    }

    private data class DavEntry(val name: String, val directory: Boolean, val length: Long, val modifiedUtcMillis: Long)

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

    private fun openText(method: String, url: String, body: ByteArray, extra: Map<String, String>): String {
        val connection = connection(method, url, extra)
        connection.doOutput = true
        connection.setFixedLengthStreamingMode(body.size)
        connection.outputStream.use { it.write(body) }
        val code = connection.responseCode
        val stream = if (code in 200..299) connection.inputStream else connection.errorStream
        val text = stream?.use { it.readBytes().toString(Charsets.UTF_8) }.orEmpty()
        connection.disconnect()
        if (code !in listOf(200, 207)) throw IllegalStateException("WebDAV $method failed (HTTP $code)")
        return text
    }

    private fun openStream(method: String, url: String, extra: Map<String, String>): InputStream {
        val connection = connection(method, url, extra)
        val code = connection.responseCode
        if (code !in 200..299) {
            connection.disconnect()
            throw IllegalStateException("WebDAV download failed (HTTP $code)")
        }
        val source = connection.inputStream
        return object : InputStream() {
            override fun read(): Int = source.read()
            override fun read(b: ByteArray, off: Int, len: Int): Int = source.read(b, off, len)
            override fun close() {
                source.close()
                connection.disconnect()
            }
        }
    }

    private fun connection(method: String, url: String, extra: Map<String, String>): HttpURLConnection {
        return (URL(url).openConnection() as HttpURLConnection).apply {
            requestMethod = method
            connectTimeout = 30_000
            readTimeout = 0
            instanceFollowRedirects = false
            setRequestProperty("Authorization", authorization())
            extra.forEach { (name, value) -> setRequestProperty(name, value) }
        }
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
