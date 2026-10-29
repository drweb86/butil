package com.siarheikuchuk.butil.ui.controls

import android.net.Uri
import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.ExperimentalLayoutApi
import androidx.compose.foundation.layout.FlowRow
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.foundation.verticalScroll
import androidx.compose.material3.Button
import androidx.compose.material3.Checkbox
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.ExposedDropdownMenuBox
import androidx.compose.material3.ExposedDropdownMenuDefaults
import androidx.compose.material3.DropdownMenuItem
import androidx.compose.material3.FilterChip
import androidx.compose.material3.LinearProgressIndicator
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.MenuAnchorType
import androidx.compose.material3.OutlinedButton
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Text
import androidx.compose.material3.TimePicker
import androidx.compose.material3.rememberTimePickerState
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.unit.dp
import com.siarheikuchuk.butil.R
import com.siarheikuchuk.butil.core.storage.FolderSettings
import com.siarheikuchuk.butil.core.storage.FtpsSettings
import com.siarheikuchuk.butil.core.storage.SftpSettings
import com.siarheikuchuk.butil.core.storage.StorageCatalog
import com.siarheikuchuk.butil.core.storage.StorageFactory
import com.siarheikuchuk.butil.core.storage.StorageFieldKind
import com.siarheikuchuk.butil.core.storage.StorageFieldSpec
import com.siarheikuchuk.butil.core.storage.StorageSettings
import com.siarheikuchuk.butil.core.tasks.Schedule
import com.siarheikuchuk.butil.core.tasks.UploadFolderStore
import com.siarheikuchuk.butil.core.tasks.UploadFolderTask
import com.siarheikuchuk.butil.core.tasks.UploadScheduler
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import java.security.MessageDigest
import java.security.cert.CertificateFactory
import java.security.cert.X509Certificate

