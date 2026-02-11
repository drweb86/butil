namespace BUtil.Core.Options;

/// <summary>
/// Command-line arguments added by the scheduler when launching the console app.
/// The console app uses these to enable scheduler-specific behavior (e.g. hide window on Windows).
/// </summary>
public static class SchedulerLaunchArguments
{
    /// <summary>
    /// When present, the console app should hide its window (Windows: no focus steal when running in background).
    /// </summary>
    public const string HideConsole = "HideConsole";
}
