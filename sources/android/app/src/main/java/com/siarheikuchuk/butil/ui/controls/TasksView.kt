package com.siarheikuchuk.butil.ui.controls

import android.Manifest
import android.content.ActivityNotFoundException
import android.content.ClipData
import android.content.Context
import android.content.Intent
import android.content.pm.PackageManager
import android.net.Uri
import android.content.res.Configuration
import android.os.Build
import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.ColumnScope
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.offset
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.MoreVert
import androidx.compose.material.icons.filled.PlayArrow
import androidx.compose.material.icons.outlined.CheckCircle
import androidx.compose.material.icons.outlined.Delete
import androidx.compose.material.icons.outlined.Edit
import androidx.compose.material.icons.outlined.Warning
import androidx.compose.material3.AlertDialog
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.DropdownMenu
import androidx.compose.material3.DropdownMenuItem
import androidx.compose.material3.FilledTonalButton
import androidx.compose.material3.HorizontalDivider
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedCard
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.SolidColor
import androidx.compose.ui.graphics.vector.ImageVector
import androidx.compose.ui.graphics.vector.PathParser
import androidx.compose.ui.layout.layout
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.style.TextOverflow
import androidx.compose.ui.unit.dp
import androidx.core.content.ContextCompat
import com.siarheikuchuk.butil.R
import com.siarheikuchuk.butil.core.tasks.TaskRunLog
import com.siarheikuchuk.butil.ui.BUtilTheme
import com.siarheikuchuk.butil.ui.LocalStatusColors
import com.siarheikuchuk.butil.core.tasks.UploadFolderService
import com.siarheikuchuk.butil.core.tasks.UploadFolderStore
import com.siarheikuchuk.butil.core.tasks.UploadScheduler
import java.time.Instant
import java.time.ZoneId
import java.time.format.DateTimeFormatter

