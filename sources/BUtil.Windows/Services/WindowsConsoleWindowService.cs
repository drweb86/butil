using System.Runtime.InteropServices;
using BUtil.Core.Services;

namespace BUtil.Windows.Services;

/// <summary>
/// Hides the console window on Windows so it does not steal focus when run by Task Scheduler.
/// </summary>
class WindowsConsoleWindowService : IConsoleWindowService
{
    public void Hide()
    {
        try
        {
            var hwnd = GetConsoleWindow();
            if (hwnd != IntPtr.Zero)
                ShowWindow(hwnd, SW_HIDE);
        }
        catch
        {
            // Ignore: we still want the app to run if something goes wrong
        }
    }

    private const int SW_HIDE = 0;

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
}
