using BUtil.Core;
using BUtil.Core.Events;
using BUtil.Core.TasksTree.Core;
using System;
using Terminal.Gui;

namespace BUtil.ConsoleBackup.UI
{
    class ListViewItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Status { get; private set; }
        public Color BackColor { get; private set; }
        public Color ForeColor { get; private set; }

        public ListViewItem(Guid id, string title)
        {
            Id = id;
            Title = title;
            SetStatus(ProcessingStatus.NotStarted);
        }

        public ListViewItem(BuTask task)
            : this(task.Id, task.Title)
        {
        }

        public void SetStatus(ProcessingStatus status)
        {
            Status = LocalsHelper.ToString(status);
            BackColor = GetBackColor(status);
            ForeColor = GetForeColor(status);
        }

        public override string ToString()
        {
            return $"[{Status}] {Title}";
        }

        private static Color GetBackColor(ProcessingStatus state)
        {
            return state switch
            {
                ProcessingStatus.FinishedSuccesfully => Color.BrightGreen,
                ProcessingStatus.FinishedWithErrors => Color.BrightRed,
                ProcessingStatus.InProgress => Color.BrightYellow,
                ProcessingStatus.NotStarted => Color.Gray,
                _ => throw new NotImplementedException(state.ToString()),
            };
        }

        private static Color GetForeColor(ProcessingStatus state)
        {
            return state switch
            {
                ProcessingStatus.FinishedSuccesfully => Color.White,
                ProcessingStatus.FinishedWithErrors => Color.White,
                ProcessingStatus.InProgress => Color.Black,
                ProcessingStatus.NotStarted => Color.Black,
                _ => throw new NotImplementedException(state.ToString()),
            };
        }
    }
}
