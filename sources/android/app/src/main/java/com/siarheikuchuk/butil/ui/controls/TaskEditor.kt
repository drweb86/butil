package com.siarheikuchuk.butil.ui.controls

import android.net.Uri
import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.ExperimentalLayoutApi
import androidx.compose.foundation.layout.FlowRow
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Close
import androidx.compose.material3.Button
import androidx.compose.material3.Checkbox
import androidx.compose.material3.DropdownMenuItem
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.ExposedDropdownMenuBox
import androidx.compose.material3.ExposedDropdownMenuDefaults
import androidx.compose.material3.FilterChip
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
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
import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.font.FontFamily
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
import com.siarheikuchuk.butil.core.tasks.AppTask
import com.siarheikuchuk.butil.core.tasks.BackupClock
import com.siarheikuchuk.butil.core.tasks.FtpsServerTask
import com.siarheikuchuk.butil.core.tasks.ImportMediaRules
import com.siarheikuchuk.butil.core.tasks.ImportMediaTask
import com.siarheikuchuk.butil.core.tasks.IncrementalBackupRunner
import com.siarheikuchuk.butil.core.tasks.IncrementalBackupTask
import com.siarheikuchuk.butil.core.tasks.Schedule
import com.siarheikuchuk.butil.core.tasks.SourceFolder
import com.siarheikuchuk.butil.core.tasks.SynchronizationTask
import com.siarheikuchuk.butil.core.tasks.UploadFolderStore
import com.siarheikuchuk.butil.core.tasks.UploadScheduler
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import java.time.LocalDate
import java.time.ZoneOffset

@OptIn(ExperimentalMaterial3Api::class, ExperimentalLayoutApi::class)
@Composable
fun TaskEditor(
    kind: String,
    existingName: String?,
    onClose: () -> Unit,
    modifier: Modifier = Modifier,
) {
    val context = LocalContext.current
    val store = remember { UploadFolderStore(context) }
    val form = remember(kind, existingName) { TaskDraft(store, kind, existingName) }
    val scope = rememberCoroutineScope()
    var message by remember { mutableStateOf<String?>(null) }
    var busy by remember { mutableStateOf(false) }
    var showTime by remember { mutableStateOf(false) }
    val timeState = rememberTimePickerState(form.hour, form.minute, true)
    val pickFolder = rememberLauncherForActivityResult(ActivityResultContracts.OpenDocumentTree()) { uri ->
        if (uri != null) {
            keepTree(context, uri)
            form.pendingFolder?.let { target -> target.value = uri.toString() }
        }
    }
    val title = stringResource(titleRes(kind))
    Column(
        modifier.padding(horizontal = 20.dp, vertical = 12.dp).verticalScroll(rememberScrollState()),
        verticalArrangement = Arrangement.spacedBy(12.dp),
    ) {
        Text(title, style = MaterialTheme.typography.headlineSmall)
        val browse: (DraftValue) -> Unit = {
            form.pendingFolder = it
            pickFolder.launch(null)
        }
        TaskSection("✅", stringResource(R.string.field_name)) {
            OutlinedTextField(form.name, { form.name = it }, modifier = Modifier.fillMaxWidth(), singleLine = true, isError = form.nameError != null, supportingText = form.nameError?.let { { Text(it) } })
            Text(stringResource(helpRes(kind)), style = MaterialTheme.typography.bodyMedium)
        }
        when (kind) {
            "Incremental" -> {
                TaskSection("💾", stringResource(R.string.left_menu_what)) { IncrementalFields(form, busy, browse) }
                TaskSection("💾", stringResource(R.string.left_menu_where)) { StorageBlock(stringResource(R.string.incremental_storage_help), form.where, busy, browse) }
                TaskSection("🔒", stringResource(R.string.left_menu_encryption)) { PasswordLine(form, busy) }
            }
            "ImportMedia" -> {
                TaskSection("💾", stringResource(R.string.import_source_title)) { StorageBlock(stringResource(R.string.import_source_help), form.source, busy, browse) }
                TaskSection("📷", stringResource(R.string.left_menu_what)) { ImportWhatFields(form) }
                TaskSection("🖼", stringResource(R.string.left_menu_where)) { ImportWhereFields(form, busy, browse) }
            }
            "Synchronization" -> {
                TaskSection("📁", stringResource(R.string.left_menu_what)) { SyncFields(form, busy, browse) }
                TaskSection("💾", stringResource(R.string.left_menu_where)) { StorageBlock(stringResource(R.string.sync_storage_help), form.where, busy, browse) }
                TaskSection("🔒", stringResource(R.string.left_menu_encryption)) { PasswordLine(form, busy) }
            }
            "BUtilServer" -> TaskSection("🖥️",stringResource(R.string.ftps_server_title)) { ServerFields(form, busy, browse) }
        }
        TaskSection("⏰", stringResource(R.string.left_menu_when)) {
            Text(stringResource(R.string.when_optional_hint), style = MaterialTheme.typography.bodySmall)
            Text(stringResource(R.string.days_choose))
            FlowRow(horizontalArrangement = Arrangement.spacedBy(8.dp)) {
                scheduleDays.forEach { (day, label) ->
                    FilterChip(selected = day in form.days, onClick = { if (day in form.days) form.days.remove(day) else form.days.add(day) }, label = { Text(stringResource(label)) })
                }
            }
            OutlinedButton(onClick = { showTime = true }, enabled = !busy) { Text("%02d:%02d".format(form.hour, form.minute)) }
            if (showTime) {
                TimePicker(state = timeState)
                Button(onClick = { form.hour = timeState.hour; form.minute = timeState.minute; showTime = false }) { Text(stringResource(R.string.button_ok)) }
            }
            Row(verticalAlignment = Alignment.CenterVertically) {
                Checkbox(form.runAtLogin, { form.runAtLogin = it })
                Text(stringResource(R.string.run_at_login))
            }
            ExactAlarmNotice(form.days.isNotEmpty() || form.runAtLogin)
        }
        if (busy) LinearProgressIndicator(Modifier.fillMaxWidth())
        if (!message.isNullOrBlank()) Text(message!!, color = MaterialTheme.colorScheme.error)
        Row(horizontalArrangement = Arrangement.spacedBy(8.dp)) {
            Button(enabled = !busy, onClick = {
                val error = form.validate(store)
                if (error != null) {
                    message = error
                    return@Button
                }
                busy = true
                scope.launch {
                    val tested = withContext(Dispatchers.IO) { form.test(context) }
                    message = tested
                    if (tested == null) {
                        form.save(store)
                        UploadScheduler.reschedule(context, form.task())
                        onClose()
                    }
                    busy = false
                }
            }) { Text(stringResource(R.string.button_ok)) }
            OutlinedButton(onClick = onClose, enabled = !busy) { Text(stringResource(R.string.button_cancel)) }
        }
    }
}

