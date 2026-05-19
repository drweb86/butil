using BUtil.Core.Localization;

namespace BUtil.UI.Controls;

public sealed class TaskUIViewModel : ViewModelBase
{
    public TaskUIViewModel(object content, string name, bool isNew)
    {
        Content = content;
        WindowTitle = $"{name} - {(isNew ? Resources.Task_WindowTitle_Creating : Resources.Task_WindowTitle_Editing)}";
    }

    public object Content { get; }
}