@OptIn(ExperimentalMaterial3Api::class, ExperimentalLayoutApi::class)
@Composable
fun UploadFolderEditor(
    existingName: String?,
    onClose: () -> Unit,
    modifier: Modifier = Modifier,
) {
    val context = LocalContext.current
    val store = remember { UploadFolderStore(context) }
    val form = remember(existingName) { UploadFolderForm(store, existingName) }
    val scope = rememberCoroutineScope()
    var message by remember { mutableStateOf<String?>(null) }
    var busy by remember { mutableStateOf(false) }
    var showTime by remember { mutableStateOf(false) }
    val timeState = rememberTimePickerState(form.hour, form.minute, true)

    val pickFolder = rememberLauncherForActivityResult(ActivityResultContracts.OpenDocumentTree()) { uri ->
        if (uri != null) {
            persistTree(context, uri)
            form.folder = uri.toString()
            form.folderLabel = uri.lastPathSegment ?: uri.toString()
        }
    }
    val pickStorageFolder = rememberLauncherForActivityResult(ActivityResultContracts.OpenDocumentTree()) { uri ->
        if (uri != null) {
            persistTree(context, uri)
            form.folderField()?.value = uri.toString()
        }
    }
    val pickKey = rememberLauncherForActivityResult(ActivityResultContracts.OpenDocument()) { uri ->
        if (uri != null) {
            try {
                context.contentResolver.takePersistableUriPermission(uri, android.content.Intent.FLAG_GRANT_READ_URI_PERMISSION)
            } catch (_: SecurityException) {
            }
            form.keyField()?.value = uri.toString()
        }
    }

    Column(
        modifier.padding(horizontal = 20.dp, vertical = 12.dp).verticalScroll(rememberScrollState()),
        verticalArrangement = Arrangement.spacedBy(12.dp),
    ) {
        Text(stringResource(R.string.upload_folder_contents_task_create), style = MaterialTheme.typography.headlineSmall)
        TaskSection("✅", stringResource(R.string.field_name)) {
            OutlinedTextField(
                value = form.name,
                onValueChange = { form.name = it },
                modifier = Modifier.fillMaxWidth(),
                singleLine = true,
                isError = form.nameError != null,
                supportingText = form.nameError?.let { { Text(it) } },
            )
            Text(stringResource(R.string.upload_folder_help), style = MaterialTheme.typography.bodyMedium)
        }
        TaskSection("📁", stringResource(R.string.left_menu_what)) {
            BrowseLine(
                label = stringResource(R.string.field_folder),
                value = form.folderLabel,
                placeholder = stringResource(R.string.folder_empty),
                action = stringResource(R.string.field_folder_browse),
                enabled = !busy,
            ) { pickFolder.launch(null) }
            Row(verticalAlignment = Alignment.CenterVertically) {
                Checkbox(checked = form.skipExisting, onCheckedChange = { form.skipExisting = it })
                Column {
                    Text(stringResource(R.string.upload_folder_skip_existing))
                    Text(stringResource(R.string.upload_folder_skip_existing_help), style = MaterialTheme.typography.bodySmall)
                }
            }
        }
        TaskSection("💾", stringResource(R.string.left_menu_where)) {
            Text(stringResource(R.string.upload_folder_storage_help), style = MaterialTheme.typography.bodyMedium)
            ProviderMenu(form)
            val help = form.selectedChoiceHelp()
            if (help.isNotBlank()) Text(help, style = MaterialTheme.typography.bodySmall)
            form.fields.filter { !it.hidden }.forEach { field ->
                when (field.spec.kind) {
                    StorageFieldKind.PASSWORD -> SecretField(field, !busy)
                    StorageFieldKind.INTEGER -> NumberField(field, !busy)
                    StorageFieldKind.ENUM -> EnumField(field, !busy) { form.applyRules() }
                    StorageFieldKind.FOLDER -> BrowseField(field, stringResource(R.string.field_folder_browse), !busy) { pickStorageFolder.launch(null) }
                    StorageFieldKind.FILE -> BrowseField(field, stringResource(R.string.field_file_browse), !busy) { pickKey.launch(arrayOf("*/*")) }
                    StorageFieldKind.TEXT -> TextField(field, !busy)
                }
            }
            NumberField(
                label = stringResource(R.string.upload_quota),
                value = form.quota,
                onValue = { form.quota = it },
                help = stringResource(R.string.upload_quota_help),
                error = form.quotaError,
                enabled = !busy,
            )
        }
        TaskSection("⏰", stringResource(R.string.left_menu_when)) {
            Text(stringResource(R.string.when_optional_hint), style = MaterialTheme.typography.bodySmall)
            Text(stringResource(R.string.days_choose))
            FlowRow(horizontalArrangement = Arrangement.spacedBy(8.dp)) {
                dayChips.forEach { (day, labelRes) ->
                    FilterChip(
                        selected = day in form.days,
                        onClick = {
                            if (day in form.days) form.days.remove(day) else form.days.add(day)
                        },
                        label = { Text(stringResource(labelRes)) },
                    )
                }
            }
            OutlinedButton(onClick = { showTime = true }, enabled = !busy) {
                Text("%02d:%02d".format(form.hour, form.minute))
            }
            if (showTime) {
                TimePicker(state = timeState)
                Button(onClick = {
                    form.hour = timeState.hour
                    form.minute = timeState.minute
                    showTime = false
                }) { Text(stringResource(R.string.button_ok)) }
            }
            Row(verticalAlignment = Alignment.CenterVertically) {
                Checkbox(checked = form.runAtLogin, onCheckedChange = { form.runAtLogin = it })
                Text(stringResource(R.string.run_at_login))
            }
            ExactAlarmNotice(form.days.isNotEmpty() || form.runAtLogin)
        }
        if (busy) LinearProgressIndicator(Modifier.fillMaxWidth())
        if (!message.isNullOrBlank()) {
            Text(message!!, color = MaterialTheme.colorScheme.error)
        }
        Row(horizontalArrangement = Arrangement.spacedBy(8.dp)) {
            Button(
                enabled = !busy,
                onClick = {
                    message = null
                    val local = form.validate(store)
                    if (local != null) {
                        message = local
                        return@Button
                    }
                    busy = true
                    scope.launch {
                        val outcome = withContext(Dispatchers.IO) { form.testAndDescribe(context) }
                        form.applyDetected(outcome.settings)
                        if (outcome.trust != null) {
                            message = outcome.trust
                            busy = false
                            return@launch
                        }
                        if (outcome.error != null) {
                            message = outcome.error
                            busy = false
                            return@launch
                        }
                        form.save(store)
                        UploadScheduler.reschedule(context, form.task())
                        busy = false
                        onClose()
                    }
                },
            ) { Text(stringResource(R.string.button_ok)) }
            OutlinedButton(onClick = onClose, enabled = !busy) { Text(stringResource(R.string.button_cancel)) }
        }
    }
}

