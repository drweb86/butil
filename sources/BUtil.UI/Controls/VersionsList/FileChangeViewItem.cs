using System;

namespace butil_ui.Controls;

public class FileChangeViewItem(string title, ChangeState state)
{
    public string Title { get; } = title;
    public string ImageSource { get; } = state switch
    {
        ChangeState.Created => "/Assets/VC-Created.png",
        ChangeState.Deleted => "/Assets/VC-Deleted.png",
        ChangeState.Updated => "/Assets/VC-Updated.png",
        _ => throw new ArgumentOutOfRangeException(nameof(state)),
    };
}
