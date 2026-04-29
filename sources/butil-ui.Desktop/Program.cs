using Avalonia;
using Avalonia.Threading;
using BUtil.Core.Misc;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BUtil.UI.Desktop;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        ImproveIt.HandleUiError = HandleUiError;
        TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        try
        {
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            ImproveIt.ProcessUnhandledException(ex);
        }
    }

    private static void HandleUiError(string message)
    {
        Dispatcher.UIThread.Invoke(async () => await Messages.ShowErrorBox(message)).Wait(10000);
    }

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        ImproveIt.ProcessUnhandledException((Exception)e.ExceptionObject);
    }

    private static void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        var exception = e.Exception;

        // Avalonia crashes at Ubuntu.
        if ((exception.Message is not null && 
            exception.Message.Contains("org.freedesktop.DBus.Error.ServiceUnknown")) ||
            
            exception.InnerExceptions.Any(x => x.Message is not null && x.Message.Contains("org.freedesktop.DBus.Error.ServiceUnknown")))
        {
            e.SetObserved();
            return;
        }

        ImproveIt.ProcessUnhandledException(exception);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();

}
