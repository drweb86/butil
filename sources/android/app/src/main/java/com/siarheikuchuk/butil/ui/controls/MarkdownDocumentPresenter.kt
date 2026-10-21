package com.siarheikuchuk.butil.ui.controls

import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.SpanStyle
import androidx.compose.ui.text.buildAnnotatedString
import androidx.compose.ui.text.font.FontFamily
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.siarheikuchuk.butil.core.legal.PrivacyInline
import com.siarheikuchuk.butil.core.legal.PrivacyMarkdown
import com.siarheikuchuk.butil.core.legal.PrivacyMdBlock

/**
 * Renders the same markdown blocks as {@code BUtil.UI.Controls.MarkdownDocumentPresenter}.
 */
object MarkdownDocumentPresenter {
    @Composable
    fun Render(markdown: String, modifier: Modifier = Modifier) {
        Column(modifier) {
            for (block in PrivacyMarkdown.parse(markdown)) {
                when (block) {
                    is PrivacyMdBlock.Heading -> Text(
                        text = block.text,
                        fontWeight = FontWeight.Bold,
                        fontSize = when (block.level) {
                            1 -> 22.sp
                            2 -> 18.sp
                            else -> 16.sp
                        },
                        color = MaterialTheme.colorScheme.onBackground,
                        modifier = Modifier.padding(
                            top = if (block.level == 1) 4.dp else 16.dp,
                            bottom = 8.dp,
                        ),
                    )
                    is PrivacyMdBlock.Paragraph -> InlineText(
                        inlines = block.inlines,
                        modifier = Modifier.padding(bottom = 10.dp),
                    )
                    is PrivacyMdBlock.Bullet -> InlineText(
                        inlines = block.inlines,
                        prefix = "•  ",
                        modifier = Modifier.padding(start = 12.dp, bottom = 6.dp),
                    )
                }
            }
        }
    }
}

@Composable
private fun InlineText(
    inlines: List<PrivacyInline>,
    modifier: Modifier = Modifier,
    prefix: String? = null,
) {
    val annotated = buildAnnotatedString {
        if (!prefix.isNullOrEmpty()) {
            append(prefix)
        }
        for (inline in inlines) {
            when (inline) {
                is PrivacyInline.Text -> {
                    if (inline.bold) {
                        pushStyle(SpanStyle(fontWeight = FontWeight.Bold))
                        append(inline.value)
                        pop()
                    } else {
                        append(inline.value)
                    }
                }
                is PrivacyInline.Link -> append(inline.label)
                is PrivacyInline.Code -> {
                    pushStyle(SpanStyle(fontFamily = FontFamily.Monospace))
                    append(inline.value)
                    pop()
                }
            }
        }
    }
    Text(
        text = annotated,
        modifier = modifier,
        fontSize = 14.sp,
        color = MaterialTheme.colorScheme.onBackground,
    )
}
