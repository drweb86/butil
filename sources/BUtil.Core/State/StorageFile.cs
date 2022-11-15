namespace BUtil.Core.State
{
    public class StorageFile
    {
        public FileState FileState { get; set; }
        public string StorageRelativeFileName { get; set; }
        public string StorageFileName { get; set; }
        public long StorageFileNameSize { get; set; }
        public string StorageMethod { get; set; } // TBD: Plain, XDelta, 7z
        public string StorageIntegrityMethod { get; set; }
        public string StorageIntegrityMethodInfo { get; set; }
        public string StoragePassword { get; set; }

        public StorageFile() { }

        public StorageFile(
            FileState fileState,
            string storageRelativeFileName,
            string storageFileName,
            long storageFileNameSize,
            string storageMethod, 
            string storageIntegriyMethod,
            string storageIntegrityMethodInfo,
            string storagePassword)
        {
            FileState = fileState;
            StorageRelativeFileName = storageRelativeFileName;
            StorageFileNameSize = storageFileNameSize;
            StorageFileName = storageFileName;
            StorageMethod = storageMethod;
            StorageIntegrityMethod = storageIntegrityMethodInfo;
            StorageIntegrityMethodInfo = storageIntegriyMethod;
            StoragePassword = storagePassword;
        }

        public StorageFile(FileState fileState)
        {
            FileState = fileState;
        }
    }
}
