using BUtil.Core.Events;
using BUtil.Core.Logs;
using System.Collections.Generic;

namespace BUtil.Core.Services;

internal class LastMinuteMessageService(ILog log) : ILastMinuteMessageService
{
    private readonly ILog _log = log;
    private readonly List<string> _lastMinuteLogMessages = [];

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