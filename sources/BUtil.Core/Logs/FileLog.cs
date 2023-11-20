
using System;
using System.IO;
using System.Globalization;
using BUtil.Core.Localization;

namespace BUtil.Core.Logs
{

    public class FileLog : LogBase
	{
        private string _fileName;
        private readonly string _taskName;
        private StreamWriter? _logFile;
        private readonly DateTime _dateTime;

        public string LogFilename => _fileName;
        
		~FileLog()
		{
            Close();
		}

        public FileLog(string taskName)
		{
            _taskName = taskName;
            try
            {
                    do
                    {
                        _dateTime = DateTime.Now;

                        _fileName = GetFileName(null);
                    }
                    while (File.Exists(_fileName));
            }
            catch (ArgumentException e)
            {
                throw new LogException(e.Message);
            }
            _taskName = taskName;
        }

        private string GetFileName(bool? isSuccess)
        {
            var logService = new LogService();
            return logService.GetFileName(_taskName, _dateTime, isSuccess);
        }

		public override void Open()
		{
			try
			{
                File.WriteAllText(
                    _fileName, 
                    @$"{_dateTime.ToString("f", CultureInfo.CurrentUICulture)} {LocalsHelper.ToString(Events.ProcessingStatus.FinishedSuccesfully)}{LocalsHelper.ToString(Events.ProcessingStatus.FinishedWithErrors)}" + Environment.NewLine);
				_logFile = File.AppendText(_fileName);
			}
			catch (Exception e)
			{
                throw new LogException(e.Message, e);
			}
		}
	
        private void WriteInFile(string message)
        {
            if (_logFile == null)
                return;

            lock (_logFile)
                _logFile.WriteLine(message);
        }

        public override void WriteLine(LoggingEvent loggingEvent, string message)
        {
            if (_logFile == null)
                return;

            PreprocessLoggingInformation(loggingEvent);

            string output = LogFormatter.GetFormattedMessage(loggingEvent, message);
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
                    WriteInFile(Resources.Task_Status_Succesfull);
                }
                
                _logFile.Flush();
                _logFile.Close();

                var fileNameUpdated = GetFileName(!HasErrors);
                File.Move(_fileName, fileNameUpdated);
                _fileName = fileNameUpdated;

                _logFile = null;

                GC.SuppressFinalize(this);
            }
        }
    }
}
