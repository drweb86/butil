using BUtil.Core.Services;

namespace BUtil.Windows.Services
{
    class LinuxUiService : IUiService
    {
        public bool CanExtendClientAreaToDecorationsHint => false;

        public void Blink()
        {
            // Not possible
        }
    }
}
