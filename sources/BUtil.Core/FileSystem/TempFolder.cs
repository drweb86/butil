using System;
using System.IO;

namespace BUtil.Core.FileSystem
{
    public class TempFolder: IDisposable
    {
        public string Folder { get; private set; }

        public TempFolder()
        {
            Folder = Path.GetTempFileName() + ".Folder";
            Directory.CreateDirectory(Folder);
        }

        public void Dispose()
        {
            Directory.Delete(Folder, true);
        }
    }
}
