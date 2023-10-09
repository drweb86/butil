using BUtil.Core.Services;
using Microsoft.Win32;

namespace BUtil.Windows.Services
{
    internal class ShowLogOnSystemLoginService : IShowLogOnSystemLoginService
    {
        void IShowLogOnSystemLoginService.ShowLogOnSystemLoginService(string fileName)
        {
            var currentVersionKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion", true);
            if (currentVersionKey == null)
                return;

            var runOnceKey = currentVersionKey.OpenSubKey("RunOnce", true);
            if (runOnceKey == null)
            {
                currentVersionKey.CreateSubKey("RunOnce");
                runOnceKey = currentVersionKey.OpenSubKey("RunOnce", true);
            }

            if (runOnceKey == null)
                return;

            runOnceKey.SetValue("BUtil Backup Report", "\"" + fileName + "\"");
        }
    }
}
