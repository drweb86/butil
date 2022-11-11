using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BUtil.Core.State
{
    public class FileState: IEqualityComparer<FileState>
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

        public bool Equals(FileState x, FileState y)
        {
            return x.FileName == y.FileName && 
                x.Size == y.Size && 
                x.Sha512 == y.Sha512 && 
                x.LastWriteTimeUtc == y.LastWriteTimeUtc;
        }

        public int GetHashCode([DisallowNull] FileState obj)
        {
            return obj.FileName.GetHashCode();
        }
    }
}
