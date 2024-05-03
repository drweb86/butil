using System;

namespace BUtil.Core.Synchronization;

class SynchronizationStateFile
{
    public SynchronizationStateFile() { } // deserialization

    public SynchronizationStateFile(string relativeFileName, DateTime modifiedAtUtc, string sha512, long size)
    {
        RelativeFileName = relativeFileName;
        ModifiedAtUtc = modifiedAtUtc;
        Sha512 = sha512;
        Size = size;
    }

    public string RelativeFileName { get; set; } = null!;
    public DateTime ModifiedAtUtc { get; set; } = DateTime.MinValue;
    public string Sha512 { get; set; } = null!;
    public long Size { get; set; }

    public bool Equal(SynchronizationStateFile other, bool excludeModifiedAtUtc = true)
    {
        return other.RelativeFileName.Equals(RelativeFileName) &&
            other.Size.Equals(Size) &&
            (excludeModifiedAtUtc || (!excludeModifiedAtUtc && other.ModifiedAtUtc.Equals(ModifiedAtUtc))) &&
            other.Sha512.Equals(Sha512);
    }
}
