using System;

namespace BUtil.UI.Controls;

public class FileChangeViewItem(string title, ChangeState state)
{
    public string Title { get; } = title;
    public string Icon { get; } = state switch
    {
        ChangeState.Created => "✨",
        ChangeState.Updated => "✏️",
        ChangeState.Deleted => "🗑️",
        _ => throw new ArgumentOutOfRangeException(nameof(state)),
    };
}
