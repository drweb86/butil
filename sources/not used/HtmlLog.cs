#region Copyright
/*
 * Copyright (c)Cuchuk Sergey Alexandrovich, 2007-2008. All rights reserved
 * Project: BUtil
 * Link: http://www.sourceforge.net/projects/butil
 * License: GNU GPL or SPL with limitations
 * E-mail:
 * Cuchuk.Sergey@gmail.com
 * toCuchukSergey@yandex.ru
 */
#endregion

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
