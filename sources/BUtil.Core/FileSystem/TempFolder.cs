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

    public void Dispose()
    {
        Directory.Delete(Folder, true);
    }
}