@Composable
fun TasksView(
    revision: Int,
    onEdit: (String) -> Unit,
    onRun: (String) -> Unit,
    onRestore: (String) -> Unit,
    modifier: Modifier = Modifier,
) {
    val context = LocalContext.current
    val store = remember { UploadFolderStore(context) }
    var localRevision by remember { mutableIntStateOf(0) }
    val names = remember(revision, localRevision) { store.names() }
    var pendingDelete by remember { mutableStateOf<String?>(null) }
    var notice by remember { mutableStateOf<String?>(null) }
    var pendingRun by remember { mutableStateOf<String?>(null) }
    var menuFor by remember { mutableStateOf<String?>(null) }
    var pendingExport by remember { mutableStateOf<String?>(null) }
    val notificationPermission = stringResource(R.string.upload_notifications_required)
    val logMissing = stringResource(R.string.task_log_missing)
    val logSaveFailed = stringResource(R.string.task_log_save_failed)
    val logOpenFailed = stringResource(R.string.task_log_open_failed)
    val saveLog = rememberLauncherForActivityResult(ActivityResultContracts.CreateDocument("text/plain")) { uri ->
        val name = pendingExport
        pendingExport = null
        if (uri == null || name == null) return@rememberLauncherForActivityResult
        val bytes = runCatching { TaskRunLog(context).readBytes(name) }.getOrNull()
        if (bytes == null) {
            notice = logMissing
            return@rememberLauncherForActivityResult
        }
        try {
            context.contentResolver.openOutputStream(uri, "w")?.use { stream ->
                stream.write(bytes)
                stream.flush()
            } ?: error("The selected file could not be opened.")
        } catch (_: Exception) {
            notice = logSaveFailed
            return@rememberLauncherForActivityResult
        }
        try {
            openSavedLog(context, uri)
            notice = null
        } catch (_: ActivityNotFoundException) {
            notice = logOpenFailed
        } catch (_: SecurityException) {
            notice = logOpenFailed
        }
    }
    val permissionLauncher = rememberLauncherForActivityResult(ActivityResultContracts.RequestPermission()) { granted ->
        val name = pendingRun
        pendingRun = null
        if (name == null) return@rememberLauncherForActivityResult
        if (!granted) {
            notice = notificationPermission
            return@rememberLauncherForActivityResult
        }
        UploadFolderService.start(context, name)
        onRun(name)
    }

    if (names.isEmpty()) {
        Column(modifier.padding(24.dp), verticalArrangement = Arrangement.spacedBy(8.dp)) {
            Text(stringResource(R.string.task_list), style = MaterialTheme.typography.headlineMedium)
            Text(stringResource(R.string.tasks_empty))
        }
        return
    }

    Column(
        modifier.padding(horizontal = 16.dp, vertical = 12.dp).verticalScroll(rememberScrollState()),
        verticalArrangement = Arrangement.spacedBy(12.dp),
    ) {
        names.forEach { name ->
            val status = store.readStatus(name)
            val kind = runCatching { store.kind(name) }.getOrDefault("")
            val canRestore = kind == "Incremental" || kind == "Synchronization"
            TaskCard(
                title = name,
                state = taskRunState(status),
                lastRun = store.readStarted(name)?.let { started ->
                    startedAtFormat.format(Instant.ofEpochMilli(started).atZone(ZoneId.systemDefault()))
                },
                onRun = {
                    val needsPermission = Build.VERSION.SDK_INT >= 33 &&
                        ContextCompat.checkSelfPermission(context, Manifest.permission.POST_NOTIFICATIONS) != PackageManager.PERMISSION_GRANTED
                    if (needsPermission) {
                        pendingRun = name
                        permissionLauncher.launch(Manifest.permission.POST_NOTIFICATIONS)
                    } else {
                        UploadFolderService.start(context, name)
                        onRun(name)
                    }
                },
                menuExpanded = menuFor == name,
                onMenuClick = { menuFor = name },
                onMenuDismiss = { menuFor = null },
            ) {
                DropdownMenuItem(
                    text = { Text(stringResource(R.string.task_edit)) },
                    leadingIcon = { Icon(Icons.Outlined.Edit, contentDescription = null) },
                    onClick = {
                        menuFor = null
                        onEdit(name)
                    },
                )
                DropdownMenuItem(
                    text = { Text(stringResource(R.string.task_duplicate)) },
                    leadingIcon = { Icon(ContentCopyIcon, contentDescription = null) },
                    onClick = {
                        menuFor = null
                        val created = runCatching { store.duplicate(name) }.getOrNull()
                        if (created != null) {
                            UploadScheduler.reschedule(context, store.loadAny(created))
                            localRevision++
                        }
                    },
                )
                if (canRestore) {
                    DropdownMenuItem(
                        text = { Text(stringResource(R.string.task_restore)) },
                        leadingIcon = { Icon(RestoreIcon, contentDescription = null) },
                        onClick = {
                            menuFor = null
                            onRestore(name)
                        },
                    )
                }
                DropdownMenuItem(
                    text = { Text(stringResource(R.string.task_open_recent_log)) },
                    leadingIcon = { Icon(DescriptionIcon, contentDescription = null) },
                    onClick = {
                        menuFor = null
                        if (!TaskRunLog(context).exists(name)) {
                            notice = logMissing
                        } else {
                            pendingExport = name
                            saveLog.launch("$name.txt")
                        }
                    },
                )
                HorizontalDivider()
                DropdownMenuItem(
                    text = { Text(stringResource(R.string.task_delete), color = MaterialTheme.colorScheme.error) },
                    leadingIcon = {
                        Icon(Icons.Outlined.Delete, contentDescription = null, tint = MaterialTheme.colorScheme.error)
                    },
                    onClick = {
                        menuFor = null
                        pendingDelete = name
                    },
                )
            }
        }
        if (!notice.isNullOrBlank()) Text(notice!!, color = MaterialTheme.colorScheme.error)
    }

    val deleting = pendingDelete
    if (deleting != null) {
        AlertDialog(
            onDismissRequest = { pendingDelete = null },
            title = { Text(deleting) },
            text = { Text(stringResource(R.string.task_delete_confirm)) },
            confirmButton = {
                TextButton(onClick = {
                    store.delete(deleting)
                    UploadScheduler.cancel(context, deleting)
                    pendingDelete = null
                    localRevision++
                }) { Text(stringResource(R.string.task_delete)) }
            },
            dismissButton = {
                TextButton(onClick = { pendingDelete = null }) { Text(stringResource(R.string.button_cancel)) }
            },
        )
    }
}

private enum class TaskRunState { Success, Failed, Stopped, None }

private data class TaskStatusUi(
    val icon: ImageVector,
    val color: Color,
    val textColor: Color,
    val label: String,
)

private val startedAtFormat = DateTimeFormatter.ofPattern("dd.MM.yyyy HH:mm")

