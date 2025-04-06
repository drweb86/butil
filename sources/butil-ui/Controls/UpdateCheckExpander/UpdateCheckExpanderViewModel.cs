using BUtil.Core;
using BUtil.Core.Localization;
using BUtil.Core.Misc;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;

namespace butil_ui.Controls.UpdateCheckExpander;

internal class UpdateCheckExpanderViewModel : ObservableObject
{
    public void Initialize()
    {
#if RELEASE
        _ = CheckForUpdates();
#endif
    }


    #region IsUpdateAvailable

    private bool _isUpdateAvailable = false;
    public bool IsUpdateAvailable
    {
        get
        {
            return _isUpdateAvailable;
        }
        set
        {
            if (value == _isUpdateAvailable)
                return;
            _isUpdateAvailable = value;
            OnPropertyChanged(nameof(IsUpdateAvailable));
        }
    }

    #endregion

    #region UpdateNews

    private string _updateNews = string.Empty;
    public string UpdateNews
    {
        get
        {
            return _updateNews;
        }
        set
        {
            if (value == _updateNews)
                return;
            _updateNews = value;
            OnPropertyChanged(nameof(UpdateNews));
        }
    }

    #endregion

    #region UpdateNewsTitle

    private string _updateNewsTitle = string.Empty;
    public string UpdateNewsTitle
    {
        get
        {
            return _updateNewsTitle;
        }
        set
        {
            if (value == _updateNewsTitle)
                return;
            _updateNewsTitle = value;
            OnPropertyChanged(nameof(UpdateNewsTitle));
        }
    }

    #endregion

    public static string UpdateLink => ApplicationLinks.LatestRelease;
    public static string Button_Download => Resources.Button_Download;
    public bool CanOpenLink { get; } = PlatformSpecificExperience.Instance.SupportManager.CanOpenLink;
#pragma warning disable CA1822 // Mark members as static
    public void OpenLatestReleaseCommand()
#pragma warning restore CA1822 // Mark members as static
    {
        PlatformSpecificExperience.Instance
            .SupportManager
            .OpenLatestRelease();
    }

    private async Task CheckForUpdates()
    {
        var update = await UpdateChecker.CheckForUpdateGithub();

        IsUpdateAvailable = update.HasUpdate;
        if (IsUpdateAvailable)
        {
            UpdateNews = update.Changes ?? string.Empty;
            UpdateNewsTitle = string.Format(Resources.Application_NewVersion_Notification, update.Version);
        }
    }
}
