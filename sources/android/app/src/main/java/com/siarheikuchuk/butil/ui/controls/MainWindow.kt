@file:OptIn(ExperimentalMaterial3Api::class)

package com.siarheikuchuk.butil.ui.controls

import androidx.activity.compose.BackHandler
import androidx.compose.foundation.layout.calculateEndPadding
import androidx.compose.foundation.layout.calculateStartPadding
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Menu
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.HorizontalDivider
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.ModalDrawerSheet
import androidx.compose.material3.ModalNavigationDrawer
import androidx.compose.material3.NavigationDrawerItem
import androidx.compose.material3.NavigationDrawerItemDefaults
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.material3.TopAppBar
import androidx.compose.material3.TopAppBarDefaults
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.vector.ImageVector
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.platform.LocalLayoutDirection
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.style.TextOverflow
import androidx.compose.ui.unit.Dp
import androidx.compose.ui.unit.dp
import com.siarheikuchuk.butil.BuildConfig
import com.siarheikuchuk.butil.R
import com.siarheikuchuk.butil.core.tasks.UploadScheduler
import kotlinx.coroutines.launch

private sealed interface AppPage {
    data object Tasks : AppPage

    data object StatusPreview : AppPage

    data class Tool(val kind: TechnicalFileToolKind) : AppPage

    data class Markdown(val page: MarkdownPage) : AppPage

    data class UploadEditor(val existingName: String?) : AppPage

    data class Editor(val kind: String, val existingName: String?) : AppPage

    data class Restore(val taskName: String? = null) : AppPage

    data class UploadRun(val name: String) : AppPage

    data object About : AppPage
}

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun MainWindow(
    onOpenPrivacy: () -> Unit,
    onOpenLicense: () -> Unit,
    showStatusPreview: Boolean = false,
) {
    val context = LocalContext.current
    var page by remember { mutableStateOf<AppPage>(if (showStatusPreview) AppPage.StatusPreview else AppPage.Tasks) }
    var tasksRevision by remember { mutableStateOf(0) }
    val drawerState = androidx.compose.material3.rememberDrawerState(androidx.compose.material3.DrawerValue.Closed)
    val scope = rememberCoroutineScope()

    LaunchedEffect(Unit) {
        UploadScheduler.rescheduleAll(context)
    }
    LaunchedEffect(showStatusPreview) {
        if (showStatusPreview) page = AppPage.StatusPreview
    }
    BackHandler(enabled = page !is AppPage.Tasks) {
            if (page is AppPage.UploadEditor || page is AppPage.UploadRun || page is AppPage.Editor || page is AppPage.Restore) {
                tasksRevision++
            }
            page = if (page is AppPage.Markdown) AppPage.About else AppPage.Tasks
        }
        ModalNavigationDrawer(
            drawerState = drawerState,
            drawerContent = {
                AppMenu(
                    page = page,
                    onSelect = { next ->
                        page = next
                        scope.launch { drawerState.close() }
                    },
                    onOpenPrivacy = onOpenPrivacy,
                    onOpenLicense = onOpenLicense,
                )
            },
        ) {
            Scaffold(
                modifier = Modifier.fillMaxSize(),
                containerColor = MaterialTheme.colorScheme.background,
                topBar = {
                    TopAppBar(
                        title = {
                            Text(
                                stringResource(R.string.app_legal_title, BuildConfig.VERSION_NAME),
                                maxLines = 2,
                            )
                        },
                        navigationIcon = {
                            IconButton(onClick = { scope.launch { drawerState.open() } }) {
                                Icon(Icons.Filled.Menu, contentDescription = stringResource(R.string.menu_open))
                            }
                        },
                        colors = TopAppBarDefaults.topAppBarColors(
                            containerColor = MaterialTheme.colorScheme.background,
                            titleContentColor = MaterialTheme.colorScheme.onBackground,
                            navigationIconContentColor = MaterialTheme.colorScheme.onBackground,
                        ),
                    )
                },
            ) { padding ->
                when (val current = page) {
                    AppPage.Tasks -> TasksView(
                        revision = tasksRevision,
                        onEdit = { name ->
                            val kind = com.siarheikuchuk.butil.core.tasks.UploadFolderStore(context).kind(name)
                            page = if (kind == "BUtilClient") AppPage.UploadEditor(name) else AppPage.Editor(kind, name)
                        },
                        onRun = { page = AppPage.UploadRun(it) },
                        onRestore = { page = AppPage.Restore(it) },
                        modifier = Modifier.padding(padding),
                    )
                    is AppPage.UploadEditor -> UploadFolderEditor(
                        existingName = current.existingName,
                        onClose = {
                            tasksRevision++
                            page = AppPage.Tasks
                        },
                        modifier = Modifier.padding(padding),
                    )
                    is AppPage.Editor -> TaskEditor(
                        kind = current.kind,
                        existingName = current.existingName,
                        onClose = {
                            tasksRevision++
                            page = AppPage.Tasks
                        },
                        modifier = Modifier.padding(padding),
                    )
                    is AppPage.Restore -> RestorePage(
                        taskName = current.taskName,
                        onClose = { page = AppPage.Tasks },
                        modifier = Modifier.padding(padding),
                    )
                    is AppPage.UploadRun -> UploadRunPage(
                        name = current.name,
                        onClose = {
                            tasksRevision++
                            page = AppPage.Tasks
                        },
                        modifier = Modifier.padding(padding),
                    )
                    is AppPage.Tool -> TechnicalFileToolView(
                        kind = current.kind,
                        onClose = { page = AppPage.Tasks },
                        modifier = Modifier.padding(padding),
                    )
                    AppPage.StatusPreview -> TaskStatusPreview(Modifier.padding(padding))
                    is AppPage.Markdown -> MarkdownPageView(
                        page = current.page,
                        onClose = { page = AppPage.About },
                        modifier = Modifier.padding(padding),
                    )
                    AppPage.About -> AboutPage(
                        onOpen = { page = AppPage.Markdown(it) },
                        modifier = Modifier.padding(padding),
                    )
                }
            }
        }
}

