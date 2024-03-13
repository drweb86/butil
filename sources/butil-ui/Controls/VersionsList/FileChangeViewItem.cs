using System;

namespace butil_ui.Controls;

public class FileChangeViewItem
{
    public FileChangeViewItem(string title, ChangeState state)
    {
        Title = title;

        switch (state)
        {
            case ChangeState.Created:
                ImageSource = "/Assets/VC-Created.png";
                break;
            case ChangeState.Deleted:
                ImageSource = "/Assets/VC-Deleted.png";
                break;
            case ChangeState.Updated:
                ImageSource = "/Assets/VC-Updated.png";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state));
        }

    }
    public string Title { get; }
    public string ImageSource { get; }
}
