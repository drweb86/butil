using System;
using System.IO;
using System.Globalization;
using BUtil.Core.FileSystem;
using BUtil.Core.Misc;
using BUtil.Core.Localization;

namespace BUtil.Core.Logs
{
    public class FileLog : LogBase
	{
        private readonly string _fileName;
        private StreamWriter _logFile;

        public string LogFilename => _fileName;
        
		~FileLog()
		{
            Close();
		}

        public FileLog(string taskName)
		{
            const string _TIME_FORMATSTRING = "dd MMMM (dddd) HH.mm.ss";

            try
            {
                    do
                    {
                        _fileName = Path.Combine(
                            Directories.LogsFolder,
                            $"{taskName} {DateTime.Now.ToString(_TIME_FORMATSTRING, CultureInfo.CurrentUICulture)}{Files.LogFilesExtension}");
                    }
                    while (File.Exists(_fileName));
            }
            catch (ArgumentException e)
            {
                throw new LogException(e.Message);
            }
        }

		public override void Open()
		{
			try
			{
                File.WriteAllText(_fileName, 
@$"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">
<html>
    <head>
	    <META HTTP-EQUIV=""CONTENT-TYPE"" CONTENT=""text/html; charset=utf-8"">
	    <TITLE>{DateTime.Now.ToString("f", CultureInfo.CurrentUICulture)} - {Resources.BackupReport}</TITLE>
		<style>
			body {{
                margin-bottom: 0cm;
				font-family: 'Courier New', Courier, monospace;
			}}
			p {{
                color: #ff0000;
				font-weight: 800;
			}}
		</style>
    </head>
    <body>");
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
                if (!HasErrors)
                {
                    //No any error or warning registered during backup!
                    WriteInFile(Resources.BackupFinishedSuccesfully);
                }
                
                WriteInFile("</body>");
				WriteInFile("</html>");
				if (HasErrors)
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
        }
    }
}
