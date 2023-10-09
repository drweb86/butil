using BUtil.Core.Services;
using System.Runtime.InteropServices;

namespace BUtil.Windows.Services
{
    internal class OsSleepPreventionService: IOsSleepPreventionService
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
            private const uint ES_CONTINUOUS = 0x80000000;
            private const uint ES_SYSTEM_REQUIRED = 0x00000001;

            public static void PreventSleep()
            {
                SetThreadExecutionState(ES_CONTINUOUS | ES_SYSTEM_REQUIRED);
            }

            public static void StopPreventSleep()
            {
                SetThreadExecutionState(ES_CONTINUOUS);
            }
        }
    }
}