@Composable
private fun IncrementalFields(form: TaskDraft, busy: Boolean, browse: (DraftValue) -> Unit) {
    Text(stringResource(R.string.source_folders))
    form.folders.forEach { folder ->
        BrowseLine(
            label = null,
            value = folderLabel(folder.value),
            placeholder = stringResource(R.string.folder_empty),
            action = stringResource(R.string.field_folder_browse),
            enabled = !busy,
            trailing = {
                IconButton(onClick = { form.folders.remove(folder) }, enabled = !busy) {
                    Icon(Icons.Filled.Close, contentDescription = stringResource(R.string.remove_folder))
                }
            },
        ) { browse(folder) }
    }
    OutlinedButton(onClick = { form.folders += DraftValue("") }, enabled = !busy) { Text(stringResource(R.string.add_folder)) }
    OutlinedTextField(form.exclude, { form.exclude = it }, modifier = Modifier.fillMaxWidth(), label = { Text(stringResource(R.string.field_exclude)) }, supportingText = { Text(stringResource(R.string.exclude_help)) }, minLines = 3)
}

@Composable
private fun PasswordLine(form: TaskDraft, busy: Boolean) {
    SecretLine(stringResource(R.string.field_password), form.password, { form.password = it }, !busy)
}

@Composable
private fun ImportWhatFields(form: TaskDraft) {
    OutlinedTextField(form.extensions, { form.extensions = it }, modifier = Modifier.fillMaxWidth(), label = { Text(stringResource(R.string.import_extensions)) }, supportingText = { Text(stringResource(R.string.import_extensions_help)) })
    Column {
        CheckLine(stringResource(R.string.import_skip), form.skipImported) { form.skipImported = it }
        Text(stringResource(R.string.import_skip_help), style = MaterialTheme.typography.bodySmall)
    }
    CheckLine(stringResource(R.string.import_delete), form.deleteSource) { form.deleteSource = it }
    OutlinedTextField(form.minimumDate, { form.minimumDate = it }, modifier = Modifier.fillMaxWidth(), label = { Text(stringResource(R.string.import_min_date)) }, singleLine = true)
}

