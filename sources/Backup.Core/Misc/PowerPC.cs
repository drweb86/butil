using System;
using System.Windows.Forms;
using System.Management;
using System.Runtime.InteropServices;

namespace BUtil.Core.Misc
{
	/// <summary>
	/// Manages power
	/// </summary>
	public static class PowerPC
	{
		/// <summary>
		/// Performs the specified task
		/// </summary>
		/// <param name="task">task to perform</param>
		/// <returns>true - if OK</returns>
		public static bool DoTask(PowerTask task)
        {
            switch (task)
            {
                case PowerTask.Hibernate: return hibernate();
				case PowerTask.LogOff: return logOff(); 
				case PowerTask.Shutdown: return shutdown();
				case PowerTask.Suspend: return suspend();
				case PowerTask.Reboot: return reboot();
                case PowerTask.None: return true;
                default: 
                	throw new NotImplementedException(task.ToString());
            }
		}

		#region Native Win 32 methods. Thank's to the nice work off Hemanth Reddy http://www.c-sharpcorner.com/Media/
		
        enum ExitFlags
        {
            Logoff = 0,
            Shutdown = 1,
            Reboot = 2,
            Force = 4,
            PowerOff = 8,
            ForceIfHung = 16
        }
        enum Reason : uint
        {
            ApplicationIssue = 0x00040000,
            HardwareIssue = 0x00010000,
            SoftwareIssue = 0x00030000,
            PlannedShutdown = 0x80000000
        }
        
        const int PrivilegeEnabled = 0x00000002;
        const int TokenQuery = 0x00000008;
        const int AdjustPrivileges = 0x00000020;
        const string ShutdownPrivilege = "SeShutdownPrivilege";

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct TokenPrivileges
        {
            public int PrivilegeCount;
            public long Luid;
            public int Attributes;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern int ExitWindowsEx(uint uFlags, uint dwReason);
        
        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern int OpenProcessToken(
            IntPtr processHandle,
            int desiredAccess,
            ref IntPtr tokenHandle);

        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern int LookupPrivilegeValue(
            string systemName, string name, ref long luid);

        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern int AdjustTokenPrivileges(
            IntPtr tokenHandle, bool disableAllPrivileges,
            ref TokenPrivileges newState,
            int bufferLength,
            IntPtr previousState,
            IntPtr length);
        
        private static void ElevatePrivileges()
        {
            IntPtr currentProcess = GetCurrentProcess();
            IntPtr tokenHandle = IntPtr.Zero;

            int result = OpenProcessToken(
                currentProcess,
                AdjustPrivileges | TokenQuery,
                ref tokenHandle);

            if (result == 0)
                throw new InvalidOperationException(string.Empty+Marshal.GetLastWin32Error());

            TokenPrivileges tokenPrivileges;
            tokenPrivileges.PrivilegeCount = 1;
            tokenPrivileges.Luid = 0;
            tokenPrivileges.Attributes = PrivilegeEnabled;

            result = LookupPrivilegeValue(
                null,
                ShutdownPrivilege,
                ref tokenPrivileges.Luid);

            if (result == 0)
                throw new InvalidOperationException(string.Empty+Marshal.GetLastWin32Error());

            result = AdjustTokenPrivileges(
                tokenHandle,
                false,
                ref tokenPrivileges,
                0, IntPtr.Zero,
                IntPtr.Zero);

            if (result == 0)
                throw new InvalidOperationException(string.Empty+Marshal.GetLastWin32Error());
        }

		#endregion
		
		#region Power operations
		
		private static bool reboot()
		{
			ElevatePrivileges();

            int result = ExitWindowsEx(
                (uint)(ExitFlags.Reboot),
                (uint)(Reason.SoftwareIssue | Reason.PlannedShutdown));

            return (result != 0);
		}

		private static bool shutdown()
		{
		    ElevatePrivileges();

            int result = ExitWindowsEx(
                (uint)(ExitFlags.Shutdown | ExitFlags.PowerOff | ExitFlags.Force),
                (uint)(Reason.HardwareIssue | Reason.PlannedShutdown));

            return (result != 0);
		}

		private static bool logOff()
		{
			ManagementBaseObject mboShutdown = null;
            ManagementClass mcWin32 = new ManagementClass("Win32_OperatingSystem");
            mcWin32.Get();
            mcWin32.Scope.Options.EnablePrivileges = true;
            ManagementBaseObject mboShutdownParams = mcWin32.GetMethodParameters("Win32Shutdown");
            mboShutdownParams["Flags"] = "0";
            mboShutdownParams["Reserved"] = "0";
            foreach (ManagementObject manObj in mcWin32.GetInstances())
            {
                mboShutdown = manObj.InvokeMethod("Win32Shutdown", mboShutdownParams, null);
            }
            
            return true;
		}

		private static bool suspend()
		{
			return Application.SetSuspendState(PowerState.Suspend, true, true);
		}

		private static bool hibernate()
		{
			return Application.SetSuspendState(PowerState.Hibernate, true, true);
		}

		#endregion
	}
}
