package com.siarheikuchuk.butil.core.storage

import java.io.InputStream
import java.time.Instant
import java.time.ZoneOffset
import java.time.format.DateTimeFormatter

internal class S3Storage(private val settings: S3Settings) : Storage {
    private val endpoint: Endpoint

    init {
        if (settings.accessKey.isBlank()) throw IllegalArgumentException("Access key is not specified.")
        if (settings.secretKey.isBlank()) throw IllegalArgumentException("Secret key is not specified.")
        if (settings.bucketName.isBlank()) throw IllegalArgumentException("Bucket name is not specified.")
        endpoint = endpoint(settings)
    }

    override fun test(): String? {
        return try {
            val result = call("GET", "", "list-type=2&max-keys=1", HttpStreams.sha256(ByteArray(0)), null, -1)
            if (result.successful()) null else "S3 test failed (HTTP ${result.code}): ${result.text()}"
        } catch (error: Exception) {
            error.message ?: error.toString()
        }
    }

    override fun exists(relativeFileName: String): Boolean {
        val result = call("HEAD", key(relativeFileName), "", HttpStreams.sha256(ByteArray(0)), null, -1)
        if (result.code == 404) return false
        if (result.successful()) return true
        throw IllegalStateException("S3 HEAD failed (HTTP ${result.code}): ${result.text()}")
    }

    override fun upload(relativeFileName: String, length: Long, open: () -> InputStream) {
        val objectKey = key(relativeFileName)
        val hash = HttpStreams.sha256(open)
        val result = call("PUT", objectKey, "", hash, open, length)
        if (!result.successful()) {
            throw IllegalStateException("S3 upload failed (HTTP ${result.code}): ${result.text()}")
        }
    }

    override fun download(relativeFileName: String): InputStream {
        return openDownload("GET", key(relativeFileName), "")
    }

    override fun delete(relativeFileName: String) {
        val deleted = call("DELETE", key(relativeFileName), "", HttpStreams.sha256(ByteArray(0)), null, -1)
        if (!deleted.successful() && deleted.code != 404) {
            throw IllegalStateException("S3 delete failed (HTTP ${deleted.code}): ${deleted.text()}")
        }
    }

    override fun listFiles(): List<RemoteFile> {
        val prefix = key(null).trim('/')
        val root = if (prefix.isEmpty()) "" else "$prefix/"
        val files = mutableListOf<RemoteFile>()
        var token: String? = null
        while (true) {
            val query = linkedMapOf("list-type" to "2", "max-keys" to "1000")
            if (root.isNotEmpty()) query["prefix"] = root
            if (token != null) query["continuation-token"] = token
            val canonical = query.toSortedMap().entries.joinToString("&") { (name, value) ->
                "${AwsSigV4.encode(name)}=${AwsSigV4.encode(value)}"
            }
            val listed = callBody("GET", "", canonical)
            if (!listed.successful()) throw IllegalStateException("S3 list failed (HTTP ${listed.code}): ${listed.text()}")
            val xml = listed.text()
            val blocks = Regex("(?s)<Contents>(.*?)</Contents>").findAll(xml)
            for (block in blocks) {
                val body = block.groupValues[1]
                val itemKey = Regex("<Key>(.*?)</Key>").find(body)?.groupValues?.get(1)?.let(::xmlUnescape) ?: continue
                if (!itemKey.startsWith(root) || itemKey.endsWith("/")) continue
                val relative = itemKey.removePrefix(root)
                val size = Regex("<Size>(\\d+)</Size>").find(body)?.groupValues?.get(1)?.toLongOrNull() ?: 0L
                val modified = Regex("<LastModified>(.*?)</LastModified>").find(body)?.groupValues?.get(1)
                val millis = modified?.let { runCatching { java.time.Instant.parse(it).toEpochMilli() }.getOrDefault(0L) } ?: 0L
                files += RemoteFile(relative, size, millis)
            }
            val truncated = Regex("<IsTruncated>(.*?)</IsTruncated>").find(xml)?.groupValues?.get(1) == "true"
            if (!truncated) break
            token = Regex("<NextContinuationToken>(.*?)</NextContinuationToken>").find(xml)?.groupValues?.get(1)
                ?.let(::xmlUnescape) ?: break
        }
        return files.sortedBy { it.relativePath }
    }

