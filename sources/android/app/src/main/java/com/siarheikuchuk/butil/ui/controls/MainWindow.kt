@file:OptIn(ExperimentalMaterial3Api::class)

package com.siarheikuchuk.butil.ui.controls

import android.content.ActivityNotFoundException
import android.content.Intent
import android.net.Uri
import androidx.activity.ComponentActivity
import androidx.activity.SystemBarStyle
import androidx.activity.compose.BackHandler
import androidx.activity.enableEdgeToEdge
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
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.material3.TopAppBar
import androidx.compose.material3.TopAppBarDefaults
import androidx.compose.runtime.Composable
import androidx.compose.runtime.SideEffect
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import com.siarheikuchuk.butil.BuildConfig
import com.siarheikuchuk.butil.R
import com.siarheikuchuk.butil.core.tasks.UploadScheduler
import com.siarheikuchuk.butil.ui.BUtilTheme
import com.siarheikuchuk.butil.ui.ThemeSetting
import kotlinx.coroutines.launch

private const val HOME_PAGE = "https://github.com/drweb86/butil"
private const val PREFS = "butil"

private sealed interface AppPage {
    data object Tasks : AppPage

    data class Tool(val kind: TechnicalFileToolKind) : AppPage

    data class Markdown(val page: MarkdownPage) : AppPage

    data class UploadEditor(val existingName: String?) : AppPage

    data class Editor(val kind: String, val existingName: String?) : AppPage

    data object Restore : AppPage

    data class UploadRun(val name: String) : AppPage

