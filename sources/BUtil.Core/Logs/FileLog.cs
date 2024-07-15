
using BUtil.Core.Localization;
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

    public string LogFilename => _fileName;

    ~FileLog()
    {
        if (_logFile != null)
        {
            WriteLine(LoggingEvent.Error, "Memory leak " + new Exception().StackTrace);
            Close(false);
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
            throw new LogException(e.Message);
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
            _logFile = File.AppendText(_fileName);
        }
        catch (Exception e)
        {
            throw new LogException(e.Message, e);
        }
    }

    private void WriteInFile(string message)
    {
        if (_logFile == null)
            return;

        lock (_logFile)
            _logFile.WriteLine(message);
    }

    public override void WriteLine(LoggingEvent loggingEvent, string message)
    {
        if (_logFile == null)
            return;

        string output = GetFormattedMessage(loggingEvent, message);
        WriteInFile(output);
        if (loggingEvent == LoggingEvent.Error)
            lock (_logFile)
                _logFile.Flush();
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
        if (_logFile != null)
        {
            if (isSuccess)
            {
                //No any error or warning registered during backup!
                WriteInFile(Resources.Task_Status_Succesfull);
            }

            _logFile.Flush();
            _logFile.Close();

            var fileNameUpdated = GetFileName(isSuccess);
            File.Move(_fileName, fileNameUpdated);
            _fileName = fileNameUpdated;

            _logFile = null;

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
            GC.SuppressFinalize(this);
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
        }
    }
}
