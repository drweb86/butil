using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BUtil.Core.Legal;

public static class DocumentLanguages
{
    public static IReadOnlyList<DocumentLanguage> License { get; } =
    [
        new("en", "en.md", "English"),
        new("am", "am.md", "አማርኛ"),
        new("ar", "ar.md", "العربية", Rtl: true),
        new("az", "az.md", "Azərbaycan"),
        new("bn", "bn.md", "বাংলা"),
        new("my", "my.md", "မြန်မာ"),
        new("ceb", "ceb.md", "Cebuano"),
        new("zh-CN", "zh-CN.md", "简体中文"),
        new("zh-HK", "zh-HK.md", "粵語"),
        new("cs", "cs.md", "Čeština"),
        new("nl", "nl.md", "Nederlands"),
        new("tl", "tl.md", "Filipino"),
        new("fr", "fr.md", "Français"),
        new("de", "de.md", "Deutsch"),
        new("el", "el.md", "Ελληνικά"),
        new("gu", "gu.md", "ગુજરાતી"),
        new("ha", "ha.md", "Hausa"),
        new("he", "he.md", "עברית", Rtl: true),
        new("hi", "hi.md", "हिन्दी"),
        new("hu", "hu.md", "Magyar"),
        new("ig", "ig.md", "Igbo"),
        new("id", "id.md", "Bahasa Indonesia"),
        new("it", "it.md", "Italiano"),
        new("ja", "ja.md", "日本語"),
        new("kn", "kn.md", "ಕನ್ನಡ"),
        new("kk", "kk.md", "Қазақша"),
        new("km", "km.md", "ខ្មែរ"),
        new("ko", "ko.md", "한국어"),
        new("ms", "ms.md", "Bahasa Melayu"),
        new("mr", "mr.md", "मराठी"),
        new("ne", "ne.md", "नेपाली"),
        new("pcm-NG", "pcm-NG.md", "Nigerian Pidgin"),
        new("or", "or.md", "ଓଡ଼ିଆ"),
        new("ps", "ps.md", "پښتو", Rtl: true),
        new("fa", "fa.md", "فارسی", Rtl: true),
        new("pl", "pl.md", "Polski"),
        new("pt-BR", "pt-BR.md", "Português (Brasil)"),
        new("pa", "pa.md", "ਪੰਜਾਬੀ"),
        new("ro", "ro.md", "Română"),
        new("ru", "ru.md", "Русский"),
        new("sr", "sr.md", "Српски"),
        new("sd", "sd.md", "سنڌي", Rtl: true),
        new("si", "si.md", "සිංහල"),
        new("so", "so.md", "Soomaali"),
        new("es", "es.md", "Español"),
        new("sw", "sw.md", "Kiswahili"),
        new("sv", "sv.md", "Svenska"),
        new("ta", "ta.md", "தமிழ்"),
        new("te", "te.md", "తెలుగు"),
        new("th", "th.md", "ไทย"),
        new("tr", "tr.md", "Türkçe"),
        new("uk", "uk.md", "Українська"),
        new("ur", "ur.md", "اردو", Rtl: true),
        new("uz", "uz.md", "Oʻzbekcha"),
        new("vi", "vi.md", "Tiếng Việt"),
        new("yo", "yo.md", "Yorùbá"),
        new("zu", "zu.md", "isiZulu"),
    ];

    public static IReadOnlyList<DocumentLanguage> Privacy => License;

    public static DocumentLanguage ByCode(IReadOnlyList<DocumentLanguage> languages, string? code) =>
        languages.FirstOrDefault(l => string.Equals(l.Code, code, StringComparison.OrdinalIgnoreCase))
        ?? languages.First(l => l.Code == "en");

    public static DocumentLanguage MatchDevice(
        IReadOnlyList<DocumentLanguage> languages,
        CultureInfo? culture = null)
    {
        culture ??= CultureInfo.CurrentUICulture;
        var lang = culture.TwoLetterISOLanguageName.ToLowerInvariant();
        var name = culture.Name;

        if (lang == "zh" && (name.Contains("Hant", StringComparison.OrdinalIgnoreCase)
            || name.EndsWith("-HK", StringComparison.OrdinalIgnoreCase)
            || name.EndsWith("-MO", StringComparison.OrdinalIgnoreCase)
            || name.EndsWith("-TW", StringComparison.OrdinalIgnoreCase)
            || name.Contains("yue", StringComparison.OrdinalIgnoreCase)))
            return ByCode(languages, "zh-HK");
        if (lang == "zh")
            return ByCode(languages, "zh-CN");
        if (lang == "pt")
            return ByCode(languages, "pt-BR");
        if (lang is "in" or "id")
            return ByCode(languages, "id");
        if (lang is "fil" or "tl")
            return ByCode(languages, "tl");
        if (string.Equals(name, "pcm-NG", StringComparison.OrdinalIgnoreCase) || lang == "pcm")
            return ByCode(languages, "pcm-NG");

        return languages.FirstOrDefault(l =>
                   l.Code.Equals(name, StringComparison.OrdinalIgnoreCase)
                   || l.Code.Equals(lang, StringComparison.OrdinalIgnoreCase))
               ?? ByCode(languages, "en");
    }
}
