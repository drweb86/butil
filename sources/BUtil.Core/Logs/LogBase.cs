using System;
using System.Text;

namespace BUtil.Core.Logs
{
    public abstract class LogBase: ILog
    {
        private bool _errorsOrWarningsRegistered;
        
        // Default encoding for processing packer messages
		private readonly Encoding _initialEncoding;
		private readonly Encoding _targetEncoding;

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

        public bool ErrorsOrWarningsRegistered
        {
            get { return _errorsOrWarningsRegistered; }
        }
		
        /// <summary>
        /// Helper method for ProcessPackerMessage
        /// </summary>
        /// <param name="data">data from packer output</param>
        /// <param name="putInLogAs">how to output them</param>
        private void OutputPackerMessageHelper(string[] data, LoggingEvent putInLogAs)
        {
            for (int i = 0; i < data.Length; i++)
                if (!string.IsNullOrEmpty(data[i]))
                    WriteLine(putInLogAs, data[i]);
        }
        
        /// <summary>
        /// Grabs output from console and converts it readable format, writes to log
        /// </summary>
        /// <param name="consoleOutput">ConsoleOutput</param>
        /// <param name="finishedSuccesfully">if any errors archiver returned</param>
        public void ProcessPackerMessage(string consoleOutput, bool finishedSuccessfully)
        {
            // Preparation of string to process
            string DestinationString = ConvertPackerOutputInLogEncoding(consoleOutput);
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
       
        private string ConvertPackerOutputInLogEncoding(string packerOutput)
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
