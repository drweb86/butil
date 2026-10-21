package com.siarheikuchuk.butil.ui.controls

import android.app.Activity
import android.content.ContentResolver
import android.content.Context
import android.content.Intent
import android.net.Uri
import android.provider.OpenableColumns
import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.result.contract.ActivityResultContract
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.foundation.verticalScroll
import androidx.compose.material3.Button
import androidx.compose.material3.LinearProgressIndicator
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedButton
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.unit.dp
import com.siarheikuchuk.butil.R
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext

@Composable
fun TechnicalFileToolView(
    kind: TechnicalFileToolKind,
    onClose: () -> Unit,
    modifier: Modifier = Modifier,
) {
    val viewModel = remember(kind) { TechnicalFileToolViewModel(kind) }
    val context = LocalContext.current
    val scope = rememberCoroutineScope()
    val title = stringResource(kind.titleRes())
    val pathRequired = stringResource(R.string.technical_tool_error_path_required)
    val passwordRequired = stringResource(R.string.technical_tool_error_password_required)
    val passwordIncorrect = stringResource(R.string.technical_tool_error_password_incorrect)
    val fileNotFound = stringResource(R.string.technical_tool_error_file_not_found)
    val completed = stringResource(R.string.technical_tool_completed)

    val openSource = rememberLauncherForActivityResult(OpenTechnicalDocument()) { uri ->
        if (uri != null) {
            persistPermission(context.contentResolver, uri, Intent.FLAG_GRANT_READ_URI_PERMISSION)
            viewModel.setSource(uri, displayName(context.contentResolver, uri))
        }
    }
    val createOutput = rememberLauncherForActivityResult(
        ActivityResultContracts.CreateDocument("application/octet-stream"),
    ) { uri ->
        if (uri != null) {
            persistPermission(
                context.contentResolver,
                uri,
                Intent.FLAG_GRANT_READ_URI_PERMISSION or Intent.FLAG_GRANT_WRITE_URI_PERMISSION,
            )
            viewModel.setOutput(uri, displayName(context.contentResolver, uri))
        }
    }

    Column(
        modifier = modifier
            .fillMaxSize()
            .verticalScroll(rememberScrollState())
            .padding(24.dp),
        verticalArrangement = Arrangement.spacedBy(12.dp),
    ) {
        Text(text = title, style = MaterialTheme.typography.headlineSmall)
        FileRow(
            label = stringResource(R.string.technical_tool_source_file),
            value = viewModel.sourceName,
            placeholder = stringResource(R.string.technical_tool_pick_source_file),
            error = viewModel.inputPathError,
            enabled = !viewModel.running,
            onBrowse = { openSource.launch(viewModel.sourceMimeTypes()) },
        )
        FileRow(
            label = stringResource(R.string.technical_tool_output_file),
            value = viewModel.outputName,
            placeholder = stringResource(R.string.technical_tool_pick_output_file),
            error = viewModel.outputPathError,
            enabled = !viewModel.running,
            onBrowse = { createOutput.launch(viewModel.outputPickerName()) },
        )
        if (viewModel.isPasswordVisible) {
            OutlinedTextField(
                value = viewModel.password,
                onValueChange = viewModel::onPasswordChange,
                modifier = Modifier.fillMaxWidth(),
                label = { Text(stringResource(R.string.password_field)) },
                visualTransformation = PasswordVisualTransformation(),
                keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Password),
                singleLine = true,
                enabled = !viewModel.running,
                isError = viewModel.passwordError != null,
                supportingText = {
                    val error = viewModel.passwordError
                    if (error != null) {
                        Text(error)
                    }
                },
            )
        }
        if (viewModel.running) {
            LinearProgressIndicator(modifier = Modifier.fillMaxWidth())
        }
        viewModel.statusText?.let { message ->
            Text(
                text = message,
                color = if (viewModel.statusIsError) {
                    MaterialTheme.colorScheme.error
                } else {
                    MaterialTheme.colorScheme.primary
                },
            )
        }
        Row(horizontalArrangement = Arrangement.spacedBy(12.dp)) {
            Button(
                enabled = !viewModel.running,
                onClick = {
                    if (!viewModel.validate(pathRequired, passwordRequired)) {
                        return@Button
                    }
                    scope.launch {
                        viewModel.running = true
                        viewModel.statusText = null
                        try {
                            withContext(Dispatchers.IO) {
                                viewModel.execute(context.contentResolver, fileNotFound)
                            }
                            viewModel.statusIsError = false
                            viewModel.statusText = completed
                        } catch (error: Exception) {
                            viewModel.statusIsError = true
                            if (viewModel.isInvalidPassword(error)) {
                                viewModel.passwordError = passwordIncorrect
                                viewModel.statusText = passwordIncorrect
                            } else {
                                viewModel.statusText = error.message ?: error.toString()
                            }
                        } finally {
                            viewModel.running = false
                        }
                    }
                },
            ) {
                Text(title)
            }
            OutlinedButton(onClick = onClose, enabled = !viewModel.running) {
                Text(stringResource(R.string.button_cancel))
            }
        }
    }
}