@Composable
private fun ImportWhereFields(form: TaskDraft, busy: Boolean, browse: (DraftValue) -> Unit) {
    FolderLine(stringResource(R.string.import_output_folder), form.output, busy) { browse(form.output) }
    ChoiceLine(stringResource(R.string.import_transform), form.transform, ImportMediaRules.presets, !busy) { form.transform = it }
    OutlinedTextField(form.transform, { form.transform = it }, modifier = Modifier.fillMaxWidth(), supportingText = { Text(stringResource(R.string.import_transform_help)) })
    val example = ImportMediaRules.example(form.transform, form.output.value.ifBlank { "Pictures" })
    if (example != null) Text(example, style = MaterialTheme.typography.bodySmall)
}

@Composable
private fun SyncFields(form: TaskDraft, busy: Boolean, browse: (DraftValue) -> Unit) {
    FolderLine(stringResource(R.string.field_folder), form.output, busy) { browse(form.output) }
    ChoiceLine(stringResource(R.string.sync_mode), form.mode, listOf("Two-way", "Read"), !busy) { form.mode = it }
    if (form.repository.isNotBlank()) {
        OutlinedTextField(form.repository, { form.repository = it }, modifier = Modifier.fillMaxWidth(), label = { Text(stringResource(R.string.sync_repository)) })
    }
}

@Composable
private fun ServerFields(form: TaskDraft, busy: Boolean, browse: (DraftValue) -> Unit) {
    FolderLine(stringResource(R.string.field_folder), form.output, busy) { browse(form.output) }
    NumberLine(stringResource(R.string.ftps_port), form.port, { form.port = it }, !busy)
    OutlinedTextField(form.user, { form.user = it }, modifier = Modifier.fillMaxWidth(), label = { Text(stringResource(R.string.ftps_user)) }, singleLine = true)
    SecretLine(stringResource(R.string.field_password), form.password, { form.password = it }, !busy)
    ChoiceLine(stringResource(R.string.ftps_access), form.access, listOf("Read and write", "Read only"), !busy) { form.access = it }
    NumberLine(stringResource(R.string.ftps_duration), form.duration, { form.duration = it }, !busy, stringResource(R.string.ftps_duration_help))
}

@OptIn(ExperimentalMaterial3Api::class)
@Composable
private fun StorageBlock(help: String, draft: StorageDraft, busy: Boolean, browse: (DraftValue) -> Unit) {
    Text(help, style = MaterialTheme.typography.bodyMedium)
    var open by remember { mutableStateOf(false) }
    val current = StorageCatalog.providers.first { it.jsonType == draft.provider }
    ExposedDropdownMenuBox(expanded = open, onExpandedChange = { open = it }) {
        OutlinedTextField(current.displayName, {}, readOnly = true, label = { Text(stringResource(R.string.field_protocol)) }, trailingIcon = { ExposedDropdownMenuDefaults.TrailingIcon(open) }, modifier = Modifier.menuAnchor(MenuAnchorType.PrimaryNotEditable).fillMaxWidth())
        ExposedDropdownMenu(expanded = open, onDismissRequest = { open = false }) {
            StorageCatalog.providers.forEach { provider ->
                DropdownMenuItem(text = { Text(provider.displayName) }, onClick = { draft.choose(provider.jsonType); open = false })
            }
        }
    }
    draft.fields.filter { !it.hidden }.forEach { field ->
        when (field.spec.kind) {
            StorageFieldKind.PASSWORD -> SecretLine(field.label, field.value, { field.value = it }, !busy)
            StorageFieldKind.INTEGER -> NumberLine(field.label, field.value, { field.value = it; draft.applyRules() }, !busy)
            StorageFieldKind.ENUM -> {
                val label = field.spec.choices.firstOrNull { it.value == field.value }?.label ?: field.value
                ChoiceLine(field.label, label, field.spec.choices.map { it.label }, !busy) { selected ->
                    field.value = field.spec.choices.first { it.label == selected }.value
                    draft.applyRules()
                }
            }
            StorageFieldKind.FOLDER, StorageFieldKind.FILE -> FolderLine(field.label, field, busy, field.spec.placeholder) { browse(field) }
            StorageFieldKind.TEXT -> {
                val fingerprints = if (field.spec.key == "certificate") ftpsCertificateFingerprints(field.value) else null
                if (fingerprints != null) {
                    Text(fingerprints, fontFamily = FontFamily.Monospace, style = MaterialTheme.typography.bodyMedium)
                } else {
                    OutlinedTextField(field.value, { field.value = it }, modifier = Modifier.fillMaxWidth(), label = { Text(field.label) }, singleLine = true, enabled = !busy)
                }
            }
        }
    }
    NumberLine(stringResource(R.string.upload_quota), draft.quota, { draft.quota = it }, !busy, stringResource(R.string.upload_quota_help))
}

