using System;
using System.IO;
using System.Reflection;

namespace BUtil.Core.Storages;

public class StorageHelper
{
    public static void WriteTest(IStorage storage)
    {
        var folder = "BUtil check " + Guid.NewGuid().ToString();
        var file = Path.Combine(folder, Guid.NewGuid().ToString());
        storage.Upload(Assembly.GetExecutingAssembly().Location, file);
        storage.DeleteFolder(folder);
    }
}