@Composable
private fun FileRow(
    label: String,
    value: String,
    placeholder: String,
    error: String?,
    enabled: Boolean,
    onBrowse: () -> Unit,
) {
    Column(verticalArrangement = Arrangement.spacedBy(4.dp)) {
        Text(text = label, style = MaterialTheme.typography.labelLarge)
        Row(
            modifier = Modifier.fillMaxWidth(),
            verticalAlignment = Alignment.CenterVertically,
            horizontalArrangement = Arrangement.spacedBy(8.dp),
        ) {
            Text(
                text = value.ifBlank { placeholder },
                modifier = Modifier.weight(1f),
                color = if (value.isBlank()) {
                    MaterialTheme.colorScheme.onSurface.copy(alpha = 0.6f)
                } else {
                    MaterialTheme.colorScheme.onSurface
                },
            )
            OutlinedButton(onClick = onBrowse, enabled = enabled) {
                Text(stringResource(R.string.field_file_browse))
            }
        }
        if (error != null) {
            Text(text = error, color = MaterialTheme.colorScheme.error, style = MaterialTheme.typography.bodySmall)
        }
    }
}

private fun persistPermission(resolver: ContentResolver, uri: Uri, flags: Int) {
    try {
        resolver.takePersistableUriPermission(uri, flags)
    } catch (_: SecurityException) {
    }
}

private fun displayName(resolver: ContentResolver, uri: Uri): String {
    resolver.query(uri, arrayOf(OpenableColumns.DISPLAY_NAME), null, null, null)?.use { cursor ->
        if (cursor.moveToFirst()) {
            val name = cursor.getString(0)
            if (!name.isNullOrBlank()) {
                return name
            }
        }
    }
    return uri.lastPathSegment ?: "file"
}

/**
 * Open picker whose initial type is the first MIME type. Extra types stay available in the
 * system picker, including star-slash-star when the caller wants every file.
 */
private class OpenTechnicalDocument : ActivityResultContract<Array<String>, Uri?>() {
    override fun createIntent(context: Context, input: Array<String>): Intent {
        val intent = Intent(Intent.ACTION_OPEN_DOCUMENT).addCategory(Intent.CATEGORY_OPENABLE)
        if (input.size <= 1) {
            intent.type = input.firstOrNull() ?: "*/*"
        } else {
            intent.type = input[0]
            intent.putExtra(Intent.EXTRA_MIME_TYPES, input)
        }
        return intent
    }

    override fun parseResult(resultCode: Int, intent: Intent?): Uri? {
        return if (resultCode == Activity.RESULT_OK) intent?.data else null
    }
}

private fun TechnicalFileToolKind.titleRes(): Int = when (this) {
    TechnicalFileToolKind.DecryptAes256 -> R.string.technical_tool_decrypt_aes256_title
    TechnicalFileToolKind.EncryptAes256 -> R.string.technical_tool_encrypt_aes256_title
    TechnicalFileToolKind.DecompressBrotli -> R.string.technical_tool_decompress_brotli_title
    TechnicalFileToolKind.CompressBrotli -> R.string.technical_tool_compress_brotli_title
}