@OptIn(ExperimentalMaterial3Api::class)
@Composable
private fun ChoiceLine(label: String, current: String, options: List<String>, enabled: Boolean, onSelect: (String) -> Unit = {}) {
    var open by remember { mutableStateOf(false) }
    var selected by remember(current) { mutableStateOf(current) }
    ExposedDropdownMenuBox(expanded = open, onExpandedChange = { if (enabled) open = it }) {
        OutlinedTextField(selected, {}, readOnly = true, label = { Text(label) }, trailingIcon = { ExposedDropdownMenuDefaults.TrailingIcon(open) }, modifier = Modifier.menuAnchor(MenuAnchorType.PrimaryNotEditable).fillMaxWidth())
        ExposedDropdownMenu(expanded = open, onDismissRequest = { open = false }) {
            options.forEach { option ->
                DropdownMenuItem(text = { Text(option) }, onClick = {
                    selected = option
                    onSelect(option)
                    open = false
                })
            }
        }
    }
}

@Composable
private fun FolderLine(label: String, value: DraftValue, busy: Boolean, placeholder: String = stringResource(R.string.folder_empty), browse: () -> Unit) {
    BrowseLine(label, folderLabel(value.value), placeholder, stringResource(R.string.field_folder_browse), !busy, onBrowse = browse)
}

@Composable
private fun SecretLine(label: String, value: String, onChange: (String) -> Unit, enabled: Boolean) {
    OutlinedTextField(value, onChange, modifier = Modifier.fillMaxWidth(), label = { Text(label) }, visualTransformation = PasswordVisualTransformation(), singleLine = true, enabled = enabled)
}

@Composable
private fun NumberLine(label: String, value: String, onChange: (String) -> Unit, enabled: Boolean, help: String? = null) {
    OutlinedTextField(value, onChange, modifier = Modifier.fillMaxWidth(), label = { Text(label) }, supportingText = help?.let { { Text(it) } }, keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Number), singleLine = true, enabled = enabled)
}

@Composable
private fun CheckLine(label: String, checked: Boolean, onChange: (Boolean) -> Unit) {
    Row(verticalAlignment = Alignment.CenterVertically) {
        Checkbox(checked, onChange)
        Text(label)
    }
}

internal fun titleRes(kind: String) = when (kind) {
    "Incremental" -> R.string.incremental_backup_task_create
    "ImportMedia" -> R.string.import_media_task_create
    "Synchronization" -> R.string.synchronization_task_create
    else -> R.string.ftps_server_task_create
}

private fun helpRes(kind: String) = when (kind) {
    "Incremental" -> R.string.incremental_help
    "ImportMedia" -> R.string.import_help
    "Synchronization" -> R.string.sync_help
    else -> R.string.ftps_help
}

private val scheduleDays = listOf(
    1 to R.string.days_monday,
    2 to R.string.days_tuesday,
    3 to R.string.days_wednesday,
    4 to R.string.days_thursday,
    5 to R.string.days_friday,
    6 to R.string.days_saturday,
    0 to R.string.days_sunday,
)

private fun keepTree(context: android.content.Context, uri: Uri) {
    val flags = android.content.Intent.FLAG_GRANT_READ_URI_PERMISSION or android.content.Intent.FLAG_GRANT_WRITE_URI_PERMISSION
    try {
        context.contentResolver.takePersistableUriPermission(uri, flags)
    } catch (_: SecurityException) {
    }
}

private fun folderLabel(value: String): String {
    if (value.isBlank()) return ""
    if (!value.startsWith("content:")) return value
    return Uri.parse(value).lastPathSegment ?: value
}

open class DraftValue(initial: String) {
    open var value by mutableStateOf(initial)
}

