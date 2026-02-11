namespace BUtil.Core.Services;

/// <summary>
/// Hides the console window (e.g. when launched from Task Scheduler so it does not steal focus).
/// Implemented only on Windows; other platforms return null from GetConsoleWindowService().
/// </summary>
public interface IConsoleWindowService
{
    void Hide();
}