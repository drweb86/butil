package com.siarheikuchuk.butil.ui.controls

import androidx.compose.animation.AnimatedVisibility
import androidx.compose.animation.core.animateFloatAsState
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.ColumnScope
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.KeyboardArrowDown
import androidx.compose.material3.HorizontalDivider
import androidx.compose.material3.Icon
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedButton
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.draw.rotate
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import com.siarheikuchuk.butil.ui.LocalTaskSectionColors

/** Collapsible card with an emoji + title header, matching the desktop task section expander. */
@Composable
internal fun TaskSection(
    icon: String,
    title: String,
    modifier: Modifier = Modifier,
    initiallyExpanded: Boolean = true,
    content: @Composable ColumnScope.() -> Unit,
) {
    var expanded by rememberSaveable(title) { mutableStateOf(initiallyExpanded) }
    val colors = LocalTaskSectionColors.current
    val shape = RoundedCornerShape(8.dp)
    val chevron by animateFloatAsState(if (expanded) 0f else -90f, label = "chevron")
    Column(
        modifier
            .fillMaxWidth()
            .clip(shape)
            .background(colors.background)
            .border(1.dp, colors.border, shape),
    ) {
        Row(
            Modifier
                .fillMaxWidth()
                .background(colors.header)
                .clickable { expanded = !expanded }
                .padding(horizontal = 12.dp, vertical = 10.dp),
            verticalAlignment = Alignment.CenterVertically,
            horizontalArrangement = Arrangement.spacedBy(8.dp),
        ) {
            Icon(Icons.Filled.KeyboardArrowDown, contentDescription = null, modifier = Modifier.rotate(chevron))
            Text(icon, style = MaterialTheme.typography.titleSmall)
            Text(title.trimEnd(':'), style = MaterialTheme.typography.titleSmall, fontWeight = FontWeight.SemiBold)
        }
        AnimatedVisibility(expanded) {
            Column {
                HorizontalDivider(color = colors.border)
                Column(
                    Modifier.padding(start = 12.dp, end = 12.dp, top = 8.dp, bottom = 12.dp),
                    verticalArrangement = Arrangement.spacedBy(10.dp),
                    content = content,
                )
            }
        }
    }
}

/** Read-only path box with a browse button beside it, like the desktop folder fields. */
@Composable
internal fun BrowseLine(
    label: String?,
    value: String,
    placeholder: String,
    action: String,
    enabled: Boolean,
    error: String? = null,
    trailing: (@Composable () -> Unit)? = null,
    onBrowse: () -> Unit,
) {
    // Label sits above the box: an empty read-only field would otherwise hide the placeholder under its label.
    Column(verticalArrangement = Arrangement.spacedBy(4.dp)) {
        if (label != null) Text(label, style = MaterialTheme.typography.bodyMedium)
        Row(verticalAlignment = Alignment.CenterVertically, horizontalArrangement = Arrangement.spacedBy(8.dp)) {
            OutlinedTextField(
                value = value,
                onValueChange = {},
                readOnly = true,
                singleLine = true,
                placeholder = { Text(placeholder, maxLines = 1) },
                trailingIcon = trailing,
                isError = error != null,
                modifier = Modifier.weight(1f),
            )
            OutlinedButton(onClick = onBrowse, enabled = enabled) { Text(action) }
        }
        if (error != null) Text(error, color = MaterialTheme.colorScheme.error, style = MaterialTheme.typography.bodySmall)
    }
}
