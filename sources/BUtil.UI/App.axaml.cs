using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using BUtil.Core;
using BUtil.Core.FileSystem;
using BUtil.Core.Options;
using BUtil.Core.Settings;
using BUtil.Tasks.BUtilClient.UI;
using BUtil.Tasks.BUtilServer.UI;
using BUtil.Tasks.ImportMedia.UI;
using BUtil.Tasks.IncrementalBackup.UI;
using BUtil.Tasks.Synchronization.UI;
using BUtil.UI.Controls;

namespace BUtil.UI;

public partial class App : Application
{
    public override void Initialize()
    {
        var settingsService = new SettingsStoreService(new LocalFileSystem());
        var theme = settingsService.Load(ThemeSetting.Name, ThemeSetting.DefaultValue);
        if (theme == ThemeSetting.DarkValue)
        {
            this.RequestedThemeVariant = ThemeVariant.Dark;
        }
        else if (theme == ThemeSetting.LightValue)
        {
            this.RequestedThemeVariant = ThemeVariant.Light;
        }
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Accessing Instance triggers platform experience initialization, which registers all storages.
        _ = PlatformSpecificExperience.Instance;

        TaskUINavigation.ReturnToTasksListAction = () => WindowManager.SwitchView(new TasksViewModel());
        IncrementalBackupTaskUIPlugin.Register();
        SynchronizationTaskUIPlugin.Register();
        ImportMediaTaskUIPlugin.Register();
        BUtilServerTaskUIPlugin.Register();
        BUtilClientTaskUIPlugin.Register();

        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
