using System;

namespace BUtil.Core.Services;

public interface ILastMinuteMessageService: IDisposable
{
    void AddLastMinuteLogMessage(string message);
    string? GetLastMinuteMessages();
}