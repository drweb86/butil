using System;
using System.IO;

namespace BUtil.Core.FileSystem
{
    public static class Files
    {
		#region Private fields
        
#if DEBUG // Providing testing ability
		const string _UPDATE_URL_XML = 
			@"E:\DOCUMENTS\My WEB\DoctorWEB\butil\update.xml";
#else
		const string _UPDATE_URL_XML =
            @"http://butil.sourceforge.net/update.xml";
#endif

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
		public const string Packer7ZipExeMd5 = "93C7B7A3E3051BBB9630E41425CFDB3C";
		public const string Packer7ZipGExeMd5 ="3F317B59A522F0BC19AC1620BBEA0718";
		public const string Packer7ZipDllMd5 = "CA41D56630191E61565A343C59695CA1";
		
		public const string LogFilesExtension = ".BUtilLog.html";
		public const string SuccesfullBackupMarkInHtmlLog = "<!-- SUCCESFULL BACKUP -->";
		public const string ErroneousBackupMarkInHtmlLog = "<!-- ERRONEOUS BACKUP -->";
		
		public const string ImageFilesExtension = ".BUtil";

		#region Properties
		
		public static string UpdateUrlXml
		{
			get { return _UPDATE_URL_XML; }
		}

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
