using BUtil.Interop.Tasks;
using BUtil.Interop.Tasks.UI;
using BUtil.Tasks.IncrementalBackup;
using BUtil.Tasks.Synchronization;
using BUtil.Core.FileSystem;
using BUtil.Core.Services;
using BUtil.UI.Controls;
using System;

namespace BUtil.UI;

internal static class WindowManager
{
    internal static Action<ViewModelBase>? _switchView;

    public static void SwitchView(ViewModelBase viewModel)
    {
        _switchView?.Invoke(viewModel);
    }

    public static void SwitchToCreateTaskView(Type modelType)
    {
        var content = TaskUIProviderRegistry.CreateNew(modelType);
        if (content != null)
            SwitchView(new TaskUIViewModel(content, TaskUIProviderRegistry.GetCreateHeader(modelType), isNew: true));
    }

    public static void SwitchToEditTaskView(Type modelType, string taskName)
    {
        var content = TaskUIProviderRegistry.CreateEdit(modelType, taskName);
        if (content != null)
            SwitchView(new TaskUIViewModel(content, taskName, isNew: false));
    }

    public static void SwitchToLaunchTask(string taskName)
    {
        SwitchView(new LaunchTaskViewModel(taskName));
    }

    public static void SwitchToRestorationView(string? taskName = null)
    {
        if (string.IsNullOrWhiteSpace(taskName))
        {
            SwitchView(new RestoreViewModel(null, null));
            return;
        }

        var task = new TaskStore(new LocalFileSystem())
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
