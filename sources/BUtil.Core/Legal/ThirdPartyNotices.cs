using System;
using System.IO;

namespace BUtil.Core.Legal;

public static class ThirdPartyNotices
{
    public const string ResourceName = "BUtil.Core.ThirdPartyNotices.md";

    public static string Load()
    {
        var assembly = typeof(ThirdPartyNotices).Assembly;
        using var stream = assembly.GetManifestResourceStream(ResourceName)
            ?? throw new InvalidOperationException("Missing embedded third-party notices.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
