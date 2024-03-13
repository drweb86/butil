using System;
using System.Globalization;

namespace BUtil.Core.Logs;

internal static class LogFormatter
{
    public static string GetFormattedMessage(LoggingEvent loggingEvent, string message)
    {
        var information = string.Format(CultureInfo.CurrentUICulture,
                        "{0:HH:mm} {1}",
                        DateTime.Now,
                        message);

        if (loggingEvent == LoggingEvent.Error)
            return $"⛔ERROR {information}";
        else
            return $"✅ {information}";
    }
}
