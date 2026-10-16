package com.siarheikuchuk.butil.core.storage

import java.io.ByteArrayOutputStream
import java.io.InputStream
import java.net.HttpURLConnection
import java.net.URL
import java.security.MessageDigest
import javax.crypto.Mac
import javax.crypto.spec.SecretKeySpec

internal object HttpStreams {
    fun sha256(open: () -> InputStream): String {
        val digest = MessageDigest.getInstance("SHA-256")
        open().use { input ->
            val buffer = ByteArray(1024 * 256)
            while (true) {
                val read = input.read(buffer)
                if (read < 0) break
                digest.update(buffer, 0, read)
            }
        }
        return toHex(digest.digest())
    }

    fun sha256(bytes: ByteArray): String = toHex(MessageDigest.getInstance("SHA-256").digest(bytes))

    fun toHex(bytes: ByteArray): String = bytes.joinToString("") { "%02x".format(it) }

    fun open(
        method: String,
        url: String,
        headers: Map<String, String>,
        body: ByteArray? = null,
        streamBody: (() -> InputStream)? = null,
        streamLength: Long = -1,
    ): HttpResult {
        val connection = (URL(url).openConnection() as HttpURLConnection).apply {
            requestMethod = method
            instanceFollowRedirects = false
            connectTimeout = 30_000
            readTimeout = 0
            doInput = true
            headers.forEach { (name, value) -> setRequestProperty(name, value) }
        }
        if (body != null) {
            connection.doOutput = true
            connection.outputStream.use { it.write(body) }
        } else if (streamBody != null) {
            connection.doOutput = true
            if (streamLength >= 0) connection.setFixedLengthStreamingMode(streamLength)
            connection.outputStream.use { output -> streamBody().use { input -> StorageIo.copy(input, output) } }
        }
        val code = connection.responseCode
        val stream = if (code in 200..299) connection.inputStream else connection.errorStream
        val payload = stream?.use { it.readBytesLimited(64 * 1024) } ?: ByteArray(0)
        val location = connection.getHeaderField("Location")
        connection.disconnect()
        return HttpResult(code, payload, location)
    }

    /** Reads a successful response body. Listings can be larger than the short probe used by [open]. */
    fun openBody(
        method: String,
        url: String,
        headers: Map<String, String>,
        limit: Int = 8 * 1024 * 1024,
    ): HttpResult {
        val connection = (URL(url).openConnection() as HttpURLConnection).apply {
            requestMethod = method
            instanceFollowRedirects = false
            connectTimeout = 30_000
            readTimeout = 0
            doInput = true
            headers.forEach { (name, value) -> setRequestProperty(name, value) }
        }
        val code = connection.responseCode
        val location = connection.getHeaderField("Location")
        val stream = if (code in 200..299) connection.inputStream else connection.errorStream
        val payload = stream?.use { it.readBytesLimited(limit) } ?: ByteArray(0)
        connection.disconnect()
        return HttpResult(code, payload, location)
    }

    fun openDownload(method: String, url: String, headers: Map<String, String>): InputStream {
        val connection = (URL(url).openConnection() as HttpURLConnection).apply {
            requestMethod = method
            instanceFollowRedirects = false
            connectTimeout = 30_000
            readTimeout = 0
            doInput = true
            headers.forEach { (name, value) -> setRequestProperty(name, value) }
        }
        val code = connection.responseCode
        if (code !in 200..299) {
            val error = connection.errorStream?.use { it.readBytesLimited(4096) }?.toString(Charsets.UTF_8).orEmpty()
            connection.disconnect()
            throw IllegalStateException("Download failed (HTTP $code). $error")
        }
        val source = connection.inputStream
        return object : InputStream() {
            override fun read(): Int = source.read()
            override fun read(b: ByteArray, off: Int, len: Int): Int = source.read(b, off, len)
            override fun close() {
                try {
                    source.close()
                } finally {
                    connection.disconnect()
                }
            }
        }
    }

    private fun InputStream.readBytesLimited(limit: Int): ByteArray {
        val out = ByteArrayOutputStream()
        val buffer = ByteArray(4096)
        var remaining = limit
        while (remaining > 0) {
            val read = read(buffer, 0, minOf(buffer.size, remaining))
            if (read < 0) break
            out.write(buffer, 0, read)
            remaining -= read
        }
        return out.toByteArray()
    }
}

internal data class HttpResult(val code: Int, val body: ByteArray, val location: String?) {
    fun text(): String = body.toString(Charsets.UTF_8)
    fun successful(): Boolean = code in 200..299
}

