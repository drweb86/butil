using BUtil.Core.Events;
using System;

namespace BUtil.Core;

public static class LocalsHelper
{
    public static string ToString(ProcessingStatus state)
    {
        switch (state)
        {
            case ProcessingStatus.NotStarted: return "⏳";
            case ProcessingStatus.InProgress: return "👉";
            case ProcessingStatus.FinishedSuccesfully: return "✅";
            case ProcessingStatus.Skipped: return "⏩";
            case ProcessingStatus.FinishedWithErrors: return "❌";
            default:
                throw new NotImplementedException("State " + state + " is not implemented");
        }
    }
}
