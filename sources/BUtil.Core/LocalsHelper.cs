using BUtil.Core.Events;
using System;

namespace BUtil.Core;

public static class LocalsHelper
{
    public static string ToString(ProcessingStatus state)
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
