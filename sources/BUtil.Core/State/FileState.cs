using System;

namespace BUtil.Core.State
{
    public class FileState
    {
        public string FileName { get; set; }
        public DateTime LastWriteTimeUtc { get; set; }
        public long Size { get; set; }
        public string Sha512 { get; set; }

        public FileState() { } // Deserialization

        public FileState(string fileName, DateTime lastWriteTimeUtc, long size, string sha512)
        {
            FileName = fileName;
            LastWriteTimeUtc = lastWriteTimeUtc;
            Size = size;
            Sha512 = sha512;
        }

        public bool CompareTo(FileState x)
        {
            return x.FileName == FileName && 
                x.Size == Size && 
                x.Sha512 == Sha512 && 
                x.LastWriteTimeUtc == LastWriteTimeUtc;
        }
    }
}
