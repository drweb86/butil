package com.siarheikuchuk.butil.ui.controls

import android.content.Intent
import android.net.Uri
import android.os.Build
import android.provider.Settings
import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedButton
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import com.siarheikuchuk.butil.R
import com.siarheikuchuk.butil.core.tasks.UploadScheduler
import com.siarheikuchuk.butil.ui.LocalStatusColors

/** Shown in When? while the task has a schedule but Android has not granted exact alarms. */
@Composable
internal fun ExactAlarmNotice(scheduled: Boolean) {
    val context = LocalContext.current
    var allowed by remember { mutableStateOf(UploadScheduler.canScheduleExact(context)) }
    val settings = rememberLauncherForActivityResult(ActivityResultContracts.StartActivityForResult()) {
        allowed = UploadScheduler.canScheduleExact(context)
    }
    if (!scheduled || allowed || Build.VERSION.SDK_INT < Build.VERSION_CODES.S) return
    Column(verticalArrangement = Arrangement.spacedBy(4.dp)) {
        Text(stringResource(R.string.exact_alarm_needed), color = LocalStatusColors.current.warning, style = MaterialTheme.typography.bodySmall)
        OutlinedButton(onClick = {
            settings.launch(Intent(Settings.ACTION_REQUEST_SCHEDULE_EXACT_ALARM, Uri.parse("package:${context.packageName}")))
        }) { Text(stringResource(R.string.exact_alarm_allow)) }
    }
}