    override fun listFolders(): List<String> {
        val prefix = key(null).trim('/')
        val root = if (prefix.isEmpty()) "" else "$prefix/"
        val query = linkedMapOf("delimiter" to "/", "list-type" to "2", "max-keys" to "1000")
        if (root.isNotEmpty()) query["prefix"] = root
        val canonical = query.toSortedMap().entries.joinToString("&") { (name, value) ->
            "${AwsSigV4.encode(name)}=${AwsSigV4.encode(value)}"
        }
        val listed = callBody("GET", "", canonical)
        if (!listed.successful()) return emptyList()
        return Regex("<Prefix>(.*?)</Prefix>").findAll(listed.text()).map { xmlUnescape(it.groupValues[1]) }
            .filter { it.startsWith(root) && it != root }
            .map { it.removePrefix(root).trim('/') }
            .filter { it.isNotEmpty() && !it.contains('/') }
            .distinct()
            .sorted()
            .toList()
    }

    private fun xmlUnescape(value: String): String {
        return value.replace("&amp;", "&").replace("&lt;", "<").replace("&gt;", ">").replace("&quot;", "\"")
    }

    private fun callBody(method: String, objectKey: String, query: String): HttpResult {
        val amzDate = DateTimeFormatter.ofPattern("yyyyMMdd'T'HHmmss'Z'")
            .withZone(ZoneOffset.UTC)
            .format(Instant.now())
        val headers = AwsSigV4.authorization(
            method = method,
            canonicalUri = endpoint.canonicalUri(objectKey),
            canonicalQuery = query,
            host = endpoint.hostHeader,
            region = endpoint.region,
            accessKey = settings.accessKey,
            secretKey = settings.secretKey,
            payloadHash = HttpStreams.sha256(ByteArray(0)),
            amzDate = amzDate,
        )
        return HttpStreams.openBody(method, endpoint.url(objectKey, query), headers)
    }

    private fun openDownload(method: String, objectKey: String, query: String): InputStream {
        val amzDate = DateTimeFormatter.ofPattern("yyyyMMdd'T'HHmmss'Z'")
            .withZone(ZoneOffset.UTC)
            .format(Instant.now())
        val headers = AwsSigV4.authorization(
            method = method,
            canonicalUri = endpoint.canonicalUri(objectKey),
            canonicalQuery = query,
            host = endpoint.hostHeader,
            region = endpoint.region,
            accessKey = settings.accessKey,
            secretKey = settings.secretKey,
            payloadHash = HttpStreams.sha256(ByteArray(0)),
            amzDate = amzDate,
        )
        return HttpStreams.openDownload(method, endpoint.url(objectKey, query), headers)
    }

