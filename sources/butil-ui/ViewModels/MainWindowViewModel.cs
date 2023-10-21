using Avalonia.Media;
using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;
using BUtil.Core.Misc;
using BUtil.Core.Options;
using BUtil.Core.Settings;
using butil_ui.Controls;
using System;
using System.Linq;

namespace butil_ui.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string Theme_Value_Dark => Resources.Theme_Value_Dark;

    public string Theme_Value_Light => Resources.Theme_Value_Light;

    #region Commands

    public void GoDarkSide()
    {
        new SettingsStoreService()
            .Save(ThemeSetting.Name, ThemeSetting.DarkValue);
        SupportManager.LaunchTasksApp();
        Environment.Exit(0);
    }

    public void GoLightSide()
    {
        new SettingsStoreService()
            .Save(ThemeSetting.Name, ThemeSetting.LightValue);
        SupportManager.LaunchTasksApp();
        Environment.Exit(0);
    }

    public void OpenLogsCommand()
    {
        SupportManager.OpenLogs();
    }

    public void RestoreCommand()
    {
        SupportManager.OpenRestorationApp();
    }

    public void IncrementalBackupTaskCreateCommand()
    {
        WindowManager.SwitchView(new EditIncrementalBackupTaskViewModel(string.Empty, true));
    }

    public void GoToWebsiteCommand()
    {
        SupportManager.OpenHomePage();
    }

    public void ImportMediaTaskCreateCommand()
    {
        WindowManager.SwitchView(new EditMediaTaskViewModel(string.Empty, true));
    }

    #endregion

    #region Labels
    public string Theme_Title => Resources.Theme_Title;
    public string Task_Restore => Resources.Task_Restore;
    public string LogFile_OpenLogs => Resources.LogFile_OpenLogs;
    public string Task_Launch_Hint => Resources.Task_Launch_Hint;
    public string Task_Create => Resources.Task_Create;
    public string ImportMediaTask_Create => Resources.ImportMediaTask_Create;
    public string IncrementalBackupTask_Create => Resources.IncrementalBackupTask_Create;

    #endregion

    public SolidColorBrush WindowBackground { get; }
    public SolidColorBrush HeaderBackground { get; }

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


    private ViewModelBase? _currentPage;
    /// <summary>
    /// Gets the current page. The property is read-only
    /// </summary>
    public ViewModelBase? CurrentPage
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
        var settingsService = new SettingsStoreService();
        ApplicationSettings.Theme = settingsService.Load(ThemeSetting.Name, ThemeSetting.DefaultValue);
        WindowBackground = new SolidColorBrush(ColorPalette.GetColor(SemanticColor.WindowBackground));
        HeaderBackground = new SolidColorBrush(ColorPalette.GetColor(SemanticColor.HeaderBackground));

        WindowManager._switchView = x => CurrentPage = x;
        var args = Environment.GetCommandLineArgs().Skip(1).ToArray();

        if (args.Length == 2 && args[0].Cmp(TasksAppArguments.LaunchTask) && args[1].StartsWith(TasksAppArguments.RunTask))
        {
            var taskName = args[1].Substring(TasksAppArguments.RunTask.Length + 1);
            WindowManager.SwitchView(new LaunchTaskViewModel(taskName));
        } else if (args.Length == 1 && args[0].Cmp(TasksAppArguments.Restore))
        {
            WindowManager.SwitchView(new RestoreViewModel(null, null));
        } else if (args.Length == 1 && args[0].EndsWith(IncrementalBackupModelConstants.StorageIncrementalEncryptedCompressedStateFile))
        {
            var folderStorage = new FolderStorageSettingsV2 { DestinationFolder = System.IO.Path.GetDirectoryName(args[0]) ?? throw new Exception() };
            WindowManager.SwitchView(new RestoreViewModel(folderStorage, null));
        }
        else if (args.Length == 2 && args[0].Cmp(TasksAppArguments.Restore) && args[1].StartsWith(TasksAppArguments.RunTask))
        {
            var taskName = args[1].Substring(TasksAppArguments.RunTask.Length + 1);
            var task = new TaskV2StoreService().Load(taskName);
            if (task == null || !(task.Model is IncrementalBackupModelOptionsV2))
                WindowManager.SwitchView(new RestoreViewModel(null, null));
            else
            {
                var incrementalOptions = (IncrementalBackupModelOptionsV2)task.Model ?? throw new Exception();
                WindowManager.SwitchView(new RestoreViewModel(incrementalOptions.To, incrementalOptions.Password));
            }
        }
        else
        {
            WindowManager.SwitchView(new TasksViewModel());
        }
    }
}
