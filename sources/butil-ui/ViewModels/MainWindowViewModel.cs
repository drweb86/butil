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
using System.Xml.Linq;

namespace butil_ui.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public static bool CanExtendClientAreaToDecorationsHint => PlatformSpecificExperience.Instance.UiService.CanExtendClientAreaToDecorationsHint;

    public static string Theme_Value_Dark => Resources.Theme_Value_Dark;

    public static string Theme_Value_Light => Resources.Theme_Value_Light;

    #region Commands

#pragma warning disable CA1822 // Mark members as static
    public void GoDarkSide()
#pragma warning restore CA1822 // Mark members as static
    {
        SettingsStoreService.Save(ThemeSetting.Name, ThemeSetting.DarkValue);
        PlatformSpecificExperience.Instance
                .SupportManager
                .LaunchTasksAppOrExit();
        Environment.Exit(0);
    }

#pragma warning disable CA1822 // Mark members as static
    public void GoLightSide()
#pragma warning restore CA1822 // Mark members as static
    {
        SettingsStoreService.Save(ThemeSetting.Name, ThemeSetting.LightValue);
        PlatformSpecificExperience.Instance
                .SupportManager
                .LaunchTasksAppOrExit();
        Environment.Exit(0);
    }

#pragma warning disable CA1822 // Mark members as static
    public void OpenLogsCommand()
#pragma warning restore CA1822 // Mark members as static
    {
        PlatformSpecificExperience.Instance
                .GetFolderService()
                .OpenFolderInShell(Directories.LogsFolder);
    }

#pragma warning disable CA1822 // Mark members as static
    public void RestoreCommand()
#pragma warning restore CA1822 // Mark members as static
    {
        WindowManager.SwitchToRestorationView();
    }

#pragma warning disable CA1822 // Mark members as static
    public void IncrementalBackupTaskCreateCommand()
#pragma warning restore CA1822 // Mark members as static
    {
        WindowManager.SwitchView(new EditIncrementalBackupTaskViewModel(string.Empty, true));
    }

#pragma warning disable CA1822 // Mark members as static
    public void GoToWebsiteCommand()
#pragma warning restore CA1822 // Mark members as static
    {
        PlatformSpecificExperience.Instance
                .SupportManager
                .OpenHomePage();
    }

#pragma warning disable CA1822 // Mark members as static
    public void ImportMediaTaskCreateCommand()
#pragma warning restore CA1822 // Mark members as static
    {
        WindowManager.SwitchView(new EditMediaTaskViewModel(string.Empty, true));
    }

#pragma warning disable CA1822 // Mark members as static
    public void SynchronizationTaskCreateCommand()
#pragma warning restore CA1822 // Mark members as static
    {
        WindowManager.SwitchView(new EditSynchronizationTaskViewModel(string.Empty, true));
    }

    #endregion

    #region Labels
    public static string Theme_Title => Resources.Theme_Title;
    public static string Task_Restore => Resources.Task_Restore;
    public static string LogFile_OpenLogs => Resources.LogFile_OpenLogs;
    public static string Task_Launch_Hint => Resources.Task_Launch_Hint;
    public static string Task_Create => Resources.Task_Create;
    public static string ImportMediaTask_Create => Resources.ImportMediaTask_Create;
    public static string IncrementalBackupTask_Create => Resources.IncrementalBackupTask_Create;
    public static string SynchronizationTask_Create => Resources.SynchronizationTask_Create;

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
        set
        {
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
        ApplicationSettings.Theme = SettingsStoreService.Load(ThemeSetting.Name, ThemeSetting.DefaultValue);
        WindowBackground = ColorPalette.GetBrush(SemanticColor.WindowBackground);
        HeaderBackground = ColorPalette.GetBrush(SemanticColor.HeaderBackground);

        WindowManager._switchView = x => CurrentPage = x;
        var args = Environment.GetCommandLineArgs().Skip(1).ToArray();

        if (args.Length == 2 && args[0].Cmp(TasksAppArguments.LaunchTask) && args[1].StartsWith(TasksAppArguments.RunTask))
        {
            var taskName = args[1][(TasksAppArguments.RunTask.Length + 1)..];
            WindowManager.SwitchToRestorationView(taskName);
        }
        else if (args.Length == 1 && args[0].EndsWith(IncrementalBackupModelConstants.BrotliAes256V1StateFile))
        {
            var folderStorage = new FolderStorageSettingsV2 { DestinationFolder = System.IO.Path.GetDirectoryName(args[0]) ?? throw new Exception() };
            WindowManager.SwitchView(new RestoreViewModel(folderStorage, null));
        }
        else
        {
            WindowManager.SwitchView(new TasksViewModel());
        }
    }
}