@Composable
private fun UploadRunView(
    name: String,
    onClose: () -> Unit,
    modifier: Modifier = Modifier,
) {
    val lines = com.siarheikuchuk.butil.core.tasks.UploadRunLog.lines
    Column(modifier.padding(20.dp).verticalScroll(rememberScrollState()), verticalArrangement = Arrangement.spacedBy(8.dp)) {
        Text(name, style = MaterialTheme.typography.headlineSmall)
        if (com.siarheikuchuk.butil.core.tasks.UploadRunLog.active) {
            LinearProgressIndicator(Modifier.fillMaxWidth())
        }
        lines.forEach { line -> Text(line) }
        OutlinedButton(onClick = onClose) { Text(stringResource(R.string.button_close)) }
    }
}

@OptIn(ExperimentalMaterial3Api::class)
@Composable
private fun ProviderMenu(form: UploadFolderForm) {
    var open by remember { mutableStateOf(false) }
    val current = StorageCatalog.providers.first { it.jsonType == form.provider }
    ExposedDropdownMenuBox(expanded = open, onExpandedChange = { open = it }) {
        OutlinedTextField(
            value = current.displayName,
            onValueChange = {},
            readOnly = true,
            label = { Text(stringResource(R.string.field_protocol)) },
            trailingIcon = { ExposedDropdownMenuDefaults.TrailingIcon(open) },
            modifier = Modifier.menuAnchor(MenuAnchorType.PrimaryNotEditable).fillMaxWidth(),
        )
        ExposedDropdownMenu(expanded = open, onDismissRequest = { open = false }) {
            StorageCatalog.providers.forEach { provider ->
                DropdownMenuItem(
                    text = { Text(provider.displayName) },
                    onClick = {
                        form.chooseProvider(provider.jsonType)
                        open = false
                    },
                )
            }
        }
    }
}

@Composable
private fun TextField(field: FieldRow, enabled: Boolean) {
    OutlinedTextField(
        value = field.value,
        onValueChange = { field.value = it; field.error = null },
        modifier = Modifier.fillMaxWidth(),
        label = { Text(field.label) },
        placeholder = { Text(field.placeholder) },
        isError = field.error != null,
        supportingText = field.error?.let { { Text(it) } },
        enabled = enabled,
    )
}

@Composable
private fun SecretField(field: FieldRow, enabled: Boolean) {
    OutlinedTextField(
        value = field.value,
        onValueChange = { field.value = it; field.error = null },
        modifier = Modifier.fillMaxWidth(),
        label = { Text(field.label) },
        visualTransformation = PasswordVisualTransformation(),
        isError = field.error != null,
        supportingText = field.error?.let { { Text(it) } },
        enabled = enabled,
    )
}

@Composable
private fun NumberField(field: FieldRow, enabled: Boolean) {
    NumberField(field.label, field.value, { field.value = it; field.error = null }, field.placeholder, field.error, enabled)
}

@Composable
private fun NumberField(label: String, value: String, onValue: (String) -> Unit, help: String, error: String?, enabled: Boolean) {
    OutlinedTextField(
        value = value,
        onValueChange = onValue,
        modifier = Modifier.fillMaxWidth(),
        label = { Text(label) },
        supportingText = { Text(error ?: help) },
        isError = error != null,
        keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Number),
        singleLine = true,
        enabled = enabled,
    )
}

