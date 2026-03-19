
using BUtil.Core.Localization;
using BUtil.Core.Misc;
using System;
using System.Globalization;
using System.IO;

namespace BUtil.Core.Logs;


public class FileLog : LogBase
{
    private string _fileName;
    private readonly string _taskName;
    private StreamWriter? _logFile;
    private readonly DateTime _dateTime;
    private readonly object _sync = new();

    public string LogFilename => _fileName;

    ~FileLog()
    {
        var logFile = DetachLogFile();
        if (logFile == null)
            return;

        try
        {
            WriteInFile(logFile, "Memory leak " + new Exception().StackTrace);
        }
        catch
        {
            // Never throw from finalizer thread.
        }

        try
        {
            Close(logFile, false);
        }
        catch
        {
            // Never throw from finalizer thread.
        }
    }

    public FileLog(string taskName)
    {
        _taskName = taskName;
        try
        {
            do
            {
                _dateTime = DateTime.Now;

                _fileName = GetFileName(null);
            }
            while (File.Exists(_fileName));
        }
        catch (ArgumentException e)
        {
            throw new LogException(ExceptionHelper.ToString(e));
        }
        _taskName = taskName;
    }

    private string GetFileName(bool? isSuccess)
    {
        return LogService.GetFileName(_taskName, _dateTime, isSuccess);
    }

    public override void Open()
    {
        try
        {
            File.WriteAllText(
                _fileName,
                @$"{_dateTime.ToString("f", CultureInfo.CurrentUICulture)} {LocalsHelper.ToString(Events.ProcessingStatus.FinishedSuccesfully)}{LocalsHelper.ToString(Events.ProcessingStatus.FinishedWithErrors)}{LocalsHelper.ToString(Events.ProcessingStatus.Skipped)}" + Environment.NewLine);
            lock (_sync)
            {
                _logFile = File.AppendText(_fileName);
            }
        }
        catch (Exception e)
        {
            throw new LogException(ExceptionHelper.ToString(e), e);
        }
    }

    private static void WriteInFile(StreamWriter logFile, string message)
    {
        lock (logFile)
            logFile.WriteLine(message);
    }

    private StreamWriter? GetCurrentLogFile()
    {
        lock (_sync)
            return _logFile;
    }

    private StreamWriter? DetachLogFile()
    {
        lock (_sync)
        {
            var current = _logFile;
            _logFile = null;
            return current;
        }
    }

    private string GetCurrentFileName()
    {
        lock (_sync)
            return _fileName;
    }

    private void SetCurrentFileName(string fileName)
    {
        lock (_sync)
            _fileName = fileName;
    }

    public override void WriteLine(LoggingEvent loggingEvent, string message)
    {
        var logFile = GetCurrentLogFile();
        if (logFile == null)
            return;

        string output = GetFormattedMessage(loggingEvent, message);
        WriteInFile(logFile, output);
        if (loggingEvent == LoggingEvent.Error)
            lock (logFile)
                logFile.Flush();
    }

    private static string GetFormattedMessage(LoggingEvent loggingEvent, string message)
    {
        var information = string.Format(CultureInfo.CurrentUICulture,
                        "{0:HH:mm} {1}",
                        DateTime.Now,
                        message);

        if (loggingEvent == LoggingEvent.Error)
            return $"⛔ERROR {information}";
        else
            return $"✅ {information}";
    }

    public override void Close(bool isSuccess)
    {
        var logFile = DetachLogFile();
        if (logFile == null)
            return;

        Close(logFile, isSuccess);

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
        GC.SuppressFinalize(this);
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
    }

    private void Close(StreamWriter logFile, bool isSuccess)
    {
        try
        {
            if (isSuccess)
            {
                //No any error or warning registered during backup!
                WriteInFile(logFile, Resources.Task_Status_Succesfull);
            }

            logFile.Flush();
            logFile.Close();

            var oldFileName = GetCurrentFileName();
            var fileNameUpdated = GetFileName(isSuccess);
            File.Move(oldFileName, fileNameUpdated);
            SetCurrentFileName(fileNameUpdated);
        }
        catch (Exception e)
        {
            throw new LogException(ExceptionHelper.ToString(e), e);
        }
    }
}
