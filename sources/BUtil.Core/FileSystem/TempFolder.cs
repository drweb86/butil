using System;
using System.IO;

namespace BUtil.Core.FileSystem;

public class TempFolder : IDisposable
{
    public string Folder { get; private set; }

    public TempFolder(string? baseFolder = null)
    {
        if (baseFolder == null)
        {
            Folder = Path.GetTempFileName() + ".Folder";
        }
        else
        {
             Folder = Path.Combine(baseFolder, Guid.NewGuid().ToString());
        }
        Directory.CreateDirectory(Folder);
    }

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
    public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
    {
        Directory.Delete(Folder, true);
    }
}