@OptIn(ExperimentalMaterial3Api::class)
@Composable
private fun EnumField(field: FieldRow, enabled: Boolean, onChange: () -> Unit) {
    var open by remember { mutableStateOf(false) }
    val label = field.spec.choices.firstOrNull { it.value == field.value }?.label ?: field.value
    ExposedDropdownMenuBox(expanded = open, onExpandedChange = { if (enabled) open = it }) {
        OutlinedTextField(
            value = label,
            onValueChange = {},
            readOnly = true,
            label = { Text(field.label) },
            trailingIcon = { ExposedDropdownMenuDefaults.TrailingIcon(open) },
            modifier = Modifier.menuAnchor(MenuAnchorType.PrimaryNotEditable).fillMaxWidth(),
            isError = field.error != null,
        )
        ExposedDropdownMenu(expanded = open, onDismissRequest = { open = false }) {
            field.spec.choices.forEach { choice ->
                DropdownMenuItem(
                    text = { Text(choice.label) },
                    onClick = {
                        field.value = choice.value
                        field.error = null
                        onChange()
                        open = false
                    },
                )
            }
        }
    }
}

@Composable
private fun BrowseField(field: FieldRow, action: String, enabled: Boolean, onBrowse: () -> Unit) {
    BrowseLine(
        label = field.label,
        value = friendlyLocation(field.value),
        placeholder = field.placeholder,
        action = action,
        enabled = enabled,
        error = field.error,
        onBrowse = onBrowse,
    )
}

private val dayChips = listOf(
    1 to R.string.days_monday,
    2 to R.string.days_tuesday,
    3 to R.string.days_wednesday,
    4 to R.string.days_thursday,
    5 to R.string.days_friday,
    6 to R.string.days_saturday,
    0 to R.string.days_sunday,
)

private fun persistTree(context: android.content.Context, uri: Uri) {
    val flags = android.content.Intent.FLAG_GRANT_READ_URI_PERMISSION or android.content.Intent.FLAG_GRANT_WRITE_URI_PERMISSION
    try {
        context.contentResolver.takePersistableUriPermission(uri, flags)
    } catch (_: SecurityException) {
    }
}

private fun friendlyLocation(value: String): String {
    if (!value.startsWith("content:")) return value
    return Uri.parse(value).lastPathSegment ?: value
}

private class FieldRow(val spec: StorageFieldSpec, initial: String) {
    var value by mutableStateOf(initial)
    var hidden by mutableStateOf(false)
    var label by mutableStateOf(spec.label)
    var placeholder by mutableStateOf(spec.placeholder)
    var error by mutableStateOf<String?>(null)
}

private class UploadFolderForm(store: UploadFolderStore, existingName: String?) {
    private val originalName = existingName
    private val loaded = existingName?.let { store.load(it) }
    var name by mutableStateOf(loaded?.name ?: "")
    var nameError by mutableStateOf<String?>(null)
    var folder by mutableStateOf(loaded?.folder ?: "")
    var folderLabel by mutableStateOf(loaded?.folder?.let(::friendlyLocation) ?: "")
    var skipExisting by mutableStateOf(loaded?.skipExistingFiles ?: false)
    var provider by mutableStateOf(loaded?.storage?.jsonType ?: "Folder")
    var quota by mutableStateOf((loaded?.storage?.quotaGb ?: 0L).toString())
    var quotaError by mutableStateOf<String?>(null)
    var hour by mutableStateOf(loaded?.schedule?.hour ?: 23)
    var minute by mutableStateOf(loaded?.schedule?.minute ?: 50)
    var runAtLogin by mutableStateOf(loaded?.schedule?.runAtLogin ?: false)
    val days = androidx.compose.runtime.mutableStateListOf<Int>().apply { addAll(loaded?.schedule?.days ?: emptySet()) }
    val fields = androidx.compose.runtime.mutableStateListOf<FieldRow>()
    private var preserved: StorageSettings = loaded?.storage ?: com.siarheikuchuk.butil.core.storage.FolderSettings()

