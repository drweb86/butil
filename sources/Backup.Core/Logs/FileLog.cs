using System;
using System.IO;
using System.Text;
using BULocalization;

using BUtil;
using BUtil.Core;
using System.Globalization;
using BUtil.Core.Synchronization;
using BUtil.Core.FileSystem;
using BUtil.Core.Misc;

namespace BUtil.Core.Logs
{
    /// <summary>
    /// Logs and outputs information to File
    /// </summary>
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

        /// <exception cref="LogException">Synchronization failled</exception>
        public FileLog(string logsFolder, LogLevel level, bool consoleApp) : base(level, LogMode.File, consoleApp)
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
                            DateTime.Now.ToString(_TIME_FORMATSTRING, CultureInfo.CurrentCulture) +
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
            // creating log on the base of template
            // required to reduce problems with encoding
			try
			{
                File.WriteAllText(_fileName, 
                                  string.Format(
                                  	CultureInfo.CurrentCulture, 
                                  	File.ReadAllText(Files.FileLogTemplate), 
                                  	DateTime.Now.ToString("f"),
                                  	Translation.Current[509],
                                  	SupportManager.GetLink(SupportRequest.Homepage),
                                  	SupportManager.GetLink(SupportRequest.Issue),
                                  	SupportManager.GetLink(SupportRequest.Issue),
                                  	Translation.Current[288],
                                  	Translation.Current[290],
                                  	Translation.Current[291]));
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
                    writeInFile(Translation.Current[503]);
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
