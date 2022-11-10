using System;
using System.Globalization;

namespace BUtil.Core.Misc
{
    public static class PrettyTimeSpanFormatter
    {
        private const string _Day = " day";
        private const string _Hour = " hour";
        private const string _Minute = " minute";
        private const string _Second = " second";

        public static string Format(DateTime zeroHour)
        {
            return Format(zeroHour.Subtract(DateTime.Now));
        }

        private static string formatValue(string word, double number)
        { 
            // getting only integer part of a number without rounding it
            int num = Convert.ToInt32( Math.Truncate( number) );

            if (num == 0)
                return string.Empty;
            else
            {
                if (num == 1)
                    return num.ToString(CultureInfo.CurrentUICulture) + word;
                else
                    return num.ToString(CultureInfo.CurrentUICulture) + word + "s";
            }
        }

        public static string Format(TimeSpan span)
        { 
            // 1+ day
            if (span.TotalDays >= 1)
                // Convert.ToInt32 rounds TotalDays to the nearest integer value
                return formatValue(_Day, Convert.ToInt32(span.TotalDays));
            else
            {
                // 1+ hour
                if (span.TotalHours >= 1)
                    return formatValue(_Hour, span.TotalHours);
                else
                {
                    // 2+ minutes
                    if (span.TotalMinutes >= 3)
                        return formatValue(_Minute, span.TotalMinutes);
                    else
                        return formatValue(_Minute, span.TotalMinutes) + " " + formatValue(_Second, span.Seconds);
                }
            }
        }
    }
}
