using BUtil.Core.Localization;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Tasks.ImportMedia;

public static class ImportMediaSourceFolderGuard
{
    /// <summary>
    /// Well-known top-level media folders on Android, iOS, Windows, Linux, and camera cards.
    /// Seeing several of these as siblings usually means the user pointed at a device/root folder.
    /// </summary>
    private static readonly HashSet<string> KnownMediaRootFolders = new(StringComparer.OrdinalIgnoreCase)
    {
        // Camera / shared
        "DCIM",
        "MISC",
        "PRIVATE",
        "MP_ROOT",
        // Android
        "Pictures",
        "Download",
        "Downloads",
        "Movies",
        "Music",
        "Alarms",
        "Notifications",
        "Podcasts",
        "Ringtones",
        "Android",
        "Documents",
        // iOS
        "Photos",
        "PhotoData",
        "Photo Library",
        // Windows / Linux home-style
        "Camera Roll",
        "Videos",
        "Desktop",
        "Documents",
        "Pictures",
        "Music",
        "Videos",
        "Downloads",
    };

    public static string? TryGetTooBroadFolderError(IEnumerable<string> relativeFolderNames)
    {
        var matches = relativeFolderNames
            .Select(GetLeafName)
            .Where(name => !string.IsNullOrWhiteSpace(name) && KnownMediaRootFolders.Contains(name))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (matches.Count < 2)
            return null;

        var suggestion = matches.Contains("DCIM", StringComparer.OrdinalIgnoreCase)
            ? "DCIM"
            : matches[0];

        return string.Format(
            Resources.ImportMediaTask_Storage_Validation_TooBroad,
            string.Join(", ", matches),
            suggestion);
    }

    private static string GetLeafName(string relativeFolderName)
    {
        var trimmed = relativeFolderName.Trim().TrimEnd('/', '\\');
        if (string.IsNullOrEmpty(trimmed))
            return string.Empty;

        var slash = trimmed.LastIndexOfAny(['/', '\\']);
        return slash >= 0 ? trimmed[(slash + 1)..] : trimmed;
    }
}
