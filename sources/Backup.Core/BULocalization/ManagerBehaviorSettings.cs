using System;

namespace BULocalization
{
	public class ManagerBehaviorSettings
	{
		private bool _UseToolGeneratedConfigFile;
		private bool _RequestLanguageIfNotSpecified;
		
		public ManagerBehaviorSettings()
		{
			;
		}
		
		public ManagerBehaviorSettings(bool useToolGeneratedConfigFile, bool requestLanguageIfNotSpecified)
		{
			_UseToolGeneratedConfigFile = useToolGeneratedConfigFile;
			_RequestLanguageIfNotSpecified = requestLanguageIfNotSpecified;
		}

		/// <summary>
		/// if yes tool generates config in UserProfile\ProjectName folder</>
		/// </summary>
		public bool UseToolGeneratedConfigFile
		{
			get { return _UseToolGeneratedConfigFile; }
			set { _UseToolGeneratedConfigFile = value;}
		}
		
		/// <summary>
		/// If not - in such cases default will be loaded
		/// </summary>
		public bool RequestLanguageIfNotSpecified
		{
			get { return _RequestLanguageIfNotSpecified; }
			set { _RequestLanguageIfNotSpecified = value;}
		}
	}
}
