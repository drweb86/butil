
        /* 
         * This code since 3-0-7 is excluded - no sense
		#region admin

		private static bool _hasAdminPrivileges = haveAdminPrivileges();

		public static bool HaveAdministratorPrivileges
		{
			get { return _hasAdminPrivileges; }
		}

		private static bool haveAdminPrivileges()
		{
			WindowsIdentity identity = WindowsIdentity.GetCurrent();
			WindowsPrincipal principal = new WindowsPrincipal(identity);

			return principal.IsInRole(WindowsBuiltInRole.Administrator);
		}

		#endregion
        */

