using butil_ui.ViewModels;
using System;

namespace butil_ui
{
    internal static class WindowManager
    {
        internal static Action<PageViewModelBase>? _switchView;

        public static void SwitchView(PageViewModelBase viewModel)
        {
            _switchView?.Invoke(viewModel);
        }
    }
}
