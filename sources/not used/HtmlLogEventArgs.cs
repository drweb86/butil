/*
* configuring for formatting archiver output
* if (_onLogEvent != null)
                    // some problems may occur with this -=_onLogEvent=-
                    // case LogMode.Html: _targetEncoding = Encoding.Default; break;
*
*/

using System;

namespace BUtil.Kernel.Logs
{
    sealed public class HtmlLogEventArgs : EventArgs
    {
        private LoggingEvent _typeOfMessage;
        private string _message;

        public LoggingEvent MessageKind
        {
            get { return _typeOfMessage; }
        }

        public string Message
        {
            get { return _message; }
        }

        public HtmlLogEventArgs(LoggingEvent typeOfMessage, string htmlFormattedMessage)
        {
            if (String.IsNullOrEmpty(htmlFormattedMessage))
                throw new ArgumentNullException("htmlFormattedMessage");

            _message = htmlFormattedMessage;
            _typeOfMessage = typeOfMessage;
        }
    }
}
