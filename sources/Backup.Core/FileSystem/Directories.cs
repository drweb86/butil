using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace BUtil.Core.FileSystem
{
	public static class Directories
	{
		private static readonly string _assembly = Assembly.GetExecutingAssembly().Location;
		private static readonly string _binariesDir = Path.GetDirectoryName(_assembly);
		private static readonly string _installdir = Path.GetDirectoryName(_binariesDir);
		private static readonly string _sevenZipFolder = Path.Combine(_installdir, "7-zip");
		private static readonly string _dataFolder = Path.Combine(_installdir, "data");
		private static readonly string _localsFolder = Path.Combine(_installdir, "local");
		
		private static readonly string _applicationDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

		#if DEBUG
		private static readonly string _userDataFolder = Path.Combine(_applicationDataFolder, "BUtil-Development");
		#else
		private static readonly string _userDataFolder = Path.Combine(_applicationDataFolder, "BUtil-" + CopyrightInfo.Version);
		#endif
		private static readonly string _logsDir = Path.Combine(_userDataFolder, "Logs");
		
        public static readonly string TempFolder = System.Environment.GetEnvironmentVariable("TEMP");

        public static string UserDataFolder
        {
            get { return _userDataFolder; }
        }

		public static string BinariesDir
		{
			get { return _binariesDir; }
		}
        
        public static string DataDir
        {
            get { return _dataFolder; }
        }
		
		public static string LogsFolder
		{
			get { return _logsDir; }
		}

	    public static string SevenZipFolder
	    {
	        get { return _sevenZipFolder; }
	    }

        public static string LocalsFolder
        {
            get { return _localsFolder; }
        }

        private static void createPersonnel(string path)
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
        	createPersonnel(_userDataFolder);
            createPersonnel(_logsDir);
            
            if (!Directory.Exists(_binariesDir))
                throw new DirectoryNotFoundException(_binariesDir);
            if (!Directory.Exists(_sevenZipFolder))
				throw new DirectoryNotFoundException(_sevenZipFolder);
            if (!Directory.Exists(_dataFolder))
				throw new DirectoryNotFoundException(_dataFolder);
            if (!Directory.Exists(_localsFolder))
				throw new DirectoryNotFoundException(_localsFolder);

        }
	}
}
