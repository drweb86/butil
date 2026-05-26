using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace BUtil.Windows.Services;

internal static class WindowsShellLink
{
    private static readonly Guid ShellLinkClsid = new("00021401-0000-0000-C000-000000000046");

    public static void Save(
        string shortcutPath,
        string targetPath,
        string arguments,
        string workingDirectory,
        string iconPath)
    {
#pragma warning disable CA1416 // WindowsShellLink is only used by the Windows platform assembly.
        var shellLink = (IShellLinkW)Activator.CreateInstance(Type.GetTypeFromCLSID(ShellLinkClsid)!)!;
#pragma warning restore CA1416
        shellLink.SetPath(targetPath);
        shellLink.SetArguments(arguments);
        shellLink.SetWorkingDirectory(workingDirectory);
        shellLink.SetIconLocation(iconPath, 0);

        ((IPersistFile)shellLink).Save(shortcutPath, true);
    }
}

[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("000214F9-0000-0000-C000-000000000046")]
internal interface IShellLinkW
{
    void GetPath(IntPtr pszFile, int cchMaxPath, IntPtr pfd, uint fFlags);
    void GetIDList(out IntPtr ppidl);
    void SetIDList(IntPtr pidl);
    void GetDescription(IntPtr pszName, int cchMaxName);
    void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
    void GetWorkingDirectory(IntPtr pszDir, int cchMaxPath);
    void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
    void GetArguments(IntPtr pszArgs, int cchMaxPath);
    void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
    void GetHotkey(out short pwHotkey);
    void SetHotkey(short wHotkey);
    void GetShowCmd(out int piShowCmd);
    void SetShowCmd(int iShowCmd);
    void GetIconLocation(IntPtr pszIconPath, int cchIconPath, out int piIcon);
    void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
    void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, uint dwReserved);
    void Resolve(IntPtr hwnd, uint fFlags);
    void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
}
