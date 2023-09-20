using BUtil.Core.Logs;

namespace BUtil.ConsoleBackup.UI.Tasks
{
    class TasksViewItem
    {
        private readonly string _prefix;
        public string TaskName { get; set; }

        public TasksViewItem(string taskName, LogFileInfo lastLogFile)
        {
            TaskName = taskName;

            _prefix = "-           ";
            if (lastLogFile != null)
            {
                var postfix = lastLogFile.IsSuccess.HasValue ?
                    (lastLogFile.IsSuccess.Value ? "✅" : "❌")
                    : "❓";
                _prefix = $"{postfix}{lastLogFile.CreatedAt:yyyy-MM-dd}";
            }
        }

        public override string ToString()
        {
            return $"{_prefix} {TaskName}";
        }
    }
}
