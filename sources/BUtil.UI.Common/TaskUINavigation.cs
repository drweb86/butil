using System;

namespace BUtil.UI;

public static class TaskUINavigation
{
    public static Action? ReturnToTasksListAction { get; set; }

    public static void ReturnToTasksList() => ReturnToTasksListAction?.Invoke();
}
