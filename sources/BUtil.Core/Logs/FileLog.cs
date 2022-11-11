using System;
using System.IO;
using System.Globalization;
using BUtil.Core.Synchronization;
using BUtil.Core.FileSystem;
using BUtil.Core.Misc;
using BUtil.Core.Localization;

namespace BUtil.Core.Logs
{
    public sealed class FileLog : LogBase
	{
        const string _TIME_FORMATSTRING = "dd MMMM (dddd) HH.mm.ss";

        readonly SyncFile _syncfile = new SyncFile();
        readonly string _fileName;
        StreamWriter _logFile;

        public string LogFilename
        {
            get { return _fileName; }
        }
        
		~FileLog()
		{
            Close();
		}

        public FileLog(string logsFolder, bool consoleApp) : base(LogMode.File, consoleApp)
		{
        	if (string.IsNullOrEmpty(logsFolder))
        	{
        		throw new ArgumentNullException("logsFolder");
        	}
        	
            try
            {
                do
                {
                    do
                    {
                        _fileName = Path.Combine(logsFolder,
                            DateTime.Now.ToString(_TIME_FORMATSTRING, CultureInfo.CurrentUICulture) +
                            Files.LogFilesExtension);
                    }
                    while (File.Exists(_fileName));
                }
                while (!_syncfile.TrySyncFile(_fileName));
            }
            catch (ArgumentException e)
            {
                throw new LogException(e.Message);
            }
        }

		/// <summary>
		/// Opens file log
		/// </summary>
        /// <exception cref="LogException">any problems</exception>
		public override void Open()
		{
			try
			{
                File.WriteAllText(_fileName, 
@$"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">
<HTML>
<HEAD>
	<META HTTP-EQUIV=""CONTENT-TYPE"" CONTENT=""text/html; charset=utf-8"">
	<TITLE>{DateTime.Now.ToString("f")} - {Resources.BackupReport}</TITLE>
</HEAD>
<BODY>
	<a target=""_new"" href=""{SupportManager.GetLink(SupportRequest.Homepage)}"">{Resources.VisitProjectHomepage}</a>
	<br />");
				_logFile = File.AppendText(_fileName);
			}
			catch (Exception e)
			{
                throw new LogException(e.Message, e);
			}
			
            IsOpened = true;
		}
	
        private void writeInFile(string message)
        {
            _logFile.WriteLine(message);
            _logFile.Flush();
        }

        public override void WriteLine(LoggingEvent loggingEvent,string message)
        {
            if (PreprocessLoggingInformation(loggingEvent, message))
            {
                string output =
                    HtmlLogFormatter.GetHtmlFormattedLogMessage(loggingEvent, message);
                writeInFile(output);
            }
        }
	  
        public override void Close()
        {
            if (IsOpened)
            {
                if (!ErrorsOrWarningsRegistered)
                {
                    //No any error or warning registered during backup!
                    writeInFile(Resources.BackupFinishedSuccesfully);
                }
                
                writeInFile("</dody>");
				writeInFile("</html>");
				if (ErrorsOrWarningsRegistered)
				{
					writeInFile(Files.ErroneousBackupMarkInHtmlLog);
				}
				else
				{
					writeInFile(Files.SuccesfullBackupMarkInHtmlLog);
				}

				_logFile.Flush();
				_logFile.Close();
                _syncfile.Dispose();
                IsOpened = false;

                GC.SuppressFinalize(this);
            }
            
            System.Diagnostics.Debug.WriteLine("log::close::finished");
        }
    }
}