    data class Stub(val titleRes: Int) : AppPage
}

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun MainWindow() {
    val context = LocalContext.current
    val activity = context as ComponentActivity
    val prefs = remember { context.getSharedPreferences(PREFS, android.content.Context.MODE_PRIVATE) }
    var dark by remember {
        mutableStateOf(prefs.getString(ThemeSetting.NAME, ThemeSetting.DARK) != ThemeSetting.LIGHT)
    }
    var page by remember { mutableStateOf<AppPage>(AppPage.Tasks) }
    var tasksRevision by remember { mutableStateOf(0) }
    val drawerState = androidx.compose.material3.rememberDrawerState(androidx.compose.material3.DrawerValue.Closed)
    val scope = rememberCoroutineScope()
    SideEffect {
        val style = if (dark) {
            SystemBarStyle.dark(android.graphics.Color.TRANSPARENT)
        } else {
            SystemBarStyle.light(android.graphics.Color.TRANSPARENT, android.graphics.Color.TRANSPARENT)
        }
        activity.enableEdgeToEdge(statusBarStyle = style, navigationBarStyle = style)
    }

    LaunchedEffect(Unit) {
        UploadScheduler.rescheduleAll(context)
    }
    BUtilTheme(dark = dark) {
        BackHandler(enabled = page !is AppPage.Tasks) {
            if (page is AppPage.UploadEditor || page is AppPage.UploadRun || page is AppPage.Editor || page is AppPage.Restore) {
                tasksRevision++
            }
            page = AppPage.Tasks
        }
        ModalNavigationDrawer(
            drawerState = drawerState,
            drawerContent = {
                AppMenu(
                    dark = dark,
                    page = page,
                    onSelect = { next ->
                        page = next
                        scope.launch { drawerState.close() }
                    },
                    onTheme = { useDark ->
                        dark = useDark
                        prefs.edit()
                            .putString(ThemeSetting.NAME, if (useDark) ThemeSetting.DARK else ThemeSetting.LIGHT)
                            .apply()
                        scope.launch { drawerState.close() }
                    },
                    onWebsite = {
                        try {
                            context.startActivity(Intent(Intent.ACTION_VIEW, Uri.parse(HOME_PAGE)))
                        } catch (_: ActivityNotFoundException) {
                        }
                        scope.launch { drawerState.close() }
                    },
                )
            },
        ) {
            val title = pageTitle(page)
            Scaffold(
                modifier = Modifier.fillMaxSize(),
                containerColor = MaterialTheme.colorScheme.background,
                topBar = {
                    TopAppBar(
                        title = { Text(title) },
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
                    AppPage.Restore -> RestorePage(
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
                    is AppPage.Markdown -> MarkdownPageView(
                        page = current.page,
                        onClose = { page = AppPage.Tasks },
                        modifier = Modifier.padding(padding),
                    )
                    is AppPage.Stub -> StubPage(
                        title = stringResource(current.titleRes),
                        modifier = Modifier.padding(padding),
                    )
                }
            }
        }
    }
}

@Composable
private fun pageTitle(page: AppPage): String = when (page) {
    AppPage.Tasks -> stringResource(R.string.app_name)
    is AppPage.UploadEditor -> stringResource(R.string.upload_folder_contents_task_create)
    is AppPage.Editor -> stringResource(titleRes(page.kind))
    AppPage.Restore -> stringResource(R.string.task_restore)
    is AppPage.UploadRun -> stringResource(R.string.app_name)
    is AppPage.Tool -> stringResource(page.kind.titleRes())
    is AppPage.Markdown -> stringResource(page.page.titleRes())
    is AppPage.Stub -> stringResource(page.titleRes)
}

@Composable
private fun AppMenu(
    dark: Boolean,
    page: AppPage,
    onSelect: (AppPage) -> Unit,
    onTheme: (Boolean) -> Unit,
    onWebsite: () -> Unit,
) {
    ModalDrawerSheet {
        Column(Modifier.verticalScroll(rememberScrollState())) {
            Text(
                text = stringResource(R.string.app_name),
                modifier = Modifier.padding(horizontal = 28.dp, vertical = 16.dp),
                style = MaterialTheme.typography.titleLarge,
            )
            NavigationDrawerItem(
                label = { Text(stringResource(R.string.app_version_and_site_link, BuildConfig.VERSION_NAME)) },
                selected = false,
                onClick = onWebsite,
            )
            SectionLabel(stringResource(R.string.theme_title))
            NavigationDrawerItem(
                label = { Text(stringResource(R.string.theme_value_dark)) },
                selected = dark,
                onClick = { onTheme(true) },
                modifier = Modifier.padding(start = 12.dp),
            )
            NavigationDrawerItem(
                label = { Text(stringResource(R.string.theme_value_light)) },
                selected = !dark,
                onClick = { onTheme(false) },
                modifier = Modifier.padding(start = 12.dp),
            )
            SectionLabel(stringResource(R.string.other_menu_title))
            StubItem(R.string.log_file_browse_logs_folder, page, onSelect)
            ToolItem(TechnicalFileToolKind.DecryptAes256, page, onSelect)
            ToolItem(TechnicalFileToolKind.EncryptAes256, page, onSelect)
            ToolItem(TechnicalFileToolKind.DecompressBrotli, page, onSelect)
            ToolItem(TechnicalFileToolKind.CompressBrotli, page, onSelect)
            StubItem(R.string.technical_tool_menu_prevent_sleep, page, onSelect)
            HorizontalDivider(modifier = Modifier.padding(vertical = 8.dp))
            StubItem(R.string.menu_credits, page, onSelect)
            StubItem(R.string.menu_license, page, onSelect)
            NavigationDrawerItem(
                label = { Text(stringResource(R.string.menu_third_party_notices)) },
                selected = page is AppPage.Markdown && page.page == MarkdownPage.ThirdPartyNotices,
                onClick = { onSelect(AppPage.Markdown(MarkdownPage.ThirdPartyNotices)) },
            )
            StubItem(R.string.menu_privacy, page, onSelect)
            HorizontalDivider(modifier = Modifier.padding(vertical = 8.dp))
            Text(
                text = stringResource(R.string.task_create),
                modifier = Modifier.padding(horizontal = 28.dp, vertical = 8.dp),
                style = MaterialTheme.typography.titleSmall,
                fontWeight = FontWeight.Bold,
            )
            EditorItem("Incremental", R.string.incremental_backup_task_create, page, onSelect)
            EditorItem("ImportMedia", R.string.import_media_task_create, page, onSelect)
            EditorItem("Synchronization", R.string.synchronization_task_create, page, onSelect)
            HorizontalDivider(modifier = Modifier.padding(start = 28.dp, end = 28.dp, top = 4.dp, bottom = 4.dp))
            EditorItem("BUtilServer", R.string.ftps_server_task_create, page, onSelect)
            NavigationDrawerItem(
                label = { Text(stringResource(R.string.upload_folder_contents_task_create)) },
                selected = page is AppPage.UploadEditor && page.existingName == null,
                onClick = { onSelect(AppPage.UploadEditor(null)) },
                modifier = Modifier.padding(start = 12.dp),
            )
            NavigationDrawerItem(
                label = { Text(stringResource(R.string.task_restore)) },
                selected = page is AppPage.Restore,
                onClick = { onSelect(AppPage.Restore) },
            )
            Spacer(Modifier.height(12.dp))
        }
    }
}

@Composable
private fun SectionLabel(text: String) {
    Text(
        text = text,
        modifier = Modifier.padding(horizontal = 28.dp, vertical = 8.dp),
        style = MaterialTheme.typography.titleSmall,
    )
}

@Composable
private fun ToolItem(
    kind: TechnicalFileToolKind,
    page: AppPage,
    onSelect: (AppPage) -> Unit,
) {
    NavigationDrawerItem(
        label = { Text(stringResource(kind.menuRes())) },
        selected = page is AppPage.Tool && page.kind == kind,
        onClick = { onSelect(AppPage.Tool(kind)) },
        modifier = Modifier.padding(start = 12.dp),
    )
}

@Composable
private fun EditorItem(kind: String, titleRes: Int, page: AppPage, onSelect: (AppPage) -> Unit) {
    NavigationDrawerItem(
        label = { Text(stringResource(titleRes)) },
        selected = page is AppPage.Editor && page.kind == kind && page.existingName == null,
        onClick = { onSelect(AppPage.Editor(kind, null)) },
        modifier = Modifier.padding(start = 12.dp),
    )
}

@Composable
private fun StubItem(
    titleRes: Int,
    page: AppPage,
    onSelect: (AppPage) -> Unit,
    indent: Boolean = false,
) {
    NavigationDrawerItem(
        label = { Text(stringResource(titleRes)) },
        selected = page is AppPage.Stub && page.titleRes == titleRes,
        onClick = { onSelect(AppPage.Stub(titleRes)) },
        modifier = if (indent) Modifier.padding(start = 12.dp) else Modifier,
    )
}

@Composable
private fun StubPage(title: String, modifier: Modifier = Modifier) {
    Column(modifier.fillMaxSize().padding(24.dp)) {
        Text(text = title, style = MaterialTheme.typography.headlineSmall)
        Spacer(Modifier.height(12.dp))
        Text(
            text = stringResource(R.string.stub_not_available),
            color = MaterialTheme.colorScheme.onBackground,
        )
    }
}

private fun TechnicalFileToolKind.titleRes(): Int = when (this) {
    TechnicalFileToolKind.DecryptAes256 -> R.string.technical_tool_decrypt_aes256_title
    TechnicalFileToolKind.EncryptAes256 -> R.string.technical_tool_encrypt_aes256_title
    TechnicalFileToolKind.DecompressBrotli -> R.string.technical_tool_decompress_brotli_title
    TechnicalFileToolKind.CompressBrotli -> R.string.technical_tool_compress_brotli_title
}

private fun TechnicalFileToolKind.menuRes(): Int = when (this) {
    TechnicalFileToolKind.DecryptAes256 -> R.string.technical_tool_menu_decrypt_aes256
    TechnicalFileToolKind.EncryptAes256 -> R.string.technical_tool_menu_encrypt_aes256
    TechnicalFileToolKind.DecompressBrotli -> R.string.technical_tool_menu_decompress_brotli
    TechnicalFileToolKind.CompressBrotli -> R.string.technical_tool_menu_compress_brotli
}