    override fun deleteFolder(relativeFolderName: String) {
        val prefix = key(relativeFolderName).trim('/') + "/"
        val listed = call(
            "GET",
            "",
            "list-type=2&prefix=${AwsSigV4.encode(prefix)}",
            HttpStreams.sha256(ByteArray(0)),
            null,
            -1,
        )
        if (!listed.successful()) {
            throw IllegalStateException("S3 list failed (HTTP ${listed.code}): ${listed.text()}")
        }
        val keys = Regex("<Key>(.*?)</Key>").findAll(listed.text()).map { it.groupValues[1] }.toList()
        for (item in keys) {
            val deleted = call("DELETE", item, "", HttpStreams.sha256(ByteArray(0)), null, -1)
            if (!deleted.successful() && deleted.code != 404) {
                throw IllegalStateException("S3 delete failed (HTTP ${deleted.code}): ${deleted.text()}")
            }
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
        objectKey: String,
        query: String,
        payloadHash: String,
        body: (() -> InputStream)?,
        length: Long,
    ): HttpResult {
        val amzDate = DateTimeFormatter.ofPattern("yyyyMMdd'T'HHmmss'Z'")
            .withZone(ZoneOffset.UTC)
            .format(Instant.now())
        val canonicalUri = endpoint.canonicalUri(objectKey)
        val headers = AwsSigV4.authorization(
            method = method,
            canonicalUri = canonicalUri,
            canonicalQuery = query,
            host = endpoint.hostHeader,
            region = endpoint.region,
            accessKey = settings.accessKey,
            secretKey = settings.secretKey,
            payloadHash = payloadHash,
            amzDate = amzDate,
        )
        val url = endpoint.url(objectKey, query)
        return HttpStreams.open(method, url, headers, streamBody = body, streamLength = length)
    }

    private data class Endpoint(
        val hostHeader: String,
        val region: String,
        val baseUrl: String,
        val pathStyle: Boolean,
        val bucket: String,
        val scheme: String,
    ) {
        fun canonicalUri(objectKey: String): String {
            val encodedKey = if (objectKey.isEmpty()) "" else AwsSigV4.encodePath(objectKey)
            return if (pathStyle) {
                if (encodedKey.isEmpty()) "/$bucket" else "/$bucket/$encodedKey"
            } else {
                if (encodedKey.isEmpty()) "/" else "/$encodedKey"
            }
        }

        fun url(objectKey: String, query: String): String {
            val path = canonicalUri(objectKey)
            val queryPart = if (query.isEmpty()) "" else "?$query"
            return "$scheme://$hostHeader$path$queryPart"
        }
    }

    companion object {
        private fun endpoint(settings: S3Settings): Endpoint {
            val region = when {
                settings.region.isNotBlank() -> settings.region
                settings.provider == "YandexObjectStorage" -> "ru-central1"
                settings.provider == "AWSS3" -> "us-east-1"
                else -> "us-east-1"
            }
            val serviceUrl = settings.serviceUrl.ifBlank {
                when (settings.provider) {
                    "Wasabi" -> "https://s3.wasabisys.com"
                    "GoogleCloudStorage" -> "https://storage.googleapis.com"
                    "YandexObjectStorage" -> "https://storage.yandexcloud.net"
                    "VKCloudStorage" -> "https://hb.ru-msk.vkcloud-storage.ru"
                    "CloudRuStorage" -> "https://obs.ru-moscow-1.hc.sbercloud.ru"
                    "SelectelStorage" -> "https://s3.storage.selcloud.ru"
                    "AWSS3" -> awsUrl(region)
                    else -> ""
                }
            }
            if (serviceUrl.isBlank() && settings.provider != "AWSS3") {
                throw IllegalArgumentException("Service URL is not specified.")
            }
            val actualUrl = serviceUrl.ifBlank { awsUrl(region) }.trimEnd('/')
            val scheme = if (actualUrl.startsWith("http://")) "http" else "https"
            val host = actualUrl.removePrefix("https://").removePrefix("http://").substringBefore('/')
            val pathStyle = settings.provider !in setOf("AWSS3", "AlibabaCloudOSS", "TencentCloudCOS") ||
                (settings.provider == "AWSS3" && settings.serviceUrl.isNotBlank())
            val virtualHost = if (pathStyle) host else "${settings.bucketName}.$host"
            return Endpoint(virtualHost, region, actualUrl, pathStyle, settings.bucketName, scheme)
        }

        private fun awsUrl(region: String): String = when (region) {
            "cn-north-1", "cn-northwest-1" -> "https://s3.$region.amazonaws.com.cn"
            "", "us-east-1" -> "https://s3.amazonaws.com"
            else -> "https://s3.$region.amazonaws.com"
        }
    }
}
