using System;
using System.IO;

namespace BUtil.Core.Legal;

public static class CreditDocuments
{
    public const string ResourceName = "BUtil.Core.Credits.md";

    public static string Load()
    {
        var assembly = typeof(CreditDocuments).Assembly;
        using var stream = assembly.GetManifestResourceStream(ResourceName)
            ?? throw new InvalidOperationException("Missing embedded credits.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
