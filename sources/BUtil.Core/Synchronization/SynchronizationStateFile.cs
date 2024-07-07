using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Misc;
using BUtil.Core.State;
using System;

namespace BUtil.Core.Synchronization;

class SynchronizationStateFile
{
    public SynchronizationStateFile() { } // deserialization

    public SynchronizationStateFile(SourceItemV2 sourceItem, FileState fileState)
        : this(SourceItemHelper.GetSourceItemRelativeFileName(sourceItem.Target, fileState), fileState.LastWriteTimeUtc, fileState.Sha512, fileState.Size )
    {
    }

    public SynchronizationStateFile(string relativeFileName, DateTime modifiedAtUtc, string sha512, long size)
    {
        RelativeFileName = relativeFileName.Replace('/', '\\');
        ModifiedAtUtc = modifiedAtUtc;
        Sha512 = sha512;
        Size = size;
    }

    public string RelativeFileName { get; set; } = null!;
    public DateTime ModifiedAtUtc { get; set; } = DateTime.MinValue;
    public string Sha512 { get; set; } = null!;
    public long Size { get; set; }

    public bool Equal(SynchronizationStateFile? other, bool excludeModifiedAtUtc = true, bool excludeRelativeFileName = true)
    {
        if (other == null)
        {  
            return false; 
        }

        return (excludeRelativeFileName || (!excludeRelativeFileName && other.RelativeFileName.Equals(RelativeFileName))) &&
            other.Size.Equals(Size) &&
            (excludeModifiedAtUtc || (!excludeModifiedAtUtc && other.ModifiedAtUtc.Equals(ModifiedAtUtc))) &&
            other.Sha512.Equals(Sha512);
    }

    internal SynchronizationStateFile Clone()
    {
        return new SynchronizationStateFile(RelativeFileName, ModifiedAtUtc, Sha512, Size);
    }
}
