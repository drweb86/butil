using butil_ui.ViewModels;
using System;

namespace butil_ui;

internal static class WindowManager
{
    internal static Action<ViewModelBase>? _switchView;

    public static void SwitchView(ViewModelBase viewModel)
    {
        _switchView?.Invoke(viewModel);
    }
}
