using Avalonia.Controls.Primitives;
using Avalonia.Media;
using BUtil.Core;
using BUtil.Core.Localization;
using BUtil.Core.Misc;
using BUtil.Core.Options;
using BUtil.Core.Settings;
using butil_ui.ViewModels;
using System;

namespace butil_ui.Controls.TasksMainMenu
{
    internal class TasksMainMenuViewModel: ViewModelBase
    {
        private readonly Color _errorForegroundColor;
        private readonly Color _successForegroundColor;

        public TasksMainMenuViewModel()
        {
        }

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

        public void GoToWebsiteCommand()
        {
            SupportManager.OpenHomePage();
        }

        public void ImportMediaTaskCreateCommand()
        {

        }

        public void IncrementalBackupTaskCreateCommand()
        {
        }

        #endregion

        #region Labels
        public string Theme_Title => "🎨 " + Resources.Theme_Title;
        public string Task_Restore => Resources.Task_Restore;
        public string LogFile_OpenLogs => Resources.LogFile_OpenLogs;
        public string Task_Launch_Hint => Resources.Task_Launch_Hint;
        public string Task_Create => Resources.Task_Create;
        public string Task_Create_Hint => Resources.Task_Create_Hint;
        public string ImportMediaTask_Create => Resources.ImportMediaTask_Create;
        public string IncrementalBackupTask_Create => Resources.IncrementalBackupTask_Create;

        #endregion

        public void Initialize()
        {
            var theme = new SettingsStoreService().Load(ThemeSetting.Name, ThemeSetting.DefaultValue);
            UpdateTheme(theme);
        }
    }
}
