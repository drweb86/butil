using Avalonia.Threading;
using BUtil.Core;
using BUtil.Core.Localization;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace butil_ui.Controls;

public class ProgressTaskViewModel : ObservableObject
{
    private Thread _thread;

    public void Activate(Func<Action<int>, Task> action)
    {
        IsVisible = true;
        _thread = new Thread(() =>
        {
            Thread.CurrentThread.IsBackground = true;
            PlatformSpecificExperience.Instance.OsSleepPreventionService.PreventSleep();
            action(progress => Dispatcher.UIThread.Invoke(() => this.Progress = progress)).Wait();
        });
        _thread.Start();
    }


    #region Labels

    public string Task_Status_InProgress => Resources.Task_Status_InProgress;

    #endregion

    #region Progress

    private int _progress = 0;

    public int Progress
    {
        get
        {
            return _progress;
        }
        set
        {
            if (value == _progress)
                return;
            _progress = value;
            OnPropertyChanged(nameof(Progress));
        }
    }

    #endregion

    #region IsVisible

    private bool _isVisible = false;

    public bool IsVisible
    {
        get
        {
            return _isVisible;
        }
        set
        {
            if (value == _isVisible)
                return;
            _isVisible = value;
            OnPropertyChanged(nameof(IsVisible));
        }
    }

    #endregion
}
