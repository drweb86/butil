using BUtil.Core.FileSystem;
using System;
using System.Linq;

namespace butil_ui.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private string _title = string.Empty;
    public string Title
    {
        get
        {
            return _title;
        }
        set
        {
            if (value == _title)
                return;
            _title = value;
            this.OnPropertyChanged(nameof(Title));
        }
    }


    /// <summary>
    /// Gets the current page. The property is read-only
    /// </summary>
    public PageViewModelBase CurrentPage
    {
        get; set;
    }

    public MainWindowViewModel()
    {
        var args = Environment.GetCommandLineArgs().Skip(1).ToArray();
        if (args.Length == 2 && args[0].ToUpperInvariant() == TasksAppArguments.LaunchTask.ToUpperInvariant())
        {
            string taskName = null;
            foreach (var argument in args)
            {
                if (argument.StartsWith(TasksAppArguments.RunTask) && argument.Length > TasksAppArguments.RunTask.Length)
                {
                    taskName = argument.Substring(TasksAppArguments.RunTask.Length + 1);
                }
            }
            CurrentPage = new LaunchTaskViewModel() { TaskName = taskName, WindowTitle = taskName };
            Title = taskName;
        }
    }
}
