using System;
using System.IO;

namespace BUtil.Core.FileSystem
{
    public static class Files
    {
		#region Private fields
        
        static readonly string ProfileConfigFile = 
			Path.Combine(Directories.UserDataFolder, "ProfileOptions.xml");
		
		static readonly string _FileLogTemplate = 
			Path.Combine(Directories.DataDir, "logtemplate.dat");
		
		static readonly string _ConsoleBackupTool = 
			Path.Combine(Directories.BinariesDir, "Backup.exe");
		
		static readonly string _Configurator = 
			Path.Combine(Directories.BinariesDir, "Configurator.exe");
		
		static readonly string _SevenZipPacker = 
			Path.Combine(Directories.SevenZipFolder , "7z.exe");
		
		static readonly string _SevenZipGPacker = 
			Path.Combine(Directories.SevenZipFolder , "7zg.exe");
		
		static readonly string _SevenZipPackerDll = 
			Path.Combine(Directories.SevenZipFolder, "7z.dll");
		
		static readonly string _Scheduler = 
			Path.Combine(Directories.BinariesDir, Constants.TrayApplicationProcessName + ".exe");
		
        static readonly string _BugReportFile = 
        	Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "BUtil BUG report.txt");

        #endregion

		// Related document: DevelopmentInitiative.7-zip protection
		public const string Packer7ZipExeMd5 = "FE522D8659618E3A50AAFD8AC1518638";
		public const string Packer7ZipGExeMd5 = "5AB26FFD7B3C23A796138640B1737B48";
		public const string Packer7ZipDllMd5 = "BBF51226A8670475F283A2D57460D46C";
		
		public const string LogFilesExtension = ".BUtilLog.html";
		public const string SuccesfullBackupMarkInHtmlLog = "<!-- SUCCESFULL BACKUP -->";
		public const string ErroneousBackupMarkInHtmlLog = "<!-- ERRONEOUS BACKUP -->";
		
		public const string ImageFilesExtension = ".BUtil";

		#region Properties
		
		public static string Configurator
		{
			get { return _Configurator; }
		}

		public static string Scheduler
		{
			get { return _Scheduler; }
		}

        public static string BugReportFile
        {
            get { return _BugReportFile; }
        }

		public static string SevenZipPackerDll
		{
			get { return _SevenZipPackerDll; }
		}
		
		public static string SevenZipGPacker
		{
			get { return _SevenZipGPacker; }
		}

		public static string ProfileFile
		{
			get { return ProfileConfigFile; }
		}

        public static string ConsoleBackupTool
        {
            get { return _ConsoleBackupTool; }
        }

		public static string SevenZipPacker
        {
			get { return _SevenZipPacker; }
        }

        public static string FileLogTemplate
        {
            get { return _FileLogTemplate; }
        }
        
        #endregion
        
        static void CheckExistenceCritical(string file)
        {
        	if (!File.Exists(file)) 
				throw new FileNotFoundException(file);
        }

        /// <summary>
        /// Checks for existence of critical files
        /// </summary>
        /// <exception cref="FileNotFoundException"></exception>
        public static void CriticalFilesCheck()
        {
        	CheckExistenceCritical(_SevenZipPacker); 
        	CheckExistenceCritical(_SevenZipPackerDll);
        	CheckExistenceCritical(_SevenZipGPacker);
        	CheckExistenceCritical(_ConsoleBackupTool);
        	CheckExistenceCritical(_FileLogTemplate);
        }
    }
}
