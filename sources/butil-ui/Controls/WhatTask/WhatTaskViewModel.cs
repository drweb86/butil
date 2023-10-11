using BUtil.Core.Localization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace butil_ui.Controls
{
    internal class WhatTaskViewModel : ObservableObject
    {
        public WhatTaskViewModel()
        {
        }

        #region Labels
        public string LeftMenu_What => Resources.LeftMenu_What;

        #endregion

        public void Initialize()
        {
        }
    }
}
