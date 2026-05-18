using BUtil.Core.Storages;

namespace BUtil.Storages.WebDav;

public static class WebDavStoragePlugin
{
    public static void Register()
    {
        StorageProviderRegistry.Register(
            "WebDav",
            "WebDAV",
            new WebDavStorageSettingsProvider(),
            typeof(WebDavStorageSettingsV2),
            (log, s, _) => new WebDavStorage(log, (WebDavStorageSettingsV2)s));
    }
}
