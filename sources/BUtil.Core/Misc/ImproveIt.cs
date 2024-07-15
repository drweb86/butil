
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace BUtil.Core.Misc;

public static class ImproveIt
{
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static Action<string>? HandleUiError;
#pragma warning restore CA2211 // Non-constant fields should not be visible

    static ImproveIt()
    {
        AppDomain.CurrentDomain.UnhandledException +=
            new UnhandledExceptionEventHandler(
                UnhandledException);
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

            HandleUiError?.Invoke(string.Format(Resources.ImproveIt_Message, Files.BugReportFile));
        }
        finally
        {
            Environment.Exit(-1);
        }
    }

    private static void UnhandledException(object sender, UnhandledExceptionEventArgs exception)
    {
        ProcessUnhandledException((Exception)exception.ExceptionObject);
    }
}
