using System;

namespace BUtil.Core.Logs
{
    public enum LoggingEvent
    {
        Error = 0,// errors{}
        PackerMessage,// Redirected output from archiver
        Debug,// For debugging
    };
}
