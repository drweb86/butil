using BUtil.Core.Localization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace butil_ui.Controls
{
    internal class NameTaskViewModel : ObservableObject
    {
        public NameTaskViewModel()
        {
        }

        #region Labels
        public string Name_Title => Resources.Name_Title;

        #endregion

        public void Initialize()
        {
        }
    }
}
