using System.Security;
using System.Text.RegularExpressions;

namespace BUtil.Core.FileSystem;

internal static class LinuxFileHelper
{
    public static string? NormalizeNullablePath(string? path)
    {
        if (path == null)
            return null;
        return NormalizeNotNullablePath(path);
    }

    public static string NormalizeNotNullablePath(string path)
    {
        if (path.Contains(".."))
            throw new SecurityException("[..] is not allowed in path.");

        return path.Trim(['\\', '/']);
    }

    public static bool FitsMask(string fileName, string fileMask)
    {
        Regex mask = new(fileMask.Replace(".", "[.]").Replace("*", ".*").Replace("?", "."));
        return mask.IsMatch(fileName);
    }
}
