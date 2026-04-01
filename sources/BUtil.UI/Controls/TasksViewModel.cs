using Avalonia.Media;
using BUtil.Core;
using BUtil.Core.Events;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace BUtil.UI.Controls;

public class TasksViewModel : ViewModelBase
{
    public TasksViewModel()
    {
        IsFullMenuVisible = true;
        ProgressGenericForeground = ColorPalette.GetBrush(SemanticColor.Normal);

        WindowTitle = "BUtil - V" + CopyrightInfo.Version.ToString(3);
        HeaderBackground = ColorPalette.GetBrush(SemanticColor.HeaderBackground);
        ForegroundWindowFontAccented = ColorPalette.GetBrush(SemanticColor.ForegroundWindowFontAccented);

        PropertyChanged += OnSelfPropertyChanged;
    }

    private void OnSelfPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SearchText))
            ApplyFilter();
    }
    public SolidColorBrush HeaderBackground { get; }
    public SolidColorBrush ForegroundWindowFontAccented { get; }
    public SolidColorBrush ProgressGenericForeground { get; }

    #region Items

    private readonly List<TaskCardViewModel> _allItems = [];

    private ObservableCollection<TaskCardViewModel> _items = [];
    public ObservableCollection<TaskCardViewModel> Items
    {
        get
        {
            return _items;
        }
        set
        {
            if (value == _items)
                return;
            _items = value;
            OnPropertyChanged(nameof(Items));
        }
    }

    private void ApplyFilter()
    {
        var filter = SearchText?.Trim() ?? string.Empty;
        var filtered = string.IsNullOrEmpty(filter)
            ? _allItems
            : _allItems.Where(x => x.Name.Contains(filter, StringComparison.OrdinalIgnoreCase)).ToList();

        _items.Clear();
        foreach (var item in filtered)
            _items.Add(item);
    }

    #endregion

    #region Commands


    #endregion

    #region Labels
    public static string Task_LastExecution_State => Resources.Task_LastExecution_State;
    public static string Task_Launch_Hint => Resources.Task_Launch_Hint;

    #endregion

    public void Initialize()
    {
        LoadTasks();
    }

    public void ReloadTasks()
    {
        _allItems.Clear();
        _items.Clear();
        LoadTasks();
    }

    private void LoadTasks()
    {
        var store = new TaskStore(new LocalFileSystem());
        var taskNames = store.GetNames();
        var lastLogs = new LogService().GetRecentLogs();

        foreach (var taskName in taskNames)
        {
            var lastLogFile = lastLogs.FirstOrDefault(x => x.TaskName.Cmp(taskName));

            var status = ProcessingStatus.NotStarted;
            if (lastLogFile != null)
            {
                if (lastLogFile.IsSuccess.HasValue)
                    status = lastLogFile.IsSuccess.Value ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors;
                else
                    status = ProcessingStatus.InProgress;
            }

            var listViewItem = new TaskCardViewModel(taskName, lastLogFile?.CreatedAt, status, _items, lastLogFile?.File, ReloadTasks);
            _allItems.Add(listViewItem);
        }

        ApplyFilter();
    }

}