private val ErrorOutlineIcon = materialIcon(
    "ErrorOutline",
    "M11,15h2v2h-2zM11,7h2v6h-2zM11.99,2C6.47,2 2,6.48 2,12s4.47,10 9.99,10C17.52,22 22,17.52 22,12S17.52,2 11.99,2zM12,20c-4.42,0 -8,-3.58 -8,-8s3.58,-8 8,-8 8,3.58 8,8 -3.58,8 -8,8z",
)

private val ContentCopyIcon = materialIcon(
    "ContentCopy",
    "M16,1H4c-1.1,0 -2,0.9 -2,2v14h2V3h12V1zM19,5H8c-1.1,0 -2,0.9 -2,2v14c0,1.1 0.9,2 2,2h11c1.1,0 2,-0.9 2,-2V7c0,-1.1 -0.9,-2 -2,-2zM19,21H8V7h11v14z",
)

private val RestoreIcon = materialIcon(
    "Restore",
    "M13,3c-4.97,0 -9,4.03 -9,9H1l3.89,3.89 0.07,0.14L9,12H6c0,-3.87 3.13,-7 7,-7s7,3.13 7,7 -3.13,7 -7,7c-1.93,0 -3.68,-0.79 -4.94,-2.06l-1.42,1.42C8.27,19.99 10.51,21 13,21c4.97,0 9,-4.03 9,-9s-4.03,-9 -9,-9zM12,8v5l4.28,2.54 0.72,-1.21 -3.5,-2.08V8H12z",
)

private val DescriptionIcon = materialIcon(
    "Description",
    "M8,16h8v2H8zM8,12h8v2H8zM14,2H6c-1.1,0 -2,0.9 -2,2v16c0,1.1 0.89,2 1.99,2H18c1.1,0 2,-0.9 2,-2V8l-6,-6zM18,20H6V4h7v5h5v11z",
)

private val NeverRunIcon = materialIcon(
    "RadioButtonUnchecked",
    "M12,2C6.48,2 2,6.48 2,12s4.48,10 10,10 10,-4.48 10,-10S17.52,2 12,2zM12,20c-4.42,0 -8,-3.58 -8,-8s3.58,-8 8,-8 8,3.58 8,8 -3.58,8 -8,8z",
)

private fun materialIcon(name: String, path: String): ImageVector = ImageVector.Builder(
    name = name,
    defaultWidth = 24.dp,
    defaultHeight = 24.dp,
    viewportWidth = 24f,
    viewportHeight = 24f,
).apply {
    addPath(pathData = PathParser().parsePathString(path).toNodes(), fill = SolidColor(Color.Black))
}.build()

private fun taskRunState(raw: String?): TaskRunState {
    val label = raw?.trim()?.trimEnd('.')?.trim().orEmpty()
    if (label.isEmpty()) return TaskRunState.None
    return when {
        label.equals("Finished successfully", ignoreCase = true) -> TaskRunState.Success
        label.endsWith("cancelled", ignoreCase = true) || label.endsWith("stopped", ignoreCase = true) -> TaskRunState.Stopped
        else -> TaskRunState.Failed
    }
}

@Composable
private fun TaskRunState.statusUi(lastRun: String?): TaskStatusUi {
    val statusColors = LocalStatusColors.current
    val muted = MaterialTheme.colorScheme.onSurfaceVariant
    val error = MaterialTheme.colorScheme.error
    val whenRun = lastRun.orEmpty()
    return when (this) {
        TaskRunState.Success -> TaskStatusUi(
            icon = Icons.Outlined.CheckCircle,
            color = statusColors.success,
            textColor = muted,
            label = whenRun,
        )
        TaskRunState.Failed -> TaskStatusUi(
            icon = ErrorOutlineIcon,
            color = error,
            textColor = error,
            label = stringResource(R.string.task_status_failed, whenRun),
        )
        TaskRunState.Stopped -> TaskStatusUi(
            icon = Icons.Outlined.Warning,
            color = statusColors.warning,
            textColor = statusColors.warning,
            label = stringResource(R.string.task_status_stopped, whenRun),
        )
        TaskRunState.None -> TaskStatusUi(
            icon = NeverRunIcon,
            color = muted,
            textColor = muted,
            label = stringResource(R.string.task_status_never_run),
        )
    }
}

