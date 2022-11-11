using System;

namespace BUtil.Core.Logs
{
	/// <summary>
	/// Logs output to console. Works with dos output from archiver
	/// </summary>
	public sealed class ConsoleLog: LogBase
	{
        public override void WriteLine(LoggingEvent loggingEvent, string message)
        {
            if (PreprocessLoggingInformation(loggingEvent, message))
            {
            	ConsoleColor previousColor = Console.ForegroundColor;
            	
            	switch (loggingEvent)
            	{
            		case LoggingEvent.Error:
            			Console.ForegroundColor = ConsoleColor.Red;
            			break;
            		case LoggingEvent.Warning:
            			Console.ForegroundColor = ConsoleColor.Yellow;
            			break;
            		case LoggingEvent.Debug:
            			Console.ForegroundColor = ConsoleColor.Gray;
            			break;
            		case LoggingEvent.PackerMessage:
            			Console.ForegroundColor = ConsoleColor.Blue;
            			break;
            			
            		default:
            			throw new NotImplementedException(string.Format("Color selection for {0} is not implemented",loggingEvent));
            	}
            	
            	Console.WriteLine(message);
            	
            	Console.ForegroundColor = previousColor;
                
            }
        }
        
        public override void Open()
        {
			IsOpened = true;
        }

        public ConsoleLog() : base(LogMode.Console, true) { ; }
        public override void Close() { ; }
	}
}
