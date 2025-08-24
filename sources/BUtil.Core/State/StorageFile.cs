using System;

namespace BUtil.Core.State;

public class StorageFile
{
    public FileState FileState { get; set; } = new();
    public string StorageRelativeFileName { get; set; } = string.Empty;
    [Obsolete("Backward compatibility up to 2025-12-30. Use StorageRelativeFileName instead")]
    public string StorageFileName { get; set; } = string.Empty;
    public long StorageFileNameSize { get; set; } = 0;
    public string StorageMethod { get; set; } = string.Empty;
    public string StorageIntegrityMethod { get; set; } = string.Empty;
    public string StorageIntegrityMethodInfo { get; set; } = string.Empty;
    public string StoragePassword { get; set; } = string.Empty;

    public StorageFile() { }

    public StorageFile(StorageFile storageFile) 
    {
        FileState = new FileState(storageFile.FileState);
        StorageRelativeFileName = storageFile.StorageRelativeFileName;
#pragma warning disable CS0618 // Type or member is obsolete
        StorageFileName  = storageFile.StorageFileName;
#pragma warning restore CS0618 // Type or member is obsolete
        StorageFileNameSize  = storageFile.StorageFileNameSize;
        StorageMethod  = storageFile.StorageMethod;
        StorageIntegrityMethod  = storageFile.StorageIntegrityMethod;
        StorageIntegrityMethodInfo  = storageFile.StorageIntegrityMethodInfo;
        StoragePassword  = storageFile.StoragePassword;
    }

    public StorageFile(FileState fileState)
    {
        FileState = fileState;
    }

    public StorageFile(FileState fileState, string storageMethod, string storageRelativeFileName)
    {
        FileState = fileState;
        StorageMethod = storageMethod;
        StorageRelativeFileName = storageRelativeFileName;
    }

    internal void SetStoragePropertiesFrom(StorageFile matchingFile)
    {
        StorageRelativeFileName = matchingFile.StorageRelativeFileName;
        StorageFileNameSize = matchingFile.StorageFileNameSize;
#pragma warning disable CS0618 // Type or member is obsolete
        StorageFileName = matchingFile.StorageFileName;
#pragma warning restore CS0618 // Type or member is obsolete
        StorageMethod = matchingFile.StorageMethod;
        StorageIntegrityMethod = matchingFile.StorageIntegrityMethod;
        StorageIntegrityMethodInfo = matchingFile.StorageIntegrityMethodInfo;
        StoragePassword = matchingFile.StoragePassword;
    }
}
