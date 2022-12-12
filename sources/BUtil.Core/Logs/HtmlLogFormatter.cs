using System;
using System.Globalization;

namespace BUtil.Core.Logs
{
    internal static class HtmlLogFormatter
    {
		const string _INFORMATION_FORMATSTRING = "{0} {1} - {2}";
        static readonly string[] _loggingEventsStrings = { "[error]", "[packer]", "[info]" };
       
        public static string GetHtmlFormattedLogMessage(LoggingEvent loggingEvent, string message)
        {
        	string information = string.Format(CultureInfo.CurrentUICulture,
							_INFORMATION_FORMATSTRING, 
							_loggingEventsStrings[(int)loggingEvent],  
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
            }
			return output + "</FONT></P>";
        }
    }
}
