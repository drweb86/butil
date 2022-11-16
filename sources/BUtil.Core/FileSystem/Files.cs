using System;
using System.IO;

namespace BUtil.Core.FileSystem
{
    public static class Files
    {
		#region Private fields
		
		static readonly string _ConsoleBackupTool = 
			Path.Combine(Directories.BinariesDir, "butilc.exe");
		
		static readonly string _Configurator = 
			Path.Combine(Directories.BinariesDir, "butil.exe");
		
		static readonly string _SevenZipPacker = 
			Path.Combine(Directories.SevenZipFolder , "7z.exe");
		
        static readonly string _BugReportFile = 
        	Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "BUtil BUG report.txt");

        #endregion

		public const string LogFilesExtension = ".BUtilLog.html";
		public const string SuccesfullBackupMarkInHtmlLog = "<!-- SUCCESFULL BACKUP -->";
		public const string ErroneousBackupMarkInHtmlLog = "<!-- ERRONEOUS BACKUP -->";
		
		#region Properties
		
		public static string Configurator
		{
			get { return _Configurator; }
		}

        public static string BugReportFile
        {
            get { return _BugReportFile; }
        }

        public static string ConsoleBackupTool
        {
            get { return _ConsoleBackupTool; }
        }

		public static string SevenZipPacker
        {
			get { return _SevenZipPacker; }
        }

        #endregion
        
        static void CheckExistenceCritical(string file)
        {
        	if (!File.Exists(file)) 
				throw new FileNotFoundException(file);
        }

        public static void CriticalFilesCheck()
        {
        	CheckExistenceCritical(_SevenZipPacker); 
        	CheckExistenceCritical(_ConsoleBackupTool);
        }
    }
}