private class DraftField(val spec: StorageFieldSpec, initial: String) : DraftValue(initial) {
    var hidden by mutableStateOf(false)
    var label by mutableStateOf(spec.label)
}

private class StorageDraft(initial: StorageSettings) {
    var provider by mutableStateOf(initial.jsonType)
    var quota by mutableStateOf(initial.quotaGb.toString())
    val fields = mutableStateListOf<DraftField>()
    private var preserved: StorageSettings = initial

    init { fill(initial) }

    fun choose(type: String) {
        if (type == provider) return
        preserved = settings()
        provider = type
        fields.clear()
        StorageCatalog.find(type).fields.forEach { fields += DraftField(it, it.defaultValue) }
        applyRules()
    }

    fun applyRules() {
        repeat(24) {
            var wrote = false
            fields.forEach { field ->
                field.hidden = false
                field.label = field.spec.label
            }
            for (field in fields) {
                val rule = field.spec.rules.firstOrNull { it.whenValue == field.value } ?: continue
                for (patch in rule.patches) {
                    val target = fields.firstOrNull { it.spec.key == patch.target } ?: continue
                    if (patch.label != null) target.label = patch.label
                    target.hidden = patch.hidden
                    if (patch.value != null && target.value != patch.value) {
                        target.value = patch.value
                        wrote = true
                    }
                }
            }
            if (!wrote) return
        }
    }

    fun settings(): StorageSettings {
        val values = fields.associate { it.spec.key to it.value }
        return StorageCatalog.toSettings(provider, values, quota.toLongOrNull() ?: 0, preserved)
    }

    private fun fill(settings: StorageSettings) {
        provider = settings.jsonType
        val values = StorageCatalog.fieldValues(settings)
        fields.clear()
        StorageCatalog.find(settings.jsonType).fields.forEach { spec ->
            fields += DraftField(spec, values[spec.key] ?: spec.defaultValue)
        }
        applyRules()
    }
}

private class TaskDraft(store: UploadFolderStore, private val kind: String, existingName: String?) {
    private val original = existingName
    private val loaded = existingName?.let { store.loadAny(it) }
    var name by mutableStateOf(loaded?.name ?: "")
    var nameError by mutableStateOf<String?>(null)
    var password by mutableStateOf(passwordOf(loaded))
    val folders = mutableStateListOf<DraftValue>().apply {
        if (loaded is IncrementalBackupTask) addAll(loaded.items.map { DraftValue(it.target) })
    }
    var exclude by mutableStateOf((loaded as? IncrementalBackupTask)?.fileExcludePatterns?.joinToString("\n").orEmpty())
    var output = DraftValue(outputOf(loaded))
    var transform by mutableStateOf((loaded as? ImportMediaTask)?.transformFileName ?: ImportMediaRules.presets.first())
    var extensions by mutableStateOf(
        when (val task = loaded) {
            is ImportMediaTask -> task.fileExtensions?.joinToString(", ").orEmpty()
            else -> if (kind == "ImportMedia") ImportMediaRules.defaultExtensions.joinToString(", ") else ""
        },
    )
    var skipImported by mutableStateOf((loaded as? ImportMediaTask)?.skipAlreadyImportedFiles ?: true)
    var deleteSource by mutableStateOf((loaded as? ImportMediaTask)?.deleteCopiedDataOnSourceMedia ?: false)
    var minimumDate by mutableStateOf((loaded as? ImportMediaTask)?.fileLastWriteTimeMinMillis?.let { millis ->
        java.time.Instant.ofEpochMilli(millis).atZone(ZoneOffset.UTC).toLocalDate().toString()
    }.orEmpty())
    var mode by mutableStateOf(if ((loaded as? SynchronizationTask)?.synchronizationMode == SynchronizationTask.MODE_READ) "Read" else "Two-way")
    var repository by mutableStateOf((loaded as? SynchronizationTask)?.repositorySubfolder.orEmpty())
    var port by mutableStateOf(((loaded as? FtpsServerTask)?.port ?: 10999).toString())
    var user by mutableStateOf((loaded as? FtpsServerTask)?.username ?: "user")
    var access by mutableStateOf(if ((loaded as? FtpsServerTask)?.folderAccess == FtpsServerTask.ACCESS_READ_ONLY) "Read only" else "Read and write")
    var duration by mutableStateOf(((loaded as? FtpsServerTask)?.durationMinutes ?: 30).toString())
    var hour by mutableStateOf(loaded?.schedule?.hour ?: 23)
    var minute by mutableStateOf(loaded?.schedule?.minute ?: 50)
    var runAtLogin by mutableStateOf(loaded?.schedule?.runAtLogin ?: false)
    val days = mutableStateListOf<Int>().apply { addAll(loaded?.schedule?.days ?: emptySet()) }
    val where = StorageDraft(whereOf(loaded, kind))
    val source = StorageDraft((loaded as? ImportMediaTask)?.from ?: FolderSettings())
    var pendingFolder: DraftValue? = null

