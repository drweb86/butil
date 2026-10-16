package com.siarheikuchuk.butil.core.ftps

import android.content.Context
import android.util.Base64
import com.siarheikuchuk.butil.core.storage.SecretCipher
import java.io.File
import java.net.Inet6Address
import java.net.InetAddress
import java.net.NetworkInterface
import java.security.KeyPairGenerator
import java.security.KeyStore
import java.security.SecureRandom
import java.security.cert.X509Certificate
import java.util.Date
import javax.net.ssl.KeyManagerFactory
import javax.net.ssl.SSLContext
import org.bouncycastle.asn1.x500.X500Name
import org.bouncycastle.asn1.x509.BasicConstraints
import org.bouncycastle.asn1.x509.Extension
import org.bouncycastle.asn1.x509.GeneralName
import org.bouncycastle.asn1.x509.GeneralNames
import org.bouncycastle.asn1.x509.KeyUsage
import org.bouncycastle.cert.jcajce.JcaX509CertificateConverter
import org.bouncycastle.cert.jcajce.JcaX509v3CertificateBuilder
import org.bouncycastle.jce.provider.BouncyCastleProvider
import org.bouncycastle.operator.jcajce.JcaContentSignerBuilder
import java.math.BigInteger

data class LoadedCertificate(
    val sslContext: SSLContext,
    val x509: X509Certificate,
    val fingerprintSha256: String,
    val fingerprintSha1: String,
    val isSelfSigned: Boolean,
)

object Certificates {
    private const val LEGACY_PASSWORD = "test"
    private const val WEEK_MS = 7L * 24 * 60 * 60 * 1000

    // Android already registers a truncated provider named "BC". Looking up "BC"
    // by name hits that stub (no SHA256withRSA). Use our bundled instance instead.
    private val bc = BouncyCastleProvider()

    fun loadOrCreate(context: Context, filesDir: File, settings: FtpsServerSettings, log: FtpsLog): LoadedCertificate {
        if (!settings.certificatePath.isNullOrBlank()) {
            return loadFromFile(File(settings.certificatePath), settings.certificatePassword, log)
        }
        return getOrCreateSelfSigned(context, filesDir, log)
    }

    private fun getOrCreateSelfSigned(context: Context, filesDir: File, log: FtpsLog): LoadedCertificate {
        val cipher = SecretCipher()
        val dir = File(filesDir, "Certificates-Android-V1")
        dir.mkdirs()
        val pfx = File(dir, "Self-Signed.pfx")
        val passwordFile = File(dir, "Self-Signed.password")
        if (pfx.exists()) {
            readStoredPassword(cipher, passwordFile)?.let { password ->
                try {
                    val loaded = loadFromKeyStore(pfx, password)
                    if (isCurrent(loaded)) {
                        log.info("Loading self-signed certificate from file ${pfx.absolutePath}.")
                        return loaded
                    }
                } catch (e: Exception) {
                    log.warn("Stored certificate could not be loaded: ${e.message}")
                }
            }
            try {
                val keyStore = loadPkcs12(pfx, LEGACY_PASSWORD)
                val loaded = fromKeyStore(keyStore, LEGACY_PASSWORD)
                if (isCurrent(loaded)) {
                    val password = newPassword()
                    rewriteEntryPassword(keyStore, LEGACY_PASSWORD, password)
                    val tmp = File(dir, "Self-Signed.pfx.tmp")
                    tmp.outputStream().use { keyStore.store(it, password.toCharArray()) }
                    passwordFile.writeText(cipher.protect(password))
                    if (!tmp.renameTo(pfx)) {
                        tmp.copyTo(pfx, overwrite = true)
                        tmp.delete()
                    }
                    log.info("Re-protected self-signed certificate ${pfx.absolutePath}.")
                    return fromKeyStore(keyStore, password)
                }
            } catch (e: Exception) {
                log.warn("Stored certificate could not be loaded: ${e.message}")
            }
        }
        log.info("Creating self-signed certificate for file ${pfx.absolutePath}.")
        val password = newPassword()
        val created = createSelfSigned(password)
        val tmp = File(dir, "Self-Signed.pfx.tmp")
        tmp.outputStream().use { created.keyStore.store(it, password.toCharArray()) }
        passwordFile.writeText(cipher.protect(password))
        if (!tmp.renameTo(pfx)) {
            tmp.copyTo(pfx, overwrite = true)
            tmp.delete()
        }
        return created.toLoaded()
    }

