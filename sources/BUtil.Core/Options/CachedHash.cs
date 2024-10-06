using System;

namespace BUtil.Core.Options;

public class CachedHash
{
    public string File { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public string Sha512 { get; set; } = string.Empty;
    public DateTime LastWriteTimeUtc { get; set; }
    public long Size { get; set; }
}
