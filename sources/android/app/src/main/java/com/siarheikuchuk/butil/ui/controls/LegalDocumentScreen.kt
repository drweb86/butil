package com.siarheikuchuk.butil.ui.controls

import androidx.activity.compose.BackHandler
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
import androidx.compose.material3.AlertDialog
import androidx.compose.material3.Button
import androidx.compose.material3.DropdownMenuItem
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.ExposedDropdownMenuBox
import androidx.compose.material3.ExposedDropdownMenuDefaults
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.MenuAnchorType
import androidx.compose.material3.OutlinedButton
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.material3.TopAppBar
import androidx.compose.material3.TopAppBarDefaults
import androidx.compose.runtime.Composable
import androidx.compose.runtime.CompositionLocalProvider
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.platform.LocalLayoutDirection
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.LayoutDirection
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.siarheikuchuk.butil.R
import com.siarheikuchuk.butil.core.legal.DocumentLanguages
import com.siarheikuchuk.butil.core.legal.LegalDocuments
import com.siarheikuchuk.butil.core.legal.PrivacyStore

enum class LegalScreenMode {
    Info,
    ConsentGate,
    InfoWithWithdraw,
}

enum class LegalDocumentKind {
    Privacy,
    License,
}

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun LegalDocumentScreen(
    mode: LegalScreenMode,
    kind: LegalDocumentKind,
    includeChinaAppendix: Boolean,
    onDismiss: () -> Unit,
    onAgree: () -> Unit = onDismiss,
    onDeclineConfirmed: () -> Unit = {},
) {
    val context = LocalContext.current
    val titleRes = if (kind == LegalDocumentKind.License) R.string.menu_license else R.string.menu_privacy
    var selected by remember {
        mutableStateOf(
            PrivacyStore.languageCode(context)?.let { DocumentLanguages.byCode(it) }
                ?: DocumentLanguages.matchDevice(),
        )
    }
    var menuOpen by remember { mutableStateOf(false) }
    var confirmOpen by remember { mutableStateOf(false) }
    val body = remember(selected.code, kind, includeChinaAppendix) {
        when (kind) {
            LegalDocumentKind.Privacy -> LegalDocuments.loadPrivacy(context, selected.assetFile)
            LegalDocumentKind.License -> LegalDocuments.loadLicense(
                context,
                selected.assetFile,
                includeChinaAppendix,
            )
        }
    }
    val direction = if (selected.rtl) LayoutDirection.Rtl else LayoutDirection.Ltr

    LaunchedEffect(Unit) {
        if (PrivacyStore.languageCode(context) == null) {
            PrivacyStore.setLanguageCode(context, selected.code)
        }
    }

    if (mode == LegalScreenMode.ConsentGate) {
        BackHandler { }
    } else {
        BackHandler { onDismiss() }
    }

    CompositionLocalProvider(LocalLayoutDirection provides direction) {
        Scaffold(
            containerColor = MaterialTheme.colorScheme.background,
            topBar = {
                TopAppBar(
                    title = { Text(stringResource(titleRes)) },
                    colors = TopAppBarDefaults.topAppBarColors(
                        containerColor = MaterialTheme.colorScheme.background,
                        titleContentColor = MaterialTheme.colorScheme.onBackground,
                    ),
                )
            },
        ) { padding ->
            Column(
                modifier = Modifier
                    .padding(padding)
                    .fillMaxSize()
                    .padding(12.dp),
            ) {
                ExposedDropdownMenuBox(
                    expanded = menuOpen,
                    onExpandedChange = { menuOpen = it },
                    modifier = Modifier.fillMaxWidth(),
                ) {
                    OutlinedTextField(
                        value = selected.nativeName,
                        onValueChange = {},
                        readOnly = true,
                        label = { Text(stringResource(R.string.privacy_language)) },
                        trailingIcon = { ExposedDropdownMenuDefaults.TrailingIcon(expanded = menuOpen) },
                        modifier = Modifier
                            .fillMaxWidth()
                            .menuAnchor(MenuAnchorType.PrimaryNotEditable),
                    )
                    ExposedDropdownMenu(
                        expanded = menuOpen,
                        onDismissRequest = { menuOpen = false },
                    ) {
                        DocumentLanguages.all.forEach { lang ->
                            DropdownMenuItem(
                                text = { Text(lang.nativeName) },
                                onClick = {
                                    selected = lang
                                    PrivacyStore.setLanguageCode(context, lang.code)
                                    menuOpen = false
                                },
                            )
                        }
                    }
                }

                Column(
                    modifier = Modifier
                        .weight(1f)
                        .fillMaxWidth()
                        .padding(top = 12.dp)
                        .verticalScroll(rememberScrollState()),
                ) {
                    MarkdownDocumentPresenter.Render(body)
                }

                when (mode) {
                    LegalScreenMode.ConsentGate -> {
                        Row(
                            modifier = Modifier
                                .fillMaxWidth()
                                .padding(top = 12.dp),
                            horizontalArrangement = Arrangement.spacedBy(8.dp),
                        ) {
                            OutlinedButton(
                                onClick = { confirmOpen = true },
                                modifier = Modifier.weight(1f),
                            ) {
                                Text(stringResource(R.string.privacy_disagree))
                            }
                            Button(
                                onClick = onAgree,
                                modifier = Modifier.weight(1f),
                            ) {
                                Text(stringResource(R.string.privacy_agree), fontWeight = FontWeight.Bold)
                            }
                        }
                    }
                    LegalScreenMode.Info, LegalScreenMode.InfoWithWithdraw -> {
                        Button(
                            onClick = onDismiss,
                            modifier = Modifier
                                .fillMaxWidth()
                                .padding(top = 12.dp),
                        ) {
                            Text(stringResource(R.string.privacy_ok), fontWeight = FontWeight.Bold)
                        }
                        if (mode == LegalScreenMode.InfoWithWithdraw) {
                            TextButton(
                                onClick = { confirmOpen = true },
                                modifier = Modifier.fillMaxWidth(),
                            ) {
                                Text(
                                    stringResource(R.string.privacy_withdraw_consent),
                                    color = MaterialTheme.colorScheme.onSurfaceVariant,
                                    fontSize = 13.sp,
                                )
                            }
                        }
                    }
                }
            }
        }
    }

    if (confirmOpen) {
        AlertDialog(
            onDismissRequest = { confirmOpen = false },
            containerColor = MaterialTheme.colorScheme.surface,
            titleContentColor = MaterialTheme.colorScheme.onSurface,
            textContentColor = MaterialTheme.colorScheme.onSurface,
            text = { Text(stringResource(R.string.privacy_decline_message)) },
            confirmButton = {
                TextButton(
                    onClick = {
                        confirmOpen = false
                        onDeclineConfirmed()
                    },
                ) {
                    Text(
                        stringResource(R.string.privacy_delete_and_close),
                        color = MaterialTheme.colorScheme.error,
                    )
                }
            },
            dismissButton = {
                TextButton(onClick = { confirmOpen = false }) {
                    Text(stringResource(R.string.privacy_cancel))
                }
            },
        )
    }
}