    init {
        fill(preserved)
    }

    fun chooseProvider(type: String) {
        if (type == provider) return
        preserved = currentSettings()
        provider = type
        fields.clear()
        StorageCatalog.find(type).fields.forEach { fields.add(FieldRow(it, it.defaultValue)) }
        applyRules()
    }

    fun applyDetected(settings: StorageSettings) {
        preserved = settings
        quota = settings.quotaGb.toString()
        fill(settings)
    }

    fun folderField() = fields.firstOrNull { it.spec.kind == StorageFieldKind.FOLDER }
    fun keyField() = fields.firstOrNull { it.spec.kind == StorageFieldKind.FILE }

    fun selectedChoiceHelp(): String {
        val choice = fields.firstOrNull { it.spec.kind == StorageFieldKind.ENUM && it.spec.choices.any { choice -> choice.help.isNotBlank() } }
            ?: return ""
        return choice.spec.choices.firstOrNull { it.value == choice.value }?.help.orEmpty()
    }

    fun applyRules() {
        repeat(24) {
            var wrote = false
            fields.forEach { field ->
                field.hidden = false
                field.label = field.spec.label
                field.placeholder = field.spec.placeholder
            }
            for (field in fields) {
                val rule = field.spec.rules.firstOrNull { it.whenValue == field.value } ?: continue
                for (patch in rule.patches) {
                    val target = fields.firstOrNull { it.spec.key == patch.target } ?: continue
                    if (patch.label != null) target.label = patch.label
                    target.hidden = patch.hidden
                    if (patch.placeholder != null) target.placeholder = patch.placeholder
                    if (patch.value != null && target.value != patch.value) {
                        target.value = patch.value
                        wrote = true
                    }
                }
            }
            if (!wrote) return
        }
    }

    fun validate(store: UploadFolderStore): String? {
        nameError = store.validateName(name, originalName)
        quotaError = null
        fields.forEach { it.error = null }
        val errors = mutableListOf<String>()
        if (nameError != null) errors += "Name: $nameError"
        if (folder.isBlank()) errors += "What?: Folder is not specified."
        fields.filter { !it.hidden }.forEach { field ->
            val error = fieldError(field)
            if (error != null) {
                field.error = error
                errors += error
            }
        }
        if (provider == "Sftp") {
            val password = fields.firstOrNull { it.spec.key == "password" }?.value.orEmpty()
            val key = fields.firstOrNull { it.spec.key == "keyfile" }?.value.orEmpty()
            if (password.isBlank() && key.isBlank()) errors += "Both password or key file are empty."
        }
        val quotaValue = quota.toLongOrNull()
        if (quotaValue == null || quotaValue < 0) {
            quotaError = "Quota should be 0 (do not apply) or value above 0."
            errors += quotaError!!
        }
        return errors.firstOrNull()
    }

    fun testAndDescribe(context: android.content.Context): TestOutcome {
        val settings = currentSettings()
        val beforeCertificate = (settings as? FtpsSettings)?.trustedCertificate.orEmpty()
        val beforeFingerprint = (settings as? SftpSettings)?.fingerPrintSha256.orEmpty()
        val storageError = StorageFactory.test(context, settings)
        val trust = trustText(settings, beforeCertificate, beforeFingerprint)
        if (trust != null) return TestOutcome(settings, trust, storageError)
        if (storageError != null) return TestOutcome(settings, null, storageError)
        val sourceError = StorageFactory.test(context, FolderSettings(destinationFolder = folder))
        return TestOutcome(settings, null, sourceError)
    }

    fun save(store: UploadFolderStore) {
        store.save(task(), originalName)
    }

    fun task(): UploadFolderTask {
        return UploadFolderTask(
            name = name.trimEnd(),
            folder = folder,
            skipExistingFiles = skipExisting,
            storage = currentSettings(),
            schedule = Schedule(days.toMutableSet(), hour, minute, runAtLogin),
        )
    }

