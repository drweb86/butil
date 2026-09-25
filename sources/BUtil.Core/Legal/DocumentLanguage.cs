namespace BUtil.Core.Legal;

public sealed record DocumentLanguage(
    string Code,
    string AssetFile,
    string NativeName,
    bool Rtl = false);
