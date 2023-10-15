using BUtil.Core.Localization;
using BUtil.Core.Misc;
using butil_ui.ViewModels;
using System;
using System.Threading.Tasks;

namespace butil_ui.Controls.UpdateCheckExpander
{
    internal class UpdateCheckExpanderViewModel: ViewModelBase
    {
        public void Initialize()
        {
#if DEBUG
            // _ = CheckForUpdates();
#else
            _ = CheckForUpdates(); // we don't wait it to not stop initialization
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

        public string UpdateLink => SupportManager.GetLink(SupportRequest.LatestRelease);
        public string Button_Download => Resources.Button_Download;

        public void OpenLatestReleaseCommand()
        {
            SupportManager.OpenLatestRelease();
        }

        private async Task CheckForUpdates()
        {
            try
            {
                var update = await UpdateChecker.CheckForUpdate();

                IsUpdateAvailable = update.HasUpdate;
                if (IsUpdateAvailable)
                {
                    UpdateNews = update.Changes ?? string.Empty;
                    UpdateNewsTitle = string.Format(Resources.Application_NewVersion_Notification, update.Version);
                }
            }
            catch (InvalidOperationException)
            {
                // For now lets eat those errors.
                // Messages.ShowErrorBox(exc.Message);
            }
        }
    }
}