    private fun fill(settings: StorageSettings) {
        provider = settings.jsonType
        val values = StorageCatalog.fieldValues(settings)
        fields.clear()
        StorageCatalog.find(settings.jsonType).fields.forEach { spec ->
            fields.add(FieldRow(spec, values[spec.key] ?: spec.defaultValue))
        }
        applyRules()
    }

    private fun currentSettings(): StorageSettings {
        val values = fields.associate { it.spec.key to it.value }
        return StorageCatalog.toSettings(provider, values, quota.toLongOrNull() ?: 0, preserved)
    }

    private fun fieldError(field: FieldRow): String? {
        if (field.spec.kind == StorageFieldKind.INTEGER) {
            val number = field.value.toLongOrNull()
            if (number == null || (field.spec.min != null && number < field.spec.min) || (field.spec.max != null && number > field.spec.max)) {
                return "Port is invalid."
            }
            return null
        }
        if (!field.spec.optional && field.value.isBlank()) return "${field.label.trimEnd(':')} is not specified."
        if (provider == "Sftp" && field.spec.key == "folder") {
            if (field.value == "/") return "SFTP folder / is not allowed. Folder must point to more specific folder."
            if (!field.value.startsWith("/")) return "SFTP folder must start with /."
            if (field.value.endsWith("/")) return "SFTP folder must not end on /."
        }
        return null
    }

    private fun trustText(settings: StorageSettings, beforeCertificate: String, beforeFingerprint: String): String? {
        if (settings is FtpsSettings && settings.trustedCertificate.isNotBlank() &&
            !settings.trustedCertificate.equals(beforeCertificate, ignoreCase = true)
        ) {
            return certificateText(settings)
        }
        if (settings is SftpSettings && settings.fingerPrintSha256.isNotBlank() &&
            !settings.fingerPrintSha256.equals(beforeFingerprint, ignoreCase = true)
        ) {
            return "SFTP server trust information detected during test.\n" +
                "Verify this fingerprint out-of-band, then click OK again to store it.\n\n" +
                "Host: ${settings.host}\nPort: ${if (settings.port == 0) "default (22)" else settings.port}\n" +
                "User: ${settings.user}\nFolder: ${settings.folder}\n" +
                "Fingerprint (SHA-256): ${settings.fingerPrintSha256}"
        }
        return null
    }

    private fun certificateText(settings: FtpsSettings): String {
        val lines = mutableListOf(
            "FTPS server certificate detected during test.",
            "Verify certificate identity out-of-band, then click OK again to store it.",
            "",
            "Host: ${settings.host}",
            "Port: ${if (settings.port == 0) "default" else settings.port}",
            "Encryption: ${settings.encryption}",
            "Folder: ${settings.folder.orEmpty()}",
            "Certificate (raw hex): ${settings.trustedCertificate}",
        )
        try {
            val bytes = settings.trustedCertificate.chunked(2).map { it.toInt(16).toByte() }.toByteArray()
            val cert = CertificateFactory.getInstance("X.509").generateCertificate(bytes.inputStream()) as X509Certificate
            val thumb = MessageDigest.getInstance("SHA-1").digest(cert.encoded).joinToString("") { "%02X".format(it) }
            lines += listOf(
                "",
                "Parsed certificate details:",
                "Subject: ${cert.subjectDN}",
                "Issuer: ${cert.issuerDN}",
                "Serial number: ${cert.serialNumber}",
                "Thumbprint: $thumb",
                "Valid from (UTC): ${cert.notBefore}",
                "Valid until (UTC): ${cert.notAfter}",
            )
        } catch (error: Exception) {
            lines += "Certificate parse warning: ${error.message}"
        }
        return lines.joinToString("\n")
    }
}

private data class TestOutcome(val settings: StorageSettings, val trust: String?, val error: String?)

@Composable
fun UploadRunPage(name: String, onClose: () -> Unit, modifier: Modifier = Modifier) {
    UploadRunView(name, onClose, modifier.fillMaxSize())
}
