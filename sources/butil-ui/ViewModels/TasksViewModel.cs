using Avalonia.Media;
using BUtil.Core;
using BUtil.Core.Localization;
using BUtil.Core.Misc;
using BUtil.Core.Options;
using BUtil.Core.Settings;
using System.Collections.ObjectModel;

namespace butil_ui.ViewModels;

public class TasksViewModel : PageViewModelBase
{
    private readonly Color _errorForegroundColor;
    private readonly Color _successForegroundColor;

    public TasksViewModel(string theme)
    {
        _theme = theme;
        UpdateTheme(theme);
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

    #region DarkThemeLabel

    private string _darkThemeLabel = string.Empty;
    public string DarkThemeLabel
    {
        get
        {
            return _darkThemeLabel;
        }
        set
        {
            if (value == _darkThemeLabel)
                return;
            _darkThemeLabel = value;
            OnPropertyChanged(nameof(DarkThemeLabel));
        }
    }

    #endregion

    #region LightThemeLabel

    private string _lightThemeLabel = string.Empty;
    public string LightThemeLabel
    {
        get
        {
            return _lightThemeLabel;
        }
        set
        {
            if (value == _lightThemeLabel)
                return;
            _lightThemeLabel = value;
            OnPropertyChanged(nameof(LightThemeLabel));
        }
    }

    #endregion

   

    #region Commands

    private void UpdateTheme(string theme)
    {
        DarkThemeLabel = theme == ThemeSetting.DarkValue ? "⚫" + Resources.Theme_Value_Dark : "⚪" + Resources.Theme_Value_Dark;
        LightThemeLabel = theme == ThemeSetting.LightValue ? "⚫" + Resources.Theme_Value_Light : "⚪" + Resources.Theme_Value_Light;
    }

    public void GoDarkSide()
    {
        var settingsService = new SettingsStoreService();
        settingsService.Save(ThemeSetting.Name, ThemeSetting.DarkValue);
        UpdateTheme(ThemeSetting.DarkValue);
    }

    public void GoLightSide()
    {
        var settingsService = new SettingsStoreService();
        settingsService.Save(ThemeSetting.Name, ThemeSetting.LightValue);
        UpdateTheme(ThemeSetting.LightValue);
    }

    public void OpenLogsCommand()
    {
        SupportManager.OpenLogs();
    }

    public void RestoreCommand()
    {
        SupportManager.OpenRestorationApp();
    }

    public void GoToWebsiteCommand()
    {
        SupportManager.OpenHomePage();
    }

    #endregion

    #region Labels
    public string Theme_Title => "🎨 " + Resources.Theme_Title;
    public string Task_Restore => Resources.Task_Restore;
    public string LogFile_OpenLogs => Resources.LogFile_OpenLogs;


    // delete.
    public string Task_Launch_Hint => Resources.Task_Launch_Hint;
    public string Button_Cancel => Resources.Button_Cancel;
    public string AfterTaskSelection_Field => Resources.AfterTaskSelection_Field;
    public string Button_Close => Resources.Button_Close;
    public string AfterTaskSelection_Help => Resources.AfterTaskSelection_Help;

    #endregion

    private readonly string _theme;

    public void Initialize()
    {
    }
}
