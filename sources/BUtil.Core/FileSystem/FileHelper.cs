using System;
using System.Collections.Generic;
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

    public static string Combine(params string?[] paths)
    {
        return Combine(null, paths);
    }

    public static string Combine(char? separator, params string?[] paths)
    {
        var actualPaths = (paths ?? [])
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x => x!)
            .ToList();

        if (actualPaths.Count == 0)
            throw new ArgumentException("No paths provided");

        var actualSeparator = separator ?? System.IO.Path.DirectorySeparatorChar;

        var parts = new List<string>();

        foreach (var path in actualPaths)
        {
            // Split the input by directory separator and add to parts list
            var segments = path.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
            parts.AddRange(segments);
        }

        // Combine the parts into a single path
        var combinedPath = string.Join(actualSeparator.ToString(), parts);

        // Prepend the leading slash if the first path had it
        if (actualPaths[0][0] == '\\' || actualPaths[0][0] == '/')
        {
            combinedPath = actualSeparator + combinedPath;
        }
        if (actualPaths[0].Length > 1)
        {
            if (actualPaths[0][1] == '\\' || actualPaths[0][1] == '/')
            {
                combinedPath = actualSeparator + combinedPath;
            }
        }

        return combinedPath;
    }

    public static bool CompareFileNames(string name1, string name2)
    {
        return name1.Replace('\\', '/').Cmp(name2.Replace('\\', '/'));
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
        return relativeFileName[(relativePathToTrimNormalized.Length + 1)..];
    }

    internal static string GetRelativeFileName(string folder, string file)
    {
        folder = folder
            .TrimEnd('\\')
            .TrimEnd('/');

        return file.Substring(folder.Length + 1);
    }
}