using System;

namespace BUtil.Core.Logs
{
    public class LogFileInfo
    {
        public string TaskName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool? IsSuccess { get; set; }
    }
}