package com.siarheikuchuk.butil.core.legal

/**
 * The markdown subset used by the desktop app ({@code BUtil.Core.Legal.PrivacyMarkdown}).
 */
object PrivacyMarkdown {
    private val inlinePattern = Regex(
        """\*\*(.+?)\*\*|\[([^\]]+)\]\(([^)]+)\)|`([^`]+)`""",
        RegexOption.DOT_MATCHES_ALL,
    )

    fun parse(src: String): List<PrivacyMdBlock> {
        val blocks = mutableListOf<PrivacyMdBlock>()
        val paragraph = StringBuilder()

        fun flushParagraph() {
            val text = paragraph.toString().trim()
            paragraph.clear()
            if (text.isNotEmpty()) {
                blocks.add(PrivacyMdBlock.Paragraph(parseInlines(text)))
            }
        }

        for (raw in src.replace("\r\n", "\n").split('\n')) {
            val trimmed = raw.trim()
            if (trimmed.isEmpty() || trimmed == "[Languages](README.md)") {
                flushParagraph()
                continue
            }
            if (trimmed.startsWith("### ")) {
                flushParagraph()
                blocks.add(PrivacyMdBlock.Heading(trimmed.substring(4).trim(), 3))
            } else if (trimmed.startsWith("## ")) {
                flushParagraph()
                blocks.add(PrivacyMdBlock.Heading(trimmed.substring(3).trim(), 2))
            } else if (trimmed.startsWith("# ")) {
                flushParagraph()
                blocks.add(PrivacyMdBlock.Heading(trimmed.substring(2).trim(), 1))
            } else if (trimmed.startsWith("- ") || trimmed.startsWith("* ")) {
                flushParagraph()
                blocks.add(PrivacyMdBlock.Bullet(parseInlines(trimmed.substring(2).trim())))
            } else {
                if (paragraph.isNotEmpty()) {
                    paragraph.append(' ')
                }
                paragraph.append(trimmed)
            }
        }
        flushParagraph()
        return blocks
    }

    fun parseInlines(text: String): List<PrivacyInline> {
        val result = mutableListOf<PrivacyInline>()
        var index = 0
        for (match in inlinePattern.findAll(text)) {
            if (match.range.first > index) {
                result.add(PrivacyInline.Text(text.substring(index, match.range.first)))
            }
            val bold = match.groups[1]
            val label = match.groups[2]
            val url = match.groups[3]
            val code = match.groups[4]
            if (bold != null) {
                result.add(PrivacyInline.Text(bold.value, bold = true))
            } else if (label != null && url != null) {
                result.add(PrivacyInline.Link(label.value, url.value))
            } else if (code != null) {
                val value = code.value
                if (value.startsWith("http://", ignoreCase = true) || value.startsWith("https://", ignoreCase = true)) {
                    result.add(PrivacyInline.Link(value, value))
                } else {
                    result.add(PrivacyInline.Code(value))
                }
            }
            index = match.range.last + 1
        }
        if (index < text.length) {
            result.add(PrivacyInline.Text(text.substring(index)))
        }
        return result
    }
}

sealed interface PrivacyMdBlock {
    data class Heading(val text: String, val level: Int) : PrivacyMdBlock

    data class Paragraph(val inlines: List<PrivacyInline>) : PrivacyMdBlock

    data class Bullet(val inlines: List<PrivacyInline>) : PrivacyMdBlock
}

sealed interface PrivacyInline {
    data class Text(val value: String, val bold: Boolean = false) : PrivacyInline

    data class Link(val label: String, val url: String) : PrivacyInline

    data class Code(val value: String) : PrivacyInline
}