internal object AwsSigV4 {
    fun authorization(
        method: String,
        canonicalUri: String,
        canonicalQuery: String,
        host: String,
        region: String,
        accessKey: String,
        secretKey: String,
        payloadHash: String,
        amzDate: String,
        extraHeaders: Map<String, String> = emptyMap(),
    ): Map<String, String> {
        val date = amzDate.substring(0, 8)
        val headers = linkedMapOf(
            "host" to host,
            "x-amz-content-sha256" to payloadHash,
            "x-amz-date" to amzDate,
        )
        extraHeaders.forEach { (name, value) -> headers[name.lowercase()] = value.trim() }
        val sorted = headers.toSortedMap()
        val canonicalHeaders = sorted.entries.joinToString("") { (name, value) -> "$name:$value\n" }
        val signedHeaders = sorted.keys.joinToString(";")
        val canonical = listOf(method, canonicalUri, canonicalQuery, canonicalHeaders, signedHeaders, payloadHash)
            .joinToString("\n")
        val scope = "$date/$region/s3/aws4_request"
        val stringToSign = listOf("AWS4-HMAC-SHA256", amzDate, scope, HttpStreams.sha256(canonical.toByteArray(Charsets.UTF_8)))
            .joinToString("\n")
        val signature = HttpStreams.toHex(sign(signingKey(secretKey, date, region), stringToSign.toByteArray(Charsets.UTF_8)))
        val authorization = "AWS4-HMAC-SHA256 Credential=$accessKey/$scope, SignedHeaders=$signedHeaders, Signature=$signature"
        return mapOf(
            "Host" to host,
            "x-amz-content-sha256" to payloadHash,
            "x-amz-date" to amzDate,
            "Authorization" to authorization,
        ) + extraHeaders
    }

    fun encode(value: String): String {
        val out = StringBuilder()
        for (byte in value.toByteArray(Charsets.UTF_8)) {
            val ch = byte.toInt() and 0xFF
            if (ch in 'A'.code..'Z'.code || ch in 'a'.code..'z'.code || ch in '0'.code..'9'.code ||
                ch == '-'.code || ch == '_'.code || ch == '.'.code || ch == '~'.code
            ) {
                out.append(ch.toChar())
            } else {
                out.append("%%%02X".format(ch))
            }
        }
        return out.toString()
    }

    fun encodePath(path: String): String = path.split('/').joinToString("/") { encode(it) }

    private fun signingKey(secret: String, date: String, region: String): ByteArray {
        val dateKey = sign(("AWS4$secret").toByteArray(Charsets.UTF_8), date.toByteArray(Charsets.UTF_8))
        val regionKey = sign(dateKey, region.toByteArray(Charsets.UTF_8))
        val serviceKey = sign(regionKey, "s3".toByteArray(Charsets.UTF_8))
        return sign(serviceKey, "aws4_request".toByteArray(Charsets.UTF_8))
    }

    private fun sign(key: ByteArray, data: ByteArray): ByteArray {
        val mac = Mac.getInstance("HmacSHA256")
        mac.init(SecretKeySpec(key, "HmacSHA256"))
        return mac.doFinal(data)
    }
}

internal object AzureSharedKey {
    fun authorization(
        method: String,
        account: String,
        accountKey: String,
        resourcePath: String,
        query: Map<String, String>,
        headers: Map<String, String>,
        contentLength: Long?,
        contentType: String?,
    ): String {
        val canonicalHeaders = headers
            .mapKeys { it.key.lowercase() }
            .filterKeys { it.startsWith("x-ms-") }
            .toSortedMap()
            .entries
            .joinToString("") { (name, value) -> "$name:${value.trim()}\n" }
        val canonicalResource = buildString {
            append('/').append(account).append(resourcePath)
            query.toSortedMap().forEach { (name, value) ->
                append('\n').append(name.lowercase()).append(':').append(value)
            }
        }
        val lengthField = if (contentLength != null && contentLength > 0) contentLength.toString() else ""
        val stringToSign = listOf(
            method,
            "",
            "",
            lengthField,
            "",
            contentType ?: "",
            "",
            "",
            "",
            "",
            "",
            "",
        ).joinToString("\n") + "\n" + canonicalHeaders + canonicalResource
        val mac = Mac.getInstance("HmacSHA256")
        mac.init(SecretKeySpec(java.util.Base64.getDecoder().decode(accountKey), "HmacSHA256"))
        val signature = java.util.Base64.getEncoder().encodeToString(mac.doFinal(stringToSign.toByteArray(Charsets.UTF_8)))
        return "SharedKey $account:$signature"
    }
}
