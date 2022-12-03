using System;

namespace BUtil.Core.Logs
{
    public class ConsoleLog: LogBase
	{
        public override void WriteLine(LoggingEvent loggingEvent, string message)
        {
            PreprocessLoggingInformation(loggingEvent);

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
        public override void Close() => FastStdConsole.Flush();
	}
}
