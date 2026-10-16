package com.siarheikuchuk.butil.ui.controls

import android.Manifest
import android.content.pm.PackageManager
import android.os.Build
import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
import androidx.compose.material3.AlertDialog
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedButton
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import androidx.core.content.ContextCompat
import com.siarheikuchuk.butil.R
import com.siarheikuchuk.butil.core.tasks.UploadFolderService
import com.siarheikuchuk.butil.core.tasks.UploadFolderStore
import com.siarheikuchuk.butil.core.tasks.UploadScheduler

@Composable
fun TasksView(
    revision: Int,
    onEdit: (String) -> Unit,
    onRun: (String) -> Unit,
    modifier: Modifier = Modifier,
) {
    val context = LocalContext.current
    val store = remember { UploadFolderStore(context) }
    var localRevision by remember { mutableIntStateOf(0) }
    val names = remember(revision, localRevision) { store.names() }
    var pendingDelete by remember { mutableStateOf<String?>(null) }
    var notice by remember { mutableStateOf<String?>(null) }
    var pendingRun by remember { mutableStateOf<String?>(null) }
    val notificationPermission = stringResource(R.string.upload_notifications_required)
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
            Column(verticalArrangement = Arrangement.spacedBy(4.dp)) {
                Text(name, style = MaterialTheme.typography.titleMedium)
                Text(stringResource(taskKindLabel(runCatching { store.kind(name) }.getOrDefault(""))), style = MaterialTheme.typography.bodySmall)
                if (!status.isNullOrBlank()) {
                    Text(status, style = MaterialTheme.typography.bodySmall)
                }
                Row(horizontalArrangement = Arrangement.spacedBy(8.dp)) {
                    OutlinedButton(onClick = {
                        val needsPermission = Build.VERSION.SDK_INT >= 33 &&
                            ContextCompat.checkSelfPermission(context, Manifest.permission.POST_NOTIFICATIONS) != PackageManager.PERMISSION_GRANTED
                        if (needsPermission) {
                            pendingRun = name
                            permissionLauncher.launch(Manifest.permission.POST_NOTIFICATIONS)
                        } else {
                            UploadFolderService.start(context, name)
                            onRun(name)
                        }
                    }) { Text(stringResource(R.string.task_run)) }
                    OutlinedButton(onClick = { onEdit(name) }) { Text(stringResource(R.string.task_edit)) }
                    OutlinedButton(onClick = { pendingDelete = name }) { Text(stringResource(R.string.task_delete)) }
                }
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
