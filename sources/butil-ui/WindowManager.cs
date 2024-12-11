using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.FileSystem;
using BUtil.Core.Options;
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

    public static void SwitchToRestorationView(string? taskName = null)
    {
        if (string.IsNullOrWhiteSpace(taskName))
        {
            SwitchView(new RestoreViewModel(null, null));
            return;
        }

        var task = new TaskV2StoreService()
                .Load(taskName);

        if (task == null || (task.Model is not IncrementalBackupModelOptionsV2 && task.Model is not SynchronizationTaskModelOptionsV2))
            SwitchView(new RestoreViewModel(null, null));
        else if (task.Model is IncrementalBackupModelOptionsV2 v)
        {
            var incrementalOptions = v ?? throw new Exception();
            SwitchView(new RestoreViewModel(incrementalOptions.To, incrementalOptions.Password));
        }
        else if (task.Model is SynchronizationTaskModelOptionsV2 synchronizationTaskViewModelOptions)
        {
            var options = synchronizationTaskViewModelOptions ?? throw new Exception();
            SwitchView(new RestoreViewModel(options.To, options.Password));
        }
    }
}
