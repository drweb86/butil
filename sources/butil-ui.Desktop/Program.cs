using Avalonia;
using Avalonia.Threading;
using BUtil.Core.Misc;
using System;
using System.Threading.Tasks;

namespace butil_ui.Desktop;

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
        ImproveIt.ProcessUnhandledException(e.Exception);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();

}
