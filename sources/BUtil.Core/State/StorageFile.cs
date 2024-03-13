namespace BUtil.Core.State;

public class StorageFile
{
    public FileState FileState { get; set; } = new();
    public string StorageRelativeFileName { get; set; } = string.Empty;
    public string StorageFileName { get; set; } = string.Empty;
    public long StorageFileNameSize { get; set; } = 0;
    public string StorageMethod { get; set; } = string.Empty;
    public string StorageIntegrityMethod { get; set; } = string.Empty;
    public string StorageIntegrityMethodInfo { get; set; } = string.Empty;
    public string StoragePassword { get; set; } = string.Empty;

    public StorageFile() { }

    public StorageFile(FileState fileState)
    {
        FileState = fileState;
    }

    internal void SetStoragePropertiesFrom(StorageFile matchingFile)
    {
        StorageRelativeFileName = matchingFile.StorageRelativeFileName;
        StorageFileNameSize = matchingFile.StorageFileNameSize;
        StorageFileName = matchingFile.StorageFileName;
        StorageMethod = matchingFile.StorageMethod;
        StorageIntegrityMethod = matchingFile.StorageIntegrityMethod;
        StorageIntegrityMethodInfo = matchingFile.StorageIntegrityMethodInfo;
        StoragePassword = matchingFile.StoragePassword;
    }
}
