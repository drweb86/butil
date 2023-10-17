using System;

namespace BUtil.Core
{
    public static class StringExtensions
    {
        public static bool Cmp(this string? str1, string? str2)
        {
            return string.Compare(str1, str2, StringComparison.OrdinalIgnoreCase) == 0;
        }
    }
}
