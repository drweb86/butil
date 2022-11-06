namespace BUtil.Core.State
{
    public class StorageFile
    {
        public FileState FileState { get; set; }
        public string StorageRelativeFileName { get; set; }
        public string StorageFileName { get; set; }
        public string StorageMethod { get; set; } // TBD: Plain, XDelta, 7z
        public string StorageIntegrityMethod { get; set; }
        public string StorageIntegrityMethodInfo { get; set; }

        public StorageFile() { }

        public StorageFile(
            FileState fileState,
            string storageRelativeFileName,
            string storageFileName, 
            string storageMethod, 
            string storageIntegriyMethod,
            string storageIntegrityMethodInfo)
        {
            FileState = fileState;
            StorageRelativeFileName = storageRelativeFileName;
            StorageFileName = storageFileName;
            StorageMethod = storageMethod;
            StorageIntegrityMethod = storageIntegrityMethodInfo;
            StorageIntegrityMethodInfo = storageIntegriyMethod;
        }

        public StorageFile(FileState fileState)
        {
            FileState = fileState;
        }
    }
}
