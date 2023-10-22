using Avalonia.Media;
using BUtil.Core.Events;
using BUtil.Core.TasksTree.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace butil_ui.ViewModels;

public class LaunchTaskViewItem: ObservableObject
{
    public LaunchTaskViewItem(BuTask task, ProcessingStatus status)
    {
        Tag = task.Id;
        Text = task.Title;
        _status = status;
    }

    public Guid Tag { get; }
    public string Text { get; }
    public ProcessingStatus _status;

    public ProcessingStatus Status
    {
        get => _status;
        set => this.SetProperty(ref _status, value);
    }
}
