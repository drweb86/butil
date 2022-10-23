using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;


namespace BUtil.Core.Logs
{
    internal static class HtmlLogFormatter
    {
		const string _INFORMATION_FORMATSTRING = "{0} {1} - {2}";
		const string _INFORMATION_FORMATSTRING_WITHOUT_PREFIX = "{0} - {1}";
        static readonly string[] _loggingEventsStrings = { "[error]", "[warning]", "[packer]", "[debug]" };
        static bool _includeLoggingEventPrefixes = false;
       
        public static bool IncludeLoggingEventPrefixes
        {
        	get { return _includeLoggingEventPrefixes; }
        	set { _includeLoggingEventPrefixes = value; }
        }
        
        public static string GetHtmlFormattedLogMessage(LoggingEvent loggingEvent, string message)
        {
        	string information = _includeLoggingEventPrefixes ?
				string.Format(CultureInfo.CurrentCulture,
							_INFORMATION_FORMATSTRING, 
							_loggingEventsStrings[(int)loggingEvent],  
							DateTime.Now.ToLongTimeString(), 
							message) :
        		string.Format(CultureInfo.CurrentCulture,
							_INFORMATION_FORMATSTRING_WITHOUT_PREFIX, 
							DateTime.Now.ToLongTimeString(), 
							message);

			string output = "<P STYLE=\"margin-bottom: 0cm\"><FONT FACE=\"Courier New, monospace\" COLOR=\"#";

            switch (loggingEvent)
            {
                case LoggingEvent.Error:
                    output += "ff0000\"><B>" + information + "</B>"; break;
                case LoggingEvent.PackerMessage:
                    output += "000000\">" + information; break;
                case LoggingEvent.Debug:
                    output += "2323dc\">" + information; break;
                case LoggingEvent.Warning:
                    output += "2300dc\"><B>" + information + "</B>"; break;
            }
			return output + "</FONT></P>";
        }
    }
}
