using System;
using System.Globalization;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.Localization;
using BUtil.Core.Events;
using BUtil.Core.Misc;

namespace BUtil.Core.Jobs
{
    class ExecuteProgramJob : IJob
    {
        #region Fields

        // log messages
        readonly string _taskName;
        readonly ExecuteProgramTaskInfo _taskInfo;
        readonly LogBase _log;
        private readonly BackupEvents _events;
        EventHandler<JobThreadEventArgs> _finished;
        ProcessPriorityClass _priority;

        #endregion

        #region Events

        EventHandler<JobThreadEventArgs> IJob.TaskFinished
        {
            get { return _finished; }
            set { _finished = value; }
        }

        #endregion

        #region Constructors

        public ExecuteProgramJob(
            LogBase log,
            BackupEvents events,
            ExecuteProgramTaskInfo taskInfo,
            ProcessPriorityClass priority,
            string prefix)
        {
            if (!log.IsOpened)
            {
                throw new InvalidOperationException("Log is not opened");
            }

            _taskName = $"{prefix} {taskInfo} :";
            _taskInfo = taskInfo;
            _priority = priority;
            _log = log;
            _events = events;
        }

        #endregion

        #region Public methods

        public void DoJob()
        {
            _log.WriteLine(LoggingEvent.Debug, _taskName);

            bool succesfull = false;
            Process program = new()
            {
                StartInfo = new ProcessStartInfo(_taskInfo.Program, _taskInfo.Arguments)
                {
                    WorkingDirectory = Path.GetDirectoryName(_taskInfo.Program)
                }
            };

            try
            {
                program.Start();

                StatusUpdate(ProcessingStatus.InProgress);

                SetPriorityHelper(program);

                // FIXME: This is a workaround of .Net 2 bugs with processing ThreadAbortException and ThreadInterruptException
                // both bugs were posted to MS
                // this code is subject to be removed when those bugs will be fixed or on updating to next .Net version
                while (!program.HasExited)
                {
                    Thread.Sleep(100);
                }
                // program.WaitForExit();

                succesfull = true;
            }
            catch (ThreadInterruptedException)
            {
                succesfull = false;
                _finished = null;
                _log.WriteLine(LoggingEvent.Debug, _taskName + "Aborting...");
                try
                {
                    program.Kill();
                    _log.WriteLine(LoggingEvent.Debug, _taskName + "Aborted");
                }
                catch (InvalidOperationException e)
                {
                    _log.WriteLine(LoggingEvent.Error,
                          string.Format(CultureInfo.CurrentCulture, _taskName + Resources.CouldNotKillProcess01, _taskInfo.Program, e.Message));
                    _log.WriteLine(LoggingEvent.Debug, _taskName + e.ToString());
                }
                catch (Win32Exception e)
                {
                    _log.WriteLine(LoggingEvent.Error,
                          string.Format(CultureInfo.CurrentCulture, _taskName + Resources.CouldNotKillProcess01, _taskInfo.Program, e.Message));
                    _log.WriteLine(LoggingEvent.Debug, _taskName + e.ToString());
                }
            }
            catch (ObjectDisposedException e)
            {
                _log.WriteLine(LoggingEvent.Error,
                      string.Format(CultureInfo.CurrentCulture, _taskName + Resources.CouldNotStartProcess01, _taskInfo.Program, e.Message));
                _log.WriteLine(LoggingEvent.Debug, _taskName + e.ToString());
            }
            catch (Win32Exception e)
            {
                _log.WriteLine(LoggingEvent.Error,
                      string.Format(CultureInfo.CurrentCulture, _taskName + Resources.CouldNotStartProcess01, _taskInfo.Program, e.Message));
                _log.WriteLine(LoggingEvent.Debug, _taskName + e.ToString());
            }
            finally
            {
                StatusUpdate(succesfull ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);

                if (_finished != null)
                    _finished.Invoke(this, new JobThreadEventArgs(Thread.CurrentThread));
            }

            _log.WriteLine(LoggingEvent.Debug, _taskName + "Finished");
        }

        #endregion

        void SetPriorityHelper(Process process)
        {
            try
            {
                process.PriorityClass = _priority;
                _log.WriteLine(LoggingEvent.Debug, _taskName + "Priority set to " + _priority);
            }
            catch (PlatformNotSupportedException)
            {
                _log.WriteLine(LoggingEvent.Debug, _taskName + string.Format(CultureInfo.InvariantCulture, "Could not set priority '{0}' - platform does not support it. Setting idle priority", _priority.ToString()));
                _priority = ProcessPriorityClass.Idle;
                SetPriorityHelper(process);
            }
            catch (InvalidOperationException e)
            {
                _log.WriteLine(LoggingEvent.Debug, _taskName + string.Format(CultureInfo.InvariantCulture, "Could not set priority to process because it already exited: {0}", e.Message));
            }
            catch (Win32Exception e)
            {
                _log.WriteLine(LoggingEvent.Debug, _taskName + string.Format(CultureInfo.InvariantCulture, "Could not set priority because '{0}'", e.Message));
            }
        }
        private void StatusUpdate(ProcessingStatus status)
        {
            _events.ExecuteProgramStatusUpdate(_taskInfo, status);
        }
    }
}
