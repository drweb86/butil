package com.siarheikuchuk.butil.ui.controls

import android.content.ContentResolver
import android.net.Uri
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import com.siarheikuchuk.butil.core.misc.SourceItemHelper
import com.siarheikuchuk.butil.core.services.CompressionService
import com.siarheikuchuk.butil.core.services.EncryptionService
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
        get() = kind == TechnicalFileToolKind.DecryptAes256 || kind == TechnicalFileToolKind.EncryptAes256

    val suggestedOutputName: String
        get() = computeSuggestedOutput(sourceName)

    fun setSource(uri: Uri, name: String) {
        sourceUri = uri
        sourceName = name
        inputPathError = null
        statusText = null
        val suggested = computeSuggestedOutput(name)
        if (outputUri == null || outputName == lastSuggestedOutput) {
            outputUri = null
            outputName = ""
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
        val out = resolver.openOutputStream(output)
            ?: throw java.io.FileNotFoundException(fileNotFound + "\n" + outputName)
        input.use { sourceStream ->
            out.use { outputStream ->
                when (kind) {
                    TechnicalFileToolKind.DecryptAes256 ->
                        EncryptionService().decryptAes256(sourceStream, outputStream, password)
                    TechnicalFileToolKind.EncryptAes256 ->
                        EncryptionService().encryptAes256(sourceStream, outputStream, password)
                    TechnicalFileToolKind.DecompressBrotli ->
                        CompressionService().decompressBrotliStream(sourceStream, outputStream)
                    TechnicalFileToolKind.CompressBrotli ->
                        CompressionService().compressBrotliStream(sourceStream, outputStream)
                }
            }
        }
        return ""
    }

    private fun computeSuggestedOutput(input: String): String {
        if (input.isBlank()) {
            return ""
        }
        return when (kind) {
            TechnicalFileToolKind.DecryptAes256 ->
                stripSuffix(input, "." + SourceItemHelper.AES256V1Extension)
            TechnicalFileToolKind.EncryptAes256 ->
                input + "." + SourceItemHelper.AES256V1Extension
            TechnicalFileToolKind.DecompressBrotli ->
                stripSuffix(input, ".$BrotliExtensionNoDot")
            TechnicalFileToolKind.CompressBrotli ->
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
    }
}