    private fun readStoredPassword(cipher: SecretCipher, passwordFile: File): String? {
        if (!passwordFile.exists()) return null
        val stored = passwordFile.readText()
        if (!stored.startsWith(SecretCipher.PREFIX)) return null
        return cipher.unprotect(stored)
    }

    private fun newPassword(): String {
        val bytes = ByteArray(32)
        SecureRandom().nextBytes(bytes)
        return Base64.encodeToString(bytes, Base64.NO_WRAP)
    }

    private fun isCurrent(loaded: LoadedCertificate): Boolean {
        if (loaded.x509.notAfter.time <= System.currentTimeMillis() + WEEK_MS) return false
        val present = sanAddresses(loaded.x509)
        return advertisedAddresses().all { it in present }
    }

    // Global and unique-local addresses go on the certificate. Link-local addresses need a
    // zone id the certificate cannot store.
    private fun advertisedHosts(): List<String> {
        val ips = linkedSetOf<String>()
        val ifaces = NetworkInterface.getNetworkInterfaces() ?: return emptyList()
        for (nic in ifaces) {
            if (!nic.isUp || nic.isLoopback) continue
            for (addr in nic.inetAddresses) {
                if (!isAdvertised(addr)) continue
                addr.hostAddress?.substringBefore('%')?.let { ips += it }
            }
        }
        return ips.toList()
    }

    private fun advertisedAddresses(): Set<String> =
        advertisedHosts().mapNotNull { addressKey(it) }.toSet()

    private fun isAdvertised(addr: InetAddress): Boolean {
        if (addr.isLoopbackAddress || addr.isLinkLocalAddress || addr.isMulticastAddress) return false
        if (addr is Inet6Address) {
            val bytes = addr.address
            val mapped = bytes.size == 16 &&
                (0..9).all { bytes[it].toInt() == 0 } &&
                bytes[10] == 0xff.toByte() &&
                bytes[11] == 0xff.toByte()
            return !mapped
        }
        return true
    }

    private fun sanAddresses(cert: X509Certificate): Set<String> {
        val ips = mutableSetOf<String>()
        val names = try {
            cert.subjectAlternativeNames
        } catch (_: Exception) {
            null
        } ?: return ips
        for (name in names) {
            if (name.size < 2) continue
            val type = (name[0] as? Number)?.toInt() ?: continue
            if (type != 7) continue
            when (val value = name[1]) {
                is String -> addressKey(value)?.let { ips += it }
                is ByteArray -> {
                    if (value.size == 4 || value.size == 16) {
                        InetAddress.getByAddress(value).hostAddress?.let { addressKey(it)?.let { key -> ips += key } }
                    }
                }
            }
        }
        return ips
    }

    private fun addressKey(text: String?): String? {
        if (text.isNullOrBlank()) return null
        val host = text.substringBefore('%')
        return try {
            InetAddress.getByName(host).address.joinToString(":") { "%02x".format(it) }
        } catch (_: Exception) {
            host
        }
    }

    private fun loadPkcs12(file: File, password: String): KeyStore {
        val ks = KeyStore.getInstance("PKCS12")
        file.inputStream().use { ks.load(it, password.toCharArray()) }
        return ks
    }

    private fun rewriteEntryPassword(ks: KeyStore, oldPassword: String, newPassword: String) {
        val alias = ks.aliases().toList().first { ks.isKeyEntry(it) }
        val key = ks.getKey(alias, oldPassword.toCharArray())
        val chain = ks.getCertificateChain(alias)
        ks.setKeyEntry(alias, key, newPassword.toCharArray(), chain)
    }

