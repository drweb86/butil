using System;
using System.Text;

namespace BUtil.Core.Misc;

public static class ExceptionHelper
{
    public static string ToString(Exception ex)
    {
        var builder = new StringBuilder();
        builder.Append(ex.Message);

        int depth = 5;
        Exception? innerException;

        do
        {
            depth--;
            innerException = ex.InnerException;
            if (innerException == null)
                break;

            builder.AppendLine();
            builder.Append(innerException.Message);
        }
        while (depth > 0);
        
        return builder.ToString();
    }
}
