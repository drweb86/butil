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
		private static readonly string _dataFolder = Path.Combine(_installdir, "data");
		private static readonly string _applicationDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

#if DEBUG
		private static readonly string _userDataFolder = Path.Combine(_applicationDataFolder, "BUtil-Development");
#else
        private static readonly string _userDataFolder = Path.Combine(_applicationDataFolder, "BUtil");
		#endif
		private static readonly string _logsDir = Path.Combine(_userDataFolder, "Logs", "v2");
		
        public static readonly string TempFolder = System.Environment.GetEnvironmentVariable("TEMP");

        public static string UserDataFolder => _userDataFolder;

        public static string BinariesDir => _binariesDir;

        public static string DataDir => _dataFolder;

        public static string LogsFolder => _logsDir;

        private static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (IOException)
                {
                    throw new DirectoryNotFoundException(path);
                }
            }
        }

        /// <summary>
        /// Checks for existence of main program folders
        /// </summary>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public static void CriticalFoldersCheck()
        {
        	CreateDirectory(_userDataFolder);
            CreateDirectory(_logsDir);
            
            if (!Directory.Exists(_binariesDir))
                throw new DirectoryNotFoundException(_binariesDir);
            if (!Directory.Exists(_dataFolder))
				throw new DirectoryNotFoundException(_dataFolder);
        }
	}
}
