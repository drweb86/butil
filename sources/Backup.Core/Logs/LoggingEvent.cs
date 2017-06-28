using System;

namespace BUtil.Core.Logs
{
	/// <summary>
    /// Type of logging event
    /// </summary>
    public enum LoggingEvent
    {
        Error = 0,// errors{}
        Warning,// warnings from cycle
        PackerMessage,// Redirected output from archiver
        Debug,// For debugging
    };
}
