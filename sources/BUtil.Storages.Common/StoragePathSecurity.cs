using System;
using System.IO;
using System.Security;

namespace BUtil.Core.Storages;

public static class StoragePathSecurity
{
    public static string ResolveRelativePathInsideRoot(string rootPath, string? relativePath, bool allowEmpty, string paramName)
    {
        var root = Path.GetFullPath(rootPath);
        if (string.IsNullOrWhiteSpace(relativePath))
        {
            if (allowEmpty)
                return root;
            throw new ArgumentException("Path cannot be empty.", paramName);
        }

        var combined = Path.GetFullPath(Path.Combine(root, relativePath));
        var comparison = OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        var normalizedRoot = root.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        var isInsideRoot = combined.Equals(normalizedRoot, comparison)
            || combined.StartsWith(normalizedRoot + Path.DirectorySeparatorChar, comparison)
            || combined.StartsWith(normalizedRoot + Path.AltDirectorySeparatorChar, comparison);

        if (!isInsideRoot)
            throw new SecurityException(paramName);

        return combined;
    }
}
