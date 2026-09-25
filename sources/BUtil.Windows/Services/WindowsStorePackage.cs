using System.Runtime.InteropServices;
using System.Text;

namespace BUtil.Windows.Services;

public static class WindowsStorePackage
{
    private const int AppModelErrorNoPackage = 15700;

    public static bool IsCurrentProcessPackaged { get; } = Detect();

    private static bool Detect()
    {
        try
        {
            var length = 0;
            var result = GetCurrentPackageFullName(ref length, null);
            return result != AppModelErrorNoPackage;
        }
        catch
        {
            return false;
        }
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
    private static extern int GetCurrentPackageFullName(
        ref int packageFullNameLength,
        StringBuilder? packageFullName);
}