    fun validate(store: UploadFolderStore): String? {
        nameError = store.validateName(name, original)
        if (nameError != null) return nameError
        return when (kind) {
            "Incremental" -> when {
                folders.none { it.value.isNotBlank() } -> "Add at least one folder."
                password.isBlank() -> "Password is not specified."
                else -> null
            }
            "ImportMedia" -> when {
                output.value.isBlank() -> "Output folder is not specified."
                else -> ImportMediaRules.validateTransform(transform)
            }
            "Synchronization" -> when {
                output.value.isBlank() -> "Folder is not specified."
                password.isBlank() -> "Password is not specified."
                repository.isNotBlank() -> "Repository subfolder is no longer supported. Clear it and save the task."
                else -> null
            }
            else -> when {
                output.value.isBlank() -> "Folder is not specified."
                user.isBlank() -> "User is not specified."
                password.isBlank() -> "Password is not specified."
                port.toIntOrNull() !in 1..65535 -> "Port is invalid."
                (duration.toLongOrNull() ?: 0) !in 1..FtpsServerTask.MAX_DURATION_MINUTES -> "Duration is invalid."
                else -> null
            }
        }
    }

    fun test(context: android.content.Context): String? {
        val settings = if (kind == "ImportMedia") source.settings() else where.settings()
        if (kind != "BUtilServer") {
            StorageFactory.test(context, settings)?.let { return it }
        }
        val folder = if (kind == "Incremental") folders.firstOrNull { it.value.isNotBlank() }?.value else output.value
        if (!folder.isNullOrBlank()) {
            StorageFactory.test(context, FolderSettings(destinationFolder = folder))?.let { return it }
        }
        return null
    }

    fun save(store: UploadFolderStore) {
        store.saveAny(task(), original)
    }

    fun task(): AppTask {
        val schedule = Schedule(days.toMutableSet(), hour, minute, runAtLogin)
        val trimmed = name.trimEnd()
        return when (kind) {
            "Incremental" -> IncrementalBackupTask(
                name = trimmed,
                items = folders.filter { it.value.isNotBlank() }.map { SourceFolder(target = it.value) }.toMutableList(),
                fileExcludePatterns = exclude.lines().map { it.trim() }.filter { it.isNotEmpty() }.toMutableList(),
                password = password,
                storage = where.settings(),
                schedule = schedule,
            )
            "ImportMedia" -> ImportMediaTask(
                name = trimmed,
                transformFileName = transform,
                from = source.settings(),
                destinationFolder = output.value,
                skipAlreadyImportedFiles = skipImported,
                deleteCopiedDataOnSourceMedia = deleteSource,
                fileExtensions = ImportMediaRules.parseExtensions(extensions).toMutableList().ifEmpty { null },
                fileLastWriteTimeMinMillis = minimumDate.takeIf { it.isNotBlank() }?.let { LocalDate.parse(it).atStartOfDay().toInstant(ZoneOffset.UTC).toEpochMilli() },
                schedule = schedule,
            )
            "Synchronization" -> SynchronizationTask(
                name = trimmed,
                localFolder = output.value,
                repositorySubfolder = repository.ifBlank { null },
                synchronizationMode = if (mode == "Read") SynchronizationTask.MODE_READ else SynchronizationTask.MODE_TWO_WAY,
                password = password,
                storage = where.settings(),
                schedule = schedule,
            )
            else -> FtpsServerTask(
                name = trimmed,
                port = port.toIntOrNull() ?: 10999,
                username = user,
                password = password,
                folder = output.value,
                folderAccess = if (access == "Read only") FtpsServerTask.ACCESS_READ_ONLY else FtpsServerTask.ACCESS_READ_WRITE,
                durationMinutes = duration.toLongOrNull() ?: 30,
                schedule = schedule,
            )
        }
    }
}

