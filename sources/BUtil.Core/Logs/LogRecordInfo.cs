
using System;

namespace BUtil.Core.Logs;

public class LogFileInfo(string taskName, DateTime createdAt, bool? isSuccess, string file)
{
    public string TaskName { get; } = taskName;
    public DateTime CreatedAt { get; } = createdAt;
    public bool? IsSuccess { get; } = isSuccess;
    public string File { get; } = file;
}