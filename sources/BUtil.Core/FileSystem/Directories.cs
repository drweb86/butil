
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BUtil.Core.FileSystem
{
	public static class Directories
	{
		private static readonly string _assembly = Assembly.GetExecutingAssembly().Location;
		private static readonly string _binariesDir = Path.GetDirectoryName(_assembly);
		private static readonly string _installdir = Path.GetDirectoryName(_binariesDir);
		private static readonly string _applicationDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

#if DEBUG
		private static readonly string _userDataFolder = Path.Combine(_applicationDataFolder, "BUtil-Development");
#else
        private static readonly string _userDataFolder = Path.Combine(_applicationDataFolder, "BUtil");
		#endif
		private static readonly string _logsDir = Path.Combine(_userDataFolder, "Logs", "v2");
        private static readonly string _settingsDir = Path.Combine(_userDataFolder, "Settings", "v1");

        public static readonly string TempFolder = System.Environment.GetEnvironmentVariable("TEMP");

        public static string UserDataFolder => _userDataFolder;

        public static string BinariesDir => _binariesDir;

        public static string LogsFolder => _logsDir;
        public static string SettingsDir => _settingsDir;

        private static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public static void EnsureFoldersCreated()
        {
        	CreateDirectory(_userDataFolder);
            CreateDirectory(_logsDir);
        }
	}
}