private fun passwordOf(task: AppTask?): String = when (task) {
    is IncrementalBackupTask -> task.password
    is SynchronizationTask -> task.password
    is FtpsServerTask -> task.password
    else -> ""
}

private fun outputOf(task: AppTask?): String = when (task) {
    is ImportMediaTask -> task.destinationFolder
    is SynchronizationTask -> task.localFolder
    is FtpsServerTask -> task.folder
    else -> ""
}

private fun whereOf(task: AppTask?, kind: String): StorageSettings = when (task) {
    is IncrementalBackupTask -> task.storage
    is SynchronizationTask -> task.storage
    is ImportMediaTask -> FolderSettings(destinationFolder = task.destinationFolder)
    else -> FolderSettings()
}.let { if (kind == "ImportMedia") FolderSettings() else it }

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun RestorePage(onClose: () -> Unit, modifier: Modifier = Modifier, taskName: String? = null) {
    val context = LocalContext.current
    val seeded = remember(taskName) {
        if (taskName.isNullOrBlank()) null
        else runCatching { UploadFolderStore(context).loadAny(taskName) }.getOrNull()
    }
    val draft = remember(taskName) {
        StorageDraft(
            when (seeded) {
                is IncrementalBackupTask -> seeded.storage
                is SynchronizationTask -> seeded.storage
                else -> FolderSettings()
            },
        )
    }
    var password by remember(taskName) {
        mutableStateOf(
            when (seeded) {
                is IncrementalBackupTask -> seeded.password
                is SynchronizationTask -> seeded.password
                else -> ""
            },
        )
    }
    val destination = remember { DraftValue("") }
    var pending by remember { mutableStateOf<DraftValue?>(null) }
    var versions by remember { mutableStateOf<List<Long>>(emptyList()) }
    var selected by remember { mutableStateOf<Long?>(null) }
    var message by remember { mutableStateOf<String?>(null) }
    var busy by remember { mutableStateOf(false) }
    val scope = rememberCoroutineScope()
    val pick = rememberLauncherForActivityResult(ActivityResultContracts.OpenDocumentTree()) { uri ->
        if (uri != null) {
            keepTree(context, uri)
            pending?.value = uri.toString()
        }
    }
    Column(modifier.padding(20.dp).verticalScroll(rememberScrollState()), verticalArrangement = Arrangement.spacedBy(12.dp)) {
        Text(stringResource(R.string.task_restore), style = MaterialTheme.typography.headlineSmall)
        Text(stringResource(R.string.restore_help))
        SecretLine(stringResource(R.string.field_password), password, { password = it }, !busy)
        StorageBlock(stringResource(R.string.incremental_storage_help), draft, busy) {
            pending = it
            pick.launch(null)
        }
        FolderLine(stringResource(R.string.restore_destination), destination, busy) {
            pending = destination
            pick.launch(null)
        }
        if (versions.isNotEmpty()) {
            Text(stringResource(R.string.restore_version))
            versions.forEach { version ->
                FilterChip(
                    selected = selected == version,
                    onClick = { selected = version },
                    label = { Text(BackupClock.versionFolder(version)) },
                )
            }
        }
        if (busy) LinearProgressIndicator(Modifier.fillMaxWidth())
        if (!message.isNullOrBlank()) Text(message!!)
        Row(horizontalArrangement = Arrangement.spacedBy(8.dp)) {
            OutlinedButton(enabled = !busy, onClick = {
                busy = true
                scope.launch {
                    val found = withContext(Dispatchers.IO) {
                        runCatching {
                            IncrementalBackupRunner(context).versions(
                                IncrementalBackupTask(password = password, storage = draft.settings()),
                            ).map { it.backupDateUtcMillis }
                        }
                    }
                    versions = found.getOrElse {
                        message = it.message
                        emptyList()
                    }
                    selected = versions.lastOrNull()
                    busy = false
                }
            }) { Text(stringResource(R.string.restore_load)) }
            Button(enabled = !busy && selected != null, onClick = {
                val version = selected ?: return@Button
                busy = true
                message = null
                scope.launch {
                    val error = withContext(Dispatchers.IO) {
                        IncrementalBackupRunner(context).restore(draft.settings(), password, version, destination.value, { message = it }, { false })
                    }
                    message = error ?: message
                    busy = false
                }
            }) { Text(stringResource(R.string.restore_run)) }
            OutlinedButton(onClick = onClose, enabled = !busy) { Text(stringResource(R.string.button_close)) }
        }
    }
}
