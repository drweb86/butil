using BUtil.Core.Events;
using BUtil.Core.Logs;
using System.Collections.Generic;

namespace BUtil.Core.Services;

internal class LastMinuteMessageService : ILastMinuteMessageService
{
    private ILog _log;
    private readonly List<string> _lastMinuteLogMessages = new();

    public LastMinuteMessageService(ILog log)
    {
        _log = log;
    }

    public void AddLastMinuteLogMessage(string message)
    {
        _lastMinuteLogMessages.Add(message);
    }

    public string? GetLastMinuteMessages()
    {
        return _lastMinuteLogMessages.Count == 0 ? null : string.Join(System.Environment.NewLine, _lastMinuteLogMessages);
    }

    private void PutLastMinuteLogMessages()
    {
        foreach (var lastMinuteLogMessage in _lastMinuteLogMessages)
            _log.WriteLine(LoggingEvent.Debug, lastMinuteLogMessage);
    }

    public void Dispose()
    {
        PutLastMinuteLogMessages();
    }
}