using System;

namespace BUtil.Core.Logs
{
    public abstract class LogBase: ILog
    {
        private bool _errorsOrWarningsRegistered;

        public bool ErrorsOrWarningsRegistered
        {
            get { return _errorsOrWarningsRegistered; }
        }
		
        private void OutputPackerMessageHelper(string[] data, LoggingEvent putInLogAs)
        {
            for (int i = 0; i < data.Length; i++)
                if (!string.IsNullOrEmpty(data[i]))
                    WriteLine(putInLogAs, data[i]);
        }
        
        public void ProcessPackerMessage(string consoleOutput, bool finishedSuccessfully)
        {
            // Preparation of string to process
            string DestinationString = consoleOutput;
            DestinationString = DestinationString.Replace("\r", string.Empty);
            string[] data = DestinationString.Split(new Char[] { '\n' });// errors /r - remains
            for (int i = 0; i < data.Length; i++)
                data[i] = data[i].Trim();// Trim required because output from archiver is bad

            // in all other log types we should output all
            // 7-zip output entirely
            if (finishedSuccessfully)
                OutputPackerMessageHelper(data, LoggingEvent.PackerMessage);
            else
                OutputPackerMessageHelper(data, LoggingEvent.Error);
        }

        protected void PreprocessLoggingInformation(LoggingEvent loggingEvent)
        {
        	if (loggingEvent == LoggingEvent.Error)
        		_errorsOrWarningsRegistered = true;
        }

        public abstract void Open();
        public abstract void Close();
        public abstract void WriteLine(LoggingEvent loggingEvent, string message);
    }
}
