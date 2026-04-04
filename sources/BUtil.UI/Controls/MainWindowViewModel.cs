using Avalonia.Media;
using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;
using BUtil.Core.Misc;
using BUtil.Core.Options;
using BUtil.Core.Settings;
using System;
using System.Linq;

namespace BUtil.UI.Controls;

public partial class MainWindowViewModel : ViewModelBase
{
    public static bool CanExtendClientAreaToDecorationsHint => PlatformSpecificExperience.Instance.UiService.CanExtendClientAreaToDecorationsHint;

    public static string Theme_Value_Dark => Resources.Theme_Value_Dark;

    public static string Theme_Value_Light => Resources.Theme_Value_Light;

    public static string AppVersionAndSiteLink => "BUtil - V" + CopyrightInfo.Version.ToString(3);

    #region Commands

#pragma warning disable CA1822 // Mark members as static
    public void GoDarkSide()
#pragma warning restore CA1822 // Mark members as static
    {
        new SettingsStoreService(new LocalFileSystem()).Save(ThemeSetting.Name, ThemeSetting.DarkValue);
        PlatformSpecificExperience.Instance
                .SupportManager
                .LaunchTasksAppOrExit();
        Environment.Exit(0);
    }

#pragma warning disable CA1822 // Mark members as static
    public void GoLightSide()
#pragma warning restore CA1822 // Mark members as static
    {
        new SettingsStoreService(new LocalFileSystem()).Save(ThemeSetting.Name, ThemeSetting.LightValue);
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
    public void TechnicalToolDecryptAes256Command()
#pragma warning restore CA1822 // Mark members as static
    {
        WindowManager.SwitchView(new TechnicalFileToolViewModel(TechnicalFileToolKind.DecryptAes256));
    }

#pragma warning disable CA1822 // Mark members as static
    public void TechnicalToolEncryptAes256Command()
#pragma warning restore CA1822 // Mark members as static
    {
        WindowManager.SwitchView(new TechnicalFileToolViewModel(TechnicalFileToolKind.EncryptAes256));
    }

#pragma warning disable CA1822 // Mark members as static
    public void TechnicalToolDecompressBrotliCommand()
#pragma warning restore CA1822 // Mark members as static
    {
        WindowManager.SwitchView(new TechnicalFileToolViewModel(TechnicalFileToolKind.DecompressBrotli));
    }

#pragma warning disable CA1822 // Mark members as static
    public void TechnicalToolCompressBrotliCommand()
#pragma warning restore CA1822 // Mark members as static
    {
        WindowManager.SwitchView(new TechnicalFileToolViewModel(TechnicalFileToolKind.CompressBrotli));
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
        WindowManager.SwitchView(new EditIncrementalBackupTaskViewModel(Resources.IncrementalBackupTask_Create, true));
    }

    public bool CanOpenLink { get; } = PlatformSpecificExperience.Instance.SupportManager.CanOpenLink;

#pragma warning disable CA1822 // Mark members as static
    public void GoToWebsiteCommand()
#pragma warning restore CA1822 // Mark members as static
    {
        if (!PlatformSpecificExperience.Instance.SupportManager.CanOpenLink)
            return;

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
    public void FileSenderServerCreateCommand()
#pragma warning restore CA1822 // Mark members as static
    {
        WindowManager.SwitchView(new EditBUtilServerTaskViewModel(Resources.FtpsServerTask_Create, true));
    }

#pragma warning disable CA1822 // Mark members as static
    public void FileSenderClientCreateCommand()
#pragma warning restore CA1822 // Mark members as static
    {
        WindowManager.SwitchView(new EditBUtilServerClientTaskViewModel(Resources.UploadFolderContentsTask_Create, true));
    }

#pragma warning disable CA1822 // Mark members as static
    public void SynchronizationTaskCreateCommand()
#pragma warning restore CA1822 // Mark members as static
    {
        WindowManager.SwitchView(new EditSynchronizationTaskViewModel(Resources.SynchronizationTask_Create, true));
    }

    #endregion

    #region Labels
    public static string Theme_Title => Resources.Theme_Title;
    public static string Task_Restore => Resources.Task_Restore;
    public static string Logs_Menu => Resources.Logs_Menu;
    public static string LogFile_BrowseLogsFolder => Resources.LogFile_BrowseLogsFolder;
    public static string Other_Menu_Title => Resources.Other_Menu_Title;
    public static string TechnicalTool_Menu_DecryptAes256 => Resources.TechnicalTool_Menu_DecryptAes256;
    public static string TechnicalTool_Menu_EncryptAes256 => Resources.TechnicalTool_Menu_EncryptAes256;
    public static string TechnicalTool_Menu_DecompressBrotli => Resources.TechnicalTool_Menu_DecompressBrotli;
    public static string TechnicalTool_Menu_CompressBrotli => Resources.TechnicalTool_Menu_CompressBrotli;
    public static string Task_Launch_Hint => Resources.Task_Launch_Hint;
    public static string Task_Create => Resources.Task_Create;
    public static string ImportMediaTask_Create => Resources.ImportMediaTask_Create;
    public static string IncrementalBackupTask_Create => Resources.IncrementalBackupTask_Create;
    public static string SynchronizationTask_Create => Resources.SynchronizationTask_Create;

    public static string FileSenderServerTask_Create => Resources.FtpsServerTask_Create;
    public static string FileSenderClientTask_Create => Resources.UploadFolderContentsTask_Create;

    /// <summary>Emoji watermark on Windows (good font coverage); localized text elsewhere (e.g. Linux without color emoji fonts).</summary>
    public static string SearchTextBoxWatermark =>
        OperatingSystem.IsWindows() ? "\uD83D\uDD0D" : Resources.MainWindow_SearchWatermark;

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
        var settingsService = new SettingsStoreService(new LocalFileSystem());
        ApplicationSettings.Theme = settingsService.Load(ThemeSetting.Name, ThemeSetting.DefaultValue);
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
