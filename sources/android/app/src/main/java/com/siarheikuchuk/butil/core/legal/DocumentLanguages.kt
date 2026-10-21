package com.siarheikuchuk.butil.core.legal

import java.util.Locale

/**
 * Same language list as {@code BUtil.Core.Legal.DocumentLanguages}.
 */
data class DocumentLanguage(
    val code: String,
    val assetFile: String,
    val nativeName: String,
    val rtl: Boolean = false,
)

object DocumentLanguages {
    val all: List<DocumentLanguage> = listOf(
        DocumentLanguage("en", "en.md", "English"),
        DocumentLanguage("am", "am.md", "አማርኛ"),
        DocumentLanguage("ar", "ar.md", "العربية", rtl = true),
        DocumentLanguage("az", "az.md", "Azərbaycan"),
        DocumentLanguage("bn", "bn.md", "বাংলা"),
        DocumentLanguage("my", "my.md", "မြန်မာ"),
        DocumentLanguage("ceb", "ceb.md", "Cebuano"),
        DocumentLanguage("zh-CN", "zh-CN.md", "简体中文"),
        DocumentLanguage("zh-HK", "zh-HK.md", "粵語"),
        DocumentLanguage("cs", "cs.md", "Čeština"),
        DocumentLanguage("nl", "nl.md", "Nederlands"),
        DocumentLanguage("tl", "tl.md", "Filipino"),
        DocumentLanguage("fr", "fr.md", "Français"),
        DocumentLanguage("de", "de.md", "Deutsch"),
        DocumentLanguage("el", "el.md", "Ελληνικά"),
        DocumentLanguage("gu", "gu.md", "ગુજરાતી"),
        DocumentLanguage("ha", "ha.md", "Hausa"),
        DocumentLanguage("he", "he.md", "עברית", rtl = true),
        DocumentLanguage("hi", "hi.md", "हिन्दी"),
        DocumentLanguage("hu", "hu.md", "Magyar"),
        DocumentLanguage("ig", "ig.md", "Igbo"),
        DocumentLanguage("id", "id.md", "Bahasa Indonesia"),
        DocumentLanguage("it", "it.md", "Italiano"),
        DocumentLanguage("ja", "ja.md", "日本語"),
        DocumentLanguage("kn", "kn.md", "ಕನ್ನಡ"),
        DocumentLanguage("kk", "kk.md", "Қазақша"),
        DocumentLanguage("km", "km.md", "ខ្មែរ"),
        DocumentLanguage("ko", "ko.md", "한국어"),
        DocumentLanguage("ms", "ms.md", "Bahasa Melayu"),
        DocumentLanguage("mr", "mr.md", "मराठी"),
        DocumentLanguage("ne", "ne.md", "नेपाली"),
        DocumentLanguage("pcm-NG", "pcm-NG.md", "Nigerian Pidgin"),
        DocumentLanguage("or", "or.md", "ଓଡ଼ିଆ"),
        DocumentLanguage("ps", "ps.md", "پښتو", rtl = true),
        DocumentLanguage("fa", "fa.md", "فارسی", rtl = true),
        DocumentLanguage("pl", "pl.md", "Polski"),
        DocumentLanguage("pt-BR", "pt-BR.md", "Português (Brasil)"),
        DocumentLanguage("pa", "pa.md", "ਪੰਜਾਬੀ"),
        DocumentLanguage("ro", "ro.md", "Română"),
        DocumentLanguage("ru", "ru.md", "Русский"),
        DocumentLanguage("sr", "sr.md", "Српски"),
        DocumentLanguage("sd", "sd.md", "سنڌي", rtl = true),
        DocumentLanguage("si", "si.md", "සිංහල"),
        DocumentLanguage("so", "so.md", "Soomaali"),
        DocumentLanguage("es", "es.md", "Español"),
        DocumentLanguage("sw", "sw.md", "Kiswahili"),
        DocumentLanguage("sv", "sv.md", "Svenska"),
        DocumentLanguage("ta", "ta.md", "தமிழ்"),
        DocumentLanguage("te", "te.md", "తెలుగు"),
        DocumentLanguage("th", "th.md", "ไทย"),
        DocumentLanguage("tr", "tr.md", "Türkçe"),
        DocumentLanguage("uk", "uk.md", "Українська"),
        DocumentLanguage("ur", "ur.md", "اردو", rtl = true),
        DocumentLanguage("uz", "uz.md", "Oʻzbekcha"),
        DocumentLanguage("vi", "vi.md", "Tiếng Việt"),
        DocumentLanguage("yo", "yo.md", "Yorùbá"),
        DocumentLanguage("zu", "zu.md", "isiZulu"),
    )

    fun byCode(code: String): DocumentLanguage =
        all.firstOrNull { it.code.equals(code, ignoreCase = true) } ?: all.first { it.code == "en" }

    fun matchDevice(locale: Locale = Locale.getDefault()): DocumentLanguage {
        val lang = locale.language.lowercase(Locale.ROOT)
        val country = locale.country.uppercase(Locale.ROOT)
        val script = locale.script
        val tag = locale.toLanguageTag()
        when {
            lang == "zh" && (
                script.equals("Hant", ignoreCase = true) ||
                    country in setOf("HK", "MO", "TW") ||
                    tag.contains("yue", ignoreCase = true)
                ) -> return byCode("zh-HK")
            lang == "zh" -> return byCode("zh-CN")
            lang == "pt" -> return byCode("pt-BR")
            lang == "in" || lang == "id" -> return byCode("id")
            lang == "fil" || lang == "tl" -> return byCode("tl")
            lang == "pcm" || tag.equals("pcm-NG", ignoreCase = true) -> return byCode("pcm-NG")
            lang == "iw" || lang == "he" -> return byCode("he")
        }
        return all.firstOrNull {
            it.code.equals(tag, ignoreCase = true) || it.code.equals(lang, ignoreCase = true)
        } ?: byCode("en")
    }
}
