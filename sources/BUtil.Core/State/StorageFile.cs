using System;

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
}
