package com.siarheikuchuk.butil.core.storage

import java.io.InputStream
import java.time.Instant
import java.time.ZoneOffset
import java.time.format.DateTimeFormatter
import java.util.Base64

internal class AzureBlobStorage(private val settings: AzureBlobSettings) : Storage {
    init {
        if (settings.accountName.isBlank()) throw IllegalArgumentException("Account name is not specified.")
        if (settings.accountKey.isBlank()) throw IllegalArgumentException("Account key is not specified.")
        if (settings.containerName.isBlank()) throw IllegalArgumentException("Container name is not specified.")
    }

    override fun test(): String? {
        return try {
            val result = call("GET", "", mapOf("restype" to "container"), null, null, null)
            if (result.successful()) null else "Azure Blob test failed (HTTP ${result.code}): ${result.text()}"
        } catch (error: Exception) {
            error.message ?: error.toString()
        }
    }

    override fun exists(relativeFileName: String): Boolean {
        val result = call("HEAD", key(relativeFileName), emptyMap(), null, null, null)
        if (result.code == 404) return false
        if (result.successful()) return true
        throw IllegalStateException("Azure HEAD failed (HTTP ${result.code}): ${result.text()}")
    }

    override fun upload(relativeFileName: String, length: Long, open: () -> InputStream) {
        val blob = key(relativeFileName)
        if (length in 0 until SINGLE_PUT_LIMIT) {
            val result = call("PUT", blob, emptyMap(), "application/octet-stream", length, open, extra = mapOf("x-ms-blob-type" to "BlockBlob"))
            if (!result.successful()) throw IllegalStateException("Azure upload failed (HTTP ${result.code}): ${result.text()}")
            return
        }
        uploadBlocks(blob, open)
    }

    override fun deleteFolder(relativeFolderName: String) {
        val prefix = key(relativeFolderName).trim('/') + "/"
        val listed = call("GET", "", mapOf("restype" to "container", "comp" to "list", "prefix" to prefix), null, null, null)
        if (!listed.successful()) throw IllegalStateException("Azure list failed (HTTP ${listed.code}): ${listed.text()}")
        val names = Regex("<Name>(.*?)</Name>").findAll(listed.text()).map { unescape(it.groupValues[1]) }
        for (name in names) {
            if (!name.startsWith(prefix)) continue
            val deleted = call("DELETE", name, emptyMap(), null, null, null)
            if (!deleted.successful() && deleted.code != 404) {
                throw IllegalStateException("Azure delete failed (HTTP ${deleted.code}): ${deleted.text()}")
            }
        }
    }

    private fun uploadBlocks(blob: String, open: () -> InputStream) {
        val ids = mutableListOf<String>()
        open().use { input ->
            val buffer = ByteArray(BLOCK_SIZE)
            var index = 0
            while (true) {
                val read = input.read(buffer)
                if (read < 0) break
                if (read == 0) continue
                val id = Base64.getEncoder().encodeToString("block-%06d".format(index).toByteArray(Charsets.US_ASCII))
                val chunk = buffer.copyOf(read)
                val result = call(
                    "PUT",
                    blob,
                    mapOf("comp" to "block", "blockid" to id),
                    "application/octet-stream",
                    read.toLong(),
                    { chunk.inputStream() },
                )
                if (!result.successful()) throw IllegalStateException("Azure block upload failed (HTTP ${result.code}): ${result.text()}")
                ids += id
                index++
            }
        }
        val xml = buildString {
            append("<?xml version=\"1.0\" encoding=\"utf-8\"?><BlockList>")
            ids.forEach { append("<Latest>").append(it).append("</Latest>") }
            append("</BlockList>")
        }.toByteArray(Charsets.UTF_8)
        val committed = call(
            "PUT",
            blob,
            mapOf("comp" to "blocklist"),
            "application/xml",
            xml.size.toLong(),
            { xml.inputStream() },
        )
        if (!committed.successful()) {
            throw IllegalStateException("Azure block list failed (HTTP ${committed.code}): ${committed.text()}")
        }
    }

    private fun key(relative: String?): String {
        val normalized = relative?.replace('\\', '/')?.trim('/') ?: ""
        val prefix = settings.pathPrefix.trim('/')
        if (prefix.isEmpty()) return normalized
        return if (normalized.isEmpty()) prefix else "$prefix/$normalized"
    }

    private fun call(
        method: String,
        blob: String,
        query: Map<String, String>,
        contentType: String?,
        length: Long?,
        open: (() -> InputStream)?,
        extra: Map<String, String> = emptyMap(),
    ): HttpResult {
        val date = DateTimeFormatter.RFC_1123_DATE_TIME.withZone(ZoneOffset.UTC).format(Instant.now())
        val versionHeaders = linkedMapOf(
            "x-ms-date" to date,
            "x-ms-version" to "2021-08-06",
        )
        versionHeaders.putAll(extra)
        val encodedBlob = if (blob.isEmpty()) "" else "/" + RemotePaths.encodeSegments(blob)
        val resourcePath = "/${settings.containerName}$encodedBlob"
        val authorization = AzureSharedKey.authorization(
            method = method,
            account = settings.accountName,
            accountKey = settings.accountKey,
            resourcePath = resourcePath,
            query = query,
            headers = versionHeaders,
            contentLength = length,
            contentType = contentType,
        )
        val headers = versionHeaders.toMutableMap()
        headers["Authorization"] = authorization
        if (contentType != null) headers["Content-Type"] = contentType
        val queryText = query.entries.joinToString("&") { (name, value) ->
            "${AwsSigV4.encode(name)}=${AwsSigV4.encode(value)}"
        }
        val url = "https://${settings.accountName}.blob.core.windows.net$resourcePath" +
            if (queryText.isEmpty()) "" else "?$queryText"
        return HttpStreams.open(method, url, headers, streamBody = open, streamLength = length ?: -1)
    }

    private fun unescape(value: String): String {
        return value.replace("&amp;", "&").replace("&lt;", "<").replace("&gt;", ">")
    }

    companion object {
        private const val BLOCK_SIZE = 8 * 1024 * 1024
        private const val SINGLE_PUT_LIMIT = 64L * 1024 * 1024
    }
}
