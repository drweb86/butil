using System;
using System.IO;
using System.Linq;

namespace BUtil.Core.FileSystem;

internal static class FileHelper
{
    public static void EnsureFolderCreatedForFile(string file)
    {
        var folder = Path.GetDirectoryName(file)!;
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
    }

    public static void EnsureFolderCreated(string folder)
    {
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
    }

    public static string Combine(string? subfolder, string relativeFileName)
    {
        return string.IsNullOrWhiteSpace(subfolder)
            ? relativeFileName
            : Path.Combine(subfolder, relativeFileName);
    }

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