using BUtil.Core.Events;
using BUtil.Core.TasksTree.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace butil_ui.ViewModels;

public class LaunchTaskViewItem(BuTask task, ProcessingStatus status) : ObservableObject
{
    public Guid Tag { get; } = task.Id;
    public string Text { get; } = task.Title;

    private ProcessingStatus _status = status;
    public ProcessingStatus Status
    {
        get => _status;
        set => this.SetProperty(ref _status, value);
    }
}
