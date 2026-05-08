using BUtil.Core.Services;

namespace BUtil.Linux.Services;

class LinuxUiService : IUiService
{
    public bool CanExtendClientAreaToDecorationsHint => false;

    public void Blink()
    {
        // Not possible
    }
}
