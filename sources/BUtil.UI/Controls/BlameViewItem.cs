using BUtil.Core.State;
using System;

namespace BUtil.UI.Controls;

public class BlameViewItem(VersionState versionState, ChangeState state)
{
    public string Title { get; } = versionState.BackupDateUtc.ToString();
    public string Icon { get; } = state switch
    {
        ChangeState.Created => "✨",
        ChangeState.Updated => "✏️",
        ChangeState.Deleted => "🗑️",
        _ => throw new ArgumentOutOfRangeException(nameof(state)),
    };
}
