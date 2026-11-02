package com.siarheikuchuk.butil.ui.controls

import android.content.Context
import com.siarheikuchuk.butil.R
import com.siarheikuchuk.butil.core.storage.FtpsSettings
import com.siarheikuchuk.butil.core.storage.SftpSettings
import com.siarheikuchuk.butil.core.storage.StorageSettings
import java.security.MessageDigest
import java.security.cert.CertificateFactory
import java.security.cert.X509Certificate

/**
 * First connection to an unknown FTPS certificate or SFTP host key fills the
 * value into [settings] and returns the message to show. The connection itself
 * stays rejected until the next attempt, when the filled value is accepted.
 */
internal fun detectedTrustMessage(
    context: Context,
    settings: StorageSettings,
    beforeCertificate: String,
    beforeFingerprint: String,
): String? {
    if (settings is FtpsSettings && settings.trustedCertificate.isNotBlank() &&
        !settings.trustedCertificate.equals(beforeCertificate, ignoreCase = true)
    ) {
        return ftpsTrustText(context, settings)
    }
    if (settings is SftpSettings && settings.fingerPrintSha256.isNotBlank() &&
        !settings.fingerPrintSha256.equals(beforeFingerprint, ignoreCase = true)
    ) {
        return sftpTrustText(context, settings)
    }
    return null
}

private fun sftpTrustText(context: Context, settings: SftpSettings): String {
    val port = if (settings.port == 0) context.getString(R.string.trust_port_default_22) else settings.port.toString()
    return listOf(
        context.getString(R.string.trust_sftp_title),
        context.getString(R.string.trust_sftp_instruction),
        "",
        context.getString(R.string.trust_host, settings.host),
        context.getString(R.string.trust_port, port),
        context.getString(R.string.trust_user, settings.user),
        context.getString(R.string.trust_folder, settings.folder),
        context.getString(R.string.trust_fingerprint, settings.fingerPrintSha256),
    ).joinToString("\n")
}

private fun ftpsTrustText(context: Context, settings: FtpsSettings): String {
    val cert = parseFtpsCertificate(settings.trustedCertificate)
    val lines = mutableListOf<String>()
    if (cert != null && isSelfSigned(cert)) {
        lines += context.getString(R.string.trust_ftps_self_signed)
    } else {
        lines += context.getString(R.string.trust_ftps_title)
    }
    if (cert != null) {
        lines += ""
        lines += ftpsFingerprints(cert)
    } else {
        lines += context.getString(R.string.trust_cert_warning, context.getString(R.string.trust_cert_unreadable))
    }
    lines += ""
    lines += context.getString(R.string.trust_ftps_next)
    lines += ""
    lines += context.getString(R.string.trust_ftps_compare)
    return lines.joinToString("\n")
}

private fun ftpsFingerprints(cert: X509Certificate): String {
    return "SHA-256:  ${certificateFingerprint(cert, "SHA-256")}\n\nSHA-1:    ${certificateFingerprint(cert, "SHA-1")}"
}

private fun parseFtpsCertificate(hex: String): X509Certificate? {
    val cleaned = hex.trim().replace(":", "").replace(" ", "")
    if (cleaned.length < 64 || cleaned.length % 2 != 0) return null
    if (cleaned.any { it !in '0'..'9' && it !in 'A'..'F' && it !in 'a'..'f' }) return null
    return try {
        val bytes = cleaned.chunked(2).map { it.toInt(16).toByte() }.toByteArray()
        CertificateFactory.getInstance("X.509").generateCertificate(bytes.inputStream()) as X509Certificate
    } catch (_: Exception) {
        null
    }
}

private fun isSelfSigned(cert: X509Certificate): Boolean {
    if (cert.subjectX500Principal == cert.issuerX500Principal) return true
    return try {
        cert.verify(cert.publicKey)
        true
    } catch (_: Exception) {
        false
    }
}

private fun certificateFingerprint(cert: X509Certificate, algorithm: String): String {
    return MessageDigest.getInstance(algorithm).digest(cert.encoded).joinToString(":") { "%02X".format(it) }
}
