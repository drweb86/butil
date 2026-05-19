using BUtil.Interop.Tasks.Events;
using System;

namespace BUtil.Core.Logs;

internal static class ProcessingStatusFormatter
{
    public static string ToSymbol(ProcessingStatus state)
    {
        return state switch
        {
            ProcessingStatus.NotStarted => "⏳",
            ProcessingStatus.InProgress => "👉",
            ProcessingStatus.FinishedSuccesfully => "✅",
            ProcessingStatus.Skipped => "⏩",
            ProcessingStatus.FinishedWithErrors => "❌",
            _ => throw new NotImplementedException("State " + state + " is not implemented"),
        };
    }
}
