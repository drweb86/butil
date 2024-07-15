using Avalonia.Media;
using BUtil.Core;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using butil_ui.Controls;
using System.Collections.ObjectModel;
using System.Linq;

namespace butil_ui.ViewModels;

public class TasksViewModel : ViewModelBase
{
    public TasksViewModel()
    {
        IsFullMenuVisible = true;
        ProgressGenericForeground = ColorPalette.GetBrush(SemanticColor.Normal);

        WindowTitle = "BUtil - V" + CopyrightInfo.Version.ToString(3);
        HeaderBackground = ColorPalette.GetBrush(SemanticColor.HeaderBackground);
        ForegroundWindowFontAccented = ColorPalette.GetBrush(SemanticColor.ForegroundWindowFontAccented);
    }
    public SolidColorBrush HeaderBackground { get; }
    public SolidColorBrush ForegroundWindowFontAccented { get; }
    public SolidColorBrush ProgressGenericForeground { get; }

    #region Items

    private ObservableCollection<TaskItemViewModel> _items = [];
    public ObservableCollection<TaskItemViewModel> Items
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

    private void LoadTasks()
    {
        var taskNames = new TaskV2StoreService().GetNames();
        var lastLogs = new LogService().GetRecentLogs();

        foreach (var taskName in taskNames)
        {
            var lastLogFile = lastLogs.FirstOrDefault(x => x.TaskName == taskName);
            string lastLaunchedAt = lastLogFile != null ? lastLogFile.CreatedAt.ToString() : string.Empty;

            var status = ProcessingStatus.NotStarted;
            if (lastLogFile != null)
            {
                if (lastLogFile.IsSuccess.HasValue)
                    status = lastLogFile.IsSuccess.Value ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors;
                else
                    status = ProcessingStatus.InProgress;
            }
            var listViewItem = new TaskItemViewModel(taskName, lastLaunchedAt, status, _items);
            Items.Add(listViewItem);
        }
    }

}
