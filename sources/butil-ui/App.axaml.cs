using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using BUtil.Core.Options;
using BUtil.Core.Settings;
using butil_ui.ViewModels;
using butil_ui.Views;

namespace butil_ui;

public partial class App : Application
{
    public override void Initialize()
    {
        var settingsService = new SettingsStoreService();
        var theme = SettingsStoreService.Load(ThemeSetting.Name, ThemeSetting.DefaultValue);
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
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new AndroidView
            {
                DataContext = new MainWindowViewModel()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
