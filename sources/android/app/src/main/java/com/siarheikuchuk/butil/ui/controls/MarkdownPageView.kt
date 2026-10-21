package com.siarheikuchuk.butil.ui.controls

import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
import androidx.compose.material3.OutlinedButton
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.remember
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import com.siarheikuchuk.butil.R
import com.siarheikuchuk.butil.core.legal.Credits
import com.siarheikuchuk.butil.core.legal.ThirdPartyNotices

/**
 * Scrollable markdown page with a Close button, matching
 * {@code BUtil.UI.Controls.MarkdownPageView}.
 */
@Composable
fun MarkdownPageView(
    page: MarkdownPage,
    onClose: () -> Unit,
    modifier: Modifier = Modifier,
) {
    val context = LocalContext.current
    val viewModel = remember(page) {
        val markdown = when (page) {
            MarkdownPage.Credits -> Credits.load(context)
            MarkdownPage.ThirdPartyNotices -> ThirdPartyNotices.load(context)
        }
        MarkdownPageViewModel(page, markdown)
    }
    Column(modifier.fillMaxSize().padding(24.dp)) {
        Column(
            Modifier
                .weight(1f)
                .verticalScroll(rememberScrollState()),
        ) {
            MarkdownDocumentPresenter.Render(viewModel.markdown)
        }
        OutlinedButton(
            onClick = onClose,
            modifier = Modifier.padding(top = 12.dp),
        ) {
            Text(stringResource(R.string.button_close))
        }
    }
}