    private fun loadFromFile(file: File, password: String?, log: FtpsLog): LoadedCertificate {
        log.info("Loading certificate from ${file.absolutePath}")
        val ext = file.extension.lowercase()
        return when (ext) {
            "pfx", "p12" -> loadFromKeyStore(file, password ?: "")
            "pem", "der", "crt", "cer" -> error("PEM/DER certificates without a private key are not supported. Use a .pfx file.")
            else -> error("Certificate path extension $ext is not recognizable. Use .pfx")
        }
    }

    private fun loadFromKeyStore(file: File, password: String): LoadedCertificate =
        fromKeyStore(loadPkcs12(file, password), password)

    private class CreatedCert(val keyStore: KeyStore, val password: String, val cert: X509Certificate) {
        fun toLoaded() = fromKeyStore(keyStore, password)
    }

    private fun createSelfSigned(password: String): CreatedCert {
        val keyPair = KeyPairGenerator.getInstance("RSA").apply { initialize(2048) }.generateKeyPair()
        val now = System.currentTimeMillis()
        val notBefore = Date(now - 24L * 60 * 60 * 1000)
        val notAfter = Date(now + 3650L * 24 * 60 * 60 * 1000)
        val name = X500Name("CN=FtpsServerLibrary-SelfSigned-Certificates")
        val builder = JcaX509v3CertificateBuilder(
            name,
            BigInteger.valueOf(now),
            notBefore,
            notAfter,
            name,
            keyPair.public,
        )
        builder.addExtension(Extension.basicConstraints, true, BasicConstraints(false))
        builder.addExtension(
            Extension.keyUsage,
            false,
            KeyUsage(KeyUsage.digitalSignature or KeyUsage.keyEncipherment or KeyUsage.dataEncipherment),
        )
        val san = mutableListOf(
            GeneralName(GeneralName.iPAddress, "127.0.0.1"),
            GeneralName(GeneralName.iPAddress, "::1"),
            GeneralName(GeneralName.dNSName, "localhost"),
        )
        for (addr in advertisedHosts()) {
            san += GeneralName(GeneralName.iPAddress, addr)
        }
        val names = GeneralNames(san.toTypedArray())
        builder.addExtension(Extension.subjectAlternativeName, false, names)
        val signer = JcaContentSignerBuilder("SHA256withRSA").setProvider(bc).build(keyPair.private)
        val cert = JcaX509CertificateConverter().setProvider(bc).getCertificate(builder.build(signer))
        val ks = KeyStore.getInstance("PKCS12")
        ks.load(null, password.toCharArray())
        ks.setKeyEntry("ftpsserver", keyPair.private, password.toCharArray(), arrayOf(cert))
        return CreatedCert(ks, password, cert)
    }

    private fun fromKeyStore(ks: KeyStore, password: String): LoadedCertificate {
        val alias = ks.aliases().toList().first { ks.isKeyEntry(it) }
        val cert = ks.getCertificate(alias) as X509Certificate
        val kmf = KeyManagerFactory.getInstance(KeyManagerFactory.getDefaultAlgorithm())
        kmf.init(ks, password.toCharArray())
        val ctx = SSLContext.getInstance("TLS")
        ctx.init(kmf.keyManagers, null, null)
        return LoadedCertificate(
            sslContext = ctx,
            x509 = cert,
            fingerprintSha256 = fingerprint(cert, "SHA-256"),
            fingerprintSha1 = fingerprint(cert, "SHA-1"),
            isSelfSigned = cert.subjectDN == cert.issuerDN || cert.subjectX500Principal == cert.issuerX500Principal,
        )
    }

    private fun fingerprint(cert: X509Certificate, alg: String): String {
        val md = java.security.MessageDigest.getInstance(alg)
        val hex = md.digest(cert.encoded).joinToString("") { "%02X".format(it) }
        return hex.chunked(2).joinToString(":")
    }
}
