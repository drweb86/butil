using System;
using System.IO;

namespace BUtil.Core.Legal;

public static class PrivacyDocuments
{
    public static string LoadMarkdown(string assetFile)
    {
        var name = "BUtil.Core.Privacy." + assetFile;
        var assembly = typeof(PrivacyDocuments).Assembly;
        using var stream = assembly.GetManifestResourceStream(name);
        if (stream == null)
        {
            if (!string.Equals(assetFile, "en.md", StringComparison.OrdinalIgnoreCase))
                return LoadMarkdown("en.md");
            return string.Empty;
        }

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
