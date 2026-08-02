namespace BUtil.Tasks.ImportMedia;

public static class ImportMediaFileExtensions
{
    public static readonly string[] Default =
    [
        // Pictures
        ".jpg", ".jpeg", ".jpe", ".jfif",
        ".png", ".gif", ".bmp", ".webp",
        ".tif", ".tiff",
        ".heic", ".heif", ".avif",
        ".raw", ".dng", ".cr2", ".cr3", ".nef", ".arw", ".orf", ".rw2", ".pef", ".srw", ".raf",
        // Audio
        ".mp3", ".wav", ".flac", ".aac", ".m4a", ".ogg", ".oga", ".wma", ".aiff", ".aif", ".opus", ".amr", ".m4b",
    ];

    public static string FormatForEditor(IEnumerable<string>? extensions)
    {
        if (extensions == null || !extensions.Any())
            return string.Empty;

        return string.Join(", ", extensions.Select(Normalize).Where(x => x.Length > 0));
    }

    public static List<string> Parse(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return [];

        return text
            .Split(['\r', '\n', ',', ';', ' ', '\t'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(Normalize)
            .Where(x => x.Length > 1)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public static bool Matches(string fileName, IReadOnlyList<string>? extensions)
    {
        if (extensions == null || extensions.Count == 0)
            return true;

        var extension = Path.GetExtension(fileName);
        if (string.IsNullOrEmpty(extension))
            return false;

        return extensions.Any(x => string.Equals(x, extension, StringComparison.OrdinalIgnoreCase));
    }

    private static string Normalize(string extension)
    {
        var value = extension.Trim();
        if (value.Length == 0)
            return string.Empty;

        if (!value.StartsWith('.'))
            value = "." + value;

        return value.ToLowerInvariant();
    }
}
