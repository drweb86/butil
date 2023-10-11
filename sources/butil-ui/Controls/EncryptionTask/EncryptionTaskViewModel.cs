using BUtil.Core.Localization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace butil_ui.Controls
{
    internal class EncryptionTaskViewModel : ObservableObject
    {
        public EncryptionTaskViewModel()
        {
        }

        #region Labels
        public string LeftMenu_Encryption => Resources.LeftMenu_Encryption;
        #endregion

        public void Initialize()
        {
        }
    }
}
