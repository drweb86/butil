using BUtil.Core.Logs;
using System;

public class ConsoleLog : LogBase
{
    public override void WriteLine(LoggingEvent loggingEvent, string message)
    {
        if (loggingEvent == LoggingEvent.Error)
        {
            FastStdConsole.Flush();
            ConsoleColor previousColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(message);
            Console.ForegroundColor = previousColor;
        }
        else
        {
            FastStdConsole.WriteLine(message);
        }
    }

    public override void Open() { }
    public override void Close(bool isSuccess) => FastStdConsole.Flush();
}
