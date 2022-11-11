using System;
using System.Text;
using System.Globalization;

namespace BUtil.Core.Logs
{

    public abstract class LogBase: ILog
    {
        private const string _LOG_NOT_OPENED = "Log is not opened";

        private bool _opened;// auto: false
        private bool _errorsOrWarningsRegistered;// auto: false
        
        // Default encoding for processing packer messages
		private Encoding _initialEncoding;
		private Encoding _targetEncoding;

        internal LogBase(LogMode mode, bool consoleApp)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _initialEncoding = Encoding.GetEncoding("cp866");
            _targetEncoding = Encoding.Default;

            // packer encodings
            switch (mode)
            {
                case LogMode.Console: 
            		_initialEncoding = _targetEncoding; 
            		break;

                case LogMode.File: 
            		if (consoleApp)
                	{
                		_targetEncoding = _initialEncoding; 
                	}
                	else
                	{
                		_targetEncoding = Encoding.Default;
	                }
    	            break;

				default:
    	            throw new NotImplementedException(mode.ToString());
            }
        }

        public bool IsOpened
        {
            get { return _opened; }
            protected set { _opened = value; }
        }
        
        public bool ErrorsOrWarningsRegistered
        {
            get { return _errorsOrWarningsRegistered; }
        }
		
        /// <summary>
        /// Helper method for ProcessPackerMessage
        /// </summary>
        /// <param name="data">data from packer output</param>
        /// <param name="putInLogAs">how to output them</param>
        private void outputPackerMessageHelper(string[] data, LoggingEvent putInLogAs)
        {
            for (int i = 0; i < data.Length; i++)
                if (!string.IsNullOrEmpty(data[i]))
                    WriteLine(putInLogAs, data[i]);
        }
        
        /// <summary>
        /// Writes message to log using formatting
        /// </summary>
        /// <param name="loggingEvent">The logging event</param>
        /// <param name="message">The format string</param>
        /// <param name="arguments">The arguments</param>
        public void WriteLine(LoggingEvent loggingEvent, string message, params string[] arguments)
        {
            WriteLine(loggingEvent, string.Format(CultureInfo.CurrentUICulture, message, arguments));
        }

        /// <summary>
        /// Grabs output from console and converts it readable format, writes to log
        /// </summary>
        /// <param name="consoleOutput">ConsoleOutput</param>
        /// <param name="finishedSuccesfully">if any errors archiver returned</param>
        public void ProcessPackerMessage(string consoleOutput, bool finishedSuccessfully)
        {
            // Preparation of string to process
            string DestinationString = convertPackerOutputInLogEncoding(consoleOutput);
            DestinationString = DestinationString.Replace("\r", string.Empty);
            string[] data = DestinationString.Split(new Char[] { '\n' });// errors /r - remains
            for (int i = 0; i < data.Length; i++)
                data[i] = data[i].Trim();// Trim required because output from archiver is bad

            // in all other log types we should output all
            // 7-zip output entirely
            if (finishedSuccessfully)
                outputPackerMessageHelper(data, LoggingEvent.PackerMessage);
            else
                outputPackerMessageHelper(data, LoggingEvent.Error);
        }

		/// <summary>
		/// Validates information
		/// </summary>
		/// <param name="loggingEvent">kind of event</param>
		/// <param name="message">message</param>
		/// <returns>true - if this message should be processed</returns>
		/// <exception cref="ArgumentException">message set invalidly or null</exception>
		/// <exception cref="InvalidOperationException">log not opened</exception>
        protected bool PreprocessLoggingInformation(LoggingEvent loggingEvent, string message)
        {
            if (!_opened) 
            {
            	System.Diagnostics.Debug.WriteLine("Not opened");
           		System.Diagnostics.Debug.WriteLine(Environment.StackTrace);
                throw new InvalidOperationException(_LOG_NOT_OPENED);
            }

        	if ((loggingEvent == LoggingEvent.Error) || 
                (loggingEvent == LoggingEvent.Warning))
        		_errorsOrWarningsRegistered = true;
        	
        	System.Diagnostics.Debug.WriteLine(loggingEvent.ToString());
        	System.Diagnostics.Debug.WriteLine(message);

            if (string.IsNullOrEmpty(message))
            {
            	if ((loggingEvent == LoggingEvent.Error) || (loggingEvent == LoggingEvent.Warning))
            	{
            		System.Diagnostics.Debug.WriteLine("Empty message");
            		System.Diagnostics.Debug.WriteLine(Environment.StackTrace);
                    throw new ArgumentException("message");
            	}
            	else 
					return false;
            }
            
            return true;
        }
       
        private string convertPackerOutputInLogEncoding(string packerOutput)
		{
            Decoder dec = _initialEncoding.GetDecoder();
            byte[] ba = _targetEncoding.GetBytes(packerOutput);
			int len = dec.GetCharCount(ba, 0, ba.Length);
			char[] ca = new char[len];
			dec.GetChars(ba, 0, ba.Length, ca, 0);
			return new string(ca);
		}

        public abstract void Open();
        public abstract void Close();
        public abstract void WriteLine(LoggingEvent loggingEvent, string message);
    }
}
