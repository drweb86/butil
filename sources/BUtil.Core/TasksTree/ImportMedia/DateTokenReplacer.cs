using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BUtil.Core.TasksTree.MediaSyncBackupModel;

public static class DateTokenReplacer
{
    private const string RegexIncludeBrackets = @"{(?<Param>.*?)}";

    public static string ParseString(string input, DateTime date)
    {
        return Regex.Replace(input, RegexIncludeBrackets, match =>
        {
            string cleanedString = match.Groups["Param"].Value;
            if (cleanedString.StartsWith("DATE:"))
            {
                var format = cleanedString.Replace("DATE:", string.Empty);
                return date.ToString(format, CultureInfo.CurrentUICulture);
            }
            return string.Empty;
        });
    }
}
