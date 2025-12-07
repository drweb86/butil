using BUtil.Core.Logs;
using BUtil.Core.Misc;
using FtpsServerLibrary;
using System;

namespace BUtil.Core.TasksTree.BUtilServer.Server;

class FtpsServerLog : IFtpsServerLog
{
    private readonly ILog _log;

    public FtpsServerLog(ILog log)
    {
        _log = log;
    }

    public void Debug(string message)
    {
        _log.WriteLine(LoggingEvent.Debug, message);
    }

    public void Error(Exception ex, string message)
    {
        _log.WriteLine(LoggingEvent.Error, message);
        _log.WriteLine(LoggingEvent.Error, ExceptionHelper.ToString(ex));
    }

    public void Fatal(Exception ex, string message)
    {
        _log.WriteLine(LoggingEvent.Error, message);
    }

    public void Info(string message)
    {
        _log.WriteLine(LoggingEvent.Debug, message);
    }

    public void Warn(string message)
    {
        _log.WriteLine(LoggingEvent.Debug, message);
    }
}