@Composable
private fun AppMenu(
    page: AppPage,
    onSelect: (AppPage) -> Unit,
    onOpenPrivacy: () -> Unit,
    onOpenLicense: () -> Unit,
) {
    val layoutDirection = LocalLayoutDirection.current
    val itemPadding = NavigationDrawerItemDefaults.ItemPadding
    val labelStart = itemPadding.calculateStartPadding(layoutDirection) + 40.dp
    ModalDrawerSheet {
        Column(Modifier.verticalScroll(rememberScrollState())) {
            Column(
                Modifier.padding(
                    start = itemPadding.calculateStartPadding(layoutDirection),
                    end = itemPadding.calculateEndPadding(layoutDirection),
                    top = 16.dp,
                    bottom = 12.dp,
                ),
            ) {
                Text(
                    text = stringResource(R.string.app_legal_name),
                    style = MaterialTheme.typography.titleMedium,
                    maxLines = 1,
                    softWrap = false,
                    overflow = TextOverflow.Clip,
                )
                Text(
                    text = stringResource(R.string.app_version, BuildConfig.VERSION_NAME),
                    style = MaterialTheme.typography.bodySmall,
                    color = MaterialTheme.colorScheme.onSurfaceVariant,
                )
            }
            DrawerDivider()
            DrawerSection(R.string.menu_section_new_task, labelStart)
            DrawerItem(
                labelRes = R.string.incremental_backup_task_create,
                icon = DrawerIcons.Backup,
                selected = page is AppPage.Editor && page.kind == "Incremental" && page.existingName == null,
                onClick = { onSelect(AppPage.Editor("Incremental", null)) },
            )
            DrawerItem(
                labelRes = R.string.import_media_task_create,
                icon = DrawerIcons.PhotoLibrary,
                selected = page is AppPage.Editor && page.kind == "ImportMedia" && page.existingName == null,
                onClick = { onSelect(AppPage.Editor("ImportMedia", null)) },
            )
            DrawerItem(
                labelRes = R.string.synchronization_task_create,
                icon = DrawerIcons.Sync,
                selected = page is AppPage.Editor && page.kind == "Synchronization" && page.existingName == null,
                onClick = { onSelect(AppPage.Editor("Synchronization", null)) },
            )
            DrawerItem(
                labelRes = R.string.ftps_server_task_create,
                icon = DrawerIcons.Dns,
                selected = page is AppPage.Editor && page.kind == "BUtilServer" && page.existingName == null,
                onClick = { onSelect(AppPage.Editor("BUtilServer", null)) },
            )
            DrawerItem(
                labelRes = R.string.upload_folder_contents_task_create,
                icon = DrawerIcons.CloudUpload,
                selected = page is AppPage.UploadEditor && page.existingName == null,
                onClick = { onSelect(AppPage.UploadEditor(null)) },
            )
            DrawerDivider()
            DrawerSection(R.string.menu_section_tools, labelStart)
            DrawerItem(
                labelRes = R.string.task_restore,
                icon = DrawerIcons.Restore,
                selected = page is AppPage.Restore,
                onClick = { onSelect(AppPage.Restore()) },
            )
            DrawerItem(
                labelRes = R.string.technical_tool_menu_encrypt_aes256,
                icon = DrawerIcons.Lock,
                selected = page is AppPage.Tool && page.kind == TechnicalFileToolKind.EncryptAes256,
                onClick = { onSelect(AppPage.Tool(TechnicalFileToolKind.EncryptAes256)) },
            )
            DrawerItem(
                labelRes = R.string.technical_tool_menu_decrypt_aes256,
                icon = DrawerIcons.LockOpen,
                selected = page is AppPage.Tool && page.kind == TechnicalFileToolKind.DecryptAes256,
                onClick = { onSelect(AppPage.Tool(TechnicalFileToolKind.DecryptAes256)) },
            )
            DrawerItem(
                labelRes = R.string.technical_tool_menu_compress_brotli,
                icon = DrawerIcons.Compress,
                selected = page is AppPage.Tool && page.kind == TechnicalFileToolKind.CompressBrotli,
                onClick = { onSelect(AppPage.Tool(TechnicalFileToolKind.CompressBrotli)) },
            )
            DrawerItem(
                labelRes = R.string.technical_tool_menu_decompress_brotli,
                icon = DrawerIcons.Expand,
                selected = page is AppPage.Tool && page.kind == TechnicalFileToolKind.DecompressBrotli,
                onClick = { onSelect(AppPage.Tool(TechnicalFileToolKind.DecompressBrotli)) },
            )
            DrawerDivider()
            DrawerItem(
                labelRes = R.string.menu_license,
                icon = DrawerIcons.Gavel,
                selected = false,
                onClick = onOpenLicense,
            )
            DrawerItem(
                labelRes = R.string.menu_privacy,
                icon = DrawerIcons.PrivacyTip,
                selected = false,
                onClick = onOpenPrivacy,
            )
            DrawerItem(
                labelRes = R.string.menu_about,
                icon = DrawerIcons.Info,
                selected = page is AppPage.About || page is AppPage.Markdown,
                onClick = { onSelect(AppPage.About) },
            )
            Spacer(Modifier.height(12.dp))
        }
    }
}

