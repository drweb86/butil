using System;
using System.Linq;

namespace BUtil.Core.FileSystem;

internal class FileHelper
{
    internal static string? NormalizeRelativePath(string? repositorySubfolder)
    {
        var parts = (repositorySubfolder ?? string.Empty)
            .Replace('\\', '/') // because of bug in .Net Core https://github.com/dotnet/runtime/issues/102841
            .Split('/', System.StringSplitOptions.RemoveEmptyEntries)
            .Where(x => x != ".")
            .Where(x => x != "..")
            .ToArray();

        if (parts.Length == 0)
        {
            return null;
        }

        return string.Join("/", parts);
    }

    internal static string TrimRelativePath(string relativeFileName, string relativePathToTrimNormalized, out bool isInvalid)
    {
        isInvalid = !relativeFileName.ToLowerInvariant().Replace('/', '\\').StartsWith(relativePathToTrimNormalized.ToLowerInvariant().Replace('/', '\\') + '\\');
        if (isInvalid)
        {
            return String.Empty;
        }
        return relativeFileName.Substring(relativePathToTrimNormalized.Length + 1);
    }
}