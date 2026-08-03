namespace BUtil.UI.Controls;

public class FileChangeViewItem(string title, ChangeState state)
{
    public string Title { get; } = title;
    public string Icon { get; } = ChangeStateIcons.Get(state);
}
