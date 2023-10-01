using System;

namespace butil_ui.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";
    public string SelectedView = "";
    public string SelectedTask = "";

    // The default is the first page
    private PageViewModelBase _CurrentPage = new LaunchTaskViewModel();

    /// <summary>
    /// Gets the current page. The property is read-only
    /// </summary>
    public PageViewModelBase CurrentPage
    {
        get { return _CurrentPage; }
        protected set => throw new NotSupportedException();
    }
}
