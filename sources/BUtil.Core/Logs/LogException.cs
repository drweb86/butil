using System;

namespace BUtil.Core.Logs;

public class LogException(string message, Exception? innerException = null) : Exception(message, innerException)
{
}
