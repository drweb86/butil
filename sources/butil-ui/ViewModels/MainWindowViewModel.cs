using BUtil.Core;
using BUtil.Core.FileSystem;
using BUtil.Core.Options;
using BUtil.Core.Settings;
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


    private PageViewModelBase _currentPage;
    /// <summary>
    /// Gets the current page. The property is read-only
    /// </summary>
    public PageViewModelBase? CurrentPage
    {
        get => _currentPage;
        set {
            if (value != null && value != _currentPage)
            {
                _currentPage = value;
                this.OnPropertyChanged(nameof(CurrentPage));
            }
        }
    }

    public MainWindowViewModel()
    {
        var args = Environment.GetCommandLineArgs().Skip(1).ToArray();
        var settingsService = new SettingsStoreService();
        ApplicationSettings.Theme = settingsService.Load(ThemeSetting.Name, ThemeSetting.DefaultValue);

        if (args.Length == 2 && args[0].ToUpperInvariant() == TasksAppArguments.LaunchTask.ToUpperInvariant())
        {
            var taskName = string.Empty;
            foreach (var argument in args)
            {
                if (argument.StartsWith(TasksAppArguments.RunTask) && argument.Length > TasksAppArguments.RunTask.Length)
                {
                    taskName = argument.Substring(TasksAppArguments.RunTask.Length + 1);
                }
            }

            CurrentPage = new LaunchTaskViewModel(taskName);
        } else
        {
            Action<PageViewModelBase> change = x =>
            {
                CurrentPage = x;
            };
            CurrentPage = new TasksViewModel(change);
        }
    }
}
