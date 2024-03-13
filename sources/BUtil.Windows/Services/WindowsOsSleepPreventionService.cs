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
        private static extern uint SetThreadExecutionState(uint esFlags);
        private const uint _eS_CONTINUOUS = 0x80000000;
        private const uint _eS_SYSTEM_REQUIRED = 0x00000001;

        public static void PreventSleep()
        {
            SetThreadExecutionState(_eS_CONTINUOUS | _eS_SYSTEM_REQUIRED);
        }

        public static void StopPreventSleep()
        {
            SetThreadExecutionState(_eS_CONTINUOUS);
        }
    }
}
