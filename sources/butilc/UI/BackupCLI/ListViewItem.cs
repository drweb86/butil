namespace BUtil.ConsoleBackup.UI
{
    using BUtil.Core;
    using BUtil.Core.Events;
    using BUtil.Core.TasksTree.Core;
    using System;

    class ListViewItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public Terminal.Gui.Color BackColor { get; set; }

        public ListViewItem(Guid id, string title)
        {
            Id = id;
            Title = title;
            Status = LocalsHelper.ToString(ProcessingStatus.NotStarted);
            BackColor = Terminal.Gui.Color.Gray;
        }

        public ListViewItem(BuTask task)
            : this(task.Id, task.Title)
        {
        }

        public override string ToString()
        {
            return $"[{Status}] {Title}";
        }
    }
}
