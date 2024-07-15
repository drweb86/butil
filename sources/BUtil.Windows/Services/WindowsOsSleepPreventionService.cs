using BUtil.Core.Services;
using System.Runtime.InteropServices;

namespace BUtil.Windows.Services;

internal class WindowsOsSleepPreventionService : IOsSleepPreventionService
{
    public void PreventSleep()
    {
        NativeMethods.PreventSleep();
    }

    public void StopPreventSleep()
    {
        NativeMethods.StopPreventSleep();
    }

    static class NativeMethods
    {
        [DllImport("kernel32.dll")]
#pragma warning disable SYSLIB1054 // Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time
        private static extern uint SetThreadExecutionState(uint esFlags);
#pragma warning restore SYSLIB1054 // Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time
        private const uint _eS_CONTINUOUS = 0x80000000;
        private const uint _eS_SYSTEM_REQUIRED = 0x00000001;

        public static void PreventSleep()
        {
#pragma warning disable CA1806 // Do not ignore method results
            SetThreadExecutionState(_eS_CONTINUOUS | _eS_SYSTEM_REQUIRED);
#pragma warning restore CA1806 // Do not ignore method results
        }

        public static void StopPreventSleep()
        {
#pragma warning disable CA1806 // Do not ignore method results
            SetThreadExecutionState(_eS_CONTINUOUS);
#pragma warning restore CA1806 // Do not ignore method results
        }
    }
}
