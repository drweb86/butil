package com.siarheikuchuk.butil.core.storage

import java.io.ByteArrayOutputStream
import java.io.InputStream
import java.net.InetSocketAddress
import java.net.Socket
import java.net.URI
import java.util.Base64
import javax.net.ssl.SSLSocket
import javax.net.ssl.SSLSocketFactory

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
        if (!settings.useHttps) throw IllegalArgumentException("WebDAV requires HTTPS.")
        val port = resolved.port
        val portPart = if (port <= 0 || port == 443) "" else ":$port"
        origin = "https://${resolved.host}$portPart"
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
        val response = exchange(method, url, body.size.toLong(), { body.inputStream() }, extra)
        val text = response.stream.use { it.readBytes() }.toString(Charsets.UTF_8)
        response.close()
        if (response.code !in listOf(200, 207)) throw IllegalStateException("WebDAV $method failed (HTTP ${response.code})")
        return text
    }

    private fun openStream(method: String, url: String, extra: Map<String, String>): InputStream {
        val response = exchange(method, url, null, null, extra)
        if (response.code !in 200..299) {
            response.close()
            throw IllegalStateException("WebDAV download failed (HTTP ${response.code})")
        }
        return response.stream
    }

    /**
     * Android's [java.net.HttpURLConnection] only allows a fixed set of methods, so PROPFIND and
     * MKCOL never leave the device. Speak HTTP/1.1 on a socket instead.
     */
    private fun request(
        method: String,
        url: String,
        length: Long?,
        open: (() -> InputStream)?,
        extra: Map<String, String> = emptyMap(),
    ): Int {
        val response = exchange(method, url, length, open, extra)
        response.close()
        return response.code
    }

    private fun exchange(
        method: String,
        url: String,
        length: Long?,
        open: (() -> InputStream)?,
        extra: Map<String, String>,
    ): DavResponse {
        val target = URI(url)
        val https = target.scheme.equals("https", ignoreCase = true)
        if (!https) throw IllegalArgumentException("WebDAV requires HTTPS.")
        val port = when {
            target.port != -1 -> target.port
            https -> 443
            else -> 80
        }
        val raw = Socket()
        raw.connect(InetSocketAddress(target.host, port), 30_000)
        raw.soTimeout = 60_000
        val socket: Socket = if (!https) {
            raw
        } else {
            val factory = SSLSocketFactory.getDefault() as SSLSocketFactory
            val ssl = factory.createSocket(raw, target.host, port, true) as SSLSocket
            ssl.startHandshake()
            ssl
        }
        val path = (target.rawPath?.takeIf { it.isNotEmpty() } ?: "/") +
            (target.rawQuery?.let { "?$it" } ?: "")
        val hostHeader = if ((https && port == 443) || (!https && port == 80)) target.host else "${target.host}:$port"
        val header = buildString {
            append(method).append(' ').append(path).append(" HTTP/1.1\r\n")
            append("Host: ").append(hostHeader).append("\r\n")
            append("Authorization: ").append(authorization()).append("\r\n")
            append("Connection: close\r\n")
            extra.forEach { (name, value) -> append(name).append(": ").append(value).append("\r\n") }
            if (open != null && length != null) append("Content-Length: ").append(length).append("\r\n")
            append("\r\n")
        }
        val output = socket.getOutputStream()
        output.write(header.toByteArray(Charsets.US_ASCII))
        if (open != null && length != null) {
            open().use { input -> StorageIo.copy(input, output) }
        }
        output.flush()
        val input = socket.getInputStream()
        val status = readAsciiLine(input)
        val code = status.split(' ').getOrNull(1)?.toIntOrNull()
            ?: throw IllegalStateException("WebDAV reply was not HTTP: $status")
        val headers = linkedMapOf<String, String>()
        while (true) {
            val line = readAsciiLine(input)
            if (line.isEmpty()) break
            val split = line.indexOf(':')
            if (split > 0) headers[line.substring(0, split).trim().lowercase()] = line.substring(split + 1).trim()
        }
        val body = when {
            headers["transfer-encoding"]?.contains("chunked", ignoreCase = true) == true -> ChunkedStream(input)
            headers.containsKey("content-length") -> LimitedStream(input, headers.getValue("content-length").toLong())
            else -> input
        }
        val closing = object : InputStream() {
            override fun read(): Int = body.read()
            override fun read(buffer: ByteArray, offset: Int, count: Int): Int = body.read(buffer, offset, count)
            override fun close() {
                socket.close()
            }
        }
        return DavResponse(code, closing, socket)
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
            return settings.copy(preset = preset, host = host, basePath = basePath, useHttps = true)
        }
    }
}

private class DavResponse(val code: Int, val stream: InputStream, private val socket: Socket) : AutoCloseable {
    override fun close() {
        try {
            stream.close()
        } finally {
            socket.close()
        }
    }
}

private class LimitedStream(private val input: InputStream, private var remaining: Long) : InputStream() {
    override fun read(): Int {
        if (remaining <= 0) return -1
        val value = input.read()
        if (value >= 0) remaining--
        return value
    }

    override fun read(buffer: ByteArray, offset: Int, count: Int): Int {
        if (remaining <= 0) return -1
        if (count == 0) return 0
        val read = input.read(buffer, offset, minOf(count.toLong(), remaining).toInt())
        if (read > 0) remaining -= read
        return read
    }
}

private class ChunkedStream(private val input: InputStream) : InputStream() {
    private var remaining = 0
    private var done = false

    override fun read(): Int {
        val one = ByteArray(1)
        val count = read(one, 0, 1)
        return if (count < 0) -1 else one[0].toInt() and 0xFF
    }

    override fun read(buffer: ByteArray, offset: Int, count: Int): Int {
        if (done) return -1
        if (count == 0) return 0
        if (remaining == 0) {
            val size = readAsciiLine(input).substringBefore(';').trim().toIntOrNull(16) ?: 0
            if (size == 0) {
                while (readAsciiLine(input).isNotEmpty()) Unit
                done = true
                return -1
            }
            remaining = size
        }
        val read = input.read(buffer, offset, minOf(count, remaining))
        if (read < 0) {
            done = true
            return -1
        }
        remaining -= read
        if (remaining == 0) readAsciiLine(input)
        return read
    }
}

private fun readAsciiLine(input: InputStream): String {
    val out = ByteArrayOutputStream()
    while (true) {
        val value = input.read()
        if (value < 0 || value == '\n'.code) break
        if (value != '\r'.code) out.write(value)
    }
    return out.toString(Charsets.US_ASCII.name())
}
