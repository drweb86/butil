using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BUtil.Core.TasksTree.MediaSyncBackupModel;

public static partial class DateTokenReplacer
{
    private const string _regexIncludeBrackets = @"{(?<Param>.*?)}";

    public static string ParseString(string input, DateTime date)
    {
        return RegexIncludeBracketsCompileTime().Replace(input, match =>
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

    [GeneratedRegex(_regexIncludeBrackets)]
    private static partial Regex RegexIncludeBracketsCompileTime();
}
