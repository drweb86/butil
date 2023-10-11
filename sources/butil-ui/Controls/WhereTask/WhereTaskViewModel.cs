using BUtil.Core.Localization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace butil_ui.Controls
{
    internal class WhereTaskViewModel : ObservableObject
    {
        public WhereTaskViewModel()
        {
        }

        #region Labels
        public string LeftMenu_Where => Resources.LeftMenu_Where;

        #endregion

        public void Initialize()
        {
        }
    }
}
