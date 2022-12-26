using System;
using System.Globalization;

namespace BUtil.Core.Logs
{
    internal static class HtmlLogFormatter
    {
        public static string GetHtmlFormattedLogMessage(LoggingEvent loggingEvent, string message)
        {
            var information = string.Format(CultureInfo.CurrentUICulture,
                            "{0:HH:mm} {1}",
                            DateTime.Now,
                            message);

            if (loggingEvent == LoggingEvent.Error)
                return $"<p>[error] {information}</p>";
            else
                return $"[info] {information}<br />";
        }
    }
}
