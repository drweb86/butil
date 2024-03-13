
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace BUtil.Core.Misc;

public static class ImproveIt
{
    public static Action<string>? HandleUiError;

    static ImproveIt()
    {
        AppDomain.CurrentDomain.UnhandledException +=
            new UnhandledExceptionEventHandler(
                unhandledException);
    }

    public static void ProcessUnhandledException(Exception exception)
    {
        try
        {
            StringBuilder builder = new();
            builder.AppendLine();
            builder.AppendLine();
            builder.AppendLine("BUtil " + CopyrightInfo.Version + " - Bug report (" + DateTime.Now.ToString("g", CultureInfo.InvariantCulture) + ")");
            builder.AppendLine("Please report about it here: ");
            builder.AppendLine(ApplicationLinks.HomePage);
            builder.AppendLine(exception.Message);
            builder.AppendLine(exception.StackTrace);
            builder.AppendLine(exception.Source);

            Exception? inner = exception.InnerException;
            if (inner != null)
            {
                builder.AppendLine(inner.Message);
                builder.AppendLine(inner.StackTrace);
                builder.AppendLine(inner.Source);
            }

            File.AppendAllText(Files.BugReportFile, builder.ToString());

            if (HandleUiError != null)
            {
                HandleUiError.Invoke(string.Format(Resources.ImproveIt_Message, Files.BugReportFile));
            }
        }
        finally
        {
            Environment.Exit(-1);
        }
    }

    private static void unhandledException(object sender, UnhandledExceptionEventArgs exception)
    {
        ProcessUnhandledException((Exception)exception.ExceptionObject);
    }
}
