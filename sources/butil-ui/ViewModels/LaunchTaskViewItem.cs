using Avalonia.Media;
using BUtil.Core.TasksTree.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace butil_ui.ViewModels;

public class LaunchTaskViewItem: ObservableObject
{
    public LaunchTaskViewItem(BuTask task)
    {
        Tag = task.Id;
        Text = task.Title;
        _backColor = new SolidColorBrush(Colors.White);
    }

    public Guid Tag { get; }
    public string Text { get; }
    public SolidColorBrush _backColor;

    public SolidColorBrush BackColor
    {
        get => _backColor;
        set => this.SetProperty(ref _backColor, value);
    }
}
