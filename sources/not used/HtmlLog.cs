using System;

namespace BUtil.Kernel.Logs
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public sealed class HtmlLog:LogBase
	{
        private event EventHandler<HtmlLogEventArgs> _onLogEvent;
		
        public override void WriteLine(LoggingEvent loggingEvent, string message)
        {
            if (PreprocessLoggingInformation(loggingEvent, message))
                _onLogEvent.Invoke(this, new HtmlLogEventArgs(loggingEvent, HtmlLogFormatter.GetHtmlFormattedLogMessage(loggingEvent, message)));
        }
        
        public override void Open()
        {
        	IsOpened = true;
        }

        public HtmlLog(LogLevel level, EventHandler<HtmlLogEventArgs> onLogEvent) 
            : base(level, LogMode.Html) 
        {
            if (onLogEvent == null)
                throw new ArgumentNullException("onLogEvent");

            _onLogEvent = onLogEvent;
        }

        public override void Close() { ; }
	}
}
