using BUtil.Core.FileSystem;
using BUtil.Core.Options;

namespace BUtil.Core.Legal;

public static class DocumentLanguageStore
{
    public const string SettingName = "DocumentLanguage";

    public static string? LanguageCode()
    {
        var code = new SettingsStoreService(new LocalFileSystem()).Load(SettingName, string.Empty).Trim();
        return string.IsNullOrEmpty(code) ? null : code;
    }

    public static void SetLanguageCode(string code)
    {
        new SettingsStoreService(new LocalFileSystem()).Save(SettingName, code);
    }
}
