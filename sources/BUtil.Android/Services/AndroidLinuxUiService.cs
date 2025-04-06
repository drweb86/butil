using BUtil.Core.Services;

namespace BUtil.Windows.Services;

class AndroidLinuxUiService : IUiService
{
    public bool CanExtendClientAreaToDecorationsHint => false;

    public void Blink()
    {
        // Not possible
    }
}