@Composable
private fun DrawerDivider() {
    HorizontalDivider(Modifier.padding(horizontal = 28.dp))
}

@Composable
private fun DrawerSection(titleRes: Int, labelStart: Dp) {
    Text(
        text = stringResource(titleRes),
        modifier = Modifier.padding(start = labelStart, top = 12.dp, end = 28.dp, bottom = 4.dp),
        style = MaterialTheme.typography.titleSmall,
        color = MaterialTheme.colorScheme.onSurfaceVariant,
    )
}

@Composable
private fun DrawerItem(
    labelRes: Int,
    icon: ImageVector,
    selected: Boolean,
    onClick: () -> Unit,
) {
    NavigationDrawerItem(
        label = { Text(stringResource(labelRes)) },
        icon = { Icon(icon, contentDescription = null) },
        selected = selected,
        onClick = onClick,
    )
}

@Composable
private fun AboutPage(onOpen: (MarkdownPage) -> Unit, modifier: Modifier = Modifier) {
    Column(modifier.padding(vertical = 8.dp)) {
        Text(
            text = stringResource(R.string.menu_about),
            modifier = Modifier.padding(horizontal = 28.dp, vertical = 16.dp),
            style = MaterialTheme.typography.titleMedium,
        )
        NavigationDrawerItem(
            label = { Text(stringResource(R.string.menu_credits)) },
            selected = false,
            onClick = { onOpen(MarkdownPage.Credits) },
        )
        NavigationDrawerItem(
            label = { Text(stringResource(R.string.menu_third_party_notices)) },
            selected = false,
            onClick = { onOpen(MarkdownPage.ThirdPartyNotices) },
        )
    }
}
