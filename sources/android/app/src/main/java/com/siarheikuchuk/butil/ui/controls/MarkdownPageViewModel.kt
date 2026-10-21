package com.siarheikuchuk.butil.ui.controls

import com.siarheikuchuk.butil.R

/**
 * Markdown pages, matching {@code BUtil.UI.Controls.MarkdownPageViewModel}.
 */
enum class MarkdownPage {
    Credits,
    ThirdPartyNotices;

    fun titleRes(): Int = when (this) {
        Credits -> R.string.menu_credits
        ThirdPartyNotices -> R.string.menu_third_party_notices
    }
}

class MarkdownPageViewModel(
    val page: MarkdownPage,
    val markdown: String,
)