@Composable
private fun TaskCard(
    title: String,
    state: TaskRunState,
    lastRun: String?,
    onRun: () -> Unit,
    menuExpanded: Boolean,
    onMenuClick: () -> Unit,
    onMenuDismiss: () -> Unit,
    menu: @Composable ColumnScope.() -> Unit,
) {
    val ui = state.statusUi(lastRun)
    val runColors = ButtonDefaults.filledTonalButtonColors(
        containerColor = MaterialTheme.colorScheme.primary.copy(alpha = 0.18f),
        contentColor = MaterialTheme.colorScheme.primary,
    )
    OutlinedCard(
        modifier = Modifier.fillMaxWidth(),
        shape = RoundedCornerShape(12.dp),
    ) {
        Column(Modifier.padding(start = 16.dp, top = 6.dp, end = 4.dp, bottom = 12.dp)) {
            Row(verticalAlignment = Alignment.Top) {
                Text(
                    text = title,
                    style = MaterialTheme.typography.titleMedium,
                    maxLines = 2,
                    overflow = TextOverflow.Ellipsis,
                    modifier = Modifier.weight(1f).padding(top = 6.dp),
                )
                Box(Modifier.menuButtonSlot()) {
                    IconButton(onClick = onMenuClick) {
                        Icon(Icons.Filled.MoreVert, contentDescription = stringResource(R.string.task_more_options))
                    }
                    DropdownMenu(expanded = menuExpanded, onDismissRequest = onMenuDismiss, content = menu)
                }
            }
            Spacer(Modifier.height(8.dp))
            Row(
                modifier = Modifier.padding(end = MenuGlyphEndInset),
                verticalAlignment = Alignment.CenterVertically,
            ) {
                Icon(ui.icon, contentDescription = null, tint = ui.color, modifier = Modifier.size(16.dp))
                Spacer(Modifier.width(6.dp))
                Text(
                    text = ui.label,
                    style = MaterialTheme.typography.bodySmall,
                    color = ui.textColor,
                    maxLines = 1,
                    overflow = TextOverflow.Ellipsis,
                    modifier = Modifier.weight(1f),
                )
                FilledTonalButton(
                    onClick = onRun,
                    colors = runColors,
                    contentPadding = ButtonDefaults.ButtonWithIconContentPadding,
                ) {
                    Icon(Icons.Filled.PlayArrow, contentDescription = null, modifier = Modifier.size(18.dp))
                    Spacer(Modifier.width(4.dp))
                    Text(stringResource(R.string.task_run))
                }
            }
        }
    }
}

/** Distance from the menu button's end to the right edge of the ⋮ glyph. */
private val MenuGlyphEndInset = 22.dp

/** Keeps the 48 dp menu target, without letting it stretch the title row. */
private fun Modifier.menuButtonSlot(): Modifier = this
    .layout { measurable, constraints ->
        val placeable = measurable.measure(constraints)
        layout(placeable.width, 0) {
            placeable.place(0, 0)
        }
    }
    .offset(y = (-6).dp)

@Composable
internal fun TaskStatusPreview(modifier: Modifier = Modifier) {
    Column(
        modifier.verticalScroll(rememberScrollState()).padding(horizontal = 16.dp, vertical = 12.dp),
        verticalArrangement = Arrangement.spacedBy(12.dp),
    ) {
        PreviewTaskCard("Incremental photos", TaskRunState.Success, "24.10.2026 18:42")
        PreviewTaskCard("Upload documents", TaskRunState.Failed, "23.10.2026 09:15")
        PreviewTaskCard(
            "Camera import with a long name that wraps onto a second line",
            TaskRunState.Stopped,
            "22.10.2026 21:04",
        )
        PreviewTaskCard("New synchronization", TaskRunState.None, null)
    }
}

@Preview(name = "Task cards", uiMode = Configuration.UI_MODE_NIGHT_YES, showBackground = true)
@Composable
private fun TaskCardsPreview() {
    BUtilTheme {
        Surface(color = MaterialTheme.colorScheme.background) {
            TaskStatusPreview()
        }
    }
}

@Composable
private fun PreviewTaskCard(title: String, state: TaskRunState, lastRun: String?) {
    TaskCard(
        title = title,
        state = state,
        lastRun = lastRun,
        onRun = {},
        menuExpanded = false,
        onMenuClick = {},
        onMenuDismiss = {},
        menu = {},
    )
}

private fun openSavedLog(context: Context, uri: Uri) {
    val view = Intent(Intent.ACTION_VIEW).apply {
        setDataAndType(uri, "text/plain")
        clipData = ClipData.newRawUri("log", uri)
        addFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION)
    }
    context.startActivity(view)
}
