using BUtil.Core.Localization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace butil_ui.Controls
{
    internal class WhenTaskViewModel : ObservableObject
    {
        public WhenTaskViewModel()
        {
        }

        #region Labels
        public string LeftMenu_When => Resources.LeftMenu_When;

        #endregion

        public void Initialize()
        {
        }
    }
}
