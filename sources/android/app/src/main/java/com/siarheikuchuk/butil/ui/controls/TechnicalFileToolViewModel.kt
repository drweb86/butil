package com.siarheikuchuk.butil.ui.controls

import android.content.ContentResolver
import android.net.Uri
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import com.siarheikuchuk.butil.core.misc.SourceItemHelper
import com.siarheikuchuk.butil.ui.controls.TechnicalFileToolKind.CompressBrotli
import com.siarheikuchuk.butil.ui.controls.TechnicalFileToolKind.DecryptAes256
import com.siarheikuchuk.butil.ui.controls.TechnicalFileToolKind.DecompressBrotli
import com.siarheikuchuk.butil.ui.controls.TechnicalFileToolKind.EncryptAes256
import android.provider.DocumentsContract
import com.siarheikuchuk.butil.core.services.CompressionService
import com.siarheikuchuk.butil.core.services.EncryptionService
import com.siarheikuchuk.butil.core.services.InvalidPasswordException
import java.io.File
import java.io.FileInputStream
import java.io.FileOutputStream
import java.util.Locale

/**
 * Form state for the Brotli and AES-256 tools, matching
 * {@code BUtil.UI.Controls.TechnicalFileToolViewModel}.
 */
class TechnicalFileToolViewModel(val kind: TechnicalFileToolKind) {
    var sourceUri by mutableStateOf<Uri?>(null)
    var sourceName by mutableStateOf("")
    var outputUri by mutableStateOf<Uri?>(null)
    var outputName by mutableStateOf("")
    var password by mutableStateOf("")
    var inputPathError by mutableStateOf<String?>(null)
    var outputPathError by mutableStateOf<String?>(null)
    var passwordError by mutableStateOf<String?>(null)
    var statusText by mutableStateOf<String?>(null)
    var statusIsError by mutableStateOf(false)
    var running by mutableStateOf(false)

    private var lastSuggestedOutput = ""

    val isPasswordVisible: Boolean
        get() = kind == DecryptAes256 || kind == EncryptAes256

    /**
     * Name placed in the system save dialog. It is not shown in the form until that dialog
     * returns a file.
     */
    fun outputPickerName(): String {
        computeSuggestedOutput(sourceName).takeIf { it.isNotBlank() }?.let { return it }
        sourceName.takeIf { it.isNotBlank() }?.let { return it }
        return when (kind) {
            EncryptAes256 -> "output.$AesExtension"
            CompressBrotli -> "output.$BrotliExtensionNoDot"
            else -> "output"
        }
    }

    fun sourceMimeTypes(): Array<String> = when (kind) {
        DecryptAes256, DecompressBrotli -> arrayOf(OCTET_STREAM, "*/*")
        else -> arrayOf("*/*")
    }

    fun setSource(uri: Uri, name: String) {
        sourceUri = uri
        sourceName = name
        inputPathError = null
        statusText = null
        val suggested = computeSuggestedOutput(name)
        if (outputUri == null || outputName == lastSuggestedOutput) {
            outputUri = null
            outputName = ""
            outputPathError = null
        }
        lastSuggestedOutput = suggested
    }

    fun setOutput(uri: Uri, name: String) {
        outputUri = uri
        outputName = name
        outputPathError = null
        statusText = null
    }

    fun onPasswordChange(value: String) {
        password = value
        passwordError = null
        statusText = null
    }

    fun validate(pathRequired: String, passwordRequired: String): Boolean {
        statusText = null
        inputPathError = if (sourceUri == null) pathRequired else null
        outputPathError = if (outputUri == null) pathRequired else null
        passwordError = if (isPasswordVisible && password.isEmpty()) passwordRequired else null
        return inputPathError == null && outputPathError == null && passwordError == null
    }

    fun execute(resolver: ContentResolver, fileNotFound: String): String {
        val source = sourceUri ?: error(fileNotFound)
        val output = outputUri ?: error(fileNotFound)
        val input = resolver.openInputStream(source)
            ?: throw java.io.FileNotFoundException(fileNotFound + "\n" + sourceName)
        if (kind == DecryptAes256) {
            decryptToOutput(resolver, input, output, fileNotFound)
            return ""
        }
        val out = resolver.openOutputStream(output)
            ?: throw java.io.FileNotFoundException(fileNotFound + "\n" + outputName)
        input.use { sourceStream ->
            out.use { outputStream ->
                when (kind) {
                    DecryptAes256 -> error("Decrypt writes the output only after the password is accepted.")
                    EncryptAes256 ->
                        EncryptionService().encryptAes256(sourceStream, outputStream, password)
                    DecompressBrotli ->
                        CompressionService().decompressBrotliStream(sourceStream, outputStream)
                    CompressBrotli ->
                        CompressionService().compressBrotliStream(sourceStream, outputStream)
                }
            }
        }
        return ""
    }

    fun isInvalidPassword(error: Throwable): Boolean {
        var current: Throwable? = error
        while (current != null) {
            if (current is InvalidPasswordException) return true
            current = current.cause
        }
        return false
    }

    /**
     * Decryption is finished in a temporary file. The chosen document is replaced only
     * after the password is accepted, and removed when it is not.
     */
    private fun decryptToOutput(resolver: ContentResolver, input: java.io.InputStream, output: Uri, fileNotFound: String) {
        val temp = File.createTempFile("butil-decrypt-", ".part")
        try {
            input.use { sourceStream ->
                FileOutputStream(temp).use { tempOut ->
                    EncryptionService().decryptAes256(sourceStream, tempOut, password)
                }
            }
            val dest = resolver.openOutputStream(output)
                ?: throw java.io.FileNotFoundException(fileNotFound + "\n" + outputName)
            dest.use { outputStream ->
                FileInputStream(temp).use { it.copyTo(outputStream) }
            }
        } catch (error: Exception) {
            if (discardOutput(resolver, output)) {
                outputUri = null
                outputName = ""
            }
            throw error
        } finally {
            temp.delete()
        }
    }

    private fun discardOutput(resolver: ContentResolver, uri: Uri): Boolean {
        return try {
            DocumentsContract.deleteDocument(resolver, uri)
        } catch (_: Exception) {
            try {
                resolver.delete(uri, null, null) > 0
            } catch (_: Exception) {
                false
            }
        }
    }

    private fun computeSuggestedOutput(input: String): String {
        if (input.isBlank()) {
            return ""
        }
        return when (kind) {
            DecryptAes256 ->
                stripSuffix(input, ".$AesExtension")
            EncryptAes256 ->
                input + ".$AesExtension"
            DecompressBrotli ->
                stripSuffix(input, ".$BrotliExtensionNoDot")
            CompressBrotli ->
                input + ".$BrotliExtensionNoDot"
        }
    }

    private fun stripSuffix(path: String, suffixWithDot: String): String {
        if (path.length > suffixWithDot.length && path.lowercase(Locale.ROOT).endsWith(suffixWithDot.lowercase(Locale.ROOT))) {
            return path.substring(0, path.length - suffixWithDot.length)
        }
        return ""
    }

    companion object {
        const val BrotliExtensionNoDot = "brotli"
        private val AesExtension = SourceItemHelper.AES256V1Extension
        private const val OCTET_STREAM = "application/octet-stream"
    }
}
