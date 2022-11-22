using System;
using System.IO;
using System.Globalization;
using BUtil.Core.FileSystem;
using BUtil.Core.Misc;
using BUtil.Core.Localization;

namespace BUtil.Core.Logs
{
    public sealed class FileLog : LogBase
	{
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
            const string _TIME_FORMATSTRING = "dd MMMM (dddd) HH.mm.ss";

            try
            {
                    do
                    {
                        _fileName = Path.Combine(logsFolder,
                            DateTime.Now.ToString(_TIME_FORMATSTRING, CultureInfo.CurrentUICulture) +
                            Files.LogFilesExtension);
                    }
                    while (File.Exists(_fileName));
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
		}
	
        private void WriteInFile(string message)
        {
            lock (_logFile)
                _logFile.WriteLine(message);
        }

        public override void WriteLine(LoggingEvent loggingEvent, string message)
        {
            PreprocessLoggingInformation(loggingEvent);

            string output = HtmlLogFormatter.GetHtmlFormattedLogMessage(loggingEvent, message);
            WriteInFile(output);
            if (loggingEvent == LoggingEvent.Error)
                lock (_logFile)
                    _logFile.Flush();
        }
	  
        public override void Close()
        {
            if (_logFile != null)
            {
                if (!ErrorsOrWarningsRegistered)
                {
                    //No any error or warning registered during backup!
                    WriteInFile(Resources.BackupFinishedSuccesfully);
                }
                
                WriteInFile("</dody>");
				WriteInFile("</html>");
				if (ErrorsOrWarningsRegistered)
				{
					WriteInFile(Files.ErroneousBackupMarkInHtmlLog);
				}
				else
				{
					WriteInFile(Files.SuccesfullBackupMarkInHtmlLog);
				}

				_logFile.Flush();
				_logFile.Close();
                _logFile = null;

                GC.SuppressFinalize(this);
            }
            
            System.Diagnostics.Debug.WriteLine("log::close::finished");
        }
    }
}
