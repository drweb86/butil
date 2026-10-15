package com.siarheikuchuk.butil.core.storage

import android.security.keystore.KeyGenParameterSpec
import android.security.keystore.KeyProperties
import java.security.KeyStore
import javax.crypto.Cipher
import javax.crypto.KeyGenerator
import javax.crypto.SecretKey
import javax.crypto.spec.GCMParameterSpec

/**
 * Protects storage secrets the same way the desktop app does at rest: the plaintext is replaced
 * by an `enc::` prefix plus protected bytes. The bytes themselves are sealed with the Android
 * keystore, so a desktop DPAPI blob cannot be opened here and an Android blob cannot be opened
 * on the desktop.
 */
class SecretCipher {
    fun protect(value: String): String {
        if (value.isBlank() || value.startsWith(PREFIX)) return value
        val cipher = Cipher.getInstance(TRANSFORMATION)
        cipher.init(Cipher.ENCRYPT_MODE, key())
        val encrypted = cipher.doFinal(value.toByteArray(Charsets.UTF_8))
        val packed = cipher.iv + encrypted
        return PREFIX + java.util.Base64.getEncoder().encodeToString(packed)
    }

    fun unprotect(value: String): String {
        if (!value.startsWith(PREFIX)) return value
        val packed = java.util.Base64.getDecoder().decode(value.substring(PREFIX.length))
        if (packed.size < IV_LENGTH + 1) {
            throw IllegalArgumentException("Stored secret is not an Android-protected value.")
        }
        val iv = packed.copyOfRange(0, IV_LENGTH)
        val body = packed.copyOfRange(IV_LENGTH, packed.size)
        val cipher = Cipher.getInstance(TRANSFORMATION)
        cipher.init(Cipher.DECRYPT_MODE, key(), GCMParameterSpec(128, iv))
        return cipher.doFinal(body).toString(Charsets.UTF_8)
    }

    fun protectSettings(settings: StorageSettings): StorageSettings {
        val updated = settings.secretFields().mapValues { (_, value) -> protect(value) }
        return settings.withSecrets(updated)
    }

    fun unprotectSettings(settings: StorageSettings): StorageSettings {
        val updated = settings.secretFields().mapValues { (_, value) -> unprotect(value) }
        return settings.withSecrets(updated)
    }

    private fun key(): SecretKey {
        val store = KeyStore.getInstance(ANDROID_KEYSTORE).apply { load(null) }
        val existing = store.getKey(ALIAS, null) as? SecretKey
        if (existing != null) return existing
        val generator = KeyGenerator.getInstance(KeyProperties.KEY_ALGORITHM_AES, ANDROID_KEYSTORE)
        val spec = KeyGenParameterSpec.Builder(
            ALIAS,
            KeyProperties.PURPOSE_ENCRYPT or KeyProperties.PURPOSE_DECRYPT,
        )
            .setBlockModes(KeyProperties.BLOCK_MODE_GCM)
            .setEncryptionPaddings(KeyProperties.ENCRYPTION_PADDING_NONE)
            .setKeySize(256)
            .build()
        generator.init(spec)
        return generator.generateKey()
    }

    companion object {
        const val PREFIX = "enc::"
        private const val ALIAS = "butil-task-secrets"
        private const val ANDROID_KEYSTORE = "AndroidKeyStore"
        private const val TRANSFORMATION = "AES/GCM/NoPadding"
        private const val IV_LENGTH = 12
    }
}
