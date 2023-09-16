using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BUtil.Configurator.Configurator
{
    internal static class TaskNameStringHelper
    {
        public static string TrimIllegalChars(string text)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r
                .Replace(text, string.Empty)
                .Replace("..", string.Empty)
                .Replace("/", string.Empty)
                .Replace("\\", string.Empty);
        }
    }
}
