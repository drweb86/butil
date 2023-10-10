using Avalonia.Media;
using BUtil.Core;
using BUtil.Core.Localization;
using System.Collections.ObjectModel;

namespace butil_ui.ViewModels;

public class TasksViewModel : PageViewModelBase
{
    private readonly Color _errorForegroundColor;
    private readonly Color _successForegroundColor;

    public TasksViewModel(string theme)
    {
        _progressGenericForeground = new SolidColorBrush(ColorPalette.GetForeground(theme, SemanticColor.Normal));
        _errorForegroundColor = ColorPalette.GetForeground(theme, SemanticColor.Error);
        _successForegroundColor = ColorPalette.GetForeground(theme, SemanticColor.Success);

        WindowTitle = "BUtil - V" + CopyrightInfo.Version.ToString(3); ;
    }

    #region ProgressGenericForeground

    private SolidColorBrush _progressGenericForeground;
    public SolidColorBrush ProgressGenericForeground
    {
        get
        {
            return _progressGenericForeground;
        }
        set
        {
            if (value == _progressGenericForeground)
                return;
            _progressGenericForeground = value;
            OnPropertyChanged(nameof(ProgressGenericForeground));
        }
    }

    #endregion

    #region Items

    private ObservableCollection<LaunchTaskViewItem> _items = new();
    public ObservableCollection<LaunchTaskViewItem> Items
    {
        get
        {
            return _items;
        }
        set
        {
            if (value == _items)
                return;
            _items = value;
            OnPropertyChanged(nameof(Items));
        }
    }

    #endregion

    #region Commands


    #endregion

    #region Labels

    public string Task_Launch_Hint => Resources.Task_Launch_Hint;

    #endregion

    public void Initialize()
    {
    }
}
